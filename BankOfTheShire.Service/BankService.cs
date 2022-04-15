using BankOfTheShire.Domain;
using System;
using System.Collections.Generic;

namespace BankOfTheShire
{
    public class BankService
    {
        //Adding a list of banks here because there is no presentation layer/ User Interface.
        public List<Bank> BankList;
        public BankService()
        {
            BankList = new List<Bank>();
        }

        public bool AddAccount (Account newAccount, int bankId)
        {
            Bank searchResult = BankList.Find(x => x.Id == bankId);
            
            if (searchResult == null)
            {
                // If there was a user interface for this application, we would notify the user the account could not be found.
                return false;
            }

            newAccount.BankId = bankId;

            // This will assign an account number that is not duplicate and iterative.
            newAccount.AccountNumber = searchResult.AccountsList.Count;

            searchResult.AccountsList.Add(newAccount);

            return true;
        }

        

        
    }
}
