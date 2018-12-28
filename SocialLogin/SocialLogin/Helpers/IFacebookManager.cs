using System;

namespace SocialLogin.Helpers
{
    public interface IFacebookManager
    {
        void Login(Action<Models.FacebookUser, string> onLoginComplete);

        void Logout();
    }
}
