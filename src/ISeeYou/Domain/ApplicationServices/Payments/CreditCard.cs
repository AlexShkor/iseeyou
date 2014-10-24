namespace ISeeYou.Domain.ApplicationServices.Payments
{
    public class CreditCard
    {
        public string Number { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Cvv { get; set; }
        public string CardholderName { get; set; }
    }
}
