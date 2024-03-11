﻿using DomainShared.Extension;
using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Stimulsoft.Report;
using System.Globalization;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class BillPage : INavigableView<BillListViewModel>
    {
        public BillListViewModel ViewModel { get; }
        private readonly ISnackbarService _snackbarService;
        public BillPage(BillListViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
        }

        private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.SearchInvoiceCommand.ExecuteAsync(null);
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
            {
                return;
            }
            var us = ((SuggestBoxViewModel<Guid, long>)args.SelectedItem);
            ViewModel.CusId = us.Id;
            ViewModel.PersonelId = us.UniqNumber;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }

        private async void btn_Print_Click(object sender, RoutedEventArgs e)
        {
            var (list, isSuccess) = await ViewModel.PrintInvoice();
            if (!isSuccess)
                return;
            if (!list.Any())
            {
                _snackbarService.Show("خطا", "در بازه انتخابی موردی برای نمایش یافت نشد!!!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            Dictionary<string, string> dic = new()
            {
                {"","" }
            };

            PrintInvoice(dic, list, "InvoiceListDtos");
        }

        private (string error, bool isSuccess) PrintInvoice(Dictionary<string, string> dic, object list, string listName)
        {
            try
            {
                #region StimulSoft
                PersianCalendar pc = new();
                var time = DateTime.Now.ToShamsiDate(pc);
                var cus = ViewModel.Cuslist.First(t => t.Id == ViewModel.CusId);
                StiReport report = new();
                report.Load(@"Reports\ReportInvoices.mrt");
                report.Compile();
                report.RegBusinessObject(listName, list);
                report["Customer_Name"] = $"({cus.UniqNumber}) _ {cus.DisplayName}";
                report["Start_Date"] = $"{Dtp_Start.DisplayDate}";
                report["End_Date"] = $"{Dtp_End.DisplayDate}";
                report["PrintTime"] = time;
                report["Total_Debt"] = list.Select(p => p.Bed).Sum().ToString("N0");
                report["Total_Credit"] = list.Select(p => p.Bes).Sum().ToString("N0");
                report["Total_LeftOVver"] = list.Last().LeftOver.ToString("N0");
                report["TotalSLeftOver"] = list.Last().LeftOver.ToString().NumberToPersianString();
                report["Status"] = $"{list.Last().Status}";

                #region ForEach On Report dataSours
                // زمانی که کامپایل میکردیم اینا کار نمیکنن دیگ
                //var collection = report.Dictionary;
                //foreach (StiDataSource item in collection.DataSources)
                //{
                //    if (item.Name == "Customers")
                //    {
                //        item.DataTable = list.ToList().ToDataTable();
                //    }
                //}

                //foreach (StiVariable item in collection.Variables)
                //{
                //    switch (item.Name)
                //    {
                //        case "Customer_Name":
                //            item.Value = $"({cus.UniqNumber}) _ {cus.DisplayName}";
                //            break;

                //        case "Start_Date":
                //            item.Value = $"{Dtp_Start.DisplayDate}";
                //            break;

                //        case "End_Date":
                //            item.Value = $"{Dtp_End.DisplayDate}";
                //            break;

                //        case "PrintTime":
                //            item.Value = DateTime.Now.ToShamsiDate(pc);
                //            break;

                //        case "Total_Debt":
                //            item.Value = list.Select(p => p.Bed).Sum().ToString("N0");
                //            break;

                //        case "Total_Credit":
                //            item.Value = list.Select(p => p.Bes).Sum().ToString("N0");
                //            break;

                //        case "Total_LeftOVver":
                //            item.Value = list.Last().LeftOver.ToString("N0");
                //            break;

                //        case "TotalSLeftOver":
                //            item.Value = list.Last().LeftOver.ToString().NumberToPersianString();
                //            break;

                //        case "Status":
                //            item.Value = $"{list.Last().Status}";
                //            break;

                //        default:
                //            break;
                //    }
                //}
                //report.Dictionary.Synchronize();
                //StiBusinessObject dataSource = report.Dictionary.BusinessObjects[0];
                //StiVariable vr = report.Dictionary.Variables[0];
                #endregion

                report.RenderWithWpf();
                report.ShowWithWpf();
                #endregion
            }
            catch (Exception ex)
            {
                return new(ex.Message, false);
            }
            return new(string.Empty, true);
        }
    }
}
