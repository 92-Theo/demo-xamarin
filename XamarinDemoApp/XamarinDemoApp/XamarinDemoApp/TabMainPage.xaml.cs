﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinDemoApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabMainPage : TabbedPage
    {
        public TabMainPage()
        {
            InitializeComponent();
        }
    }
}