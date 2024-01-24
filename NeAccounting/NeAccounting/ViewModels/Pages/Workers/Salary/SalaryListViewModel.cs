﻿using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class SalaryListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;


        [ObservableProperty]
        private int _workerId = -1;

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _endDate;

        [ObservableProperty]
        private IEnumerable<PersonnerlSuggestBoxViewModel> _auSuBox;

        [ObservableProperty]
        private IEnumerable<SalaryViewModel> _list;

        public SalaryListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
            _contentDialogService = contentDialogService;
        }

        public void OnNavigatedFrom()
        {
        }

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using UnitOfWork db = new();
            AuSuBox = await db.workerManager.GetWorkers();
            List = await db.workerManager.GetSalaryList(WorkerId, StartDate, EndDate);
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            List = await db.workerManager.GetSalaryList(WorkerId, StartDate, EndDate);
        }

        [RelayCommand]
        private void OnAddClick(string parameter)
        {
            if (String.IsNullOrWhiteSpace(parameter))
            {
                return;
            }

            Type? pageType = NameToPageTypeConverter.Convert(parameter);

            if (pageType == null)
            {
                return;
            }

            _ = _navigationService.Navigate(pageType);
        }

        [RelayCommand]
        private async Task OnRemove(SalaryDetails parameter)
        {
            var result = await _contentDialogService.ShowSimpleDialogAsync(
            new SimpleContentDialogCreateOptions()
            {
                Title = "آیا از حذف اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
                using UnitOfWork db = new();
                var isSuccess = await db.workerManager.DeleteSalary(parameter.WorkerId, parameter.Id);
                if (!isSuccess.isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", isSuccess.error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
                await db.SaveChangesAsync();

                _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));

                await OnSearchWorker();
            }
        }

        [RelayCommand]
        private async Task OnUpdate(SalaryDetails parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateSalary");

            if (pageType == null)
            {
                return;
            }
            var service = _navigationService.GetNavigationControl();

            using UnitOfWork db = new();
            var s = await db.workerManager.GetSalaryDetailBySalaryId(parameter.WorkerId, parameter.Id);

            var context = new UpdateSalaryPage(new UpdateSalaryViewModel(_snackbarService, _navigationService)
            {
                WorkerId = parameter.WorkerId,
                SalaryId = parameter.Id,
                AmountOf = s.AmountOf,
                OverTime = s.OverTime,
                Description = s.Description,
                PersonnelName = s.WorkerName,
                SubmitDate = s.SubmitDate,
                RightHousingAndFood = s.RightHousingAndFood,
                ShiftStatus = s.ShiftStatus,
                ChildAllowance = s.ChildAllowance,
                FinancialAid = s.FinancialAid,
                PersonnelId= s.PersonelId,
                Insurance = s.Insurance,
                LoanInstallment = s.LoanInstallment,
                Tax = s.Tax,
                OtherAdditions = s.OtherAdditions,
                OtherDeductions = s.OtherDeductions,
                LeftOver = s.LeftOver,
            });

            service.Navigate(pageType, context);
        }
    }
}