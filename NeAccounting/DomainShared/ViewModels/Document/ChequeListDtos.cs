﻿using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class ChequeListDtos
    {
        public int Row { get; set; }
        /// <summary>
        /// DocId
        /// </summary>
        public Guid Id { get; set; }
        public string? CheckNumber { get; set; }
        public bool IsRecived { get; set; }
        public string? DueShamsiDate { get; set; }
        public string? Payer { get; set; }
        public string? Reciver { get; set; }
        public string? StatusName { get; set; }
        public string? Price { get; set; }
        public ChequeStatus Status { get; set; }
        public bool IsDeletable { get; set; } = false;
        public bool IsEditable { get; set; } = false;
        public bool IsPrintable { get; set; } = false;
    }

}
