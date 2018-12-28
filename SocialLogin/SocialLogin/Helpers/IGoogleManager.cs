using System;

namespace SocialLogin.Helpers
{
    public interface IGoogleManager
    {
        void Login(Action<Models.GoogleUser, string> OnLoginComplete);
        void Logout();
    }
}
