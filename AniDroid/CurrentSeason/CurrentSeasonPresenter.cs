using System;
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
using AniDroid.AniList.Dto;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.Base;
using AniDroid.Utils.Interfaces;

namespace AniDroid.CurrentSeason
{
    public class CurrentSeasonPresenter : BaseAniDroidPresenter<ICurrentSeasonView>
    {
        private readonly Media.MediaSort _sortType;
        private readonly Media.MediaSeason _currentSeason;
        private readonly int _currentSeasonYear;
        private readonly Media.MediaSeason _previousSeason;
        private readonly int _previousSeasonYear;

        public CurrentSeasonPresenter(ICurrentSeasonView view, IAniListService service, IAniDroidSettings settings) : base(view, service, settings)
        {
            var titleLanguage = settings?.LoggedInUser?.Options?.TitleLanguage ??
                             AniList.Models.AniListObject.AniListTitleLanguage.English;

            if (titleLanguage.Equals(AniList.Models.AniListObject.AniListTitleLanguage.Native) ||
                titleLanguage.Equals(AniList.Models.AniListObject.AniListTitleLanguage.NativeStylised))
            {
                _sortType = Media.MediaSort.TitleEnglish;
            }
            else if (titleLanguage.Equals(AniList.Models.AniListObject.AniListTitleLanguage.Romaji) ||
                     titleLanguage.Equals(AniList.Models.AniListObject.AniListTitleLanguage.RomajiStylised))
            {
                _sortType = Media.MediaSort.TitleRomaji;
            }
            else
            {
                _sortType = Media.MediaSort.TitleEnglish;
            }
        }

        public override Task Init()
        {
            return Task.CompletedTask;
        }

        public void GetCurrentSeasonLists()
        {
            View.ShowCurrentTv(AniListService.BrowseMedia(new BrowseMediaDto
            {
                Season = _currentSeason,
                SeasonYear = _currentSeasonYear,
                Type = Media.MediaType.Anime,
                Format = Media.MediaFormat.Tv,
                Sort = new List<Media.MediaSort> { _sortType }
            }, 5));

            View.ShowCurrentLeftovers(AniListService.BrowseMedia(new BrowseMediaDto
            {
                Season = _previousSeason,
                SeasonYear = _previousSeasonYear,
                Type = Media.MediaType.Anime,
                Format = Media.MediaFormat.Tv,
                EpisodesGreaterThan = 16,
                Sort = new List<Media.MediaSort> { _sortType }
            }, 5));
        }

        private Tuple<Media.MediaSeason, int> CalculateSeason(DateTime date)
        {
            var currentSeason = Media.MediaSeason.Winter;

            if (date.Month > 1 && date.Month < 5)
            {
                currentSeason = Media.MediaSeason.Spring;
            }
            else if (date.Month > 4 && date.Month < 8)
            {
                currentSeason = Media.MediaSeason.Summer;
            }
            else if (date.Month > 7 && date.Month < 11)
            {
                currentSeason = Media.MediaSeason.Fall;
            }

            return new Tuple<Media.MediaSeason, int>(currentSeason, date.Year);
        }

        private Tuple<Media.MediaSeason, int> CalculatePreviousSeason(Tuple<Media.MediaSeason, int> currentSeason)
        {
            var previousYear = currentSeason.Item2;
            var previousSeason = currentSeason.Item1;

            //if (previousSeason == )
        }
    }
}