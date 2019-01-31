using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using Google.SignIn;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(SocialLogin.iOS.Helpers.GoogleManager))]
namespace SocialLogin.iOS.Helpers
{
    /// <summary>
    /// INFORMAÇÕES PARA A CRIAÇÃO DAS CONTAS NO SITE:
    /// https://causerexception.com/2017/12/03/google-native-login-with-xamarin-forms/?fbclid=IwAR08oocqiAETJvZa0sJ7kmMnTwhU5nfiP2Vj6_AmuehyOOYz8aQHLgbsRes
    /// </summary>
    public class GoogleManager : NSObject, SocialLogin.Helpers.IGoogleManager, ISignInDelegate, ISignInUIDelegate
    {
        private Action<SocialLogin.Models.GoogleUser, string> _onLoginComplete;
        private UIViewController _viewController { get; set; }

        public GoogleManager()
        {
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;
        }

        public void Login(Action<SocialLogin.Models.GoogleUser, string> OnLoginComplete)
        {
            _onLoginComplete = OnLoginComplete;

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            _viewController = vc;

            SignIn.SharedInstance.SignInUser();
        }

        public void Logout()
        {
            SignIn.SharedInstance.SignOutUser();
        }

        public void DidSignIn(SignIn signIn, Google.SignIn.GoogleUser user, NSError error)
        {
            if (user != null && error == null)
                _onLoginComplete?.Invoke(new SocialLogin.Models.GoogleUser()
                {
                    Name = user.Profile.Name,
                    Email = user.Profile.Email,
                    Picture = user.Profile.HasImage ? new Uri(user.Profile.GetImageUrl(500).ToString()) : new Uri(string.Empty)
                }, string.Empty);
            else
                _onLoginComplete?.Invoke(null, error.LocalizedDescription);
        }

        [Export("signIn:didDisconnectWithUser:withError:")]
        public void DidDisconnect(SignIn signIn, GoogleUser user, NSError error)
        {
            // Perform any operations when the user disconnects from app here.
        }

        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {
            //myActivityIndicator.StopAnimating();
        }

        [Export("signIn:presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            _viewController?.PresentViewController(viewController, true, null);
        }

        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            _viewController?.DismissViewController(true, null);
        }
    }
}
