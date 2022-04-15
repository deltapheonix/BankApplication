using BankOfTheShire.Domain;
using System.Collections.Generic;
using System.Linq;

namespace BankOfTheShire
{
    public class AccountService
    {
        public Account CreateAccount(string ownerName, int bankId)
        {
            return new Account()
            {
                AccountNumber = 1,
                BankId = bankId,
                Owner = ownerName,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = new List<decimal>()
            };
        }

        // If an owner opens an account with inital funds.
        public Account CreateAccount(string ownerName, int bankId, decimal initialAmount)
        {
            List<decimal> initialTransactionList = new List<decimal>();
            initialTransactionList.Add(initialAmount);

            return new Account()
            {
                AccountNumber = 1,
                BankId = bankId,
                Owner = ownerName,
                Balance = initialAmount,
                AccountInvestmentType = AccountInvestmentType.NotApplicable,
                TransactionList = initialTransactionList
            };
        }

        public Account CreateInvestmentAccount(string ownerName, int bankId, AccountInvestmentType investmentType)
        {
            return new Account()
            {
                AccountNumber = 1,
                BankId = bankId,
                Owner = ownerName,
                AccountType = AccountType.Investment,
                AccountInvestmentType = investmentType,
                TransactionList = new List<decimal>()
            };
        }

        // If an owner opens an account with inital funds.
        public Account CreateInvestmentAccount(string ownerName, int bankId, decimal initialAmount, AccountInvestmentType investmentType)
        {
            List<decimal> initialTransactionList = new List<decimal>();
            initialTransactionList.Add(initialAmount);

            return new Account()
            {
                AccountNumber = 1,
                BankId = bankId,
                Owner = ownerName,
                Balance = initialAmount,
                AccountType = AccountType.Investment,
                AccountInvestmentType = investmentType,
                TransactionList = initialTransactionList
            };
        }

        public bool InvestmentWithdrawl(Account patronAccount, decimal withdrawlAmount)
        {
            if (patronAccount.AccountInvestmentType == AccountInvestmentType.Individual && withdrawlAmount > 500.00M)
            {
                return false;
            }

            // No need to duplicate code. Just re-use the base withdrawl function.
            return Withdrawl(patronAccount, withdrawlAmount);

        }


        public bool Withdrawl(Account patronAccount, decimal withdrawlAmount)
        {
            if (patronAccount.Balance < withdrawlAmount)
            {
                return false;
            }

            patronAccount.TransactionList.Add(-withdrawlAmount);

            patronAccount.Balance = patronAccount.TransactionList.Sum();

            return true;

        }

        public bool Deposit(Account patronAccount, decimal depositAmount)
        {
            if (depositAmount == 0.00M)
            {
                return false;
            }

            patronAccount.TransactionList.Add(depositAmount);

            patronAccount.Balance = patronAccount.TransactionList.Sum();

            return true;
        }


        public bool InvestmentTransfer(Account sendingAccount, Account receivingAccount, decimal transferAmount)
        {
            if (sendingAccount.AccountInvestmentType == AccountInvestmentType.Individual && transferAmount > 500.00M)
            {
                return false;
            }

            // Repeatable code not necessary. Re-using base transfer method.
            return Transfer(sendingAccount, receivingAccount, transferAmount);
        }

        public bool Transfer(Account sendingAccount, Account receivingAccount, decimal amount)
        {
            if (sendingAccount.Balance < amount)
            {
                return false;
            }

            sendingAccount.TransactionList.Add(-amount);
            receivingAccount.TransactionList.Add(amount);

            sendingAccount.Balance = sendingAccount.TransactionList.Sum();
            receivingAccount.Balance = receivingAccount.TransactionList.Sum();

            return true;
            
        }
    }
}
