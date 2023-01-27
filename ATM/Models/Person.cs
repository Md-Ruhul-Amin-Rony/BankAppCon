using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

        public MainAccout MainAccout { get; set; }

        public List<SavingsAccout> SavingsAccount { get; set; }

    }

    public class MainAccout : Account
    {
        public string Type { get; set; }
    }

    public class SavingsAccout : Account
    {
        public string Type { get; set; }
    }

    public class Account
    {
        public int AccountID { get; set; }
        public double Amount { get; set; }

    }
}
