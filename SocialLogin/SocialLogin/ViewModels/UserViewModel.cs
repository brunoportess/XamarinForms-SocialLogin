using SocialLogin.Models;
using System.Threading.Tasks;

namespace SocialLogin.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private string title = "User Information";

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }


        private GoogleUser googleUser;
        public GoogleUser GUser
        {
            get { return googleUser; }
            set { SetProperty(ref googleUser, value); }
        }

        private FacebookUser facebookUser;
        public FacebookUser FUser
        {
            get { return facebookUser; }
            set { SetProperty(ref facebookUser, value); }
        }

        private bool _fuVisible = false;
        public bool FUVisible
        {
            get { return _fuVisible; }
            set { SetProperty(ref _fuVisible, value); }
        }

        private bool _guVisible = false;
        public bool GUVisible
        {
            get { return _guVisible; }
            set { SetProperty(ref _guVisible, value); }
        }



        public UserViewModel()
        {

        }

        public override Task InitializeAsync(object[] args)
        {
            if (args != null && args.Length > 0)
            {
                if((GoogleUser)args[0] != null)
                {
                    GUser = (GoogleUser)args[0];
                    GUVisible = true;
                    
                }
                if ((FacebookUser)args[1] != null)
                {
                    FUser = (FacebookUser)args[1];
                    FUVisible = true;
                }
            }
            

            return base.InitializeAsync(args);
        }
    }
}
