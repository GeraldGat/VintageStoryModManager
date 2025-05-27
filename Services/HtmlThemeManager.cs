using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using VintageStoryModManager.Services.Interfaces;

namespace VintageStoryModManager.Services
{
    internal class HtmlThemeManager : IHtmlThemeManager
    {
        private string GetColor(string key)
        {
            if (Application.Current.Resources[key] is SolidColorBrush brush)
                return $"rgba({brush.Color.R}, {brush.Color.G}, {brush.Color.B}, {(brush.Color.A / 255.0).ToString("F2", CultureInfo.InvariantCulture)})";
            return "#000000";
        }
        public string GetHtmlThemeCss()
        {
            return $@"
            body {{
                background-color: {GetColor("WindowBackgroundColor")};
                color: {GetColor("ForegroundColor")};
                font-family: 'Roboto', sans-serif;
                font-size: 14px;
            }}

            a {{
                color: {GetColor("AccentColor")};
                text-decoration: underline;
            }}
            a:hover {{
                color: {GetColor("ForegroundColor")};
            }}

            button {{
                background-color: {GetColor("ButtonBackground")};
                color: {GetColor("ForegroundColor")};
                border: 1px solid {GetColor("BorderColor")};
                padding: 6px 12px;
                font-size: 22px;
                font-weight: bold;
                border-radius: 6px;
                cursor: pointer;
                height: 30px;
            }}
            button:hover {{
                background-color: {GetColor("ButtonHoverBackground")};
            }}
            button:active {{
                background-color: {GetColor("ButtonPressedBackground")};
            }}
            button:disabled {{
                opacity: 0.5;
                cursor: default;
            }}

            .card {{
                background-color: {GetColor("CardBackgroundColor")};
                padding: 20px;
                border-radius: 8px;
                border: 1px solid {GetColor("BorderColor")};
                margin-bottom: 20px;
            }}

            .tab-item:hover {{
                background-color: {GetColor("HoverColor")};
            }}
            .tab-item.active {{
                border-color: {GetColor("AccentColor")};
                background-color: {GetColor("ButtonHoverBackground")};
                font-weight: bold;
            }}

            .spoiler {{
                background-color: {GetColor("CardBackgroundColor")};
                border: 1px solid {GetColor("BorderColor")};
                padding: 10px;
                border-radius: 6px;
                transition: color 0.3s ease;
                cursor: pointer;
            }}
            .spoiler-text {{
                display: none;
            }}
            .spoiler.revealed .spoiler-text {{
                display: block;
            }}
            ";
        }

        public string WrapInTemplate(string html, bool httpsForYoutubeLinks = true)
        {
            string fullHtml = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Vintage Story Mod Manager</title>
                <style>
                    {GetHtmlThemeCss()}
                </style>
            </head>
            <body>
                {html}
            </body>
            <script>
                document.addEventListener('DOMContentLoaded', function () {{
                    const spoilers = document.querySelectorAll('.spoiler');

                    spoilers.forEach(spoiler => {{
                        spoiler.addEventListener('click', function () {{
                            spoiler.classList.toggle('revealed');
                        }});
                    }});
                }});
            </script>
            <script>
                function sendHeight() {{
                    const height = document.body.scrollHeight;
                    window.chrome.webview.postMessage({{ type: 'height', height: height }});
                }}

                window.addEventListener('load', sendHeight);
                window.addEventListener('resize', sendHeight);
                new ResizeObserver(sendHeight).observe(document.body);

                function sendHeight() {{
                    const height = document.body.scrollHeight;
                    window.chrome.webview.postMessage({{ type: ""height"", height: height }});
                }}

                window.addEventListener('load', sendHeight);
                window.addEventListener('resize', sendHeight);
                new ResizeObserver(sendHeight).observe(document.body);

                window.addEventListener('wheel', function(event) {{
                    window.chrome.webview.postMessage({{
                        type: ""scroll"",
                        direction: event.deltaY > 0 ? ""down"" : ""up""
                    }});
                }});
            </script>
            </html>";

            if(httpsForYoutubeLinks)
            {
                fullHtml = fullHtml.Replace("\"//www.youtube.com", "\"https://www.youtube.com");
                fullHtml = fullHtml.Replace("\"//youtube.com", "\"https://www.youtube.com");
                fullHtml = fullHtml.Replace("\"//youtu.be", "\"https://www.youtube.com");
                fullHtml = fullHtml.Replace("http://www.youtube.com", "https://www.youtube.com");
                fullHtml = fullHtml.Replace("http://youtube.com", "https://www.youtube.com");
                fullHtml = fullHtml.Replace("http://youtu.be", "https://www.youtube.com");
            }

            return fullHtml;
        }
    }
}
