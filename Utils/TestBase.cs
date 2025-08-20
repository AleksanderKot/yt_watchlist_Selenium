using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Microsoft.Extensions.Configuration;

namespace yt_watchlist_Selenium.Utils
{
    public class TestBase
    {
        protected IWebDriver Driver = null!;
        protected IConfiguration Configuration = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
        }

        [SetUp]
        public void SetUp()
        {
            var headless = Configuration.GetValue<bool>("Browser:Headless");

            var options = new ChromeOptions();
            if (headless) options.AddArgument("--headless=new");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--disable-infobars");

            Driver = new ChromeDriver(options);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
        }

        [TearDown]
        public void TearDown()
        {
            try { Driver.Quit(); Driver.Dispose(); } catch { }
        }
    }
}
