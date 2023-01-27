using ATM.Models;

namespace ATM.Services
{
    public interface IPeronalRepository
    {
        void AddAmount(Account account, int amount);
        Person CreatePersonalAccount(Person person);
        Person CreateSavingsAccount(Person person, SavingsAccout SavingsAccount);
        Account GetOwnAccountByAccountID(Person person, int AccountId);
        List<Person> GetAllPersonal();
        Person GetPersonalById(int id);
        Person SubtractAmount(Person person, double amount);
        void TransferAmount(Account fromAccount, Account ToAccount, double amountToTransfer);
        Person GetAccountByAccountID(int toAccountId);
        string TransferToOther(Account fromAccount, int ToAccountId, double AmountToAdd);
    }
}