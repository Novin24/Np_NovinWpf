﻿using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Customers;
using Domain.NovinEntity.Documents;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Extension;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;
using System.Globalization;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {

        public async Task<(string error, bool isSuccess)> CreateDocument(Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool receivedOrPaid)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, type, payType, descripion, submitDate, receivedOrPaid));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        #region Document
        public async Task<(string error, bool isSuccess)> CreatePayDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.PayDoc, paymentType, descripion, submitDate, false));

                if (discount != null && discount != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, discount.Value, DocumntType.PayDiscount, PaymentType.Other,$" تخفیف فاکتور( {t.Entity.Serial} )",submitDate,false)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateRecDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.RecDoc, paymentType, descripion, submitDate, true));

                if (discount != null && discount != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, discount.Value, DocumntType.RecDiscount, PaymentType.Other,$" تخفیف فاکتور( {t.Entity.Serial} )",submitDate,true)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
        #endregion

        #region Invoice(CRUD)
        public async Task<(string error, bool isSuccess)> CreateSellDocument(Guid customerId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            List<SellRemittance> list = remittances.Select(t => new SellRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, t.Description)).ToList();

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.SellInv, PaymentType.Other, descripion, submitDate, false)
                .AddSellRemittance(list));
                if (commission != null && commission != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, (long)(price * (commission.Value / 100)), DocumntType.PayCom,
                        PaymentType.Other,$" پورسانت فاکتور ( {t.Entity.Serial} )",submitDate,true,(byte)commission.Value)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                }
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateBuyDocument(Guid customerId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            List<BuyRemittance> list = remittances.Select(t => new BuyRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, descripion)).ToList();

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.BuyInv, PaymentType.Other, descripion, submitDate, true)
                .AddBuyRemittance(list));
                if (commission != null && commission != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, (long)(price * (commission.Value / 100)), DocumntType.RecCom, PaymentType.Other,$" پورسانت فاکتور( {t.Entity.Serial} )",submitDate,false,(byte)commission.Value)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<string> GetLastDocumntNumber(DocumntType type)
        {
            return (await TableNoTracking.OrderByDescending(t => t.CreationTime).Where(t => t.Type == type).Select(c => c.Serial).FirstOrDefaultAsync()).ToString();
        }

        public async Task<(bool isSuccess, InvoiceDetailUpdateDto itm)> GetSellInvoiceDetail(Guid invoiceId)
        {
            var inv = await TableNoTracking
                .Include(r => r.SellRemittances)
                .Include(r => r.RelatedDocuments)
                .Where(t => t.Id == invoiceId)
                .Select(c => new InvoiceDetailUpdateDto()
                {
                    CustomerId = c.CustomerId,
                    Serial = c.Serial.ToString(),
                    Date = c.SubmitDate,
                    TotalPrice = c.Price,
                    Commission = c.RelatedDocuments.Sum(t => t.Commission),
                    InvoiceDescription = c.Description,
                    RemList = c.SellRemittances.Select(t => new RemittanceListViewModel()
                    {
                        AmountOf = t.AmountOf,
                        Description = t.Description,
                        MaterialId = t.MaterialId,
                        Price = t.Price,
                        RremId = t.Id,
                        TotalPrice = t.TotalPrice
                    }).ToList(),
                }).FirstOrDefaultAsync();

            if (inv == null)
            {
                return new(false, new InvoiceDetailUpdateDto());
            }

            return new(true, inv);
        }

        public async Task<(bool isSuccess, InvoiceDetailUpdateDto itm)> GetBuyInvoiceDetail(Guid invoiceId)
        {
            var inv = await TableNoTracking
                .Include(r => r.BuyRemittances)
                .Include(r => r.RelatedDocuments)
                .Where(t => t.Id == invoiceId)
                .Select(c => new InvoiceDetailUpdateDto()
                {
                    CustomerId = c.CustomerId,
                    Serial = c.Serial.ToString(),
                    Date = c.SubmitDate,
                    TotalPrice = c.Price,
                    Commission = c.RelatedDocuments.Sum(t => t.Commission),
                    InvoiceDescription = c.Description,
                    RemList = c.BuyRemittances.Select(t => new RemittanceListViewModel()
                    {
                        AmountOf = t.AmountOf,
                        Description = t.Description,
                        MaterialId = t.MaterialId,
                        Price = t.Price,
                        RremId = t.Id,
                        TotalPrice = t.TotalPrice
                    }).ToList(),
                }).FirstOrDefaultAsync();

            if (inv == null)
            {
                return new(false, new InvoiceDetailUpdateDto());
            }

            return new(true, inv);
        }

        public async Task<(bool isSuccess, PayDocUpdateDto itm)> GetPayDocumentById(Guid docId)
        {
            var inv = await TableNoTracking
                 .Where(t => t.Id == docId)
                 .Include(r=> r.RelatedDocuments)
                 .Select(c => new PayDocUpdateDto()
                 {
                     CustomerId = c.CustomerId,
                     Serial = c.Serial.ToString(),
                     Date = c.SubmitDate,
                     Type = c.PayType,
                     DocDescription = c.Description,
                     Price = c.Price,
                     Dicount = c.RelatedDocuments.Sum(t=> t.Price)
                 }).FirstOrDefaultAsync();

            if (inv == null)
            {
                return new(false, new PayDocUpdateDto());
            }

            return new(true, inv);
        }
        #endregion

        #region Status
        public async Task<long> GetDebt(Guid customerId)
        {
            return await TableNoTracking.Where(p => !p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

        }

        public async Task<long> GetCredit(Guid customerId)
        {
            return await TableNoTracking.Where(p => p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();
        }

        public async Task<UserDebtStatus> GetStatus(Guid customerId)
        {
            var tal = await TableNoTracking.Where(p => !p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

            var bed = await TableNoTracking.Where(p => p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

            long res = tal - bed;
            if (res > 0)
            {
                return new UserDebtStatus
                {
                    Status = "بدهکار",
                    Amount = res,
                    Credit = "0",
                    Debt = Math.Abs(res).ToString("N0"),

                };
            }
            if (res < 0)
            {
                return new UserDebtStatus()
                {
                    Status = "طلبکار",
                    Amount = res,
                    Debt = "0",
                    Credit = Math.Abs(res).ToString("N0")
                };
            }
            return new UserDebtStatus()
            {
                Status = "تسویه",
                Amount = 0,
                Credit = "0",
                Debt = "0"
            };
        }
        #endregion

        #region report
        public async Task<PagedResulViewModel<InvoiceListDto>> GetInvoicesByDate(DateTime startTime,
            DateTime endTime,
            string desc,
            Guid cusId,
            bool leftOver,
            bool ignorePagination,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            int i = 1;
            PersianCalendar pc = new();
            List<InvoiceListDto> Remittances = [];
            var MyDoc = await TableNoTracking
                .Where(st => st.SubmitDate > startTime)
                .Where(et => et.SubmitDate < endTime)
                .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
                .Where(p => p.CustomerId == cusId)
                .Select(t => new InvoiceDto()
                {
                    Id = t.Id,
                    Type = t.Type,
                    Date = t.SubmitDate,
                    Serial = t.Serial,
                    Description = t.Description,
                    Price = t.Price,
                    ReceivedOrPaid = t.IsReceived
                }).OrderBy(p => p.Date)
                .ToListAsync();


            if (leftOver)
            {
                InvoiceListDto rem = new()
                {
                    Row = 0,
                    Date = startTime,
                    IsDeletable = false,
                    IsEditable = false,
                    ShamsiDate = startTime.ToShamsiDate(pc),
                    Description = "باقی مانده از قبل",
                    Bed = MyDoc.Where(p => p.Date < startTime && !p.ReceivedOrPaid).Sum(p => p.Price),
                    Bes = MyDoc.Where(p => p.Date < startTime && p.ReceivedOrPaid).Sum(p => p.Price),
                };
                Remittances.Add(rem);
            }

            Remittances.AddRange(MyDoc.Where(p => !p.ReceivedOrPaid && p.Date >= startTime).Select(t => new InvoiceListDto
            {
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                Serial = t.Serial.ToString(),
                Id = t.Id,
                IsEditable = true,
                IsDeletable = true,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = t.Price,
                Bes = 0,
            }).ToList());

            Remittances.AddRange(MyDoc.Where(p => p.ReceivedOrPaid && p.Date >= startTime).Select(t => new InvoiceListDto
            {
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                Id = t.Id,
                Serial = t.Serial.ToString(),
                IsEditable = true,
                IsDeletable = true,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = 0,
                Bes = t.Price,
            }).ToList());

            Remittances = [.. Remittances.OrderByDescending(t => t.Date)];

            foreach (var item in Remittances)
            {
                item.Row = i;
                long bed = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bed);
                long bes = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bes);

                item.LeftOver = Math.Abs(bed - bes);
                if (bes > bed)
                {
                    item.Status = "طلبکار";
                }
                else if (bed > bes)
                {
                    item.Status = "بدهکار";
                }
                else
                {
                    item.Status = "تسویه";
                }
                i++;
            }

            var totalCount = Remittances.Count;

            if (!ignorePagination)
            {
                Remittances = Remittances.Skip(--pageNum * pageCount).Take(pageCount).ToList();
            }

            return new PagedResulViewModel<InvoiceListDto>(totalCount, pageCount, Remittances);
        }

        public Task<IEnumerable<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime, DateTime EndTime, Guid CusId, bool LeftOver, string Description)
        {
            throw new NotImplementedException();
        }


        public async Task<List<SummaryDoc>> GetSummaryDocs(Guid? CusId, DocumntType type)
        {
            PersianCalendar pc = new();
            var list = await (from doc in DbContext.Set<Document>()
                                               .AsNoTracking()
                                               .Where(t => t.Type == type)
                                               .Where(t => CusId == null || t.CustomerId == CusId)

                              join cus in DbContext.Set<Customer>()
                                                      on doc.CustomerId equals cus.Id

                              orderby doc.CreationTime descending
                              select new SummaryDoc()
                              {
                                  SubmitDate = doc.SubmitDate,
                                  Cus_Name = cus.Name,
                                  Price = doc.Price.ToString("N0"),
                              })
                      .Take(15)
                      .ToListAsync();

            list.ForEach(c =>
            {
                c.ShamsiDate = c.SubmitDate.ToShamsiDate(pc);
            });
            return list;
        }

        public async Task<PagedResulViewModel<DalyBookDto>> GetDalyBook(int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            var list = await (from doc in DbContext.Set<Document>()
                                               .AsNoTracking()
                                               .Where(t => t.CreationTime.Day == DateTime.Now.Day)
                              join cus in DbContext.Set<Customer>()
                                                                       on doc.CustomerId equals cus.Id

                              orderby doc.CreationTime descending
                              select new DalyBookDto()
                              {
                                  SubmitDate = doc.SubmitDate,
                                  Bed = doc.IsReceived ? "0" : doc.Price.ToString("N0"),
                                  Bes = doc.IsReceived ? doc.Price.ToString("N0") : "0",
                                  CustomerName = cus.Name,
                                  Description = doc.Description,
                                  Id = doc.Id,
                                  Type = doc.Type,
                                  Serial = doc.Serial.ToString()
                              })
                      .ToListAsync();

            int row = 1;
            PersianCalendar pc = new();
            foreach (var item in list)
            {
                item.ShamsiDate = item.SubmitDate.ToShamsiDate(pc);
                item.Row = row;
                row++;
            }
            var totalCount = list.Count;
            list = list.Skip(--pageNum * pageCount).Take(pageCount).ToList();
            return new PagedResulViewModel<DalyBookDto>(totalCount, pageCount, list);
        }
        #endregion
    }
}
