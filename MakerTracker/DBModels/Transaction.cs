namespace MakerTracker.DBModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Transaction
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Profile From { get; set; }

        public Profile To { get; set; }

        public int Amount { get; set; }

        public string ConfirmationCode { get; set; }

        public DateTime TransactionDate { get; set; }

        public DateTime ConfirmationDate { get; set; }

        public TransactionStatus Status { get; set; }

        public TransactionType TransactionType { get; set; }

        public int? NeedId { get; set; }

        [ForeignKey(nameof(NeedId))]
        public virtual Need Need { get; set; }
    }

    public enum TransactionStatus
    {
        Pending,
        Confirmed,
    }

    public enum TransactionType
    {
        Reconciliation,
        Stock,
        Delivery,
        DeliveryCorrection,
        Withdrawal
    }
}
