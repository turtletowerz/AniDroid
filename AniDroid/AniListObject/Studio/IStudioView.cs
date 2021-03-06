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
using AniDroid.Base;

namespace AniDroid.AniListObject.Studio
{
    public interface IStudioView : IAniListObjectView
    {
        int GetStudioId();
        void SetupStudioView(AniList.Models.Studio studio);
    }
}