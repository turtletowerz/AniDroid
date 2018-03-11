using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Com.Gordonwong.Materialsheetfab.Animations;

namespace Com.Gordonwong.Materialsheetfab
{
    public class FabAnimation
    {
        //protected View Fab;
        //protected IInterpolator Interpolator;

        //public FabAnimation(View fab, IInterpolator interpolator)
        //{
        //    Fab = fab;
        //    Interpolator = interpolator;
        //}

        //public void MorphIntoSheet(int endX, int endY, Side side, int arcDegrees, float scaleFactor, long duration,
        //    AnimationListener listener)
        //{
        //    Morph(endX, endY, side, arcDegrees, scaleFactor, duration, listener);
        //}

        //public void MorphFromSheet(int endX, int endY, Side side, int arcDegrees, float scaleFactor,
        //    long duration, AnimationListener listener)
        //{
        //    Fab.Visibility = ViewStates.Visible;
        //    Morph(endX, endY, side, arcDegrees, scaleFactor, duration, listener);
        //}

        //protected void Morph(float endX, float endY, Side side, float arcDegrees, float scaleFactor, long duration,
        //    AnimationListener listener)
        //{
        //    StartArcAnimation(Fab, endX, endY, arcDegrees, side, duration, Interpolator, listener);
        //    Fab.Animate().ScaleXBy(scaleFactor).ScaleYBy(scaleFactor).SetDuration(duration)
        //        .SetInterpolator(Interpolator).Start();
        //}

        //protected void StartArcAnimation(View view, float endX, float endY, float degrees, Side side, long duration,
        //    IInterpolator interpolator, AnimationListener listener)
        //{
        //    var anim = ArcAnimator.CreateArcAnimator(view, endX, endY, degrees, side);
        //    anim.SetDuration(duration);
        //    anim.SetInterpolator(interpolator);
        //    anim.AddListener(new FabAnimatorListenerAdapter(listener));
        //    anim.Start();
        //}

        //private class FabAnimatorListenerAdapter : AnimatorListenerAdapter
        //{
        //    private readonly AnimationListener _listener;

        //    public FabAnimatorListenerAdapter(AnimationListener listener)
        //    {
        //        _listener = listener;
        //    }

        //    public override void OnAnimationStart(Animator animation)
        //    {
        //        _listener?.OnStart();
        //    }

        //    public override void OnAnimationEnd(Animator animation)
        //    {
        //        _listener?.OnEnd();
        //    }
        //}
    }
}