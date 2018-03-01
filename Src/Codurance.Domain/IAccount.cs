namespace Codurance.Domain
{
    public interface IAccount
    {
        void Deposit(int amount);
        void PrintStatement();
        void Withdraw(int amount);
    }
}