
using Xamarin.Forms;

namespace SocialLogin.Views
{
    public class MDPage : MasterDetailPage
    {
        public MDPage()
        {
            Detail = new NavigationPage(new MainPage());
            Master = new MainPage();
        }
        
    }
}