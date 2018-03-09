using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AniDroid.Widgets
{
    public class LoadableContainer : LinearLayout
    {
        private const string ProgressBarName = "PROGRESS_BAR";

        private LinearLayout _progressBarContainer;
        private Dictionary<int, ViewStates> _viewVisibilityList;

        public LoadableContainer(Context context) : base(context)
        {
            Initialize(context, null, null, null);
        }

        public LoadableContainer(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize(context, attrs, null, null);
        }

        public LoadableContainer(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs,
            defStyleAttr)
        {
            Initialize(context, attrs, defStyleAttr, null);
        }

        public LoadableContainer(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context,
            attrs, defStyleAttr, defStyleRes)
        {
            Initialize(context, attrs, defStyleAttr, defStyleRes);
        }

        private void Initialize(Context context, IAttributeSet attrs, int? defStyleAttr, int? defStyleRes)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.Widget_LoadableContainer, this, true);
            _progressBarContainer = FindViewById<LinearLayout>(Resource.Id.LoadableContainer_ProgressBarContainer);
            _progressBarContainer.Visibility = ViewStates.Visible;
            _progressBarContainer.SetTag(Resource.Id.Object_Name, "PROGRESS_BAR");
            _progressBarContainer.Visibility = ViewStates.Visible;
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _viewVisibilityList = new Dictionary<int, ViewStates>();

            for (var i = 0; i < ChildCount; i++)
            {
                var view = GetChildAt(i);
                var name = view.GetTag(Resource.Id.Object_Name);
                if (name == null && (string)name != ProgressBarName)
                {
                    _viewVisibilityList[i] = view.Visibility;
                    view.Visibility = ViewStates.Invisible;
                }
            }

        }

        public void ShowContent()
        {
            if (_viewVisibilityList == null)
            {
                return;
            }

            _progressBarContainer.Visibility = ViewStates.Gone;

            for (var i = 0; i < ChildCount; i++)
            {
                var view = GetChildAt(i);
                var name = view.GetTag(Resource.Id.Object_Name);
                if (name == null && (string)name != ProgressBarName)
                {
                    view.Visibility = _viewVisibilityList[i];
                }
            }
        }
    }
}