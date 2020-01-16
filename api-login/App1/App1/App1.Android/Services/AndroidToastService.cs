using Android.App;
using Android.Widget;
using App1.Droid.Services;
using App1.Services;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidToastService))]

namespace App1.Droid.Services
{
    public class AndroidToastService : IToastService
    {
        public void Show(string message)
        {
            Console.WriteLine($"Android Show: {message}");
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }
    }
}