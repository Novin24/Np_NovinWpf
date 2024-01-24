﻿using Domain.NovinEntity.Workers;
using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IWorkerManager : IRepository<Worker>
    {

        Task<List<PersonnerlSuggestBoxViewModel>> GetWorkers();

        Task<WorkerVewiModel> GetWorker(int workerId);

        Task<List<WorkerVewiModel>> GetWorkers(string fullName,
            string jobTitle,
            string nationalCode,
            Status status);

        Task<(string error, bool isSuccess)> Create(
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
            byte dayInMonth);

        Task<(string error, bool isSuccess)> Update(
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
            byte dayInMonth);

        Task<(string error, bool isSuccess)> AddSalary(int workerId,
            DateTime submitDate,
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
            string? description);
        
        
        Task<(string error, bool isSuccess)> UpdateSalary(int workerId,
            int salaryId,
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
            string? description);


        Task<(string error, bool isSuccess)> AddOrUpdateFunctuion(
            int workerId,
            DateTime submitDate,
            byte amountOf,
            byte amountOfOverTime,
            string? description);


        Task<(string error, bool isSuccess)> AddOrUpdateAid(
            int workerId,
            DateTime submitDate,
            uint amountOf,
            string? description);

        Task<(string error, bool isSuccess)> UpdateAid(
            int workerId,
            int salaryId,
            int aidId,
            uint amountOf,
            string? description);
        Task<(string error, bool isSuccess)> UpdateFunc(
            int workerId,
            int salaryId,
            int funcId,
            byte amountOf,
            byte overTime,
            string? description);

        Task<SalaryWorkerViewModel> GetSalaryDetailByWorkerId(int workerId, DateTime submitDate);

        Task<SalaryWorkerViewModel> GetSalaryDetailBySalaryId(int workerId, int salaryId);

        Task<List<AidViewModel>> GetAidList(int workerId);

        Task<List<FunctionViewModel>> GetFunctionList(int workerId);

        Task<List<SalaryViewModel>> GetSalaryList(int workerId , DateTime? start , DateTime? end);

        Task<(string error, bool isSuccess)> DeleteAid(int workerId, int salaryId, int aidId);

        Task<(string error, bool isSuccess)> DeleteSalary(int workerId, int salaryId);

        Task<(string error, bool isSuccess)> DeleteFunc(int workerId, int salaryId, int aidId);

    }
}
