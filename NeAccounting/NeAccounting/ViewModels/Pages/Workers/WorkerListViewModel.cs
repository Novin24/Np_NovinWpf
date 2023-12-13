﻿using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Workers;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class WorkerListViewModel : ObservableObject, INavigationAware
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;
        private readonly IContentDialogService _contentDialogService;

        [ObservableProperty]
        private string _fullName = "";

        [ObservableProperty]
        private string _jobTitle = "";

        [ObservableProperty]
        private string _nationalCode = "";

        private Status Statu { get { return (Status)StatusByte; } set { StatusByte = (byte)value; } }

        [ObservableProperty]
        private byte _statusByte = 0;

        [ObservableProperty]
        private IEnumerable<WorkerVewiModel> _list;

        public WorkerListViewModel(ISnackbarService snackbarService, INavigationService navigationService, IContentDialogService contentDialogService)
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
            List = await db.workerManager.GetWorkers(
                        FullName,
                        JobTitle,
                        NationalCode,
                        Statu);

            if (!List.Any())
                _snackbarService.Show("اوه", "هیچ کارگری در پایگاه داده یافت نشد!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
        }

        [RelayCommand]
        private async Task OnSearchWorker()
        {
            using UnitOfWork db = new();
            List = await db.workerManager.GetWorkers(
                        FullName,
                        JobTitle,
                        NationalCode,
                        Statu);

            if (!List.Any())
                _snackbarService.Show("اوه", "هیچ کارگری در پایگاه داده با این مشخصات یافت نشد!!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
        }

        [RelayCommand]
        private void OAddClick(string parameter)
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
        private async Task OnRemoveWorker(int parameter)
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
            }
        }

        [RelayCommand]
        private async Task OnUpdateWorker(int parameter)
        {
            Type? pageType = NameToPageTypeConverter.Convert("UpdateMaterail");

            if (pageType == null)
            {
                return;
            }
            var servise = _navigationService.GetNavigationControl();

            var worker = List.First(t => t.Id == parameter);

            IEnumerable<SuggestBoxViewModel<int>> asuBox;

            using (UnitOfWork db = new())
            {
                asuBox = await db.unitManager.GetUnits();
            }

            var context = new UpdateWorkerPage(new UpdateWorkerViewModel( _navigationService, _snackbarService)
            {
                Id = worker.Id,
                Shift = worker.Shift,
                StartDate = worker.StartDate,
                Status = worker.Status,
                AccountNumber = worker.AccountNumber,
                Address = worker.Address,
                Description = worker.Description,
                FullName= worker.FullName,
                JobTitle = worker.JobTitle, 
                Mobile = worker.Mobile, 
                NationalCode = worker.NationalCode,
                PersonalId = worker.PersonnelId,
            });

            servise.Navigate(pageType ,context);
        }
    }
}
