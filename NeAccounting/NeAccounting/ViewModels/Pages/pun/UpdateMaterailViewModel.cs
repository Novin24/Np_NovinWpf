﻿using DomainShared.Errore;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels.Pages
{
    public partial class UpdateMaterailViewModel : ObservableObject, INavigationAware
    {

        private bool _isInitialized = false;
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        public UpdateMaterailViewModel(ISnackbarService snackbarService, INavigationService navigationService)

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
        private long _lastSellPrice;

        [ObservableProperty]
        private double _entity = 0;

        [ObservableProperty]
        private int _unitId = 0;

        [ObservableProperty]
        private bool _isManufacturedGoods ;

        [ObservableProperty]
        private int _materialId = 0;

        public void OnNavigatedFrom()
        {

        }

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _isInitialized = true;
        }

        [RelayCommand]
        private async Task OnUpdate()
        {
            if (string.IsNullOrEmpty(MaterialName))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Serial))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("سریال کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (!IsManufacturedGoods && Entity == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("موجودی انبار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (UnitId == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("واحد کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            if (string.IsNullOrEmpty(Address))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مکان فیزیکی کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }

            using UnitOfWork db = new();
            (string error, bool isSuccess) = await db.materialManager.UpdateMaterial(MaterialId, MaterialName, Entity, UnitId, Serial, Address,LastSellPrice, IsManufacturedGoods);

            if (!isSuccess)
            {
                _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                return;
            }
            await db.SaveChangesAsync();


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
