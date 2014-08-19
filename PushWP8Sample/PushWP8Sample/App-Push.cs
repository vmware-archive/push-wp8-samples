using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSSPush_Base.Models;
using MSSPush_WP8;
using MSSPush_WP8.Models;

namespace PushWP8Sample
{
    /// <summary>
    /// This partial class shows an example of how to Register/Unregister for Push with the 
    /// Pivotal Mobile Services Suite Push Server using the WP8 Push Client SDK.
    /// </summary>
    public partial class App
    {
        private MSSParameters _parameters;

        private MSSParameters Parameters
        {
            get
            {
                return _parameters ?? (_parameters = new MSSParameters(
                                                        "bd9060e3-77cc-4e68-ab1a-572cb0f8a5f2",                 /* VariantUuid */
                                                        "83c775f8-cbca-40b3-b1d9-b805dadd43c8",                 /* VariantSecret */
                                                        "http://cfms-push-service-dev.main.vchs.cfms-apps.com", /* BaseServerUrl */
                                                        "SampleDevice",                                         /* DeviceAlias */
                                                        new List<string> { "Tag1", "Tag2" })                    /* Tags */);
            }
        }

        private async void StartPushRegistration()
        {
            //Example of how to register for Push with the Pivotal Mobile Services Suite Push Server
            try
            {
                await MSSPush.SharedInstance.RegisterForPushAsync(Parameters, OnRegistrationCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine("Registration verification failed.");
            }
        }

        private async void StartPushUnregistration()
        {
            //Example of how to unregister for Push with the Pivotal Mobile Services Suite Push Server
            try
            {
                await MSSPush.SharedInstance.UnregisterForPushAsync(Parameters, OnUnregistrationCompleted);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine("Unregistration verification failed.");
            }
        }

        #region Completion Actions

        private void OnRegistrationCompleted(HttpCompletionArgs args)
        {
            if (args.Succeeded)
            {
                Debug.WriteLine("Successfully registered for Push");

                //HttpNotificationChannel can be accessed from the args for additional use
                //var channel = args.RawNotificationChannel;
                //channel.BindToShellToast();

                //e.g. Unregister for Push after successfully registering
                StartPushUnregistration();
            }
            else
            {
                Debug.WriteLine("Failed to register for Push");
                Debug.WriteLine(args.ErrorMessage);
            }
        }

        private void OnUnregistrationCompleted(HttpCompletionArgs args)
        {
            if (args.Succeeded)
            {
                Debug.WriteLine("Successfully unregistered for Push");
            }
            else
            {
                Debug.WriteLine("Failed to unregister for Push");
                Debug.WriteLine(args.ErrorMessage);
            }
        }

        #endregion
    }
}
