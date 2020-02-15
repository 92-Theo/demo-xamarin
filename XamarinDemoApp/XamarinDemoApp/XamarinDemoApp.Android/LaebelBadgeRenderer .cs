
using System;
using Android.Graphics;
using XamarinDemoApp;
using XamarinDemoApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LabelBadge), typeof(LabelBadgeRenderer))]

namespace XamarinDemoApp.Droid
{
    public class LabelBadgeRenderer : ViewRenderer
    {
        private Paint _paintText;
        private Paint _paintCircle;
        private float _radius;
        private PointF _pointText;
        private PointF _pointCircle;
        private bool _sizeChanged = false;

        public LabelBadgeRenderer() : base(MainActivity.Instance) 
        {
            SetWillNotDraw(false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            var labelCircle = (LabelBadge)Element;

            if (_paintCircle == null)
            {
                _paintCircle = new Paint();
                _paintCircle.Color = labelCircle.BadgeColor.ToAndroid();
            }

            if (_paintText == null)
            {
                _paintText = new Paint();
                _paintText.TextAlign = Paint.Align.Center;
                _paintText.Color = labelCircle.TextColor.ToAndroid();
            }

            if (_pointText == null  || _pointCircle == null || _sizeChanged)
            {
                _sizeChanged = false;

                _radius = Math.Min(canvas.ClipBounds.Width(), canvas.ClipBounds.Height()) / 2f;
                var cx = canvas.ClipBounds.CenterX();
                var cy = canvas.ClipBounds.CenterY();
                // Circle
                _pointCircle = new PointF(cx, cy);
                // Text
                var textSize = Math.Min(canvas.ClipBounds.Width(), canvas.ClipBounds.Height()) / 1.4f;
                _paintText.TextSize = textSize;
                _pointText = new PointF(cx, cy + (textSize / 3));
            }
            
            canvas.DrawCircle(_pointCircle.X, _pointCircle.Y, _radius, _paintCircle);
            canvas.DrawText(labelCircle.Text, _pointText.X, _pointText.Y, _paintText);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == LabelBadge.TextProperty.PropertyName)
            {
                Invalidate();
            }

            if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                e.PropertyName == VisualElement.HeightProperty.PropertyName)
            {
                _sizeChanged = true;
                Invalidate();
            }
        }
    }
}