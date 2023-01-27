using AtmView;

namespace ATM
{
    public class App 
    {
        private readonly IAtmOperations _repository;

        public App(IAtmOperations peronal)
        {
            _repository = peronal;
        }

        public void Run(string[] args)
        {
            _repository.MainMenu();
        }
    }
}
