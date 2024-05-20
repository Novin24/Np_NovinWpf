﻿using Domain.Common;
using DomainShared.Enums;

namespace Domain.NovinEntity.Customers
{
    public class Customer : LocalEntity<Guid>
    {
        #region navigation
        #endregion

        #region ctor
        internal Customer() { }

        public Customer(
            string name,
            string mobile,
            long totalCredit,
            long cashCredit,
            long promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller)
        {
            HaveChequeGuarantee = false;
            ChequeCredit = 0;
            Name = name;
            Mobile = mobile;
            TotalCredit = totalCredit;
            CashCredit = cashCredit;
            PromissoryNote = promissoryNote;
            NationalCode = nationalCode;
            HavePromissoryNote = havePromissoryNote;
            HaveCashCredit = haveCashCredit;
            Address = address;
            Buyer = isBuyer;
            Seller = isSeller;
            Type = type;
            IsActive = true;
        }

        public Customer(
            string name,
            string mobile,
            long totalCredit,
            long cashCredit,
            long promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller,
            Guid id) : this(name,mobile,totalCredit,cashCredit,promissoryNote,nationalCode,address,type,havePromissoryNote,haveCashCredit,isBuyer,isSeller)
        {
            Id = id;
        }
        #endregion

        #region properties
        public string Name { get; set; }
        public long CusId { get; set; }
        public string Mobile { get; set; }
        /// <summary>
        /// مجموع اعتبار
        /// </summary>
        public long TotalCredit { get; set; }
        /// <summary>
        /// مبلغ چک ضمانتی
        /// </summary>
        public long ChequeCredit { get; set; }
        /// <summary>
        /// ضمانت نقدی
        /// </summary>
        public long CashCredit { get; set; }

        /// <summary>
        /// ضمانت سفته
        /// </summary>
        public long PromissoryNote { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public bool Buyer { get; set; }
        public bool Seller { get; set; }
        public CustomerType Type { get; set; }
        public bool HaveChequeGuarantee { get; set; }
        public bool HaveCashCredit { get; set; }
        public bool HavePromissoryNote { get; set; }
        public bool IsActive { get; set; }
        #endregion
    }
}
