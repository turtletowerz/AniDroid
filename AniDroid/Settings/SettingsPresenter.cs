﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.Base;
using AniDroid.Adapters.MediaAdapters;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.Base;
using AniDroid.Utils.Comparers;
using AniDroid.Utils.Interfaces;

namespace AniDroid.Settings
{
    public class SettingsPresenter : BaseAniDroidPresenter<ISettingsView>
    {
        public SettingsPresenter(ISettingsView view, IAniListService service, IAniDroidSettings settings) : base(view, service, settings)
        {
        }

        public override Task Init()
        {
            View.CreateCardTypeSettingItem(AniDroidSettings.CardType);
            View.CreateAniDroidThemeSettingItem(AniDroidSettings.Theme);
            View.CreateDisplayBannersSettingItem(AniDroidSettings.DisplayBanners);

            if (AniDroidSettings.IsUserAuthenticated)
            {
                View.CreateGroupCompletedSettingItem(AniDroidSettings.GroupCompletedLists);
                View.CreateMediaListViewTypeSettingItem(AniDroidSettings.MediaViewType);
                View.CreateAnimeListSortItem(AniDroidSettings.AnimeListSortType,
                    AniDroidSettings.AnimeListSortDirection);
                View.CreateMangaListSortItem(AniDroidSettings.MangaListSortType,
                    AniDroidSettings.MangaListSortDirection);
                View.CreateHighlightPriorityMediaListItemsItem(AniDroidSettings.HighlightPriorityMediaListItems);
                View.CreateDisplayProgressColorsItem(AniDroidSettings.DisplayMediaListItemProgressColors);
            }

            View.CreateWhatsNewSettingItem();

            return Task.CompletedTask;
        }

        public override async Task RestoreState(IList<string> savedState)
        {
            await Init();
        }

        public void SetCardType(BaseRecyclerAdapter.RecyclerCardType cardType)
        {
            AniDroidSettings.CardType = cardType;
        }

        public void SetTheme(BaseAniDroidActivity.AniDroidTheme theme)
        {
            AniDroidSettings.Theme = theme;
        }

        public void SetDisplayBanners(bool displayBanners)
        {
            AniDroidSettings.DisplayBanners = displayBanners;
        }

        // Auth Settings

        public void SetGroupCompleted(bool groupCompleted)
        {
            AniDroidSettings.GroupCompletedLists = groupCompleted;
        }

        public void SetMediaListViewType(MediaListRecyclerAdapter.MediaListItemViewType viewType)
        {
            AniDroidSettings.MediaViewType = viewType;
        }

        public void SetHighlightPriorityMediaListItems(bool highlightListItems)
        {
            AniDroidSettings.HighlightPriorityMediaListItems = highlightListItems;
        }

        public void SetDisplayProgressColorsItem(bool displayProgressColors)
        {
            AniDroidSettings.DisplayMediaListItemProgressColors = displayProgressColors;
        }

        public void SetAnimeListSort(MediaListSortComparer.MediaListSortType sort,
            MediaListSortComparer.MediaListSortDirection direction)
        {
            AniDroidSettings.AnimeListSortType = sort;
            AniDroidSettings.AnimeListSortDirection = direction;
        }

        public void SetMangaListSort(MediaListSortComparer.MediaListSortType sort,
            MediaListSortComparer.MediaListSortDirection direction)
        {
            AniDroidSettings.MangaListSortType = sort;
            AniDroidSettings.MangaListSortDirection = direction;
        }
    }
}