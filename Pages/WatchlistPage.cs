using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace yt_watchlist_Selenium.Pages
{
    public class WatchlistPage
    {
        private readonly IWebDriver _driver;
        public WatchlistPage(IWebDriver driver) => _driver = driver;

        private By PlaylistVideo => By.CssSelector("ytd-playlist-video-renderer");
        private By MenuItems => By.CssSelector("ytd-menu-service-item-renderer tp-yt-paper-item");
        private By VideoMenuButtonWithin(IWebElement video) => By.CssSelector("div#menu button#button");

        public void OpenWatchLater()
        {
            _driver.Navigate().GoToUrl("https://www.youtube.com/playlist?list=WL");
            new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(ExpectedConditions.ElementExists(PlaylistVideo));
        }

        private string? GetChannelName(IWebElement video)
        {
            try
            {
                var channel = video.FindElement(By.CssSelector("div#byline-container a[href*='/@']"));
                return channel.Text?.Trim();
            }
            catch { return null; }
        }

        public void DeleteAllFromChannel(string channelName)
        {
            for (int guard = 0; guard < 300; guard++)
            {
                var videos = _driver.FindElements(PlaylistVideo).ToList();
                var foundAny = false;

                foreach (var v in videos)
                {
                    var name = GetChannelName(v);
                    if (string.Equals(name, channelName, StringComparison.OrdinalIgnoreCase))
                    {
                        DeleteSingle(v);
                        foundAny = true;
                        break;
                    }
                }

                if (!foundAny) break;
            }
        }

        private void DeleteSingle(IWebElement video)
        {
            new Actions(_driver).MoveToElement(video).Perform();

            var menuBtn = video.FindElement(VideoMenuButtonWithin(video));
            menuBtn.Click();

            var items = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(MenuItems));

            var target = items.FirstOrDefault(i =>
            {
                var t = i.Text?.Trim();
                return !string.IsNullOrWhiteSpace(t) &&
                       (t.Contains("Usuń z", StringComparison.OrdinalIgnoreCase) ||
                        t.Contains("Remove from", StringComparison.OrdinalIgnoreCase));
            });

            if (target == null)
                throw new InvalidOperationException("Nie znaleziono przycisku do usunięcia z listy.");

            target.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            wait.Until(_ =>
            {
                try
                {
                    var _x = video.Displayed;
                    return false;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            });
        }
    }
}
