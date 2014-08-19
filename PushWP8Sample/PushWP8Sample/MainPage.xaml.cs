using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Shell;
using MSSPush_Base.Models;
using MSSPush_WP8;
using MSSPush_WP8.Models;
using PushWP8Sample.Resources;

namespace PushWP8Sample
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Constants

        // Set to your "Variant UUID", as provided by the Pivotal Mobile Services Suite console
        private const string VariantUuid = "bd9060e3-77cc-4e68-ab1a-572cb0f8a5f2";

        // Set to your "Variant Secret" as provided by the Pivotal Mobile Services Suite console
        private const string VariantSecret = "83c775f8-cbca-40b3-b1d9-b805dadd43c8";

        // Set to your instance of the Pivotal Mobile Services Suite server providing your push services.
        private const string BaseServerUrl = "http://cfms-push-service-dev.main.vchs.cfms-apps.com";

        // Set to your own defined alias for this device.  May be null.
        private const string DeviceAlias = "SampleDeviceAlias";

        // Set to your own defined tags. May be null.
        private static readonly List<string> Tags = new List<string> { "SampleTag1", "SampleTag2"};

        #endregion

        #region Properties

        private MSSParameters _parameters;
        private MSSParameters Parameters
        {
            get
            {
                return _parameters ?? (_parameters = new MSSParameters(VariantUuid, VariantSecret, BaseServerUrl, DeviceAlias, Tags));
            }
        }

        public ObservableCollection<string> Logs
        {
            get { return (ObservableCollection<string>)GetValue(LogsProperty); }
            set { SetValue(LogsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Logs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogsProperty =
            DependencyProperty.Register("Logs", typeof(ObservableCollection<string>), typeof(MainPage), new PropertyMetadata(null));

        #endregion

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Logs = new ObservableCollection<string>();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            QueuePushLog("Registering...");
            StartPushRegistration();
        }

        #region WP8 Push Client SDK Methods

        private async void StartPushRegistration()
        {
            //Example of how to register for Push with the Pivotal Mobile Services Suite Push Server
            try
            {
                await MSSPush.SharedInstance.RegisterForPushAsync(Parameters, OnRegistrationCompleted);
            }
            catch (Exception e)
            {
                QueuePushLog("Registration verification failed.");
                QueuePushLog(e.ToString());
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
                QueuePushLog("Unregistration verification failed.");
                QueuePushLog(e.ToString());
            }
        }

        private void QueuePushLog(string message)
        {
            Dispatcher.BeginInvoke(() => Logs.Add(message));
        }

        #endregion

        #region Completion Actions

        private void OnRegistrationCompleted(HttpCompletionArgs args)
        {
            if (args.Succeeded)
            {
                QueuePushLog("Successfully registered for Push");

                //e.g. HttpNotificationChannel can be accessed from the args for additional use
                //var channel = args.RawNotificationChannel;
                //channel.BindToShellToast();
                //channel.ShellToastNotificationReceived += ChannelOnShellToastNotificationReceived;

                //e.g. Unregister for Push after successfully registering
                QueuePushLog("Unregistering...");
                StartPushUnregistration();
            }
            else
            {
                QueuePushLog("Failed to register for Push");
                QueuePushLog(args.ErrorMessage);
            }
        }

        private void OnUnregistrationCompleted(HttpCompletionArgs args)
        {
            if (args.Succeeded)
            {
                QueuePushLog("Successfully unregistered for Push");
            }
            else
            {
                QueuePushLog("Failed to unregister for Push");
                QueuePushLog(args.ErrorMessage);
            }
        }

        #endregion
    }
}