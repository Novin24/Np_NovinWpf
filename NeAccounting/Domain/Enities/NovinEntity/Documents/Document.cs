﻿using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;
using DomainShared.Enums;

namespace Domain.NovinEntity.Documents
{
    public class Document : LocalEntity<Guid>
    {
        #region Navigation
        public Guid CustomerId { get; private set; }
        public Guid? DocumentId { get; private set; }
        public Document P_Document { get; set; }
        public List<Document> RelatedDocuments { get; private set; }
        public List<SellRemittance> SellRemittances { get; private set; }
        public List<BuyRemittance> BuyRemittances { get; private set; }
        #endregion

        #region Ctor
        internal Document()
        {

        }

        public Document(
            Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool isReceived)
        {
            CustomerId = customerId;
            Price = price;
            Type = type;
            PayType = payType;
            Description = descripion;
            SubmitDate = submitDate;
            IsReceived = isReceived;
            RelatedDocuments = [];
            SellRemittances = [];
            BuyRemittances = [];
        }
        #endregion

        #region Properties
        public long Price { get; set; }
        public string? Description { get; set; }
        public DateTime SubmitDate { get; set; }
        public PaymentType PayType { get; set; }
        public DocumntType Type { get; set; }
        /// <summary>
        /// ما دریافت کردیم
        /// </summary>
        public bool IsReceived { get; set; }
        public long Serial { get; set; }
        #endregion

        #region Methods
        public Document AddSellRemittance(List<SellRemittance> list)
        {
            SellRemittances.AddRange(list);
            return this;
        }

        public Document AddBuyRemittance(List<BuyRemittance> list)
        {
            BuyRemittances.AddRange(list);
            return this;
        }

        public Document AddDocument(List<Document> list)
        {
            RelatedDocuments.AddRange(list);
            return this;
        }
        #endregion
    }
}
