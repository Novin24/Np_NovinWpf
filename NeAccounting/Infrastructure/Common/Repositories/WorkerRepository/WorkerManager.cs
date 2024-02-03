﻿using Domain.NovinEntity.Workers;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.PagedResul;
using DomainShared.ViewModels.Workers;
using Infrastructure.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NeApplication.IRepositoryies;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Infrastructure.Repositories
{
    public class WorkerManager(NovinDbContext context) : Repository<Worker>(context), IWorkerManager
    {

        #region sp
        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = DbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = DbContext.Database.CurrentTransaction?.GetDbTransaction();
            command.CommandTimeout = 20;
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private async Task EnsureConnectionOpenAsync(CancellationToken cancellationToken = default)
        {
            var connection = DbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }
        }
        #endregion

        #region Worker

        public Task<List<PersonnerlSuggestBoxViewModel>> GetWorkers()
        {
            return TableNoTracking.Select(x => new PersonnerlSuggestBoxViewModel
            {
                Id = x.Id,
                DisplayName = x.FullName,
                PersonnelId = x.PersonnelId

            }).ToListAsync();
        }

        public Task<WorkerVewiModel> GetWorker(int workerId)
        {
            return TableNoTracking.Where(t => t.Id == workerId)
                .Select(w => new WorkerVewiModel
                {
                    Shift = w.ShiftStatus,
                    ShiftOverTimeSalary = w.ShiftOverTimeSalary,
                    ShiftSalary = w.ShiftSalary,
                    StartDate = w.StartDate,
                    Status = w.Status,
                    OverTimeSalary = w.OverTimeSalary,
                    AccountNumber = w.AccountNumber,
                    Address = w.Address,
                    DayInMonth = w.DayInMonth,
                    Description = w.Description,
                    FullName = w.FullName,
                    PersonnelId = w.PersonnelId,
                    WorkerStatus = w.Status.ToDisplay(DisplayProperty.Name),
                    NationalCode = w.NationalCode,
                    EndDate = w.EndDate,
                    Id = w.Id,
                    InsurancePremium = w.InsurancePremium,
                    JobTitle = w.JobTitle,
                    Mobile = w.Mobile
                }).FirstOrDefaultAsync();
        }


        public Task<List<WorkerVewiModel>> GetWorkers(string fullName, string jobTitle, string nationalCode, Status status, int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            return TableNoTracking
                .Where(x => string.IsNullOrEmpty(fullName) || x.FullName.Contains(fullName))
                .Where(x => string.IsNullOrEmpty(jobTitle) || x.JobTitle.Contains(jobTitle))
                .Where(x => string.IsNullOrEmpty(nationalCode) || x.NationalCode.Contains(nationalCode))
                .Where(x => status == Status.All || x.Status == status)
                .Select(t => new WorkerVewiModel
                {
                    Id = t.Id,
                    JobTitle = t.JobTitle,
                    WorkerStatus = t.Status.ToDisplay(DisplayProperty.Name),
                    Status = t.Status,
                    Shift = t.ShiftStatus,
                    ShiftSalary = t.ShiftSalary,
                    ShiftOverTimeSalary = t.ShiftOverTimeSalary,
                    StartDate = t.StartDate,
                    AccountNumber = t.AccountNumber,
                    Address = t.Address,
                    Description = t.Description,
                    EndDate = t.EndDate,
                    Mobile = t.Mobile,
                    PersonnelId = t.PersonnelId,
                    FullName = t.FullName,
                    NationalCode = t.NationalCode,
                    Salary = t.Salary,
                    OverTimeSalary = t.OverTimeSalary,
                    DayInMonth = t.DayInMonth,
                    InsurancePremium = t.InsurancePremium,
                })
                .Skip(pageNum * NeAccountingConstants.PageCount)
                .Take(NeAccountingConstants.PageCount)
                .ToListAsync();
        }

        public async Task<(string error, bool isSuccess)> Create(
            string fullName,
            string natinalCode,
            string mobile,
            string address,
            int personalId,
            string accountNumber,
            string description,
            string jobTitle,
            DateTime startDate,
            Shift shift,
            uint salary,
            uint overtimeSalary,
            uint shiftSalary,
            uint shiftOvertimeSalary,
            uint insurancePremium,
            byte dayInMonth)
        {
            var worker = await TableNoTracking.FirstOrDefaultAsync(t => t.NationalCode == natinalCode || t.PersonnelId == personalId);

            if (worker != null)
            {
                if (worker.NationalCode == natinalCode)
                    return new($"کاربر گرامی کارگر {worker.FullName} با این کد ملی در پایگاه داده موجود می‌باشد!!!", false);

                if (worker.PersonnelId == personalId)
                    return new($"کاربر گرامی کارگر {worker.FullName} با این کد پرسنلی در پایگاه داده موجود می‌باشد!!!", false);
            }

            try
            {
                var t = await Entities.AddAsync(new Worker(
                    fullName,
                    natinalCode,
                    mobile,
                    address,
                    personalId,
                    accountNumber,
                    description,
                    jobTitle,
                    startDate,
                    shift,
                    salary,
                    overtimeSalary,
                    shiftSalary,
                    shiftOvertimeSalary,
                    insurancePremium,
                    dayInMonth));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> Update(
            int id,
            string fullName,
            string natinalCode,
            string mobile,
            string address,
            DateTime startDate,
            int personalId,
            string accountNumber,
            string description,
            string jobTitle,
            Status status,
            Shift shift,
            uint salary,
            uint overtimeSalary,
            uint shiftSalary,
            uint shiftOvertimeSalary,
            uint insurancePremium,
            byte dayInMonth)
        {
            var worker = await TableNoTracking.FirstOrDefaultAsync(t => t.Id == id);


            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            if (natinalCode != worker.NationalCode)
            {
                var w = await TableNoTracking.FirstOrDefaultAsync(t => t.Id != id && t.NationalCode == natinalCode);
                if (w != null)
                {
                    return new("کارگر دیگری با این کد ملی موجود می‌باشد!!!!", false);
                }
            }

            worker.NationalCode = natinalCode;
            worker.PersonnelId = personalId;
            worker.AccountNumber = accountNumber;
            worker.Description = description;
            worker.Status = status;
            worker.FullName = fullName;
            worker.Mobile = mobile;
            worker.Address = address;
            worker.StartDate = startDate;
            worker.ShiftStatus = shift;
            worker.JobTitle = jobTitle;
            worker.Salary = salary;
            worker.OverTimeSalary = overtimeSalary;
            worker.ShiftSalary = shiftSalary;
            worker.ShiftOverTimeSalary = shiftOvertimeSalary;
            worker.InsurancePremium = insurancePremium;
            worker.DayInMonth = dayInMonth;

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
        #endregion

        #region Salary
        public async Task<SalaryWorkerViewModel> GetSalaryDetailBySalaryId(int workerId, int salaryId, int persianMonth, int persianYear)
        {
            var salarise = await (from w in DbContext.Set<Worker>()
                                                 .AsNoTracking()
                                                 .Where(t => t.Id == workerId)

                                  join s in DbContext.Set<Salary>()
                                                          .Where(t => t.Id == salaryId)
                                                          on w.Id equals s.WorkerId

                                  join a in DbContext.Set<FinancialAid>()
                                  .Where(c => c.PersanMonth == persianMonth && c.PersianYear == persianYear)
                                                          on w.Id equals a.WorkerId into ai
                                  from aid in ai.DefaultIfEmpty()

                                  select new SalaryWorkerViewModel()
                                  {
                                      WorkerName = w.FullName,
                                      PersonelId = w.PersonnelId,
                                      ShiftStatus = w.ShiftStatus,
                                      Insurance = w.InsurancePremium,
                                      FinancialAid = aid.AmountOf == null ? 0 : aid.AmountOf,
                                      AmountOf = s.AmountOf,
                                      OverTime = s.OverTime,
                                      SubmitMonth = s.PersianMonth,
                                      SubmitYear = s.PersianYear,
                                      ChildAllowance = s.ChildAllowance,
                                      Description = s.Description,
                                      LeftOver = s.LeftOver,
                                      LoanInstallment = s.LoanInstallment,
                                      OtherAdditions = s.OtherAdditions,
                                      OtherDeductions = s.OtherDeductions,
                                      RightHousingAndFood = s.RightHousingAndFood,
                                      Tax = s.Tax,
                                      Error = string.Empty,
                                      Success = true,
                                  }).ToListAsync();

            uint amountOf = (uint)salarise.Sum(x => x.FinancialAid);
            salarise.First().FinancialAid = amountOf;
            return salarise.First();
        }

        public async Task<PagedResulViewModel<SalaryViewModel>> GetSalaryList(int? workerId,
            int? startMonth,
            int? startYear,
            int? endMonth,
            int? endYear,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            await EnsureConnectionOpenAsync();
            var parameters = new[] // sqlINput
        {
            new SqlParameter(nameof(workerId), workerId == null ? DBNull.Value : workerId),
            new SqlParameter(nameof(startMonth), startMonth == null ? DBNull.Value : startMonth),
            new SqlParameter(nameof(startYear), startYear == null ? DBNull.Value : startYear),
            new SqlParameter(nameof(endMonth), endMonth == null ? DBNull.Value : endMonth),
            new SqlParameter(nameof(endYear), endYear == null ? DBNull.Value : endYear),
            new SqlParameter("skipCount", pageNum * pageCount),
            new SqlParameter("maxResultCount",pageCount)
        };

            string totalCount = "0";
            List<SalaryViewModel> rows = new();
            using (var command = CreateCommand(SqlStoredProcedureConstants.GetSalaryList, CommandType.StoredProcedure, parameters))
            {
                using var dataReader = await command.ExecuteReaderAsync();
                while (await dataReader.ReadAsync()) //Sql OutPut
                {
                    SalaryViewModel row = new();
                    row.FullName = (string)dataReader[nameof(row.FullName)];
                    row.AmountOf = ((long)dataReader[nameof(row.AmountOf)]).ToString("N0");
                    row.LeftOver = ((long)dataReader[nameof(row.LeftOver)]).ToString("N0");
                    row.OverTime = ((long)dataReader[nameof(row.OverTime)]).ToString("N0");
                    row.TotalDebt = ((long)dataReader[nameof(row.TotalDebt)]).ToString("N0");
                    row.PersianMonth = (int)dataReader[nameof(row.PersianMonth)];
                    row.PersianYear = (int)dataReader[nameof(row.PersianYear)];
                    row.Details = new SalaryDetails()
                    {
                        Id = (int)dataReader[nameof(row.Details.Id)],
                        WorkerId = (int)dataReader[nameof(row.Details.WorkerId)],
                        PersianMonth = (int)dataReader[nameof(row.PersianMonth)],
                        PersianYear = (int)dataReader[nameof(row.PersianYear)]
                    };
                    totalCount = ((int)dataReader[("TotalRecord")]).ToString("N0");
                    rows.Add(row);
                }
            }
            return new PagedResulViewModel<SalaryViewModel>(totalCount, rows);
        }

        public async Task<(string error, bool isSuccess)> DeleteSalary(int workerId, int salaryId)
        {
            var worker = await Table
               .Include(t => t.Salaries.Where(s => s.Id == salaryId))
               .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null || worker.Salaries.Count == 0)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            var salary = worker.Salaries.First();

            worker.Salaries.Remove(salary);
            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateSalary(int workerId,
            int salaryId,
            int persianYear,
            int persianMonth,
            uint amountOf,
            uint financialAid,
            uint overTime,
            uint tax,
            uint childAllowance,
            uint rightHousingAndFood,
            uint insurance,
            uint loanInstallment,
            uint otherAdditions,
            uint otherDeductions,
            uint leftOver,
            string? description)
        {
            var worker = await Entities
                .Include(s => s.Salaries)
                .Include(s => s.Functions)
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.FirstOrDefault(t => t.Id == salaryId) == null)
                return new("فیش مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.Any(t =>
                t.Id != salaryId && t.PersianMonth == persianMonth && t.PersianYear == persianYear))
            {
                return new("کاربر گرامی برای این پرسنل در این ماه فیش حقوقی صادر شده !!!", false);
            }

            if (!worker.Functions.Any(t =>
                t.PersianYear == persianYear && t.PersianMonth == persianMonth))
            {
                return new("کاربر گرامی برای ماه مورد نظر هیچ کارکردی ثبت نشده!!!", false);
            }

            var salary = worker.Salaries.First(s => s.Id == salaryId);
            salary.PersianMonth = persianMonth;
            salary.PersianYear = persianYear;
            salary.Insurance = insurance;
            salary.Description = description;
            salary.OtherDeductions = otherDeductions;
            salary.ChildAllowance = childAllowance;
            salary.AmountOf = amountOf;
            salary.LeftOver = leftOver;
            salary.OtherAdditions = otherAdditions;
            salary.RightHousingAndFood = rightHousingAndFood;
            salary.FinancialAid = financialAid;
            salary.OverTime = overTime;
            salary.Tax = tax;
            salary.LoanInstallment = loanInstallment;

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> AddSalary(int workerId,
            int persianMonth,
            int persianYear,
            uint amountOf,
            uint financialAid,
            uint overTime,
            uint tax,
            uint childAllowance,
            uint rightHousingAndFood,
            uint insurance,
            uint loanInstallment,
            uint otherAdditions,
            uint otherDeductions,
            uint leftOver,
            string? description)
        {
            var worker = await Entities
                .Include(s => s.Salaries)
                .Include(s => s.Functions.Where(t =>
                t.PersianYear == persianYear && t.PersianMonth == persianMonth))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            if (worker.Functions.Count == 0)
            {
                return new("کاربر گرامی برای ماه مورد نظر هیچ کارکردی ثبت نشده!!!", false);
            }

            if (worker.Salaries.Any(t =>
            t.PersianMonth == persianMonth && t.PersianYear == persianYear))
            {
                return new("کاربر گرامی برای این پرسنل در این ماه فیش حقوقی صادر شده !!!", false);
            }

            worker.AddSalary(
                new Salary(
                    persianYear,
                    persianMonth,
                    amountOf,
                    financialAid,
                    overTime,
                    tax,
                    childAllowance,
                    insurance,
                    rightHousingAndFood,
                    loanInstallment,
                    otherAdditions,
                    otherDeductions,
                    leftOver,
                    description));

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);

        }

        public async Task<SalaryWorkerViewModel> GetSalaryDetailByWorkerId(int workerId, int persianMonth,
            int persianYear)
        {
            uint aid = 0;
            uint ssalary = 0;
            uint overtime = 0;

            var worker = await TableNoTracking
                .Include(t => t.Salaries.Where(s => s.PersianYear == persianYear))
                .Include(t => t.Salaries.Where(s => s.PersianYear == persianYear))
                .FirstAsync(t => t.Id == workerId);


            var salary = worker.Salaries.FirstOrDefault(t => t.PersianMonth == persianMonth);

            //if (salary == null || salary.Functions.Count == 0)
            //    return new SalaryWorkerViewModel() { Error = "برای پرسنل مورد نظر کارکردی در این ماه ثبت نشده !!!", Success = false };


            //if (salary.Aids.Count != 0)
            //{
            //    aid = (uint)salary.Aids.Sum(t => t.AmountOf);
            //}

            //var func = salary.Functions.First();


            //if (worker.ShiftStatus == Shift.ByMounth)
            //{
            //    if (func.AmountOf == worker.DayInMonth)
            //    {
            //        ssalary = worker.Salary;
            //    }
            //    else
            //    {
            //        ssalary = func.AmountOf * (worker.Salary / worker.DayInMonth);
            //    }
            //    overtime = func.AmountOfOverTime * worker.OverTimeSalary;

            //}
            //else
            //{
            //    ssalary = func.AmountOf * worker.ShiftSalary;
            //    overtime = func.AmountOfOverTime * worker.ShiftOverTimeSalary;

            //}
            return new SalaryWorkerViewModel()
            {
                PersonelId = worker.PersonnelId,
                ShiftStatus = worker.ShiftStatus,
                Insurance = worker.InsurancePremium,
                FinancialAid = aid,
                AmountOf = ssalary,
                OverTime = overtime,
                Error = string.Empty,
                Success = true,
            };
        }
        #endregion
    }


    public class FunctionManager(NovinDbContext context) : Repository<Function>(context), IFunctionManager
    {
        #region Function
        public async Task<(string error, bool isSuccess)> AddOrUpdateFunctuion(
            int workerId,
            DateTime submitDate,
            byte amountOf,
            byte amountOfOverTime,
            string? description)
        {
            PersianCalendar pc = new();
            var persianMonth = pc.GetMonth(submitDate);
            var persianYear = pc.GetYear(submitDate);

            var worker = await Entities
                //.Include(t => t.Salaries.Where(s => s.PersianYear == persianYear))
                //.ThenInclude(c => c.Functions)
                .FirstOrDefaultAsync(t => t.Id == workerId);

            //if (worker == null)
            //    return new("کارگر مورد نظر یافت نشد!!!!", false);

            //var salary = worker.Salaries.FirstOrDefault(t => t.PersianMonth == persianMonth);


            //if (salary != null)
            //{
            //    if (salary.Functions.Count != 0)
            //    {
            //        return new("برای این پرسنل در این ماه کارکرد ثبت شده !!!", false);
            //    }

            //    salary.AddFunction(
            //        new Function(
            //            submitDate,
            //            amountOf,
            //            amountOfOverTime,
            //            description));
            //}
            //else
            //{
            //    worker.AddSalary(
            //        new Salary(
            //            submitDate,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            string.Empty)
            //        .AddFunction(
            //            new Function(
            //                submitDate,
            //                amountOf,
            //                amountOfOverTime,
            //                description)));
            //}

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }


        public async Task<List<FunctionViewModel>> GetFunctionList(int workerId,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            return await (from worker in DbContext.Set<Worker>()
                                                         .AsNoTracking()
                                                         .Where(t => workerId == -1 || t.Id == workerId)

                          join func in DbContext.Set<Function>()
                                                  on worker.Id equals func.WorkerId

                          select new FunctionViewModel()
                          {
                              Name = worker.FullName,
                              Amountof = func.AmountOf,
                              OverTime = func.AmountOfOverTime,
                              Description = func.Description,
                              PersonelId = worker.PersonnelId,
                              PersianMonth = func.PersianMonth,
                              PersianYear = func.PersianYear,
                              Details = new FucntionDetails() { Id = func.Id, WorkerId = worker.Id }
                          })
                      .OrderByDescending(c => c.PersianYear)
                      .ThenByDescending(c => c.PersianMonth)
                      .Skip(pageNum * pageCount)
                      .Take(pageCount)
                      .ToListAsync();
        }

        public async Task<(string error, bool isSuccess)> UpdateFunc(
           int workerId,
           int funcId,
           byte amountOf,
           byte overTime,
           string? description)
        {

            var worker = await Entities
                //.Include(t => t.Salaries.Where(s => s.Id == salaryId))
                //.ThenInclude(c => c.Functions.Where(c => c.Id == funcId))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            //if (worker == null || worker.Salaries.Count == 0 || worker.Salaries.First().Functions.Count == 0)
            //    return new("کارگر مورد نظر یافت نشد!!!!", false);

            //if (worker.Salaries.First().AmountOf != 0)
            //{
            //    return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);
            //}
            //var func = worker.Salaries.First().Functions.First();
            //func.AmountOf = amountOf;
            //func.AmountOfOverTime = overTime;
            //func.Description = description;

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> DeleteFunc(int workerId, int aidId)
        {
            var worker = await Entities
               //.Include(t => t.Salaries.Where(s => s.Id == salaryId))
               //.ThenInclude(c => c.Functions.Where(c => c.Id == aidId))
               .FirstOrDefaultAsync(t => t.Id == workerId);

            //if (worker == null || worker.Salaries.Count == 0 || worker.Salaries.First().Functions.Count == 0)
            //{
            //    return new("کارگر مورد نظر یافت نشد!!!!", false);
            //}

            //if (worker.Salaries.First().AmountOf != 0)
            //{
            //    return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);
            //}

            //var func = worker.Salaries.First().Functions;
            //func.Remove(worker.Salaries.First().Functions.First());
            try
            {
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
        #endregion
    }

    public class AidManager(NovinDbContext context) : Repository<FinancialAid>(context), IAidManager
    {
        #region Aid
        public async Task<(string error, bool isSuccess)> AddOrUpdateAid(
            int workerId,
            DateTime submitDate,
            uint amountOf,
            string? description)
        {
            PersianCalendar pc = new();
            var persianMonth = pc.GetMonth(submitDate);
            var persianYear = pc.GetYear(submitDate);

            var worker = await Entities
                //.Include(t => t.Salaries.Where(s => s.PersianYear == persianYear))
                //.ThenInclude(c => c.Aids)
                .FirstOrDefaultAsync(t => t.Id == workerId);

            //if (worker == null)
            //    return new("کارگر مورد نظر یافت نشد!!!!", false);

            //var salary = worker.Salaries.FirstOrDefault(t => t.PersianMonth == persianMonth);


            //if (salary != null)
            //{
            //    if (salary.AmountOf != 0)
            //    {
            //        return new("کاربر گرامی پس از صدور فیش حقوقی امکان ثبت مساعده در این ماه وجود ندارد !!!", false);
            //    }

            //    salary.AddFinancialAid(
            //        new FinancialAid(
            //            submitDate,
            //            amountOf,
            //            description));
            //}
            //else
            //{
            //    worker.AddSalary(
            //        new Salary(
            //            submitDate,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            uint.MinValue,
            //            string.Empty)
            //        .AddFinancialAid(
            //            new FinancialAid(
            //                submitDate,
            //                amountOf,
            //                description)));
            //}

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateAid(
            int workerId,
            int aidId,
            uint amount,
            string? description)
        {

            var worker = await Entities
                //.Include(t => t.Salaries.Where(s => s.Id == salaryId))
                //.ThenInclude(c => c.Aids.Where(c => c.Id == aidId))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            //if (worker == null || worker.Salaries.Count == 0 || worker.Salaries.First().Aids.Count == 0)
            //    return new("کارگر مورد نظر یافت نشد!!!!", false);

            //if (worker.Salaries.First().AmountOf != 0)
            //{
            //    return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);
            //}

            //var aid = worker.Salaries.First().Aids.First();
            //aid.AmountOf = amount;
            //aid.Description = description;

            try
            {
                Entities.Update(worker);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<List<AidViewModel>> GetAidList(int workerId, int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            return await (from worker in DbContext.Set<Worker>()
                                               .AsNoTracking()
                                               .Where(t => workerId == -1 || t.Id == workerId)


                          join aid in DbContext.Set<FinancialAid>()
                                                  on worker.Id equals aid.Id

                          select new AidViewModel()
                          {
                              Name = worker.FullName,
                              AmountPrice = aid.AmountOf,
                              Description = aid.Description,
                              PersonelId = worker.PersonnelId,
                              Price = aid.AmountOf.ToString("N0"),
                              PersianMonth = aid.PersanMonth,
                              PersianYear = aid.PersianYear,
                              Details = new AidDetails() { Id = aid.Id, WorkerId = worker.Id }
                          })
                      .OrderByDescending(c => c.PersianYear)
                      .ThenByDescending(c => c.PersianMonth)
                      .Skip(pageNum * pageCount)
                      .Take(pageCount)
                      .ToListAsync();

        }


        public async Task<(string error, bool isSuccess)> DeleteAid(int workerId, int aidId)
        {
            var worker = await Entities
               //.Include(t => t.Salaries.Where(s => s.Id == salaryId))
               //.ThenInclude(c => c.Aids.Where(c => c.Id == aidId))
               .FirstOrDefaultAsync(t => t.Id == workerId);

            //if (worker == null || worker.Salaries.Count == 0 || worker.Salaries.First().Aids.Count == 0)
            //{
            //    return new("کارگر مورد نظر یافت نشد!!!!", false);
            //}
            //if (worker.Salaries.First().AmountOf != 0)
            //{
            //    return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);
            //}
            //var aid = worker.Salaries.First().Aids.First();
            //worker.Salaries.First().Aids.Remove(aid);
            try
            {
                Entities.Update(worker);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
        #endregion
    }
}
