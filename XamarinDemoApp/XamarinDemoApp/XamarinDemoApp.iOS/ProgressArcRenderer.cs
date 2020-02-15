using System;
using CoreGraphics;
using Foundation;
using XamarinDemoApp;
using XamarinDemoApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;

[assembly: ExportRenderer(typeof(ProgressArc), typeof(ProgressArcRenderer))]
namespace XamarinDemoApp.iOS
{
    [Preserve(AllMembers = true)]
    public class ProgressArcRenderer : ViewRenderer
    {
        public static readonly float _RATIO_CONVERT_RADIAN = (float)Math.PI / 180;
        float? _radius;
        bool _sizeChanged = false;

        /// <summary>
        /// Necessary to register this class with the Xamarin.Forms with dependency service
        /// </summary>
		public async static void Init()
        {
            var temp = DateTime.Now;
        }

        private nfloat GetRadius(nfloat lineWidth)
        {
            if (_radius == null || _sizeChanged)
            {
                _sizeChanged = false;

                nfloat width = Bounds.Width;
                nfloat height = Bounds.Height;
                var size = (float)Math.Min(width, height);

                _radius = (size / 2f) - ((float)lineWidth / 2f);
            }

            return _radius.Value;
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                var progressArc = (ProgressArc)Element;

                var lineWidth = (float)progressArc.ArcThickness;
                var radius = (int)(GetRadius(lineWidth));
                var progress = (float)progressArc.Progress;
                var backColor = progressArc.ArcBaseColor.ToUIColor();
                var frontColor = progressArc.ArcProgressColor.ToUIColor();
                var startAngle = (float)progressArc.ArcStartAngle;
                var sweepAngle = (float)progressArc.ArcSweepAngle;

                DrawProgressArc(g, Bounds.GetMidX(), Bounds.GetMidY(), progress, lineWidth, radius, startAngle, sweepAngle, backColor, frontColor);
            };
        }

        // TODO Optimize circle drawing by removing allocation of CGPath
        // (maybe by drawing via BitmapContext, per pixel:
        // https://stackoverflow.com/questions/34987442/drawing-pixels-on-the-screen-using-coregraphics-in-swift)
        private void DrawProgressArc(CGContext g, nfloat x0, nfloat y0,
                                     nfloat progress, nfloat lineThickness, nfloat radius,
                                     nfloat startAngle, nfloat sweepAngle,
                                     UIColor backColor, UIColor frontColor)
        {
            g.SetLineWidth(lineThickness);

            // Degree to radian
            var startingAngle = startAngle * _RATIO_CONVERT_RADIAN;
            var sweepingAngle = sweepAngle * _RATIO_CONVERT_RADIAN;
            var maxAngle = startingAngle + sweepingAngle;
            var curAngle = startingAngle + (sweepingAngle * progress);

            // Draw background arc
            CGPath path = new CGPath();

            backColor.SetStroke();

            path.AddArc(x0, y0, radius, startingAngle, maxAngle, false);
            g.AddPath(path);
            g.DrawPath(CGPathDrawingMode.Stroke);

            // Draw progress arc
            var pathStatus = new CGPath();

            frontColor.SetStroke();

            pathStatus.AddArc(x0, y0, radius, startingAngle, curAngle, false);


#if DEBUG
            Debug.WriteLine($"[START]======================================== ");
            Debug.WriteLine($"DrawProgressArc ");
            Debug.WriteLine($"Process: {progress * 100}% ");
            Debug.WriteLine($"Degree - Start: {startAngle} Sweep {sweepAngle}");
            Debug.WriteLine($"Radian - Start: {startingAngle} Sweep {sweepingAngle}");
            Debug.WriteLine($"Radian - MIN: {startingAngle} MAX: {maxAngle} CUR:{curAngle} ");
            Debug.WriteLine($"[END]======================================== ");
#endif
            g.AddPath(pathStatus);
            g.DrawPath(CGPathDrawingMode.Stroke);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var progressArc = (ProgressArc)this.Element;

            if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName ||
                e.PropertyName == ProgressArc.ArcThicknessProperty.PropertyName ||
                e.PropertyName == ProgressArc.ArcBaseColorProperty.PropertyName ||
                e.PropertyName == ProgressArc.ArcProgressColorProperty.PropertyName)
            {
                SetNeedsDisplay();
            }

            if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
               e.PropertyName == VisualElement.HeightProperty.PropertyName)
            {
                _sizeChanged = true;
                SetNeedsDisplay();
            }
        }
    }
}
