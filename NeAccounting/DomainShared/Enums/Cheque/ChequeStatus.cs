﻿using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums.Cheque
{
    public enum ChequeStatus : byte
    {
        /// <summary>
        /// نقد شده
        /// </summary>
        [Display(Name = "نقد شده")]
        Cashed,
        /// <summary>
        /// پرداختی
        /// </summary>
        [Display(Name = "پرداختی")]
        Payed,
        /// <summary>
        /// ضمانتی
        /// </summary>
        [Display(Name = "ضمانتی")]
        Guarantee,
        /// <summary>
        /// واگذار شده
        /// </summary>
        [Display(Name = "واگذار شده")]
        Transferred,
        /// <summary>
        /// داخل صندوق
        /// </summary>
        [Display(Name = "داخل صندوق")]
        InBox
    }
}