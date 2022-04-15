using BankOfTheShire.Domain;
using System.Collections.Generic;
using Xunit;

namespace BankOfTheShire.Test
{
    public class BankServiceTests
    {

        private BankService _bankService;
        public BankServiceTests()
        {
            _bankService = new BankService();

            _bankService.BankList.Add(new Bank
            {
                Id = 1,
                Name = "Bank Of The Shire",
                AccountsList = new List<Account>()
            });

            _bankService.BankList.Add(new Bank
            {
                Id = 2,
                Name = "Gotham Bank",
                AccountsList = new List<Account>()
            });
        }

        [Fact]
        public void Task_AddAccount_Successful()
        {

            int expectedBankId = 1;

            Account testAccount = new Account
            {
                AccountNumber = 1,
                BankId = -1,
                AccountType = AccountType.Checking,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };

            bool result = _bankService.AddAccount(testAccount, expectedBankId);

            Assert.True(result);
            Assert.Equal(expectedBankId, testAccount.BankId);
        }

        [Fact]
        public void Task_AddAccount_Failure()
        {

            int expectedBankId = 3;

            Account testAccount = new Account
            {
                AccountNumber = 1,
                BankId = -1,
                AccountType = AccountType.Checking,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };

            bool result = _bankService.AddAccount(testAccount, expectedBankId);

            Assert.False(result);
            Assert.NotEqual(expectedBankId, testAccount.BankId);
        }

    }
}
