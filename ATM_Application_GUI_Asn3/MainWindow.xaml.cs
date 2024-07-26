using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AtmApp
{
    public partial class MainWindow : Window
    {
        private Bank bank;
        private Account currentAccount;

        public MainWindow()
        {
            InitializeComponent();
            bank = new Bank();
        }

        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int accountNumber = int.Parse(AccountNumberTextBox.Text);
                double initialBalance = double.Parse(InitialBalanceTextBox.Text);
                double interestRate = double.Parse(InterestRateTextBox.Text) / 100;
                string accountHolderName = AccountHolderNameTextBox.Text;

                if (bank.RetrieveAccount(accountNumber) != null)
                {
                    MessageBox.Show("Account with this number already exists.");
                    return;
                }

                var account = new Account(accountNumber, initialBalance, interestRate, accountHolderName);
                bank.AddAccount(account);

                MessageBox.Show($"Account created successfully for {accountHolderName}!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void SelectAccountButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int accountNumber = int.Parse(AccountNumberTextBox.Text);
                currentAccount = bank.RetrieveAccount(accountNumber);

                if (currentAccount != null)
                {
                    MessageBox.Show($"Account selected: {currentAccount.AccountHolderName}");
                    // You can enable account-related actions here
                }
                else
                {
                    MessageBox.Show("Account not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void CheckBalanceButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentAccount != null)
            {
                MessageBox.Show($"Balance: ${currentAccount.Balance:F2}");
            }
            else
            {
                MessageBox.Show("No account selected.");
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double depositAmount = double.Parse(DepositAmountTextBox.Text);
                if (currentAccount != null)
                {
                    currentAccount.Deposit(depositAmount);
                    MessageBox.Show("Deposit successful.");
                }
                else
                {
                    MessageBox.Show("No account selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double withdrawalAmount = double.Parse(WithdrawAmountTextBox.Text);
                if (currentAccount != null)
                {
                    currentAccount.Withdraw(withdrawalAmount);
                    MessageBox.Show("Withdrawal processed.");
                }
                else
                {
                    MessageBox.Show("No account selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void DisplayTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentAccount != null)
            {
                var transactions = currentAccount.GetTransactions();
                string transactionList = string.Join("\n", transactions);
                MessageBox.Show(transactionList);
            }
            else
            {
                MessageBox.Show("No account selected.");
            }
        }
    }

    public class Account
    {
        public int AccountNumber { get; set; }
        public double Balance { get; set; }
        public double InterestRate { get; set; }
        public string AccountHolderName { get; set; }
        private List<string> transactions;

        public Account(int accountNumber, double initialBalance, double interestRate, string accountHolderName)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
            InterestRate = interestRate;
            AccountHolderName = accountHolderName;
            transactions = new List<string>();
            RecordTransaction($"Account created with initial balance: ${initialBalance:F2}");
        }

        public void Deposit(double amount)
        {
            Balance += amount;
            RecordTransaction($"Deposited: ${amount:F2}");
        }

        public void Withdraw(double amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                RecordTransaction($"Withdrew: ${amount:F2}");
            }
            else
            {
                RecordTransaction($"Failed withdrawal attempt: ${amount:F2}");
            }
        }

        public void RecordTransaction(string transaction)
        {
            transactions.Add(transaction);
        }

        public List<string> GetTransactions()
        {
            return transactions;
        }
    }

    public class Bank
    {
        private List<Account> accounts;

        public Bank()
        {
            accounts = new List<Account>();
            for (int i = 100; i < 110; i++)
            {
                accounts.Add(new Account(i, 100.0, 0.03, $"Default User {i - 99}"));
            }
        }

        public void AddAccount(Account account)
        {
            accounts.Add(account);
        }

        public Account RetrieveAccount(int accountNumber)
        {
            return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
        }
    }
}
