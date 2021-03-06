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

namespace AniDroid.Login
{
    public interface ILoginView : IAniDroidView
    {
        string GetAuthCode();
        void OnErrorAuthorizing();
        void OnAuthorized();
    }
}