using System;
using System.Collections.Generic;
using System.Text;

namespace BankOfTheShire.Domain
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Account> AccountsList { get; set; }
    }
}
