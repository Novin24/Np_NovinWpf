﻿using DomainShared.Errore;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.Pun;
using Infrastructure.UnitOfWork;
using NeAccounting.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;


namespace NeAccounting.ViewModels
{
    public partial class UpdateSellInvoiceViewModel : ObservableObject
    {
        private readonly ISnackbarService _snackbarService;
        private readonly INavigationService _navigationService;

        public UpdateSellInvoiceViewModel(ISnackbarService snackbarService, INavigationService navigationService)
        {
            _snackbarService = snackbarService;
            _navigationService = navigationService;
        }
        #region properties

        private int RowId = 1;

        [ObservableProperty]
        private Guid _invoiceId;

        /// <summary>
        /// لیست اجناس  فاکتور
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<RemittanceListViewModel> _list = [];

        /// <summary>
        /// لیست ثابت اجناس  فاکتور
        /// </summary>
        public List<RemittanceListViewModel> StaticList = [];

        /// <summary>
        /// لیست کلیه اجناس
        /// </summary>
        [ObservableProperty]
        private List<MatListDto> _matList;

        /// <summary>
        /// نام مشتری
        /// </summary>
        [ObservableProperty]
        private string _cusName;

        /// <summary>
        /// شماره مشتری
        /// </summary>
        [ObservableProperty]
        private long _cusNumber;

        [ObservableProperty]
        private DateTime? _submitDate = DateTime.Now;

        /// <summary>
        /// مقدار پورسانت
        /// </summary>
        [ObservableProperty]
        private double? _commission;

        /// <summary>
        /// وضعیت مشتری
        /// </summary>
        [ObservableProperty]
        private string _status = "تسویه";

        /// <summary>
        /// بدهکاری مشتری
        /// </summary>
        [ObservableProperty]
        private string _debt = "0";

        /// <summary>
        /// نام واحد
        /// </summary>
        [ObservableProperty]
        private string? _unitName = string.Empty;

        /// <summary>
        /// قیمت واحد
        /// </summary>
        [ObservableProperty]
        private string? _unitPrice = string.Empty;

        /// <summary>
        /// جمع کل قیمت در تعداد
        /// </summary>
        [ObservableProperty]
        private string? _tPrice = string.Empty;

        /// <summary>
        /// طلبکاری مشتری
        /// </summary>
        [ObservableProperty]
        private string _credit = "0";

        /// <summary>
        /// مبلغ کل فاکتور
        /// </summary>
        [ObservableProperty]
        private string _totalPrice = "0";

        /// <summary>
        /// مبلغ کل پورسانت
        /// </summary>
        [ObservableProperty]
        private string _totalcommission = "0";

        /// <summary>
        /// مبلغ باقی مانده
        /// </summary>
        [ObservableProperty]
        private string _remainPrice = "0";

        /// <summary>
        /// شماره اخرین فاکتور
        /// </summary>
        [ObservableProperty]
        private string _lastInvoice;

        /// <summary>
        /// شناسه جنس انتخاب شده در سلکت باکس
        /// </summary>
        [ObservableProperty]
        private int _materialId = -1;

        /// <summary>
        /// مقدار انتخاب شده
        /// </summary>
        [ObservableProperty]
        private double? _amountOf;

        /// <summary>
        /// ردیف انتخاب شده
        /// </summary>
        [ObservableProperty]
        private Guid? _remId;

        /// <summary>
        /// مبلغ انتخابی 
        /// </summary>
        [ObservableProperty]
        private long? _matPrice;

        /// <summary>
        /// توضیحات ردیف
        /// </summary>
        [ObservableProperty]
        private string? _description;

        /// <summary>
        /// توضیحات فاکتور
        /// </summary>
        [ObservableProperty]
        private string? _invDescription;
        #endregion

        #region Commands
        /// <summary>
        /// افزودن ردیف
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private void OnAdd()
        {
            #region validation

            if (MaterialId < 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("نام کالا"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (AmountOf == null || AmountOf <= 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مقدار"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (MatPrice == null || MatPrice == 0)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("مبلغ"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            #endregion

            var mat = MatList.First(t => t.Id == MaterialId);
            List.Add(new RemittanceListViewModel()
            {
                AmountOf = AmountOf.Value,
                UnitName = mat.UnitName,
                MatName = mat.MaterialName,
                Price = MatPrice.Value,
                RremId = RemId ?? Guid.Empty,
                RowId = RowId,
                IsDeleted = false,
                TotalPrice = (long)(MatPrice.Value * AmountOf.Value),
                Description = Description,
                MaterialId = MaterialId,
            });
            SetCommisionValue();
            AmountOf = null;
            MaterialId = -1;
            Description = null;
            MatPrice = null;
            UnitName = null;
            RemId = null;
            UnitPrice = null;
            TPrice = null;
            UnitName = string.Empty;
            UnitPrice = string.Empty;
            TPrice = string.Empty;
            RefreshRow(ref RowId);
            return;
        }

        /// <summary>
        /// ویرایش ردیف
        /// </summary>
        /// <param name="rowId"></param>
        /// <returns></returns>
        [RelayCommand]
        private void OnUpdate(int rowId)
        {
            if (RemId != null)
            {
                return;
            }
            var itm = List.FirstOrDefault(t => t.RowId == rowId);
            if (itm == null)
                return;
            MaterialId = itm.MaterialId;
            RemId = itm.RremId;
            AmountOf = itm.AmountOf;
            MatPrice = itm.Price;
            TPrice = itm.TotalPrice.ToString("N0");
            UnitName = itm.UnitName;
            UnitPrice = itm.Price.ToString("N0");
            Description = itm.Description;
            List.Remove(itm);
            RefreshRow(ref rowId);
            SetCommisionValue();
        }

        /// <summary>
        /// حذف ردیف
        /// </summary>
        /// <param name="rowId"></param>s
        [RelayCommand]
        private void OnRemove(int rowId)
        {
            var itm = List.FirstOrDefault(t => t.RowId == rowId);
            if (itm == null)
                return;
            itm.IsDeleted = !itm.IsDeleted;
            SetCommisionValue();
        }

        /// <summary>
        /// ثبت فاکتور
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSumbit()
        {
            #region validation
            if (string.IsNullOrEmpty(Description))
            {
                Description = "فاکتور فروش";
            }

            if (RemId != null)
            {
                _snackbarService.Show("خطا", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ثبت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (SubmitDate == null)
            {
                _snackbarService.Show("خطا", NeErrorCodes.IsMandatory("تاریخ ثبت"), ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            if (List == null || !List.Any(t => !t.IsDeleted))
            {
                _snackbarService.Show("خطا", "وارد کردن حداقل یک ردیف الزامیست !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            #endregion

            #region UpdateMaterial
            using UnitOfWork db = new();
            var li = new List<RemittanceListViewModel>(List.Where(t => !t.IsDeleted || t.RremId != Guid.Empty));
            foreach (var item in li)
            {
                if (item.RremId == Guid.Empty)
                {
                    var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf, false, item.Price);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }
                    continue;
                }
                var oldItm = StaticList.First(t => t.RremId.Equals(item.RremId));

                if (oldItm.AmountOf == item.AmountOf)
                    continue;

                if (item.AmountOf < oldItm.AmountOf)
                {
                    var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, oldItm.AmountOf - item.AmountOf, true, item.Price);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }
                    continue;
                }
                else
                {
                    var (errore, isSuccess) = await db.MaterialManager.UpdateMaterialEntity(item.MaterialId, item.AmountOf - oldItm.AmountOf, false, item.Price);
                    if (!isSuccess)
                    {
                        _snackbarService.Show("خطا", errore, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                        return;
                    }
                    continue;
                }

            }
            #endregion

            #region UpdateSellDoc
            var totalInvoicePrice = li.Where(t => !t.IsDeleted).Sum(t => t.TotalPrice);
            var (e, s) = await db.DocumentManager.UpdateSellDocument(InvoiceId, totalInvoicePrice, Commission, InvDescription, SubmitDate.Value, li);
            if (!s)
            {
                _snackbarService.Show("خطا", e, ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            await db.SaveChangesAsync();
            _snackbarService.Show("کاربر گرامی", $"ثبت فاکتور با موفقیت انجام شد", ControlAppearance.Success, new SymbolIcon(SymbolRegular.CheckmarkCircle20), TimeSpan.FromMilliseconds(3000));
            #endregion

            #region reDirect
            Type? pageType = NameToPageTypeConverter.Convert("Bill");

            if (pageType == null)
            {
                return;
            }
            _ = _navigationService.Navigate(pageType);
            #endregion
        }

        /// <summary>
        /// به روز رسانی شماره ردیف ها
        /// </summary>
        /// <param name="rowId"></param>
        private void RefreshRow(ref int rowId)
        {
            int row = 1;
            foreach (var item in List.OrderBy(t => t.RowId))
            {
                item.RowId = row;
                row++;
            }
            rowId = row;
        }


        /// <summary>
        /// به روز رسانی مبلغ پورسانت
        /// </summary>
        private void SetCommisionValue()
        {
            long total = List.Where(t => !t.IsDeleted).Sum(t => t.TotalPrice);
            TotalPrice = total.ToString("N0");
            if (Commission != null && Commission != 0)
            {
                var com = (long)(total * (Commission / 100));
                Totalcommission = com.ToString("N0");
                total -= com;
            }
            else
            {
                Totalcommission = "0";
            }
            RemainPrice = total.ToString("N0");
        }
        #endregion
    }
}