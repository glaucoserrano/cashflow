﻿using CashFlow.Application.UseCases.Expenses.Reports.PDF.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.PDF.Fontes;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.PDF;
public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;
    private readonly IExpensesReadOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();

        var expenses = await _repository.FilterByMonth(loggedUser, month);
        if (expenses.Count == 0)
        {
            return Array.Empty<byte>();
        }
        var document = CreateDocument(loggedUser.Name, month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(loggedUser.Name, page);

        var totalExpenses = expenses.Sum(e => e.Amount);
        CreateTotalSpentSection(page, month, totalExpenses);

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row.Cells[0], expense.Title);
            AddHeaderForAmount(row.Cells[3]);
            

            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;
            row.Cells[0].AddParagraph(expense.Date.ToString("d"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddAmountForExpense(row.Cells[3], expense.Amount);

            if (string.IsNullOrWhiteSpace(expense.Description) == false)
            {
                
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

                descriptionRow.Cells[0].AddParagraph(expense.Description);

                descriptionRow.Cells[0].Format.Font = new Font
                {
                    Name = FontHelper.WORKSANS_REGULAR,
                    Size = 10,
                    Color = ColorsHelpers.BLACK,
                };
                descriptionRow.Cells[0].Shading.Color = ColorsHelpers.GREEN_LIGHT;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }
            AddWhiteSpace(table);
        }
        
        return RenderDocuments(document);
    }
    private Document CreateDocument(string author,DateOnly month)
    {
        var document = new Document();
        document.Info.Title = string.Format(ResourceReportGenerationMessage.EXPANSE_FOR, month.ToString("Y")); ;
        document.Info.Author = author;

        var style = document.Styles["Normal"];

        style.Font.Name = FontHelper.RALEWAY_REGULAR;
        
        return document;
    }
    private Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;
     
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;


        return section;

    }
    private void CreateHeaderWithProfilePhotoAndName(string name, Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "Fotodeperfil.jpg");
        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph($"Olá {name}");
        row.Cells[1].Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 16
        };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;

    }
    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportGenerationMessage.EXPANSE_FOR, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font
        {
            Name = FontHelper.RALEWAY_REGULAR,
            Size = 15
        });
        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses:f2}", new Font
        {
            Name = FontHelper.WORKSANS_BLACK,
            Size = 50
        });
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        

        return table;
    }
    private void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelpers.BLACK,
        };
        cell.Shading.Color = ColorsHelpers.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }
    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessage.AMOUNT);
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorsHelpers.WHITE,
        };
        cell.Shading.Color = ColorsHelpers.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    private void AddAmountForExpense(Cell cell,decimal ExpenseAmount)
    {
        cell.AddParagraph($"-{CURRENCY_SYMBOL} {ExpenseAmount:f2}");
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 14,
            Color = ColorsHelpers.BLACK,
        };
        cell.Shading.Color = ColorsHelpers.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }
    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 12,
            Color = ColorsHelpers.BLACK,
        };
        cell.Shading.Color = ColorsHelpers.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }
    private byte[] RenderDocuments(Document document)
    {
        var render = new PdfDocumentRenderer
        {
            Document = document
        };

        render.RenderDocument();

        using var file = new MemoryStream();
        render.PdfDocument.Save(file);

        return file.ToArray();

    }
 }
