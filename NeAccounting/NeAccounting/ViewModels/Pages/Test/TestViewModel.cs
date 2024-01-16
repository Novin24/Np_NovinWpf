﻿using NeAccounting.Views.Pages;
using Wpf.Ui;

namespace NeAccounting.ViewModels
{
    public partial class TestViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public TestViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }


        [ObservableProperty]
        private long _ts = 240000000;

        [RelayCommand]
        private void OnMyClick()
        {
            var navigationView = _navigationService.GetNavigationControl();
            //navigationView.Navigate(typeof(UpdateMaterailPage), new UpdateMaterailPage(new ViewModels.Pages.UpdateMaterailViewModel() { MaterialId = 398 }));
        }
    }
}
