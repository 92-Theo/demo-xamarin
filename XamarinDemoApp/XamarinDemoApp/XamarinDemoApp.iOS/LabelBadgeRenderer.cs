using System;
using CoreGraphics;
using Foundation;
using XamarinDemoApp;
using XamarinDemoApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;
using CoreText;

[assembly: ExportRenderer(typeof(LabelBadge), typeof(LabelBadgeRenderer))]
namespace XamarinDemoApp.iOS
{
    [Preserve(AllMembers = true)]
    public class LabelBadgeRenderer : ViewRenderer
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
                var labelBadge = (LabelBadge)Element;

                var badgeColor = labelBadge.BadgeColor.ToUIColor();
                var textColor = labelBadge.TextColor.ToUIColor();
                var radius = (float)(Math.Min(Bounds.GetMaxX(), Bounds.GetMaxY()) / 2f);
                var text = labelBadge.Text;

                DrawLabelBadge(g, Bounds.GetMidX(), Bounds.GetMidY(), radius, text, textColor, badgeColor);
            };
        }

        private void DrawLabelBadge(CGContext g, nfloat cx, nfloat cy,
                                     nfloat radius, string text,
                                     UIColor textColor, UIColor badgeColor)
        {
            // g.SetLineWidth(lineThickness);
            // Draw Badge
            var path = new CGPath();
            badgeColor.SetStroke();
            path.AddArc(cx, cy, radius, 0, 360, false);
            g.AddPath(path);
            g.DrawPath(CGPathDrawingMode.Fill);

            // Draw Text
            path = new CGPath();
            textColor.SetStroke();
            var attributedString = new NSAttributedString(text,
                new CTStringAttributes
                {
                    Font = new CTFont("Arial", radius)
                });
            using (var textLine = new CTLine(attributedString))
            {
                textLine.Draw(g);
            }
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
