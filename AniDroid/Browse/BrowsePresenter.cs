﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Widget;
using AniDroid.AniList.Dto;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.AniListObject.Media;
using AniDroid.Base;
using AniDroid.Utils.Interfaces;
using AniDroid.Utils.Logging;

namespace AniDroid.Browse
{
    public class BrowsePresenter : BaseAniDroidPresenter<IBrowseView>, IAniListMediaListEditPresenter, IBrowsePresenter
    {
        private BrowseMediaDto _browseDto;

        public BrowsePresenter(IAniListService service, IAniDroidSettings settings,
            IAniDroidLogger logger) : base(service, settings, logger)
        {
        }

        public void BrowseAniListMedia(BrowseMediaDto browseDto)
        {
            _browseDto = browseDto;
            View.ShowMediaSearchResults(AniListService.BrowseMedia(browseDto, 20));
        }

        public override Task Init()
        {
            // TODO: does this need anything?
            return Task.CompletedTask;
        }

        public bool GetIsUserAuthenticated()
        {
            return AniDroidSettings.IsUserAuthenticated;
        }

        public User GetAuthenticatedUser()
        {
            return AniDroidSettings.LoggedInUser;
        }

        public async Task SaveMediaListEntry(MediaListEditDto editDto, Action onSuccess, Action onError)
        {
            var mediaUpdateResp = await AniListService.UpdateMediaListEntry(editDto, default);

            mediaUpdateResp.Switch(mediaList =>
            {
                onSuccess();
                View.DisplaySnackbarMessage("Saved", Snackbar.LengthShort);
                View.UpdateMediaListItem(mediaList);
            }).Switch(error => onError());
        }

        public async Task DeleteMediaListEntry(int mediaListId, Action onSuccess, Action onError)
        {
            var mediaDeleteResp = await AniListService.DeleteMediaListEntry(mediaListId, default);

            mediaDeleteResp.Switch((bool success) =>
            {
                onSuccess();
                View.DisplaySnackbarMessage("Deleted", Snackbar.LengthShort);
                View.RemoveMediaListItem(mediaListId);
            }).Switch(error =>
                onError());
        }

        public BrowseMediaDto GetBrowseDto()
        {
            return _browseDto;
        }

        public IList<Media.MediaTag> GetMediaTags()
        {
            return AniDroidSettings.MediaTagCache;
        }

        public IList<string> GetGenres()
        {
            return AniDroidSettings.GenreCache;
        }
    }
}