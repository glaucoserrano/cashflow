﻿using CashFlow.Domain.Enum;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;
public static class PaymentTypeExtensions
{
    public static string PaymentTypeToString(this PaymentType payment)
    {
        return payment switch
        {
            PaymentType.Cash => ResourceReportGenerationMessage.CASH,
            PaymentType.CreditCard => ResourceReportGenerationMessage.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportGenerationMessage.DEBIT_CARD,
            PaymentType.EletronicTransfer => ResourceReportGenerationMessage.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}
