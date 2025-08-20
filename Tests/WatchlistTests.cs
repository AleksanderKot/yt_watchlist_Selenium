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
            var login        = Configuration["YouTube:Login"]    ?? throw new InvalidOperationException("Missing YouTube:Login");
            var password     = Configuration["YouTube:Password"] ?? throw new InvalidOperationException("Missing YouTube:Password");
            var channelsFile = Configuration["Paths:ChannelsFile"] ?? throw new InvalidOperationException("Missing Paths:ChannelsFile");

            var channelsPath = Path.IsPathRooted(channelsFile)
                ? channelsFile
                : Path.Combine(AppContext.BaseDirectory, channelsFile);

            var loginPage = new LoginPage(Driver);
            loginPage.NavigateHome();
            loginPage.AcceptCookies();
            loginPage.ClickSignIn();
            loginPage.LoginWithGoogle(login, password);

            var watchlist = new WatchlistPage(Driver);
            watchlist.OpenWatchLater();

            var channels = File.ReadAllLines(channelsPath)
                   .Select(l => (l ?? string.Empty).Trim())
                   .Where(l => !string.IsNullOrWhiteSpace(l))
                   .Distinct(StringComparer.OrdinalIgnoreCase)
                   .ToList();

            TestContext.WriteLine($"Channels to delete: {string.Join(", ", channels)}");

            foreach (var ch in channels)
            {
                TestContext.WriteLine($"Deleting: {ch}");
                watchlist.DeleteAllFromChannel(ch);
            }
        }
    }
}
