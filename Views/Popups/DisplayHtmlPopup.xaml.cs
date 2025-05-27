using Microsoft.Web.WebView2.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace VintageStoryModManager.Views.Popups
{
    public partial class DisplayHtmlPopup : Window
    {
        public DisplayHtmlPopup()
        {
            InitializeComponent();
        }

        public async void LoadHtmlRichText(string html)
        {
            await HtmlViewer.EnsureCoreWebView2Async(null);
            HtmlViewer.CoreWebView2.Settings.IsScriptEnabled = true;
            HtmlViewer.CoreWebView2.Settings.IsWebMessageEnabled = true;
            HtmlViewer.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;

            HtmlViewer.WebMessageReceived += HtmlViewer_WebMessageReceived;

            HtmlViewer.NavigateToString(html);
        }

        private void HtmlViewer_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            try
            {
                var json = e.WebMessageAsJson;
                var data = System.Text.Json.JsonDocument.Parse(json);

                if (data.RootElement.TryGetProperty("type", out var typeElement))
                {
                    string type = typeElement.GetString() ?? "";

                    if (type == "height" && data.RootElement.TryGetProperty("height", out var heightElement))
                    {
                        int height = heightElement.GetInt32() + 50;
                        HtmlViewer.Height = height;
                    }
                    else if (type == "scroll" && data.RootElement.TryGetProperty("direction", out var directionElement))
                    {
                        string direction = directionElement.GetString() ?? "down";
                        var scrollViewer = FindParentScrollViewer(HtmlViewer);
                        if (scrollViewer != null)
                        {
                            double offset = direction == "down"
                                ? scrollViewer.VerticalOffset + 50
                                : scrollViewer.VerticalOffset - 50;

                            scrollViewer.ScrollToVerticalOffset(offset);
                        }
                    }
                }
            }
            catch {}
        }

        private ScrollViewer? FindParentScrollViewer(DependencyObject? current)
        {
            while (current != null)
            {
                if (current is ScrollViewer sv)
                    return sv;
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
