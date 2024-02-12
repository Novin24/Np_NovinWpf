﻿using DomainShared.Errore;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateSellInvoicePage.xaml
    /// </summary>
    public partial class CreateSellInvoicePage : INavigableView<CreateSellInviceViewModel>
    {
        private readonly ISnackbarService _snackbarService;


        public CreateSellInviceViewModel ViewModel { get; }
        private double _totalEntity;
        private long _price;

        public CreateSellInvoicePage(CreateSellInviceViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.OnAdd())
            {
                ViewModel.AmountOf = null;
                ViewModel.MaterialId = -1;
                ViewModel.Description = null;
                ViewModel.MatPrice = null;
                txt_MaterialName.Text = string.Empty;
                txt_UnitName.Text = string.Empty;
                txt_Unit_price.Text = string.Empty;
                txt_total_price.Text = string.Empty;
                txt_UnitDescription.Text = string.Empty;
                txt_MaterialName.Focus();
            }
            dgv_Inv.Items.Refresh();
        }

        private void Txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            ViewModel.CusId = user.Id;
            lbl_cusId.Text = user.UniqNumber.ToString();
        }

        private void Txt_mat_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var mat = (MatListDto)args.SelectedItem;
            ViewModel.MaterialId = mat.Id;
            _totalEntity = mat.Entity;
            txt_UnitName.Text = mat.UnitName;
            txt_Unit_price.Text = mat.LastPrice.ToString("N0");
            _price = mat.LastPrice;
        }

        private void Txt_amount_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not NumberBox nb)
                return;

            if (nb.Value == null)
                return;

            if (nb.Value > _totalEntity)
            {
                _snackbarService.Show("اخطار", "موجودی انبار منفی میشود !!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
            }
            txt_total_price.Text = (nb.Value.Value * _price).ToString("N0");
        }

        private void Txt_Unit_price_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = MyRegex().IsMatch(e.Text);
        }


        private void Txt_Unit_price_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (sender is not TextBox txt_price)
                return;

            if (txt_price.Text == "" || txt_price.Text == "0") return;
            CultureInfo culture = new("en-US");
            long valueBefore = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);
            _price = valueBefore;
            txt_price.Text = String.Format(culture, "{0:N0}", valueBefore);
            txt_price.Select(txt_price.Text.Length, 0);
        }

        private void Txt_Unit_price_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox txt_price)
                return;

            if (ViewModel.AmountOf == null)
                return;


            ViewModel.MatPrice = _price = Int64.Parse(txt_price.Text, NumberStyles.AllowThousands);

            txt_total_price.Text = (ViewModel.AmountOf.Value * _price).ToString("N0");
        }
        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation())
            {
                _snackbarService.Show("اخطار", "کاربر گرامی ابتدا فیلدهای ویرایشی را ثبت سپس اقدام به ثبت فاکتور نمایید!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Red)), TimeSpan.FromMilliseconds(3000));
                return;
            }
            if (await ViewModel.OnSumbit())
            {
                txt_CustomerName.Text = string.Empty;
                txt_MaterialName.Text = string.Empty;
                txt_UnitName.Text = string.Empty;
                txt_Unit_price.Text = string.Empty;
                txt_total_price.Text = string.Empty;
                txt_UnitDescription.Text = string.Empty;
                lbl_cusId.Text = string.Empty;
            }

        }

        private bool Validation()
        {
            if (txt_MaterialName.Text != string.Empty)
                return false;

            if (txt_amount.Value != null && txt_amount.Value != 0)
                return false;

            if (txt_Unit_price.Text != string.Empty)
                return false;

            return true;
        }


        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();
    }
}
