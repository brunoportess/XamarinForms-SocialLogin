using Android.App;
using Org.Json;
using System;
using System.Collections.Generic;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(SocialLogin.Droid.Helpers.FacebookManager))]
namespace SocialLogin.Droid.Helpers
{
    [Android.Runtime.Preserve(AllMembers = true)]
    public class FacebookManager : Java.Lang.Object, SocialLogin.Helpers.IFacebookManager, IFacebookCallback, GraphRequest.IGraphJSONObjectCallback
    {
        public Action<Models.FacebookUser, string> _onLoginComplete;
        public ICallbackManager _callbackManager;

        public FacebookManager()
        {
            _callbackManager = CallbackManagerFactory.Create();
            LoginManager.Instance.RegisterCallback(_callbackManager, this);
        }

        public void Login(Action<Models.FacebookUser, string> onLoginComplete)
        {
            _onLoginComplete = onLoginComplete;
            LoginManager.Instance.SetLoginBehavior(LoginBehavior.NativeWithFallback);
            var context = Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity;
            LoginManager.Instance.LogInWithReadPermissions(context as Activity, new List<string> { "public_profile", "email" });
        }

        public void Logout()
        {
            LoginManager.Instance.LogOut();
        }

        #region IFacebookCallback
        public void OnSuccess(Java.Lang.Object result)
        {
            if (result is LoginResult n)
            {
                var request = GraphRequest.NewMeRequest(n.AccessToken, this);
                var bundle = new Bundle();
                //var bundle = new Android.Content.Intent(this);
                bundle.PutString("fields", "id, first_name, email, last_name, picture.width(500).height(500)");
                request.Parameters = bundle;
                request.ExecuteAsync();
            }
        }

        public void OnCancel()
        {
            _onLoginComplete?.Invoke(null, "Canceled!");
        }

        public void OnError(FacebookException error)
        {
            _onLoginComplete?.Invoke(null, error.Message);
        }
        public void OnCompleted(JSONObject p0, GraphResponse p1)
        {
            var id = string.Empty;
            var first_name = string.Empty;
            var email = string.Empty;
            var last_name = string.Empty;
            var pictureUrl = string.Empty;

            if (p0.Has("id"))
                id = p0.GetString("id");

            if (p0.Has("first_name"))
                first_name = p0.GetString("first_name");

            if (p0.Has("email"))
                email = p0.GetString("email");

            if (p0.Has("last_name"))
                last_name = p0.GetString("last_name");

            if (p0.Has("picture"))
            {
                var p2 = p0.GetJSONObject("picture");
                if (p2.Has("data"))
                {
                    var p3 = p2.GetJSONObject("data");
                    if (p3.Has("url"))
                    {
                        pictureUrl = p3.GetString("url");
                    }
                }
            }

            _onLoginComplete?.Invoke(new Models.FacebookUser(id, AccessToken.CurrentAccessToken.Token, first_name, first_name, email, pictureUrl), string.Empty);
        }
        #endregion
    }
}