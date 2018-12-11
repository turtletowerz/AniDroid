using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.Adapters.ViewModels;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.AniListObject;
using AniDroid.Base;
using AniDroid.Dialogs;
using OneOf;

namespace AniDroid.Adapters.UserAdapters
{
    public class AniListActivityRecyclerAdapter : AniDroidRecyclerAdapter<AniListActivityViewModel, AniListActivity>
    {
        public Action<string> EditActivityAction { get; set; }
        public Action DeleteActivityAction { get; set; }

        private readonly IAniListActivityPresenter _presenter;
        private readonly int? _userId;

        public AniListActivityRecyclerAdapter(BaseAniDroidActivity context,
            AniListActivityRecyclerAdapter adapter) : base(context, adapter)
        {
            CustomCardUseItemDecoration = true;
            _presenter = adapter._presenter;
            _userId = adapter._userId;
        }

        public AniListActivityRecyclerAdapter(BaseAniDroidActivity context,
            IAsyncEnumerable<OneOf<IPagedData<AniListActivity>, IAniListError>> enumerable, int? currentUserId,
            IAniListActivityPresenter presenter, Func<AniListActivity, AniListActivityViewModel> createViewModelFunc) :
            base(context, enumerable, RecyclerCardType.Custom, createViewModelFunc)
        {
            _userId = currentUserId;
            _presenter = presenter;
            CustomCardUseItemDecoration = true;
        }

        public override void BindCustomViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (!(holder is AniListActivityViewHolder viewHolder))
            {
                return;
            }

            var viewModel = Items[position];

            viewHolder.Timestamp.Text = viewModel.Timestamp;
            viewHolder.Title.TextFormatted = viewModel.FormattedTitle;
            viewHolder.ContentText.TextFormatted = viewModel.FormattedBody;

            viewHolder.ReplyCountContainer.Visibility = viewModel.ReplyCountVisibility;
            viewHolder.ReplyCount.Text = viewModel.ReplyCount;
            viewHolder.LikeCount.Text = viewModel.LikeCount;

            Context.LoadImage(viewHolder.Image, viewModel.ImageUri);

            //if (item.ReplyCount > 0)
            //{
            //    viewHolder.ReplyCountContainer.Visibility = ViewStates.Visible;
            //    viewHolder.ReplyCount.Text = item.ReplyCount.ToString();
            //}
            //else
            //{
            //    viewHolder.ReplyCountContainer.Visibility = ViewStates.Gone;
            //}

            //viewHolder.LikeCount.Text = item.Likes?.Count.ToString();
            //viewHolder.LikeIcon.ImageTintList = ColorStateList.ValueOf(item.Likes?.Any(x => x.Id == _userId) == true
            //    ? Color.Crimson
            //    : _defaultIconColor);

            viewHolder.ReplyLikeContainer.SetTag(Resource.Id.Object_Position, position);
            viewHolder.ReplyLikeContainer.Click -= ShowReplyDialog;
            viewHolder.ReplyLikeContainer.Click += ShowReplyDialog;

            viewHolder.Container.SetTag(Resource.Id.Object_Position, position);
            viewHolder.Container.Click -= RowClick;
            viewHolder.Container.Click += RowClick;
            viewHolder.Container.LongClick -= RowLongClick;
            viewHolder.Container.LongClick += RowLongClick;
        }

        public override RecyclerView.ViewHolder CreateCustomViewHolder(ViewGroup parent, int viewType)
        {
            var holder = new AniListActivityViewHolder(
                Context.LayoutInflater.Inflate(Resource.Layout.View_AniListActivityItem, parent, false));

            if (!_userId.HasValue)
            {
                holder.ReplyButton.Visibility = ViewStates.Gone;
            }

            return holder;
        }

        private void ShowReplyDialog(object sender, EventArgs e)
        {
            var senderView = sender as View;
            var activityPosition = (int)senderView.GetTag(Resource.Id.Object_Position);
            var viewModel = Items[activityPosition];

            if (_userId.HasValue || viewModel.Model.Likes?.Any() == true || viewModel.Model.Replies?.Any() == true)
            {
                AniListActivityRepliesDialog.Create(Context, viewModel.Model, _userId, PostReply, ToggleLikeActivity);
            }
        }

        private async void ToggleLikeActivity(int activityId)
        {
            //var activityItemPosition = Items.FindIndex(x => x?.Id == activityId);
            //var activityItem = Items[activityItemPosition];
            //Items[activityItemPosition] = null;
            //NotifyItemChanged(activityItemPosition);

            //await _presenter.ToggleActivityLikeAsync(activityItem, activityItemPosition);
        }

        private async void PostReply(int activityId, string text)
        {
            //var activityItemPosition = Items.FindIndex(x => x?.Id == activityId);
            //var activityItem = Items[activityItemPosition];
            //Items[activityItemPosition] = null;
            //NotifyItemChanged(activityItemPosition);

            //await _presenter.PostActivityReplyAsync(activityItem, activityItemPosition, text);
        }

        private new void RowClick(object sender, EventArgs e)
        {
            var view = sender as View;
            var position = (int)view.GetTag(Resource.Id.Object_Position);
            var viewModel = Items[position];
            viewModel.ClickAction?.Invoke();
        }

        private new void RowLongClick(object sender, View.LongClickEventArgs e)
        {
            var view = sender as View;
            var position = (int)view.GetTag(Resource.Id.Object_Position);
            var viewModel = Items[position];
            viewModel.LongClickAction?.Invoke(position, _presenter);
        }

        public class AniListActivityViewHolder : RecyclerView.ViewHolder
        {
            public View Container { get; set; }
            public ImageView Image { get; set; }
            public TextView Title { get; set; }
            public TextView Timestamp { get; set; }
            public TextView ContentText { get; set; }
            public LinearLayout ContentImageContainer { get; set; }
            public View ReplyContainer { get; set; }
            public View ReplyCountContainer { get; set; }
            public TextView ReplyCount { get; set; }
            public View ReplyLikeContainer { get; set; }
            public TextView LikeCount { get; set; }
            public ImageView LikeIcon { get; set; }
            public View ReplyButton { get; set; }

            public AniListActivityViewHolder(View view) : base(view)
            {
                Container = view.FindViewById(Resource.Id.AniListActivity_Container);
                Image = view.FindViewById<ImageView>(Resource.Id.AniListActivity_Image);
                Title = view.FindViewById<TextView>(Resource.Id.AniListActivity_Title);
                Timestamp = view.FindViewById<TextView>(Resource.Id.AniListActivity_Timestamp);
                ContentText = view.FindViewById<TextView>(Resource.Id.AniListActivity_ContentText);
                ContentImageContainer = view.FindViewById<LinearLayout>(Resource.Id.AniListActivity_ContentImageContainer);
                ReplyContainer = view.FindViewById(Resource.Id.AniListActivity_ReplyContainer);
                ReplyCountContainer = view.FindViewById(Resource.Id.AniListActivity_ReplyCountContainer);
                ReplyCount = view.FindViewById<TextView>(Resource.Id.AniListActivity_ReplyCount);
                ReplyLikeContainer = view.FindViewById(Resource.Id.AniListActivity_ReplyLikeContainer);
                LikeCount = view.FindViewById<TextView>(Resource.Id.AniListActivity_LikeCount);
                LikeIcon = view.FindViewById<ImageView>(Resource.Id.AniListActivity_LikeIcon);
                ReplyButton = view.FindViewById(Resource.Id.AniListActivity_ReplyButton);
            }
        }
    }
}