using ATM.Models;
using ATM.Services;
using System.Security.Cryptography;

namespace AtmView
{
    public class AtmOperations : IAtmOperations
    {
        private readonly IPeronalRepository _personalRepository;
        Random random = new Random();

        public AtmOperations(IPeronalRepository peronal)
        {
            _personalRepository = peronal;
        }
        public void MainMenu()
        {
            try
            {
                Console.WriteLine("Welocme");
                Console.WriteLine("1.Login to your account.");
                Console.WriteLine("2.Create new account.");
                var choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 1)
                {
                    ExistingUserMenu();
                }
                else if (choice == 2)
                {
                    CreateAccount();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        private void CreateAccount()
        {
            Console.WriteLine("Write your Name:");
            var name = Console.ReadLine();

            Console.WriteLine("Write your personalnumber:");
            var personalNumber = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Write your password:");
            var password = Console.ReadLine();

            Console.WriteLine("Amount To deposite:");
            var amount = Convert.ToInt32(Console.ReadLine());
            if (amount < 0)
            {
                amount = 0;
            }
            MainAccout mainAccout = new MainAccout()
            {
                Amount = amount,
                AccountID = random.Next(99),
            };

            Person person = new Person()
            {
                Name = name,
                Password = password,
                PersonId = personalNumber,
                MainAccout = mainAccout,
                SavingsAccount = new List<SavingsAccout>()
            };

            var newPersonal = _personalRepository.CreatePersonalAccount(person);
            if (newPersonal is not null)
            {
                UserOptions(newPersonal);
            }
            else
            {
                Console.WriteLine("Could not create the account, try agin later!!!");
                MainMenu();
            }
        }

        private void ExistingUserMenu()
        {
            Console.WriteLine("1.Write your Id:");
            var id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Wite your pass:");
            var pass = Console.ReadLine();
            if (id != 0 && pass != null)
            {
                var user = _personalRepository.GetPersonalById(id);
                if (user is not null && user.Password == pass)
                {
                    Console.Write($"Hi {user.Name},");
                    UserOptions(user);
                }
                else
                {
                    Console.WriteLine("Wrong Password, try again!!");
                    Console.WriteLine("**************************************************************");
                    MainMenu();
                }
            }
            else
            {
                Console.WriteLine("Please enter your ID and Password.");
                Console.WriteLine("**************************************************************");
                MainMenu();
            }
        }

        private void MenuOptions(int choice, Person person)
        {
            switch (choice)
            {
                case 1:
                    ViewAccountsDetails(person);
                    break;

                case 2:
                    Transfer(person);
                    break;

                case 3:
                    Withdraw(person);
                    break;

                case 4:
                    InsertMoney(person);
                    break;

                case 5:
                    CreateSavingsAccount(person);
                    break;

                case 6:
                    SendMoneyToOthers(person);
                    break;

                case 7:
                    LogOut();
                    break;

                default:
                    Console.WriteLine("Invalid Choice");
                    break;

            }
        }

        private void SendMoneyToOthers(Person person)
        {
            try
            {
                Console.WriteLine("Your Account ID:");
                var fromAccountId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Transfer To Account ID:");
                var toAccountId = Convert.ToInt32(Console.ReadLine());
                var getfromAccount = _personalRepository.GetOwnAccountByAccountID(person, fromAccountId);
                var getToAccount = _personalRepository.GetAccountByAccountID(toAccountId);

                if (getfromAccount is not null && getToAccount is not null)
                {
                    Console.WriteLine("Write amount to transfer:");
                    var amount = Convert.ToDouble(Console.ReadLine());
                    if (getfromAccount.Amount >= amount)
                    {
                       var response = _personalRepository.TransferToOther(getfromAccount, toAccountId, amount);
                        Console.WriteLine(response);
                        ViewAccountsDetails(person);
                        UserOptions(person);
                    }
                    else
                    {
                        Console.WriteLine("Not sufficient funds try again!!");
                        Console.WriteLine("**************************************************************");
                        UserOptions(person);
                    }
                }
                else
                {
                    Console.WriteLine("No account found!");
                }
                Console.WriteLine("**************************************************************");
                UserOptions(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void CreateSavingsAccount(Person person)
        {
            SavingsAccout savings = new SavingsAccout()
            {
                AccountID = random.Next(999),
                Amount = 0,
                Type = "SavingsAccount"
            };

            var updatedInfo = _personalRepository.CreateSavingsAccount(person, savings);
            Console.WriteLine("Account added");
            ViewAccountsDetails(updatedInfo);
        }

        private void InsertMoney(Person person)
        {
            try
            {
                Console.WriteLine("Account Id to insert:");
                var yourAccountId = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Amount To deposite:");
                var amount = Convert.ToInt32(Console.ReadLine());

                var account = _personalRepository.GetOwnAccountByAccountID(person, yourAccountId);
                if (account is not null && amount > 0)
                {
                    _personalRepository.AddAmount(account, amount);
                    ViewAccountsDetails(person);
                }
                else
                {
                    Console.WriteLine("Account not found or cannot add 0.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LogOut()
        {
            Console.WriteLine("**************************************************************");
            MainMenu();
        }

        private void Transfer(Person person)
        {
            if (person.SavingsAccount.Count > 0)
            {
                try
                {
                    Console.WriteLine("Transfer From Account ID:");
                    var fromAccountId = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Transfer To Account ID:");
                    var toAccountId = Convert.ToInt32(Console.ReadLine());
                    var getfromAccount = _personalRepository.GetOwnAccountByAccountID(person, fromAccountId);
                    var getToAccount = _personalRepository.GetOwnAccountByAccountID(person, toAccountId);

                    if (getfromAccount is not null && getToAccount is not null)
                    {
                        Console.WriteLine("Write amount to transfer:");
                        var amount = Convert.ToDouble(Console.ReadLine());
                        if (getfromAccount.Amount >= amount)
                        {
                            _personalRepository.TransferAmount(getfromAccount, getToAccount, amount);
                            ViewAccountsDetails(person);
                            UserOptions(person);
                        }
                        else
                        {
                            Console.WriteLine("Not sufficient funds try again!!");
                            Console.WriteLine("**************************************************************");
                            UserOptions(person);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No account found!");
                    }
                    Console.WriteLine("**************************************************************");
                    UserOptions(person);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("User have only 1 account.");
                Console.WriteLine("**************************************************************");
                UserOptions(person);
            }
        }

        private void ViewAccountsDetails(Person person)
        {
            Console.WriteLine("**************************************************************");
            Console.WriteLine($"Main Account ID: {person.MainAccout.AccountID}, Balance: {person.MainAccout.Amount}");
            if (person.SavingsAccount.Count > 0)
            {
                for (int i = 0; i < person.SavingsAccount.Count; i++)
                {
                    Console.WriteLine($"Saving Account #{i + 1}");
                    Console.WriteLine($"Account ID: {person.SavingsAccount[i].AccountID}, Balance: {person.SavingsAccount[i].Amount}");

                }
            }
            Console.WriteLine("**************************************************************");
            UserOptions(person);
        }

        private void Withdraw(Person person)
        {
            try
            {
                Console.WriteLine("Amount to Withdraw:");
                var amount = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Write your password to confirm Withdraw:");
                var password = Console.ReadLine();
                if (person.Password == password)
                {
                    if (person.MainAccout.Amount >= amount)
                    {
                        var updatedInfo = _personalRepository.SubtractAmount(person, amount);
                        Console.WriteLine("Take your money. Thank you for your Withdraw.");
                        ViewAccountsDetails(updatedInfo);
                        Console.WriteLine("**************************************************************");
                        UserOptions(updatedInfo);
                    }
                    else
                    {
                        Console.WriteLine("Not sufficient funds try again!!");
                        Console.WriteLine("**************************************************************");
                        UserOptions(person);
                    }
                }
                else
                {
                    Console.WriteLine("Wrong password, try again!!");
                    Console.WriteLine("**************************************************************");
                    UserOptions(person);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UserOptions(Person person)
        {
            try
            {
                Console.WriteLine("Chose your option bellow:");
                Console.WriteLine("1.View your accounts and balance");
                Console.WriteLine("2.Transfer between accounts");
                Console.WriteLine("3.Withdraw money");
                Console.WriteLine("4.Insert money");
                Console.WriteLine("5.Create savings account");
                Console.WriteLine("6.Send money to other account");
                Console.WriteLine("7.Log out");
                var choice = Convert.ToInt32(Console.ReadLine());
                MenuOptions(choice, person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}