using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinDemoApp
{
    public class LabelBadge : Label
    {
        /// <summary>
        /// Sets the ring's progress color. 
        /// HINT: The ring progress color covers the ring base color
        /// </summary>
        public static readonly BindableProperty BadgeColorProperty = BindableProperty.Create("BadgeColor", typeof(Color),
                                                                                                    typeof(LabelBadge), Color.FromRgb(234, 105, 92));
        public Color BadgeColor
        {
            get { return (Color)base.GetValue(BadgeColorProperty); }
            set { base.SetValue(BadgeColorProperty, value); }
        }
    }
}
