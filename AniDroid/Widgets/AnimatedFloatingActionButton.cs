using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Gordonwong.Materialsheetfab;

namespace AniDroid.Widgets
{
    public class AnimatedFloatingActionButton : FloatingActionButton, IAnimatedFab
    {
        private const int FabAnimationDuration = 200;

        protected AnimatedFloatingActionButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public AnimatedFloatingActionButton(Context context) : base(context)
        {
        }

        public AnimatedFloatingActionButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public AnimatedFloatingActionButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public override void Show()
        {
            Show(0, 0);
        }

        public void Show(float translationX, float translationY)
        {
            SetTranslation(translationX, translationY);

            if (Visibility != ViewStates.Visible)
            {
                var pivotX = PivotX + translationX;
                var pivotY = PivotY + translationY;

                var anim = pivotX == 0 || pivotY == 0
                    ? new ScaleAnimation(0, 1, 0, 1, Dimension.RelativeToSelf, .5F, Dimension.RelativeToSelf, .5F)
                    : new ScaleAnimation(0, 1, 0, 1, pivotX, pivotY);

                anim.Duration = FabAnimationDuration;
                anim.Interpolator = Interpolator;
                StartAnimation(anim);
            }

            Visibility = ViewStates.Visible;
        }

        public override void Hide()
        {
            if (Visibility == ViewStates.Visible)
            {
                var pivotX = PivotX + TranslationX;
                var pivotY = PivotY + TranslationY;

                var anim = new ScaleAnimation(1, 0, 1, 0, pivotX, pivotY)
                {
                    Duration = FabAnimationDuration,
                    Interpolator = Interpolator
                };
                StartAnimation(anim);
            }

            Visibility = ViewStates.Invisible;
        }

        private void SetTranslation(float translationX, float translationY)
        {
            Animate().SetInterpolator(Interpolator).SetDuration(FabAnimationDuration).TranslationX(translationX)
                .TranslationY(translationY);
        }

        private IInterpolator Interpolator =>
            AnimationUtils.LoadInterpolator(Context, Resource.Interpolator.msf_interpolator);
    }
}