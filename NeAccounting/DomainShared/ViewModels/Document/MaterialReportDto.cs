﻿namespace DomainShared.ViewModels.Document
{
    public class MaterialReportDto
    {
        public int Row { get; set; }
        public string? MatName { get; set; }
        public string? CusName{ get; set; }
        public string? Status { get; set; }
        public DateTime Date { get; set; }
        public string? ShamsiDate { get; set; }
        public string? AmuontOf { get; set; }
        public string? Price { get; set; }
    }
}
