﻿using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using System.Text.RegularExpressions;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateRecCheque.xaml
    /// </summary>
    public partial class CreateRecChequePage : INavigableView<CreateRecChequeViewModel>
    {
        public CreateRecChequeViewModel ViewModel { get; }
        public CreateRecChequePage(CreateRecChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_Name.Focus();
            Cmb_Status.ItemsSource = SubmitChequeStatus.Register.ToEnumDictionary();
        }

        private void Txt_Name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            ViewModel.CusId = user.Id;
            lbl_cusId.Text = user.UniqNumber.ToString();
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = MyRegex().IsMatch(e.Text);
        }

        [RelayCommand]
        private async Task OnSubmit()
        {

            Btn_submit.Focus();
            await ViewModel.SubmitCommand.ExecuteAsync(null);
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();
    }
}
