using BankOfTheShire.Domain;
using System.Collections.Generic;
using Xunit;

namespace BankOfTheShire.Test
{
    public class AccountServiceTests
    {
        AccountService _accountService;
        BankService _bankService;
        public AccountServiceTests()
        {
            _accountService = new AccountService();
            _bankService = new BankService();

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

        [Theory]
        [InlineData("Frodo Baggins", 1)]
        [InlineData("Bruce Wayne", 2)]
        public void Test_CreateAccount_Success(string testOwner, int bankId)
        {
            Account result = _accountService.CreateAccount(testOwner, bankId);

            Assert.Equal(testOwner, result.Owner);
            Assert.Equal(bankId, result.BankId);
            Assert.Equal(0.00m, result.Balance);
        }

        [Theory]
        [InlineData("Frodo Baggins", 1, 51.34)]
        [InlineData("Bruce Wayne", 2, 56.30)]
        public void Test_CreateAccount_Initial_Deposit_Success(string testOwner, int bankId, decimal testInitialBalance)
        {
            Account result = _accountService.CreateAccount(testOwner, bankId, testInitialBalance);

            Assert.Equal(testOwner, result.Owner);
            Assert.Equal(bankId, result.BankId);
            Assert.Equal(testInitialBalance, result.Balance);
        }


        [Theory]
        [InlineData("Frodo Baggins", 1, AccountInvestmentType.Individual)]
        [InlineData("Bruce Wayne", 2, AccountInvestmentType.Corporate)]
        public void Test_CreateInvestmentAccount_Success(string testOwner, int bankId, AccountInvestmentType testInvestmentType)
        {
            Account result = _accountService.CreateInvestmentAccount(testOwner, bankId, testInvestmentType);

            Assert.Equal(testOwner, result.Owner);
            Assert.Equal(bankId, result.BankId);
            Assert.Equal(0.00m, result.Balance);
        }

        [Theory]
        [InlineData("Frodo Baggins", 1, 51.34, AccountInvestmentType.Individual)]
        [InlineData("Bruce Wayne", 2, 56.30, AccountInvestmentType.Corporate)]
        public void Test_CreateInvestmentAccount_Initial_Deposit_Success(string testOwner, int bankId, decimal testInitialBalance, AccountInvestmentType testInvestmentType)
        {
            Account result = _accountService.CreateInvestmentAccount(testOwner, bankId, testInitialBalance, testInvestmentType);

            Assert.Equal(testOwner, result.Owner);
            Assert.Equal(bankId, result.BankId);
            Assert.Equal(testInitialBalance, result.Balance);
        }


        // Using an IEnumberable object to pass decimals through the attribute statement via MemberData.
        // InlineData does not support passing through decimals.
        public static IEnumerable<object[]> DepositData =>
        new List<object[]>
        {
            new object[] { 100.2M, 20.1M, 32.3M },
            new object[] { 4.00M, 6.12M, 10.12M }
        };


        [Theory]
        [MemberData(nameof(DepositData))]
        public void Test_Account_Deposit_Success(decimal depositAmount1, decimal depositAmount2, decimal depositAmount3)
        {
            decimal expectedBalance = depositAmount1 + depositAmount2 + depositAmount3;

            Account testAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                AccountType = AccountType.Checking,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };

            _accountService.Deposit(testAccount, depositAmount1);
            _accountService.Deposit(testAccount, depositAmount2);
            _accountService.Deposit(testAccount, depositAmount3);

            Assert.Equal(expectedBalance, testAccount.Balance);
            Assert.Equal(depositAmount1, testAccount.TransactionList[0]);
            Assert.Equal(depositAmount2, testAccount.TransactionList[1]);
            Assert.Equal(depositAmount3, testAccount.TransactionList[2]);

        }

        // Using an IEnumberable object to pass decimals through the attribute statement via MemberData.
        // InlineData does not support passing through decimals.
        public static IEnumerable<object[]> WithdrawlData =>
        new List<object[]>
        {
            new object[] { 500.00M, 20.1M, 32.3M, 34.50M },
            new object[] { 350.00M, 60.12M, 10.12M, 22.60M },
            new object [] { 1000.00M, 600.12M, 10.12M, 22.60M }
        };

        [Theory]
        [MemberData(nameof(WithdrawlData))]
        public void Test_Withdrawl_Success(decimal initialAmount, decimal withdrawlAmount1, decimal withdrawlAmount2, decimal withdrawlAmount3)
        {
            decimal expectedBalance = initialAmount - (withdrawlAmount1 + withdrawlAmount2 + withdrawlAmount3);

            Account testAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                Balance = initialAmount,
                AccountType = AccountType.Checking,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };

            // Adding initial amount to transaction list.
            testAccount.TransactionList.Add(initialAmount);

            _accountService.Withdrawl(testAccount, withdrawlAmount1);
            _accountService.Withdrawl(testAccount, withdrawlAmount2);
            _accountService.Withdrawl(testAccount, withdrawlAmount3);

            Assert.Equal(expectedBalance, testAccount.Balance);

            // Testing to ensure the transaction list is in the proper order.
            Assert.Equal(withdrawlAmount1, -testAccount.TransactionList[1]);
            Assert.Equal(withdrawlAmount2, -testAccount.TransactionList[2]);
            Assert.Equal(withdrawlAmount3, -testAccount.TransactionList[3]);
        }

        // Using an IEnumberable object to pass decimals through the attribute statement via MemberData.
        // InlineData does not support passing through decimals.
        public static IEnumerable<object[]> InvestmentWithdrawlData =>
        new List<object[]>
        {
            new object[] { 500.00M, 20.1M, 32.3M, 34.50M },
            new object[] { 350.00M, 60.12M, 10.12M, 22.60M },
            new object [] { 1000.00M, 600.12M, 10.12M, 22.60M }
        };

        [Theory]
        [MemberData(nameof(InvestmentWithdrawlData))]
        public void Test_InvestmentWithdrawl_Success(decimal initialAmount, decimal withdrawlAmount1, decimal withdrawlAmount2, decimal withdrawlAmount3)
        {
            decimal expectedBalance = initialAmount - (withdrawlAmount1 + withdrawlAmount2 + withdrawlAmount3);

            Account testAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                Balance = initialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = AccountInvestmentType.Corporate,
                TransactionList = new List<decimal>()
            };

            // Adding initial amount to transaction list.
            testAccount.TransactionList.Add(initialAmount);

            _accountService.InvestmentWithdrawl(testAccount, withdrawlAmount1);
            _accountService.InvestmentWithdrawl(testAccount, withdrawlAmount2);
            _accountService.InvestmentWithdrawl(testAccount, withdrawlAmount3);

            Assert.Equal(expectedBalance, testAccount.Balance);

            // Testing to ensure the transaction list is in the proper order.
            Assert.Equal(withdrawlAmount1, -testAccount.TransactionList[1]);
            Assert.Equal(withdrawlAmount2, -testAccount.TransactionList[2]);
            Assert.Equal(withdrawlAmount3, -testAccount.TransactionList[3]);
        }

        // Using an IEnumberable object to pass decimals through the attribute statement via MemberData.
        // InlineData does not support passing through decimals.
        public static IEnumerable<object[]> InvestmentWithdrawlAccountLimitData =>
        new List<object[]>
        {
            new object [] { 1000.00M, 600.12M }
        };

        [Theory]
        [MemberData(nameof(InvestmentWithdrawlAccountLimitData))]
        public void Test_InvestmentWithdrawl_Individual_Account_Limit_Failure(decimal initialAmount, decimal withdrawlAmount1)
        {

            Account testAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                Balance = initialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = AccountInvestmentType.Individual,
                TransactionList = new List<decimal>()
            };

            // Adding initial amount to transaction list.
            testAccount.TransactionList.Add(initialAmount);

            bool result = _accountService.InvestmentWithdrawl(testAccount, withdrawlAmount1);

            Assert.False(result);

            Assert.Equal(initialAmount, testAccount.Balance);
        }

        public static IEnumerable<object[]> TransferData =>
        new List<object[]>
        {
            new object[] { 500.00M, 220.60M, 32.30M },
            new object[] { 333.00M, 123.12M, 100.12M },
            new object [] { 1000.00M, 600.12M, 10.12M }
        };

        [Theory]
        [MemberData(nameof(TransferData))]
        public void Test_Transfer_Success(decimal senderInitialAmount, decimal receiverInitialAmount, decimal amountSent)
        {
            Account testSenderAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                Balance = senderInitialAmount,
                AccountType = AccountType.Checking,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };

            testSenderAccount.TransactionList.Add(senderInitialAmount);

            Account testReceiverAccount = new Account
            {
                AccountNumber = 1,
                BankId = 1,
                Owner = "Frodo Baggins",
                Balance = receiverInitialAmount,
                AccountType = AccountType.Checking,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };

            testReceiverAccount.TransactionList.Add(receiverInitialAmount);

            decimal expectedSenderBalance = senderInitialAmount - amountSent;

            decimal expectedReceiverBalance = receiverInitialAmount + amountSent;

            bool result = _accountService.Transfer(testSenderAccount, testReceiverAccount, amountSent);

            Assert.True(result);

            Assert.Equal(expectedSenderBalance, testSenderAccount.Balance);
            Assert.Equal(expectedReceiverBalance, testReceiverAccount.Balance);
        }

        public static IEnumerable<object[]> InvestmentTransferData =>
        new List<object[]>
        {
            new object[] { 500.00M, 220.60M, 32.30M },
            new object[] { 333.00M, 123.12M, 100.12M },
            new object [] { 1000.00M, 600.12M, 10.12M }
        };

        [Theory]
        [MemberData(nameof(InvestmentTransferData))]
        public void Test_InvestmentTransfer_Success(decimal senderInitialAmount, decimal receiverInitialAmount, decimal amountSent)
        {
            Account testSenderAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                Balance = senderInitialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = AccountInvestmentType.Corporate,
                TransactionList = new List<decimal>()
            };

            testSenderAccount.TransactionList.Add(senderInitialAmount);

            Account testReceiverAccount = new Account
            {
                AccountNumber = 1,
                BankId = 1,
                Owner = "Frodo Baggins",
                Balance = receiverInitialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = AccountInvestmentType.Individual,
                TransactionList = new List<decimal>()
            };

            testReceiverAccount.TransactionList.Add(receiverInitialAmount);

            decimal expectedSenderBalance = senderInitialAmount - amountSent;

            decimal expectedReceiverBalance = receiverInitialAmount + amountSent;

            bool result = _accountService.InvestmentTransfer(testSenderAccount, testReceiverAccount, amountSent);

            Assert.True(result);

            Assert.Equal(expectedSenderBalance, testSenderAccount.Balance);
            Assert.Equal(expectedReceiverBalance, testReceiverAccount.Balance);
        }



        public static IEnumerable<object[]> InvestmentTransferAccountLimitData =>
       new List<object[]>
       {
            new object [] { 1000.00M, 600.12M, 600.12M }
       };

        // Here we test to see if the $500 withdrawl limit works as expected. 
        //The method should return false if the amountSent is greater than $500.
        [Theory]
        [MemberData(nameof(InvestmentTransferAccountLimitData))]
        public void Test_InvestmentTransfer_Individual_Account_Limit_Failure(decimal senderInitialAmount, decimal receiverInitialAmount, decimal amountSent)
        {
            Account testSenderAccount = new Account
            {
                AccountNumber = 1,
                BankId = 2,
                Owner = "Bruce Wayne",
                Balance = senderInitialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = AccountInvestmentType.Individual,
                TransactionList = new List<decimal>()
            };

            testSenderAccount.TransactionList.Add(senderInitialAmount);

            Account testReceiverAccount = new Account
            {
                AccountNumber = 1,
                BankId = 1,
                Owner = "Frodo Baggins",
                Balance = receiverInitialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = AccountInvestmentType.Individual,
                TransactionList = new List<decimal>()
            };

            testReceiverAccount.TransactionList.Add(receiverInitialAmount);

            decimal expectedSenderBalance = senderInitialAmount - amountSent;

            decimal expectedReceiverBalance = receiverInitialAmount + amountSent;

            bool result = _accountService.InvestmentTransfer(testSenderAccount, testReceiverAccount, amountSent);

            Assert.False(result);

            Assert.Equal(senderInitialAmount, testSenderAccount.Balance);
            Assert.Equal(receiverInitialAmount, testReceiverAccount.Balance);
        }
    }
}
