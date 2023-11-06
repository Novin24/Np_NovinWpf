﻿using DomainShared.Pun;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class PunListViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _punName = "";

        [ObservableProperty]
        private string _serial = "";

        [ObservableProperty]
        private IEnumerable<PunListDto> _list;
        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo()
        {
            await InitializeViewModel();
        }

        private async Task InitializeViewModel()
        {

            using (UnitOfWork db = new ())
            {
                List = await db.materialManager.GetMaterails(PunName,Serial);
            }
        }
    }
}
