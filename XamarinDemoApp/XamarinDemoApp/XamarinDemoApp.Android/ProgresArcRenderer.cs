
using System;
using Android.Graphics;
using XamarinDemoApp;
using XamarinDemoApp.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(ProgressArc), typeof(ProgressArcRenderer))]

namespace XamarinDemoApp.Android
{
    public class ProgressArcRenderer : ViewRenderer
    {
        private Paint _paint;
        private RectF _arcDrawArea;
        private bool _sizeChanged = false;

        public ProgressArcRenderer()
        {
            SetWillNotDraw(false);
        }

        protected override void OnDraw(Canvas canvas)
        {
            var progressArc = (ProgressArc)Element;

            if (_paint == null)
            {
                var displayDensity = Context.Resources.DisplayMetrics.Density;
                var strokeWidth = (float)Math.Ceiling(progressArc.ArcThickness * displayDensity);

                _paint = new Paint();
                _paint.StrokeWidth = strokeWidth;
                _paint.SetStyle(Paint.Style.Stroke);
                _paint.Flags = PaintFlags.AntiAlias;
            }

            if (_arcDrawArea == null || _sizeChanged)
            {
                _sizeChanged = false;

                var arcAreaSize = Math.Min(canvas.ClipBounds.Width(), canvas.ClipBounds.Height());

                var arcDiameter = arcAreaSize - _paint.StrokeWidth;

                var left = canvas.ClipBounds.CenterX() - arcDiameter / 2;
                var top = canvas.ClipBounds.CenterY() - arcDiameter / 2;

                _arcDrawArea = new RectF(left, top, left + arcDiameter, top + arcDiameter);
            }

            var backColor = progressArc.ArcBaseColor;
            var frontColor = progressArc.ArcProgressColor;
            var progress = (float)progressArc.Progress;
            DrawProgressArc(canvas, progress, backColor, frontColor);
        }

        private void DrawProgressArc(Canvas canvas, float progress,
                                      Color arcBaseColor,
                                      Color arcProgressColor)
        {
            _paint.Color = arcBaseColor.ToAndroid();
            canvas.DrawArc(_arcDrawArea, 270, 360, false, _paint);

            _paint.Color = arcProgressColor.ToAndroid();
            canvas.DrawArc(_arcDrawArea, 270, 360 * progress, false, _paint);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName ||
                e.PropertyName == ProgressArc.ArcThicknessProperty.PropertyName ||
                e.PropertyName == ProgressArc.ArcBaseColorProperty.PropertyName ||
                e.PropertyName == ProgressArc.ArcProgressColorProperty.PropertyName)
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