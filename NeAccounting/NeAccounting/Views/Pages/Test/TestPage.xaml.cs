﻿using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Test
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : INavigableView<TestViewModel>
    {

        public TestViewModel ViewModel{ get;}
        public TestPage(TestViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void PersianDatePicker_DateChosen(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {

        }
    }
}
