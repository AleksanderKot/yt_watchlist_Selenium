using System;
using System.IO;
using System.Text.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace yt_watchlist_Selenium.Utils
{
    public class TestBase
    {
        protected IWebDriver Driver = null!;
        protected dynamic Config = null!;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var json = File.ReadAllText("appsettings.json");
            Config = JsonSerializer.Deserialize<dynamic>(json);
        }

        [SetUp]
        public void SetUp()
        {
            var headless = (bool?)Config?["Browser"]?["Headless"] ?? true;

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
            try
            {
                Driver.Quit();
                Driver.Dispose();
            }
            catch (Exception)
            {
                /* Ignore */
            }
        }
    }
}