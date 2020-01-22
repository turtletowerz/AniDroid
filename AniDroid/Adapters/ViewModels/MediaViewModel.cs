﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AniDroid.AniList.Models;

namespace AniDroid.Adapters.ViewModels
{
    public class MediaViewModel : AniDroidAdapterViewModel<Media>
    {
        public Media.Edge ModelEdge { get; protected set; }
        public Color ImageColor { get; protected set; }

        private MediaViewModel(Media model, MediaDetailType primaryMediaDetailType, MediaDetailType secondaryMediaDetailType, bool isButtonVisible) : base(model)
        {
            TitleText = Model.Title?.UserPreferred;
            DetailPrimaryText = GetDetail(primaryMediaDetailType);
            DetailSecondaryText = GetDetail(secondaryMediaDetailType);
            ImageUri = model.CoverImage?.Large ?? model.CoverImage?.Medium;
            IsButtonVisible = isButtonVisible;
            ImageColor = Color.ParseColor(model.CoverImage?.Color ?? "#00000000");
        }

        public enum MediaDetailType
        {
            None,
            Format,
            FormatRating,
            Genres,
            UserScore,
            ListStatus,
            ListStatusThenGenres
        }

        public static MediaViewModel CreateMediaViewModel(Media model)
        {
            return new MediaViewModel(model, MediaDetailType.FormatRating, MediaDetailType.ListStatusThenGenres, model.IsFavourite);
        }

        private string GetDetail(MediaDetailType mediaDetailType)
        {
            var retString = mediaDetailType switch
            {
                MediaDetailType.Format => $"{Model.Format?.DisplayValue}{(Model.IsAdult ? " (Hentai)" : "")}",
                MediaDetailType.FormatRating => (
                    Model.Status?.EqualsAny(Media.MediaStatus.NotYetReleased, Media.MediaStatus.Cancelled) == true
                        ? Model.Format?.DisplayValue
                        : $"{Model.Format?.DisplayValue}  ({(Model.AverageScore != 0 ? $"{Model.AverageScore}%" : "No Rating Data")})"
                ),
                MediaDetailType.Genres => (Model.Genres?.Any() == true
                    ? string.Join(", ", Model.Genres)
                    : "(No Genres)"),
                MediaDetailType.ListStatusThenGenres => (Model.MediaListEntry?.Status != null
                    ? $"On List: {Model.MediaListEntry?.Status}"
                    : GetDetail(MediaDetailType.Genres)),
                _ => null
            };

            return retString;
        }
    }
}