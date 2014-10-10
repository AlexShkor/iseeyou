using System;

namespace ISeeYou.Helpers
{
    public static class FinancialFormatter
    {
        public static long DollarsToCents(decimal dollars)
        {
            return (long)(dollars * 100);
        }

        public static decimal CentsToDollars(long cents)
        {
            return cents / 100m;
        }

        public static string CentsToDollarsString(long cents, bool showDollarSign = false)
        {
            return (cents / 100m).ToString(showDollarSign ? "C2" : "F2");
        }

        public static long ToCents(this decimal value)
        {
            return DollarsToCents(value);
        }

        public static decimal ToDollars(this long value)
        {
            return CentsToDollars(value);
        }
    }

    public static class FinancialHelper
    {
        public static long BalanceDueInCents(float discountPerc, float taxesPerc, float subtotalAmountInCents, float amountPaidInCents)
        {
            var discount = NonNegative(1 - discountPerc / 100);
            var taxes = NonNegative(1 + taxesPerc / 100);
            var balanceDue = subtotalAmountInCents * discount * taxes - amountPaidInCents;
            return (long)Math.Round(balanceDue, 0);
        }

        private static float NonNegative(float value)
        {
            return value > 0 ? value : 0;
        }
    }
}