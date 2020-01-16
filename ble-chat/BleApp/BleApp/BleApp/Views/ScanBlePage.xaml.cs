using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanBlePage : ContentPage
    {
        public ScanBlePage()
        {
            InitializeComponent();

            this.BindingContext = ViewModels.ScanBleViewModel.Instance;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModels.ScanBleViewModel.Instance.Clear();
            Models.BleManager.Instance.StartScan();
        }
    }
}