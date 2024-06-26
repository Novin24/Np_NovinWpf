﻿namespace DomainShared.ViewModels.Document
{
    public class UserLeftOve
    {
        public Guid UserId { get; set; }
        public long LeftOver { get; set; }
        /// <summary>
        /// طلبکاری های مشتری
        /// پولایی که ما گرفتیم
        /// </summary>
        public long Debt { get; set; }

        /// <summary>
        /// بدهکاری های مشتری
        /// پولاییکه ما دادیم
        /// </summary>
        public long Credit { get; set; }
    }
}
