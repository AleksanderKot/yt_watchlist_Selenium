// Tests/WatchlistTests.cs
using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using yt_watchlist_Selenium.Pages;
using yt_watchlist_Selenium.Utils;

namespace yt_watchlist_Selenium.Tests
{
    [TestFixture]
    public class WatchlistTests : TestBase
    {
        [Test]
        public void Clean_WatchLater_By_Channels_From_File()
        {
            var login = (string) Config!["YouTube"]!["Login"]!;
            var password = (string) Config!["YouTube"]!["Password"]!;
            var channelsFile = (string) Config!["Paths"]!["ChannelsFile"]!;

            var loginPage = new LoginPage(Driver);
            loginPage.NavigateHome();
            loginPage.AcceptCookies();
            loginPage.ClickSignIn();
            loginPage.LoginWithGoogle(login, password);

            var watchlist = new WatchlistPage(Driver);
            watchlist.OpenWatchLater();

            var channels = File.ReadAllLines(channelsFile)
                               .Select(l => (l ?? string.Empty).Trim())
                               .Where(l => !string.IsNullOrWhiteSpace(l))
                               .Distinct(StringComparer.OrdinalIgnoreCase)
                               .ToList();

            TestContext.WriteLine($"Kanały do wyczyszczenia: {string.Join(", ", channels)}");

            foreach (var ch in channels)
            {
                TestContext.WriteLine($"Czyszczę kanał: {ch}");
                watchlist.DeleteAllFromChannel(ch);
            }
        }
    }
}
