using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace XamarinDemoApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            map.Pins.Clear();
            Pin _GPin = new Pin()
            {
                Icon = BitmapDescriptorFactory.DefaultMarker(Color.Gray),
                Type = PinType.Place,
                Label = "키플러스",
                Address = "대전시 유성구 대덕대로 52번길",
                Position = new Position(127.3780804, 36.3782418),
            };
            map.Pins.Add(_GPin);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(_GPin.Position, Distance.FromMeters(1000)), true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

           

        }
    }
}
