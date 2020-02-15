using Xamarin.Forms;

namespace XamarinDemoApp
{
    public class ProgressArc : ProgressBar
    {
        #region Properties


        // Source: https://www.jimbobbennett.io/animating-xamarin-forms-progress-bars/
        /// <summary>
        /// Let's you animate from the current progress to a new progress using
        /// the values of the properties AnimationLength and AnimationEasing.
        /// </summary>
        public static readonly BindableProperty AnimatedProgressProperty = BindableProperty.Create("AnimatedProgress", typeof(double),
                                                                                                   typeof(ProgressArc), 0.0,
                                                                                                   propertyChanged: HandleAnimatedProgressChanged);
        public double AnimatedProgress
        {
            get { return (double)base.GetValue(AnimatedProgressProperty); }
            set
            {
                base.SetValue(AnimatedProgressProperty, value);

                StartProgressToAnimation();
            }
        }

        /// <summary>
        /// Set's the animation length that is used when using the AnimatedProgress
        /// property to animate to a certain progress.
        /// </summary>
        public static readonly BindableProperty AnimationLengthProperty = BindableProperty.Create("AnimationLength", typeof(uint),
                                                                                                  typeof(ProgressArc), (uint)800,
                                                                                                  propertyChanged: HandleAnimationLengthChanged);
        public uint AnimationLength
        {
            get { return (uint)base.GetValue(AnimationLengthProperty); }
            set { base.SetValue(AnimationLengthProperty, value); }
        }

        /// <summary>
        /// Set's the easing function that is used when using the AnimatedProgress
        /// property to animate to a certain progress.
        /// </summary>
        public static readonly BindableProperty AnimationEasingProperty = BindableProperty.Create("AnimationEasing", typeof(uint),
                                                                                                  typeof(ProgressArc), (uint)0,
                                                                                                  propertyChanged: HandleAnimationEasingChanged);

        public Easing AnimationEasing
        {
            get
            {
                var intValue = (uint)base.GetValue(AnimationEasingProperty);
                var easingMethod = ProgressRingUtils.IntToEasingMethod((int)intValue);

                return easingMethod;
            }
            set
            {
                var easingMethod = ProgressRingUtils.EasingMethodToInt(value);

                base.SetValue(AnimationEasingProperty, easingMethod);
            }
        }

        /// <summary>
        /// Sets the ring's progress color. 
        /// HINT: The ring progress color covers the ring base color
        /// </summary>
        public static readonly BindableProperty ArcProgressColorProperty = BindableProperty.Create("ArcProgressColor", typeof(Color),
                                                                                                    typeof(ProgressArc), Color.FromRgb(234, 105, 92));
        public Color ArcProgressColor
        {
            get { return (Color)base.GetValue(ArcProgressColorProperty); }
            set { base.SetValue(ArcProgressColorProperty, value); }
        }

        /// <summary>
        /// Sets the ring's base (background) color.
        /// </summary>
        public static readonly BindableProperty ArcBaseColorProperty = BindableProperty.Create("ArcBaseColor", typeof(Color),
                                                                                                typeof(ProgressArc), Color.FromRgb(46, 60, 76));
        public Color ArcBaseColor
        {
            get { return (Color)base.GetValue(ArcBaseColorProperty); }
            set { base.SetValue(ArcBaseColorProperty, value); }
        }

        /// <summary>
        /// Sets the thickness of the progress Ring. The thickness "grows" into the
        /// center of the ring (so the outer dimensions are not incluenced by the
        /// ring thickess.
        /// </summary>
        public static readonly BindableProperty ArcThicknessProperty = BindableProperty.Create("ArcThickness", typeof(double),
                                                                                                typeof(ProgressArc), 5.0);
        public double ArcThickness
        {
            get { return (double)base.GetValue(ArcThicknessProperty); }
            set { base.SetValue(ArcThicknessProperty, value); }
        }


        public static readonly BindableProperty ArcStartAngleProperty = BindableProperty.Create("ArcStartAngle", typeof(float),
                                                                                                typeof(ProgressArc), 270f);

        /// <summary>
        /// Start Degree, 시계 방향으로 순(0:우, 90:하, 180: 좌, 270: 상)
        /// </summary>
        public float ArcStartAngle
        {
            get { return (float)base.GetValue(ArcStartAngleProperty); }
            set { base.SetValue(ArcStartAngleProperty, value); }
        }

        
        public static readonly BindableProperty ArcSweepAngleProperty = BindableProperty.Create("ArcSweepAngle", typeof(float),
                                                                                                typeof(ProgressArc), 360f);
        /// <summary>
        /// Sweep Degree, ArcStartAngle부터 ArcSweepAngle까지의 Degree
        /// </summary>
        public float ArcSweepAngle
        {
            get { return (float)base.GetValue(ArcSweepAngleProperty); }
            set { base.SetValue(ArcSweepAngleProperty, value); }
        }

        #endregion

        #region Animation

        public void StartProgressToAnimation()
        {
            ViewExtensions.CancelAnimations(this);
            var length = base.GetValue(AnimationLengthProperty);

            ProgressTo(AnimatedProgress, AnimationLength, AnimationEasing);
        }

        #endregion

        #region static handlers

        static void HandleAnimatedProgressChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var progressArc = (ProgressArc)bindable;
            progressArc.AnimatedProgress = (double)newValue;
        }

        static void HandleAnimationLengthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var progressArc = (ProgressArc)bindable;

            var animationIsRunning = progressArc.AnimationIsRunning("Progress");

            // If the progress animation is already running
            // rerun it with the new length value.
            if (animationIsRunning)
                progressArc.StartProgressToAnimation();
        }

        static void HandleAnimationEasingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var progressArc = (ProgressArc)bindable;
            var animationIsRunning = progressArc.AnimationIsRunning("Progress");

            // If the progress animation is already running
            // rerun it with the new length value.
            if (animationIsRunning)
                progressArc.StartProgressToAnimation();
        }

        #endregion
    }
}
