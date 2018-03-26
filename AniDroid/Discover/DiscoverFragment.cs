﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using AniDroid.Adapters.MediaAdapters;
using AniDroid.AniList.Interfaces;
using AniDroid.AniList.Models;
using AniDroid.Base;
using AniDroid.Utils;
using AniDroid.Utils.Interfaces;
using Ninject;
using OneOf;

namespace AniDroid.Discover
{
    public class DiscoverFragment : BaseMainActivityFragment<DiscoverPresenter>, IDiscoverView
    {
        private DiscoverMediaRecyclerAdapter _trendingRecyclerAdapter;
        private DiscoverMediaRecyclerAdapter _newAnimeRecyclerAdapter;
        private DiscoverMediaRecyclerAdapter _newMangaRecyclerAdapter;
        private static DiscoverFragment _instance;

        public override bool HasMenu => true;
        public override string FragmentName => "DISCOVER_FRAGMENT";
        protected override IReadOnlyKernel Kernel => new StandardKernel(new ApplicationModule<IDiscoverView, DiscoverFragment>(this));

        protected override void SetInstance(BaseMainActivityFragment instance)
        {
            _instance = instance as DiscoverFragment;
        }

        public override void ClearState()
        {
            _instance = null;
        }

        public override View CreateMainActivityFragmentView(ViewGroup container, Bundle savedInstanceState)
        {
            CreatePresenter(savedInstanceState).GetAwaiter().GetResult();

            return LayoutInflater.Inflate(Resource.Layout.Fragment_Discover, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            Presenter.GetDiscoverLists();
        }

        public override void SetupMenu(IMenu menu)
        {
            menu.Clear();
            var inflater = new MenuInflater(Context);
            inflater.Inflate(Resource.Menu.Discover_ActionBar, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.Menu_Discover_Refresh)
            {
                _trendingRecyclerAdapter.ResetAdapter();
                _newAnimeRecyclerAdapter.ResetAdapter();
                _newMangaRecyclerAdapter.ResetAdapter();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnError(IAniListError error)
        {
            throw new NotImplementedException();
        }

        public void ShowTrendingResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.Discover_TrendingRecyclerView);
            recycler.SetAdapter(_trendingRecyclerAdapter = new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }

        public void ShowNewAnimeResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.Discover_NewAnimeRecyclerView);
            recycler.SetAdapter(_newAnimeRecyclerAdapter = new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }

        public void ShowNewMangaResults(IAsyncEnumerable<OneOf<IPagedData<Media>, IAniListError>> mediaEnumerable)
        {
            var recycler = View.FindViewById<RecyclerView>(Resource.Id.Discover_NewMangaRecyclerView);
            recycler.SetAdapter(_newMangaRecyclerAdapter = new DiscoverMediaRecyclerAdapter(Activity, mediaEnumerable));
        }
    }
}