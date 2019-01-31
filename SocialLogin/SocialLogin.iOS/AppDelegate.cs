using System;
using System.Collections.Generic;
using System.Linq;
using Facebook.CoreKit;
using Foundation;
using Google.SignIn;
using UIKit;

namespace SocialLogin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            // ****************************************************************
            // * TODO: Alterar info.plist, adicionar GoogleService-Info.plist *
            // ****************************************************************
            var googleServiceDictionary = NSDictionary.FromFile("GoogleService-Info.plist");
            SignIn.SharedInstance.ClientID = googleServiceDictionary["CLIENT_ID"].ToString();

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            var openUrlOptions = new UIApplicationOpenUrlOptions(options);
            return SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            AppEvents.ActivateApp();
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            //return base.OpenUrl(application, url, sourceApplication, annotation);
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }
    }
}
