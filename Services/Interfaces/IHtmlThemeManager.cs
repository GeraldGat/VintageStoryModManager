namespace VintageStoryModManager.Services.Interfaces
{
    public interface IHtmlThemeManager
    {
        public string GetHtmlThemeCss();
        public string WrapInTemplate(string html, bool httpsForYoutubeLinks = true);
    }
}
