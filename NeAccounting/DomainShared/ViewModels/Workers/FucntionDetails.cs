﻿namespace DomainShared.ViewModels.Workers
{
    public struct FucntionDetails
    {
        /// <summary>
        /// شناسه کارکرد
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// شناسه حقوق
        /// </summary>
        public int SalaryId { get; set; }
        /// <summary>
        /// شناسه کارگر
        /// </summary>
        public int WorkerId { get; set; }
    }
}
