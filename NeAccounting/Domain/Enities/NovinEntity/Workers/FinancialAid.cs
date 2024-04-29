﻿using Domain.Common;

namespace Domain.NovinEntity.Workers
{
    public class FinancialAid : LocalEntity
    {
        #region Navigation
        public Personel Worker { get; set; }
        public Guid WorkerId { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// تاریخ پرداخت مساعده
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// سال شمسی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// ماه شمسی
        /// </summary>
        public byte PersianMonth { get; set; }

        /// <summary>
        /// مبلغ مساعده
        /// </summary>
        public long AmountOf { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Constructor
        public FinancialAid()
        {

        }

        public FinancialAid(
            DateTime submitDate,
            byte persianMonth,
            int persianYear,
            long amountOf,
            string? description)
        {
            SubmitDate = submitDate;
            PersianMonth = persianMonth;
            PersianYear = persianYear;
            AmountOf = amountOf;
            Description = description;
        }
        #endregion
    }
}
