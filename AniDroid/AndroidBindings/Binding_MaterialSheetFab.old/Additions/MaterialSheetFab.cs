using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
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
    public class MaterialSheetFab<TFab> where TFab : View, IAnimatedFab
    {
        //private const int AnimationSpeed = 1;

        //// Animation durations
        //private const int SheetAnimationDuration = 600 * AnimationSpeed;
        //private const int ShowSheetColorAnimationDuration = (int)(SheetAnimationDuration * .75);
        //private const int HideSheetColorAnimationDuration = (int)(SheetAnimationDuration * 1.5);
        //private const int FabAnimationDuration = 300 * AnimationSpeed;
        //private const int ShowOverlayAnimationDuration = ShowSheetAnimationDelay + SheetAnimationDuration;
        //private const int HideOverlayAnimationDuration = SheetAnimationDuration;

        //// Animation delays
        //private const int ShowSheetAnimationDelay = (int)(FabAnimationDuration * .5);
        //private const int MoveFabAnimationDelay = (int)(SheetAnimationDuration * .3);

        //// Other animation constants
        //private const float FabScaleFactor = .6F;
        //private const int FabArcDegrees = 0;

        //// Views
        //protected TFab Fab;

        //// Animations
        //protected FabAnimation FabAnimation;
        //protected MaterialSheetAnimation SheetAnimation;
        //protected OverlayAnimation OverlayAnimation;

        //// State
        //protected int AnchorX;
        //protected int AnchorY;
        //private bool _isShowing;
        //private bool _isHiding;
        //private bool _hideSheetAfterSheetIsShown;

        //// Listeners
        //public MaterialSheetFabEventListener EventListener { get; set; }



        //public MaterialSheetFab(TFab fab, View sheet, View overlay, int sheetColor, int fabColor, int interpolatorRes)
        //{
        //    var interpolator = AnimationUtils.LoadInterpolator(sheet.Context, interpolatorRes);

        //    Fab = fab;

        //    // Create animations
        //    FabAnimation = new FabAnimation(fab, interpolator);
        //    SheetAnimation = new MaterialSheetAnimation(sheet, sheetColor, fabColor, interpolator);
        //    OverlayAnimation = new OverlayAnimation(overlay, interpolator);

        //    // Set initial visibilities
        //    sheet.Visibility = ViewStates.Invisible;
        //    overlay.Visibility = ViewStates.Gone;

        //    fab.Click += (sender, args) => ShowSheet();
        //    overlay.Touch += (sender, args) =>
        //    {
        //        if (IsSheetVisible && args.Event.Action == MotionEventActions.Down)
        //        {
        //            HideSheet();
        //        }
        //    };

        //    fab.ViewTreeObserver.GlobalLayout += ViewTreeObserverOnGlobalLayout;
        //}

        //private void ViewTreeObserverOnGlobalLayout(object o, EventArgs eventArgs)
        //{
        //    var fab = o as TFab;
        //    fab.ViewTreeObserver.GlobalLayout -= ViewTreeObserverOnGlobalLayout;
        //    UpdateFabAnchor();
        //}

        //public void ShowFab() => ShowFab(0, 0);

        //public void ShowFab(float translationX, float translationY)
        //{
        //    SetFabAnchor(translationX, translationY);

        //    if (!IsSheetVisible)
        //    {
        //        Fab.Show(translationX, translationY);
        //    }

        //}

        //public void ShowSheet()
        //{
        //    if (IsAnimating)
        //    {
        //        return;
        //    }

        //    _isShowing = true;

        //    OverlayAnimation.Show(ShowOverlayAnimationDuration, null);

        //    MorphIntoSheet(new MorphIntoSheetAnimationListener<TFab>(this));

        //    EventListener?.OnShowSheet();
        //}

        //public void HideSheet() => HideSheet(null);

        //public void HideSheet(AnimationListener endListener)
        //{
        //    if (IsAnimating)
        //    {
        //        if (_isShowing)
        //        {
        //            _hideSheetAfterSheetIsShown = true;
        //        }

        //        return;
        //    }

        //    _isHiding = true;

        //    OverlayAnimation.Hide(HideOverlayAnimationDuration, null);

        //    MorphFromSheet(new MorphFromSheetAnimationListener<TFab>(this, endListener));

        //    EventListener?.OnHideSheet();
        //}

        //public void HideSheetThenFab()
        //{
        //    if (IsSheetVisible)
        //    {
        //        HideSheet(new HideSheetThenFabAnimationListener<TFab>(this));
        //    }
        //    else
        //    {
        //        Fab.Hide();
        //    }
        //}

        //protected void MorphIntoSheet(AnimationListener endListener)
        //{
        //    UpdateFabAnchor();

        //    SheetAnimation.AlignSheetWithFab(Fab);

        //    FabAnimation.MorphIntoSheet(SheetAnimation.SheetRevealCenterX, SheetAnimation.GetSheetRevealCenterY(Fab),
        //        GetFabArcSide(SheetAnimation.RevealXDirection), FabArcDegrees, FabScaleFactor, FabAnimationDuration,
        //        null);

        //    // might need to use a java Handler here
        //    //Task.Run(() => {
        //    //    Task.Delay(ShowSheetAnimationDelay);
        //    //    Fab.Visibility = ViewStates.Invisible;
        //    //    SheetAnimation.MorphFromFab(Fab, SheetAnimationDuration, ShowSheetColorAnimationDuration, endListener);
        //    //});
        //}

        //protected void MorphFromSheet(AnimationListener endListener)
        //{
        //    SheetAnimation.MorphIntoFab(Fab, SheetAnimationDuration, HideSheetColorAnimationDuration, null);

        //    // might need to use a java Handler here
        //    //Task.Run(() => {
        //    //    Task.Delay(MoveFabAnimationDelay);
        //    //    SheetAnimation.SheetVisibility = ViewStates.Invisible;
        //    //    FabAnimation.MorphFromSheet(AnchorX, AnchorY, GetFabArcSide(SheetAnimation.RevealXDirection),
        //    //        FabArcDegrees, -FabScaleFactor, FabAnimationDuration, endListener);
        //    //});
        //}

        //protected void UpdateFabAnchor() => SetFabAnchor(Fab.TranslationX, Fab.TranslationY);

        //protected void SetFabAnchor(float translationX, float translationY)
        //{
        //    AnchorX = (int)Math.Round(Fab.GetX() + (float)Fab.Width / 2 + (translationX - Fab.TranslationX));
        //    AnchorY = (int)Math.Round(Fab.GetY() + (float)Fab.Height / 2 + (translationY - Fab.TranslationY));
        //}

        //private Side GetFabArcSide(MaterialSheetFab.RevealXDirection revealXDirection) =>
        //    revealXDirection == MaterialSheetFab.RevealXDirection.Left ? Side.Left : Side.Right;

        //private bool IsAnimating => _isShowing || _isHiding;

        //public bool IsSheetVisible => SheetAnimation.IsSheetVisible;

        //private class MorphIntoSheetAnimationListener<TFab> : AnimationListener where TFab : View, IAnimatedFab
        //{
        //    private readonly MaterialSheetFab<TFab> _fab;

        //    public MorphIntoSheetAnimationListener(MaterialSheetFab<TFab> fab)
        //    {
        //        _fab = fab;
        //    }

        //    public override void OnStart()
        //    {
        //    }

        //    public override void OnEnd()
        //    {
        //        _fab.EventListener?.OnSheetShown();

        //        _fab._isShowing = false;

        //        if (_fab._hideSheetAfterSheetIsShown)
        //        {
        //            _fab.HideSheet();
        //            _fab._hideSheetAfterSheetIsShown = false;
        //        }
        //    }
        //}

        //private class MorphFromSheetAnimationListener<TFab> : AnimationListener where TFab : View, IAnimatedFab
        //{
        //    private readonly MaterialSheetFab<TFab> _fab;
        //    private readonly AnimationListener _endListener;

        //    public MorphFromSheetAnimationListener(MaterialSheetFab<TFab> fab, AnimationListener endListener)
        //    {
        //        _fab = fab;
        //        _endListener = endListener;
        //    }

        //    public override void OnStart()
        //    {
        //    }

        //    public override void OnEnd()
        //    {
        //        _endListener?.OnEnd();

        //        _fab.EventListener?.OnSheetHidden();

        //        _fab._isHiding = false;
        //    }
        //}

        //private class HideSheetThenFabAnimationListener<TFab> : AnimationListener where TFab : View, IAnimatedFab
        //{
        //    private readonly MaterialSheetFab<TFab> _fab;

        //    public HideSheetThenFabAnimationListener(MaterialSheetFab<TFab> fab)
        //    {
        //        _fab = fab;
        //    }

        //    public override void OnStart()
        //    {
        //    }

        //    public override void OnEnd()
        //    {
        //        _fab.Fab.Hide();
        //    }
        //}
    }

    public abstract class MaterialSheetFab
    {
        // Enums
        public enum RevealXDirection
        {
            Left,
            Right
        }

        public enum RevealYDirection
        {
            Up,
            Down
        }
    }
}