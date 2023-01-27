
using ATM.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace ATM.Services
{
    public class PeronalRepository : IPeronalRepository
    {
        int i = 0;
        public List<Person> AllPersonal { get; set; }
        public PeronalRepository()
        {

        }

        public void AddAmount(Account account, int amount)
        {
            account.Amount += amount;
        }

        public Person CreatePersonalAccount(Person person)
        {
            var allAccounts = GetAllPersonal();
            allAccounts.Add(person);
            return person;
        }

        public Person CreateSavingsAccount(Person person, SavingsAccout SavingsAccount)
        {
            person.SavingsAccount.Add(SavingsAccount);
            return person;
        }

        public Account GetOwnAccountByAccountID(Person person, int AccountId)
        {
            var account = person.MainAccout.AccountID == AccountId;
            var savingsAccount = person.SavingsAccount.FirstOrDefault(s => s.AccountID == AccountId);
            if (account)
            {
                return person.MainAccout;
            }
            else if (savingsAccount is not null)
            {
                return savingsAccount;
            }
            else
            {
                return null;
            }
        }

        public List<Person> GetAllPersonal()
        {
            if (AllPersonal is null)
            {
                AllPersonal = InitialPersonals();
                return AllPersonal;
            }
            else
            {
                return AllPersonal;
            }
        }

        public Person GetPersonalById(int id)
        {
            var allAccountHolders = GetAllPersonal();
            var account = allAccountHolders.FirstOrDefault(a => a.PersonId == id);

            return account;
        }

        public Person SubtractAmount(Person person, double amount)
        {
            person.MainAccout.Amount -= amount;
            return person;
        }

        public void TransferAmount(Account fromAccount, Account ToAccount, double amountToTransfer)
        {
            fromAccount.Amount -= amountToTransfer;
            ToAccount.Amount += amountToTransfer;
        }

        private List<Person> InitialPersonals()
        {

            //Shawon saving accout
            List<SavingsAccout> saccout1 = new List<SavingsAccout>();
            SavingsAccout savings = new SavingsAccout()
            {
                AccountID = 12,
                Amount = 5000,
                Type = "SavingsAccount"
            };
            saccout1.Add(savings);

            //Abhi savings account
            List<SavingsAccout> saccout2 = new List<SavingsAccout>();
            SavingsAccout savings2 = new SavingsAccout()
            {
                AccountID = 23,
                Amount = 5000,
                Type = "SavingsAccount"
            }; SavingsAccout savings3 = new SavingsAccout()
            {
                AccountID = 34,
                Amount = 8000,
                Type = "SavingsAccount"
            };
            saccout2.Add(savings2);
            saccout2.Add(savings3);

            //Hasan savings account
            List<SavingsAccout> saccout3 = new List<SavingsAccout>();
            SavingsAccout savings4 = new SavingsAccout()
            {
                AccountID = 63,
                Amount = 5000,
                Type = "SavingsAccount"
            };
            SavingsAccout savings5 = new SavingsAccout()
            {
                AccountID = 55,
                Amount = 5000,
                Type = "SavingsAccount"
            };
            saccout3.Add(savings4);
            saccout3.Add(savings5);

            //Adil savings account
            List<SavingsAccout> saccout4 = new List<SavingsAccout>();
            SavingsAccout savings6 = new SavingsAccout()
            {
                AccountID = 66,
                Amount = 22,
                Type = "SavingsAccount"
            };
            saccout4.Add(savings6);

            //Ruhul savings account
            List<SavingsAccout> saccout5 = new List<SavingsAccout>();
            SavingsAccout savings7 = new SavingsAccout()
            {
                AccountID = 77,
                Amount = 890,
                Type = "SavingsAccount"
            };
            SavingsAccout savings8 = new SavingsAccout()
            {
                AccountID = 57,
                Amount = 5000,
                Type = "SavingsAccount"
            };
            SavingsAccout savings9 = new SavingsAccout()
            {
                AccountID = 99,
                Amount = 55487,
                Type = "SavingsAccount"
            };
            saccout5.Add(savings7);
            saccout5.Add(savings8);
            saccout5.Add(savings9);

            //List of 5 initial personal and their details
            List<Person> people = new List<Person>()
             {
               new Person {Name ="Shawon", Password="123", PersonId=1, MainAccout = new MainAccout{ AccountID = 11, Amount = 100000, Type = "SalaryAccount"}, SavingsAccount = saccout1},
               new Person {Name ="Abhi", Password="123", PersonId=2, MainAccout = new MainAccout{ AccountID = 22, Amount = 566, Type = "SalaryAccount"}, SavingsAccount = saccout2},
               new Person {Name ="Hasan", Password="123", PersonId=3, MainAccout = new MainAccout{ AccountID = 33, Amount = 22325, Type = "SalaryAccount"}, SavingsAccount = saccout3},
               new Person {Name ="Adil", Password="123", PersonId=4, MainAccout = new MainAccout{ AccountID = 44, Amount = 100, Type = "SalaryAccount"}, SavingsAccount = saccout4},
               new Person {Name ="Ruhul", Password="123", PersonId=5, MainAccout = new MainAccout{ AccountID = 55, Amount = 9999999, Type = "SalaryAccount"}, SavingsAccount = saccout5}
             };


            //JsonSerializerOptions options = new()
            //{
            //    PropertyNameCaseInsensitive = true,
            //};
            //try
            //{
            //    List<Person> peopleData = JsonSerializer.Deserialize<List<Person>>
            //                                 (
            //                                   File.ReadAllText("Data.json"), options
            //                                  );

            //    if (peopleData is not null)
            //    {
            //        people = peopleData;
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}

            return people;

        }

        public Person GetAccountByAccountID(int toAccountId)
        {
            var allAccounts = GetAllPersonal();
            var personFromMainAccout = allAccounts.FirstOrDefault(a => a.MainAccout.AccountID == toAccountId);
            var personFromSavingsAccount = new Person();
            if (personFromMainAccout is not null)
            {
                return personFromMainAccout;
            }
            else 
            {
                foreach (var acc in allAccounts)
                {
                    foreach (var item in acc.SavingsAccount)
                    {
                        if (item.AccountID == toAccountId)
                        {
                            personFromSavingsAccount = acc;
                        }
                    }
                }
                return personFromSavingsAccount;
            }
        }

        public string TransferToOther(Account fromAccount, int ToAccountId, double AmountToAdd)
        {
            var getAccountToSendMoney = GetAccountByAccountID(ToAccountId);
            if (getAccountToSendMoney.MainAccout.AccountID == ToAccountId)
            {
                getAccountToSendMoney.MainAccout.Amount += AmountToAdd;
                fromAccount.Amount -= AmountToAdd;
                return $"Transfer successful to Name: {getAccountToSendMoney.Name}, Account ID:{ToAccountId}";
            }
            else if (getAccountToSendMoney.SavingsAccount.Exists(s => s.AccountID == ToAccountId))
            {
                var accountToAdd = getAccountToSendMoney.SavingsAccount.FirstOrDefault(s => s.AccountID == ToAccountId);
                accountToAdd.Amount += AmountToAdd;
                fromAccount.Amount -= AmountToAdd;
                return $"Transfer successful to Name: {getAccountToSendMoney.Name}, Account ID:{ToAccountId}";
            }
            else
            {
                return $"Transfer faild.!!";
            }
        }
    }
}
