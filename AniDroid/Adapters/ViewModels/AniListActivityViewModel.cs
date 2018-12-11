using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AniDroid.AniList.Models;
using AniDroid.AniListObject;
using AniDroid.AniListObject.Media;
using AniDroid.AniListObject.User;
using AniDroid.Base;
using AniDroid.Dialogs;

namespace AniDroid.Adapters.ViewModels
{
    public class AniListActivityViewModel : AniDroidAdapterViewModel<AniListActivity>
    {
        public ISpanned FormattedTitle { get; protected set; }
        public ISpanned FormattedBody { get; protected set; }
        public string Timestamp { get; protected set; }
        public string LikeCount { get; protected set; }
        public string ReplyCount { get; protected set; }
        public bool HasUserLiked { get; protected set; }
        public Action ClickAction { get; protected set; }
        public Action<int, IAniListActivityPresenter> LongClickAction { get; protected set; }

        public ViewStates ReplyCountVisibility => Model.ReplyCount > 0 ? ViewStates.Visible : ViewStates.Gone;
        public ViewStates BodyViewState => FormattedBody != null ? ViewStates.Visible : ViewStates.Gone;

        public AniListActivityViewModel(AniListActivity model, BaseAniDroidActivity context, int? currentUserId, Color userNameColor, Color accentColor) : base(model)
        {
            Timestamp = Model.GetAgeString(Model.CreatedAt);
            LikeCount = Model.Likes?.Count.ToString() ?? "0";
            ReplyCount = Model.ReplyCount.ToString();

            if (Model.Type?.Equals(AniListActivity.ActivityType.Text) == true)
            {
                FormattedTitle =
                    BaseAniDroidActivity.FromHtml(
                        $"<b><font color='#{userNameColor & 0xffffff:X6}'>{Model.User?.Name}</font></b>");
                FormattedBody = BaseAniDroidActivity.FromHtml(Model.Text);
                ImageUri = Model.User?.Avatar?.Large;
                ClickAction = () => UserActivity.StartActivity(context, Model.User?.Id ?? 0);

                if (currentUserId.HasValue && currentUserId == Model.User?.Id)
                {
                    LongClickAction = (position, presenter) =>
                        AniListActivityCreateDialog.CreateEditActivity(context, Model.Text,
                            text => presenter.EditStatusActivityAsync(Model, position, text),
                            () => presenter.DeleteActivityAsync(Model.Id, position));
                }
            }
            else if (Model.Type?.Equals(AniListActivity.ActivityType.Message) == true)
            {
                FormattedTitle =
                    BaseAniDroidActivity.FromHtml(
                        $"<b><font color='#{userNameColor & 0xffffff:X6}'>{Model.Messenger?.Name}</font></b>");
                FormattedBody = BaseAniDroidActivity.FromHtml(Model.Message);
                ImageUri = Model.Messenger?.Avatar?.Large;
                ClickAction = () => UserActivity.StartActivity(context, Model.Messenger?.Id ?? 0);
            }
            else if (Model.Type?.EqualsAny(AniListActivity.ActivityType.AnimeList,
                         AniListActivity.ActivityType.MangaList) == true)
            {
                FormattedTitle = BaseAniDroidActivity.FromHtml(
                    $"<b><font color='#{userNameColor & 0xffffff:X6}'>{Model.User?.Name}</font></b> {Model.Status} {(!string.IsNullOrWhiteSpace(Model.Progress) ? $"{Model.Progress} of" : "")} <b><font color='#{accentColor & 0xffffff:X6}'>{Model.Media?.Title?.UserPreferred}</font></b>");
                ImageUri = Model.Media?.CoverImage?.Large ?? Model.Media?.CoverImage?.Medium;
                ClickAction = () => MediaActivity.StartActivity(context, Model.Media?.Id ?? 0);
            }
        }

        public static AniListActivityViewModel CreateViewModel(AniListActivity model, BaseAniDroidActivity context,
            int? currentUserId, Color userNameColor, Color accentColor)
        {
            return new AniListActivityViewModel(model, context, currentUserId, userNameColor, accentColor);
        }
    }
}