namespace TranslateSystem.Data
{
    public class AccountData
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int CurrencyId { get; set; }

        public double Balance { get; set; }
        
        public User User { get; set; }
        public Currency Currency { get; set; }
    }
}
