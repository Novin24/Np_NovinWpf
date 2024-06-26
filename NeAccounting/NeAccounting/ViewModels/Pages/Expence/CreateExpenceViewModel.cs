﻿using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Errore;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class CreateExpenceViewModel(ISnackbarService snackbarService, INavigationService navigationService) : ObservableObject
    {
        private readonly INavigationService _navigationService = navigationService;
        private readonly ISnackbarService _snackbarService = snackbarService;
        private readonly bool _isreadonly = NeAccountingConstants.ReadOnlyMode;
        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// عنوان هزینه
        /// </summary>
        [ObservableProperty]
        private string _expensetype;

        /// <summary>
        /// مبلغ
        /// </summary>
        [ObservableProperty]
        private long? _amount = 0;

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        [ObservableProperty]
        private PaymentType _payType = PaymentType.CardToCard;

        /// <summary>
        /// دریافت کننده
        /// </summary>
        [ObservableProperty]
        private string _receiver;

        /// <summary>
        /// توضیحات
        /// </summary>
        [ObservableProperty]
        private string _description;


        [RelayCommand]
        private async Task OnCreateExpense()
        {
            if (_isreadonly)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ویرایش در سال مالی گذشته امکان پذیر نمی باشد", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.IndianRed)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (string.IsNullOrEmpty(Expensetype))
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نوع هزینه"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (Amount == null || Amount == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            using (UnitOfWork db = new())
            {
                var (error, isSuccess) = await db.ExpenseManager.CreateExpense(SubmitDate.Value, Expensetype, Amount.Value, PayType, Receiver, Description);
                if (!isSuccess)
                {
                    _snackbarService.Show("کاربر گرامی", error, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                    return;
                }
            }
            _snackbarService.Show("کاربر گرامی", "عملیات با موفقیت انجام شد.", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));


            Type? pageType = NameToPageTypeConverter.Convert("ExpencesList");

            if (pageType == null)
            {
                return;
            }

            _navigationService.Navigate(pageType);
        }

    }
}
