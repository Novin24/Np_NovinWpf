﻿using System.ComponentModel.DataAnnotations;

namespace DomainShared.Enums
{
    public enum Priority : byte
    {
        [Display(Name = "همه")]
        All = 0,

        [Display(Name = "کم")]
        Low = 1,

        [Display(Name = "متوسط")]
        Medium = 2,

        [Display(Name = "بالا")]
        High = 3,
    }
}
