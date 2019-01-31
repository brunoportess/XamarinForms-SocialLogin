using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook.LoginKit;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(SocialLogin.iOS.Helpers.FacebookManager))]
namespace SocialLogin.iOS.Helpers
{
    /// <summary>
    /// INFORMAÇÕES PARA A CRIAÇÃO DAS CONTAS NO SITE:
    /// https://causerexception.com/2017/10/21/facebook-native-login-with-xamarin-forms/?fbclid=IwAR0DjwMpKMm85h9GiV179wmX3Zi8XaR-Y1TVbfwzPmDoGoxGcvWSBXsLlbc
    /// </summary>
    class FacebookManager : SocialLogin.Helpers.IFacebookManager
    {
        public Action<Models.FacebookUser, string> _onLoginComplete;

        public void Login(Action<Models.FacebookUser, string> onLoginComplete)
        {
            _onLoginComplete = onLoginComplete;
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            var tcs = new TaskCompletionSource<Models.FacebookUser>();
            LoginManager manager = new LoginManager();
            manager.LogOut();
            manager.LoginBehavior = LoginBehavior.SystemAccount;
            manager.LogInWithReadPermissions(new string[] { "public_profile", "email" }, vc, (result, error) =>
            {
                if (error != null || result == null || result.IsCancelled)
                {
                    if (error != null)
                        _onLoginComplete?.Invoke(null, error.LocalizedDescription);
                    if (result.IsCancelled)
                        _onLoginComplete?.Invoke(null, "User Cancelled!");

                    tcs.TrySetResult(null);
                }
                else
                {
                    var request = new Facebook.CoreKit.GraphRequest("me", new NSDictionary("fields", "id, first_name, email, last_name, picture.width(1000).height(1000)"));
                    request.Start((connection, result1, error1) =>
                    {
                        if (error1 != null || result1 == null)
                        {
                            tcs.TrySetResult(null);
                        }
                        else
                        {
                            var id = string.Empty;
                            var first_name = string.Empty;
                            var email = string.Empty;
                            var last_name = string.Empty;
                            var url = string.Empty;

                            try
                            {
                                id = result1.ValueForKey(new NSString("id"))?.ToString();
                            }
                            catch (Exception e)
                            {
                            }

                            try
                            {
                                first_name = result1.ValueForKey(new NSString("first_name"))?.ToString();
                            }
                            catch (Exception e)
                            {
                            }

                            try
                            {
                                email = result1.ValueForKey(new NSString("email"))?.ToString();
                            }
                            catch (Exception e)
                            {
                            }

                            try
                            {
                                last_name = result1.ValueForKey(new NSString("last_name"))?.ToString();
                            }
                            catch (Exception e)
                            {
                            }

                            try
                            {
                                url = ((result1.ValueForKey(new NSString("picture")) as NSDictionary).ValueForKey(new NSString("data")) as NSDictionary).ValueForKey(new NSString("url")).ToString();
                            }
                            catch (Exception e)
                            {
                            }
                            if (tcs != null)
                            {
                                tcs.TrySetResult(new Models.FacebookUser(id, result.Token.TokenString, first_name, last_name, email, url));
                                _onLoginComplete?.Invoke(new Models.FacebookUser(id, result.Token.TokenString, first_name, last_name, email, url), string.Empty);
                            }
                        }
                    });
                }
            });
        }

        public void Logout()
        {
            LoginManager manager = new LoginManager();
            manager.LogOut();
        }
    }
}