﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AniDroid.AniList.Interfaces;

namespace AniDroid.Utils.Integration
{
    internal class AniDroidAniListServiceConfig : IAniListServiceConfig
    {
        public AniDroidAniListServiceConfig(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
    }
}