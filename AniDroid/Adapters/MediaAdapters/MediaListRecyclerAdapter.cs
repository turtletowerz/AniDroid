﻿using Android.Content.Res;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using AniDroid.Adapters.Base;
using AniDroid.AniList.Models;
using AniDroid.AniListObject.Media;
using AniDroid.Base;
using AniDroid.Dialogs;
using AniDroid.MediaList;
using System;
using System.Linq;

namespace AniDroid.Adapters.MediaAdapters
{
    public class MediaListRecyclerAdapter : BaseRecyclerAdapter<Media.MediaList>
    {
        private readonly MediaListPresenter _presenter;
        private readonly User.UserMediaListOptions _mediaListOptions;
        private readonly bool _isCustomList;
        private readonly string _listName;
        private readonly Media.MediaListStatus _listStatus;
        private readonly MediaListItemViewType _viewType;
        private readonly bool _highlightPriorityItems;
        private readonly bool _displayProgressColors;
        private readonly ColorStateList _priorityBackgroundColor;
        private readonly ColorStateList _upToDateTitleColor;
        private readonly ColorStateList _behindTitleColor;

        public MediaListRecyclerAdapter(BaseAniDroidActivity context, Media.MediaListGroup mediaListGroup,
            User.UserMediaListOptions mediaListOptions, MediaListPresenter presenter, RecyclerCardType cardType,
            MediaListItemViewType viewType, bool highlightPriorityItems, bool displayProgressColors, int verticalCardColumns = 2) : base(context, mediaListGroup.Entries,
            cardType, verticalCardColumns)
        {
            _presenter = presenter;
            _mediaListOptions = mediaListOptions;
            _isCustomList = mediaListGroup.IsCustomList;
            _listName = mediaListGroup.Name;
            _listStatus = mediaListGroup.Status;
            _viewType = viewType;
            _highlightPriorityItems = highlightPriorityItems;
            _displayProgressColors = displayProgressColors;
            _priorityBackgroundColor =
                ColorStateList.ValueOf(new Color(Context.GetThemedColor(Resource.Attribute.ListItem_Priority)));
            _upToDateTitleColor =
                ColorStateList.ValueOf(new Color(Context.GetThemedColor(Resource.Attribute.ListItem_UpToDate)));
            _behindTitleColor =
                ColorStateList.ValueOf(new Color(Context.GetThemedColor(Resource.Attribute.ListItem_Behind)));

            if (_viewType != MediaListItemViewType.Normal)
            {
                CardType = RecyclerCardType.Custom;
                CustomCardUseItemDecoration = true;
            }
        }

        public void UpdateMediaListItem(int mediaId, Media.MediaList updatedMediaList)
        {
            var position = Items.FindIndex(x => x.Media.Id == mediaId);

            if (position >= 0)
            {
                if (updatedMediaList.HiddenFromStatusLists && !_isCustomList ||
                    _isCustomList && !updatedMediaList.CustomLists.Any(x => x.Enabled && x.Name == _listName) ||
                    !_isCustomList && _listStatus != updatedMediaList.Status)
                {
                    Items.RemoveAt(position);
                    NotifyDataSetChanged();
                }
                else
                {
                    var oldMedia = Items[position].Media;
                    Items[position] = updatedMediaList;
                    Items[position].Media = oldMedia;
                    NotifyItemChanged(position);
                }
            }
            else if (_isCustomList && updatedMediaList.CustomLists.Any(x => x.Enabled && x.Name == _listName) ||
                     !_isCustomList && _listStatus == updatedMediaList.Status)
            {
                Items.Insert(0, updatedMediaList);
                NotifyDataSetChanged();
            }

        }

        public void ResetMediaListItem(int mediaId)
        {
            var position = Items.FindIndex(x => x.Media.Id == mediaId);
            NotifyItemChanged(position);
        }

        public override void BindCardViewHolder(CardItem holder, int position)
        {
            var item = Items[position];

            holder.Name.Text = item.Media.Title.UserPreferred;
            holder.DetailPrimary.Text = GetDetailOne(item);
            holder.DetailSecondary.Text = GetDetailTwo(item);
            holder.Button.SetTag(Resource.Id.Object_Position, position);
            Context.LoadImage(holder.Image, item.Media.CoverImage.Large);
            holder.ContainerCard.CardBackgroundColor = _highlightPriorityItems && item.Priority > 0 ? _priorityBackgroundColor : holder.DefaultBackgroundColor;

            holder.ContainerCard.SetTag(Resource.Id.Object_Position, position);
            holder.ContainerCard.Click -= RowClick;
            holder.ContainerCard.Click += RowClick;
            holder.ContainerCard.LongClick -= RowLongClick;
            holder.ContainerCard.LongClick += RowLongClick;

            if (_displayProgressColors && item.Media.Type == Media.MediaType.Anime && item.Status == Media.MediaListStatus.Current && item.Media.Status == Media.MediaStatus.Releasing && item.Media.NextAiringEpisode?.Episode  > 0)
            {
                holder.Name.SetTextColor(item.Media.NextAiringEpisode.Episode - 1 <= item.Progress
                    ? _upToDateTitleColor
                    : _behindTitleColor);
            }
            else
            {
                holder.Name.SetTextColor(holder.DefaultNameColor);
            }

            if (item.Status != Media.MediaListStatus.Current || _viewType == MediaListItemViewType.TitleOnly)
            {
                holder.Button.Visibility = ViewStates.Gone;
            }
            else
            {
                holder.Button.Enabled = true;
                //holder.Button.Rotation = 0;
                //holder.Button.ScaleX = holder.Button.ScaleY = 1;
                //holder.Button.Alpha = 1;
                //holder.Button.Animation?.Cancel();
                //holder.Button.ClearAnimation();
                holder.Button.Visibility = ViewStates.Visible;

                holder.ButtonIcon.SetImageResource(item.Progress + 1 >= (item.Media.Episodes ?? item.Media.Chapters)
                    ? Resource.Drawable.svg_check_circle_outline
                    : Resource.Drawable.svg_plus_circle_outline);
            }
        }

        public override CardItem SetupCardItemViewHolder(CardItem item)
        {
            item.Button.Click -= ButtonClick;
            item.Button.Click += ButtonClick;

            return item;
        }

        private void RowClick(object sender, EventArgs e)
        {
            var senderView = sender as View;
            var mediaListPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaList = Items[mediaListPos];

            MediaActivity.StartActivity(Context, mediaList.Media.Id, BaseAniDroidActivity.ObjectBrowseRequestCode);
        }

        private void RowLongClick(object sender, View.LongClickEventArgs longClickEventArgs)
        {
            var senderView = sender as View;
            var mediaListPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaList = Items[mediaListPos];

            EditMediaListItemDialog.Create(Context, _presenter, mediaList.Media, mediaList, _mediaListOptions);
        }

        private async void ButtonClick(object sender, EventArgs eventArgs)
        {
            var senderView = sender as View;
            var iconView = senderView.FindViewById(Resource.Id.CardItem_ButtonIcon);
            var mediaListPos = (int)senderView.GetTag(Resource.Id.Object_Position);
            var mediaList = Items[mediaListPos];

            if (mediaList.Progress + 1 == (mediaList.Media.Episodes ?? mediaList.Media.Chapters))
            {
                senderView.Enabled = false;
                iconView?.StartAnimation(AnimationUtils.LoadAnimation(Context,
                    Resource.Animation.Button_Animation_FinishProgress));
                await _presenter.CompleteMedia(mediaList);
            }
            else
            {
                senderView.Enabled = false;
                iconView?.StartAnimation(AnimationUtils.LoadAnimation(Context,
                    Resource.Animation.Button_Animation_AddProgress));

                await _presenter.IncreaseMediaProgress(mediaList);
            }
        }

        private string GetDetailOne(Media.MediaList mediaList)
        {
            if (mediaList.Status == Media.MediaListStatus.Planning)
            {
                if (mediaList.Media.Type == Media.MediaType.Anime)
                {
                    return $"{((mediaList.Media.Episodes ?? 0) > 0 ? mediaList.Media.Episodes?.ToString() : "?")} episode(s)";
                }

                if (mediaList.Media.Type == Media.MediaType.Manga)
                {
                    return $"{((mediaList.Media.Chapters ?? 0) > 0 ? mediaList.Media.Chapters?.ToString() : "?")} chapter(s)";
                }
            }

            if (mediaList.Status == Media.MediaListStatus.Current || mediaList.Status == Media.MediaListStatus.Paused || mediaList.Status == Media.MediaListStatus.Dropped || mediaList.Status == Media.MediaListStatus.Repeating)
            {
                if (mediaList.Progress.HasValue && mediaList.Progress > 0 &&
                    mediaList.Progress == (mediaList.Media.Episodes ?? mediaList.Media.Chapters))
                {
                    return "Status needs to be marked as Completed";
                }

                if (mediaList.Media.Type == Media.MediaType.Anime)
                {
                    return $"Watched {mediaList.Progress ?? 0} out of {((mediaList.Media.Episodes ?? 0) > 0 ? mediaList.Media.Episodes?.ToString() : "?")}";
                }

                if (mediaList.Media.Type == Media.MediaType.Manga)
                {
                    return $"Read {mediaList.Progress ?? 0} out of {((mediaList.Media.Chapters ?? 0) > 0 ? mediaList.Media.Chapters?.ToString() : "?")}";
                }
            }

            return mediaList.GetScoreString(_mediaListOptions.ScoreFormat);
        }

        private string GetDetailTwo(Media.MediaList mediaList)
        {
            return $"{mediaList.Media.Format?.DisplayValue}{(mediaList.Media.IsAdult ? " (Hentai)" : "")}";
        }

        public enum MediaListItemViewType
        {
            Normal = 0,
            Compact = 1,
            TitleOnly = 2
        }

        public override RecyclerView.ViewHolder CreateCustomViewHolder(ViewGroup parent, int viewType)
        {
            if (_viewType == MediaListItemViewType.Compact)
            {
                return SetupCardItemViewHolder(new CardItem(Context.LayoutInflater.Inflate(Resource.Layout.View_CardItem_FlatHorizontalCompact,
                    parent, false)));
            }
            if (_viewType == MediaListItemViewType.TitleOnly)
            {
                return SetupCardItemViewHolder(new CardItem(Context.LayoutInflater.Inflate(Resource.Layout.View_CardItem_FlatHorizontalTitleOnly,
                    parent, false)));
            }

            return base.CreateCustomViewHolder(parent, viewType);
        }

        public override void BindCustomViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            BindCardViewHolder(holder as CardItem, position);
        }
    }
}