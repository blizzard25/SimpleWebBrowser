using Android.App;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using System;

namespace WebBrowser
{
    [Activity(Label = "WebBrowser", MainLauncher = true, Icon = "@drawable/icon", Theme = "@android:style/Theme.NoTitleBar")]
    public class WebBrowserActivity : Activity
    {
        private WebView webView;
        private EditText txtUrl;
        private Button backButton;
        private Button forwardButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);

            webView = FindViewById<WebView>(Resource.Id.webView1);
            txtUrl = FindViewById<EditText>(Resource.Id.editText1);
            backButton = FindViewById<Button>(Resource.Id.btnBack);
            forwardButton = FindViewById<Button>(Resource.Id.btnForward);

            backButton.Text = "<";
            forwardButton.Text = ">";
            txtUrl.SetWidth(Resources.DisplayMetrics.WidthPixels - 160);

            backButton.Click += (sender, ea) =>
            {
                if (webView.CanGoBack())
                {
                    webView.GoBack();
                    txtUrl.Text = webView.Url.ToString();
                }
            };
            forwardButton.Click += (sender, ea) =>
            {
                if (webView.CanGoForward())
                {
                    webView.GoForward();
                    txtUrl.Text = webView.Url.ToString();
                }
            };
            txtUrl.EditorAction += (sender, ea) =>
            {
                if (ea.ActionId == Android.Views.InputMethods.ImeAction.Done)
                {
                    try
                    {
                        var currentInput = txtUrl.Text;
                        if (!IsValidUrl(currentInput))
                        {
                            try
                            {
                                currentInput = new UriBuilder(currentInput).Uri.ToString();
                            }
                            catch(Exception ex)
                            {
                                return;
                            }
                            webView.LoadUrl(currentInput);
                        }
                    }
                    catch (Exception ex)
                    {
                        // TODO : Display message that there was an error loading webpage
                    }
                }
            };

            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new WebBrowserWebClient());
            webView.LoadUrl("http://www.google.com");
        }

        private bool IsValidUrl(string uriName)
        {
            Uri uriResult;
            return Uri.TryCreate(uriName, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public class WebBrowserWebClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView view, string url)
            {
                view.LoadUrl(url);
                return true;
            }
        }
    }
}

