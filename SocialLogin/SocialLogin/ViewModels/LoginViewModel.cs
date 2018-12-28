using SocialLogin.Helpers;
using SocialLogin.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace SocialLogin.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginFacebookCommand { get; set; }
        public Command LoginGoogleCommand { get; set; }

        public LoginViewModel()
        {
            LoginFacebookCommand = new Command(ExecuteLoginFacebookCommand);
            LoginGoogleCommand = new Command(ExecuteLoginGoogleCommand);
        }

        private void ExecuteLoginFacebookCommand()
        {
            DependencyService.Get<IFacebookManager>().Login(OnLoginCompleteFacebook);
        }
        private void ExecuteLoginGoogleCommand()
        {
            DependencyService.Get<IGoogleManager>().Login(OnLoginCompleteGoogle);
        }

        private async void OnLoginCompleteGoogle(GoogleUser googleUser, string message)
        {
            if (googleUser != null)
            {
                try
                {
                    //aqui você coloca o codigo que invoca sua app de registro e cadastrar o usuario com os dados do google
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    var p = new Object[2];
                    p[0] = googleUser;
                    p[1] = null;
                    await Navigation.PushAsync<UserViewModel>(false, p);
                }
            }
            else
            {
                await DisplayAlert("Error", message, "Ok");
            }
        }

        private async void OnLoginCompleteFacebook(FacebookUser facebookUser, string message)
        {
            if (facebookUser != null)
            {
                try
                {
                    //aqui você coloca o codigo que invoca sua app de registro e cadastrar o usuario com os dados do facebook
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
                finally
                {
                    var p = new Object[2];
                    p[0] = null;
                    p[1] = facebookUser;
                    await Navigation.PushAsync<UserViewModel>(false, p);
                }
            }
            else
            {
                await DisplayAlert("Error", message, "Ok");
            }
        }
    }
}
