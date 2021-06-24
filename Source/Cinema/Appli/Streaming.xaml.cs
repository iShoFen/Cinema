using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;
using Modele;

namespace Appli
{
    /// <summary>
    /// Logique d'interaction pour Streaming.xaml
    /// </summary>
    public partial class Streaming : UserControl
    {
        private static MainWindow Windows => (Application.Current as App)?.MainWindow as MainWindow;
        private static Manager Man => (Application.Current as App)?.Man;

        private int _nb = 1;

        private bool FullScreen
        {
            set
            {
                if (value)
                {
                    Windows.ContentControl.Content = null;
                    Windows.Window.Content = this;
                    Windows.IgnoreTaskbarOnMaximize = true;
                    Windows.WindowState = WindowState.Maximized;
                    Windows.ShowTitleBar = false;
                    Windows.ShowCloseButton = false;
                    Windows.ShowMinButton = false;
                    Windows.ShowMaxRestoreButton = false;
                }
                else
                {
                    Windows.WindowState = WindowState.Normal;
                    Windows.IgnoreTaskbarOnMaximize = false;
                    Windows.ShowTitleBar = true;
                    Windows.ShowCloseButton = true;
                    Windows.ShowMinButton = true;
                    Windows.ShowMaxRestoreButton = true;
                    Windows.Window.Content = Windows.Principal;
                    Windows.ContentControl.Content = this;
                }
            }
        }

        public Streaming()
        {
            InitializeComponent();
            (Application.Current as App)!.Web = Web;
            DataContext = Man.CurrentStream;
        }

        private void Web_OnCoreWebView2InitializationCompleted(object sender,
            CoreWebView2InitializationCompletedEventArgs e)
        {
            if (Web.CoreWebView2 is null) return;

            Web.CoreWebView2.ContainsFullScreenElementChanged += (_, _) => 
            { 
                FullScreen = Web.CoreWebView2.ContainsFullScreenElement;
            };
        }

        private void Web_OnContentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            switch (Web.Source.AbsoluteUri)
            {
                case "https://www.netflix.com/browse" when _nb == 1:
                    _nb++;
                    break;

                case "https://www.netflix.com/browse":
                    Web.Source = new Uri(Man.CurrentStream);
                    break;

                case "https://www.disneyplus.com/eu/fr-fr/home":
                    Web.Source = new Uri(Man.CurrentStream);
                    break;

                case "https://go.ocs.fr/?externalId=h_nEiDYDmM3EAQ6cXAjsu5GYkpouKeOGv9wn2hnVmCY": 
                    Thread.Sleep(1000);
                    Web.Source = new Uri(Man.CurrentStream);
                    break;

                case "https://www.primevideo.com/storefront/ref=atv_pr_sw_sc?language=fr_FR&switchSuccess=1":
                    Web.Source = new Uri(Man.CurrentStream);
                    break;

            }
        }
    }
}
