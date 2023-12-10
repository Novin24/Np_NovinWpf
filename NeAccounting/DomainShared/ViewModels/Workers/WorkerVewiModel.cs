﻿namespace DomainShared.ViewModels.Workers
{
    public struct WorkerVewiModel
    {
        public int Id { get; set; }
        public int PersonelId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string NationalCode { get; set; }
        public string WorkerStatus { get; set; }
    }
}
