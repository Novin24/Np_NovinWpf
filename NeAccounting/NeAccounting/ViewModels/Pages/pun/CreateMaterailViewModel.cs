﻿using DomainShared.Errore;
using DomainShared.ViewModels;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NeAccounting.ViewModels.Pages
{
    public partial class CreateMaterailViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;
        private readonly INavigationService _navigationService;
        private readonly ISnackbarService _snackbarService;
        public CreateMaterailViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private IEnumerable<SuggestBoxViewModel<int>> _asuBox;

        [ObservableProperty]
        private string _materialName;

        [ObservableProperty]
        private string _serial;

        [ObservableProperty]
        private string _address;

        [ObservableProperty]
        private long _lastSellPrice = 0;

        [ObservableProperty]
        private double _entity = 0;

        [ObservableProperty]
        private int _unitId = 0;

        [ObservableProperty]
        private bool _isManufacturedGoods = false;

        public void OnNavigatedFrom()
        {

        }

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {
            using (UnitOfWork db = new())
            {
                AsuBox = await db.unitManager.GetUnits();
            }
            _isInitialized = true;
        }

        [RelayCommand]
        private async Task OnCreateMaterial()
        {
            if (string.IsNullOrEmpty(MaterialName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Serial))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سریال کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (!IsManufacturedGoods && Entity == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موجودی انبار"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (UnitId == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("واحد کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مکان فیزیکی کالا"), ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.materialManager.CreateMaterial(MaterialName, Entity, UnitId, Serial, Address, IsManufacturedGoods);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Primary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                    return;
                }
                await db.SaveChangesAsync();
            }


            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.Accessibility24), TimeSpan.FromMilliseconds(2000));

            Type? pageType = NameToPageTypeConverter.Convert("MaterailList");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
        }
    }
}
