using System.Collections.Generic;

namespace BankOfTheShire.Domain
{
    public class Account
    {
        public int AccountNumber { get; set; }
        public int BankId { get; set; }
        public string Owner { get; set; }
        public decimal Balance { get; set; } = 0.00m;
        public AccountType AccountType { get; set; } = AccountType.Checking;
        public AccountInvestmentType AccountInvestmentType { get; set; }

        public List<decimal> TransactionList { get; set; }

    }

    public enum AccountType
    {
        Checking,
        Investment
    }

    public enum AccountInvestmentType
    {
        NotApplicable,
        Individual,
        Corporate
    }
}
