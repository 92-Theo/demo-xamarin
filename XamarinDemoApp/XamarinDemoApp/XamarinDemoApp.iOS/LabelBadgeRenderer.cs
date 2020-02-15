using System;
using CoreGraphics;
using Foundation;
using XamarinDemoApp;
using XamarinDemoApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.Diagnostics;
using GameplayKit;
using CoreText;

[assembly: ExportRenderer(typeof(LabelBadge), typeof(LabelBadgeRenderer))]
namespace XamarinDemoApp.iOS
{
    [Preserve(AllMembers = true)]
    public class LabelBadgeRenderer : ViewRenderer
    {
        public static readonly float _RATIO_CONVERT_RADIAN = (float)Math.PI / 180;
        public static readonly float _BADGE_START_ANGLE = 0;
        public static readonly float _BADGE_END_ANGLE = (2.0f * (float)Math.PI);
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

            var labelBadge = (LabelBadge)this.Element;

            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                var x0 = Bounds.GetMidX();
                var y0 = Bounds.GetMidY();
                var badgeColor = labelBadge.BadgeColor.ToUIColor();
                var textColor = labelBadge.TextColor.ToUIColor();
                var text = labelBadge.Text;
                if (text == default)
                    text = "";
                var radius = (float)(Math.Min(Bounds.GetMaxX(), Bounds.GetMaxY()) / 2f);
                var textSize = (float)(Math.Min(Bounds.GetMaxX(), Bounds.GetMaxY()) / 1.4f);

                DrawLabelBadge(g, x0, y0, radius, text, textSize, badgeColor, textColor);
            };
        }

        private void DrawLabelBadge(CGContext g, nfloat x0, nfloat y0,
                                     nfloat radius, string text,
                                     nfloat textSize,
                                     UIColor badgeColor, UIColor textColor)
        {

            // 뱃지
            var path = new CGPath();

            badgeColor.SetColor();
            path.AddArc(x0, y0, radius, _BADGE_START_ANGLE, _BADGE_END_ANGLE, true);
            g.AddPath(path);
            g.DrawPath(CGPathDrawingMode.Fill);


            CGRect textRect = new CGRect(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            {
                textColor.SetFill();
                //var textStyle = new NSMutableParagraphStyle();
                // textStyle.Alignment = UITextAlignment.Center;

                //Measure Height
                var textFontAttributes = new UIStringAttributes()
                {
                    Font = UIFont.FromName("ArialMT", textSize)
                };
                var textTextHeight = new NSString(text).GetBoundingRect(
                    new CGSize(textRect.Width, nfloat.MaxValue),
                    NSStringDrawingOptions.UsesLineFragmentOrigin,
                    textFontAttributes, null).Height;

                g.SaveState();
                g.ClipToRect(textRect);
                var w = textRect.Width;
                var h = textRect.Height;
                var x = textRect.X;
                var y = textRect.Y + ((h - textTextHeight) / 2);
                new NSString(text).DrawString(
                    new CGRect(x, y, w, h),
                    textFontAttributes.Font,
                    UILineBreakMode.WordWrap,
                    UITextAlignment.Center);
                g.RestoreState();
            }

        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            // var progressArc = (LabelBadge)this.Element;

            if (e.PropertyName == VisualElement.WidthProperty.PropertyName ||
               e.PropertyName == VisualElement.HeightProperty.PropertyName ||
               e.PropertyName == LabelBadge.TextProperty.PropertyName)
            {
                _sizeChanged = true;
                SetNeedsDisplay();
            }
        }
    }
}
