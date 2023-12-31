﻿using BankTransfer.Interfaces;
using BankTransfer.Models;

namespace BankTransfer
{
    internal class Account : IAccount
    {

        public string AccountNumber { get; } = Guid.NewGuid().ToString();
        public double Balance { get; private set; }
        private readonly Stack<Transaction> _transactions = new Stack<Transaction>();
        public event EventHandler<Transaction>? TransactionExecuted;

        public Account()
        {
            Balance = new Random(AccountNumber.GetHashCode()).Next(0, 100) * 1000;
        }

        public void AddTransaction(Transaction transaction)
        {
            switch (transaction.Type)
            {
                case TransactionType.Credit:
                    Balance += transaction.Amount;
                    break;
                case TransactionType.Debit:
                    if (Balance < transaction.Amount)
                        throw new Exception();
                    Balance -= transaction.Amount;
                    break;
                default:
                    return;
            }
            _transactions.Push(transaction);
            TransactionExecuted?.Invoke(this, transaction);
        }

        public IEnumerable<Transaction> FilterTransactions(TransactionType? type, IAccount? account)
        {
            var query = _transactions.AsQueryable();
            if (type.HasValue)
                query = query.Where(x => x.Type == type);
            if (account != null)
                query = query.Where(x => x.Account.AccountNumber == account.AccountNumber);
            return query.ToList();
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return FilterTransactions(null, null);
        }

        public Task TransferAsync(IAccount toAccount, double amount, ITransactionProvider provider)
        {
            provider.From(this, amount);
            provider.To(toAccount, amount);
            return Task.CompletedTask;
        }
    }
}