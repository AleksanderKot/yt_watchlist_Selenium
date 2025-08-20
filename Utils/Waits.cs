using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace yt_watchlist_Selenium.Utils
{
    public static class Wait
    {
        public static IWebElement WaitVisible(IWebDriver driver, By by, double seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public static IWebElement WaitClickable(IWebDriver driver, By by, double seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(ExpectedConditions.ElementIsVisible(by));
        }

        public static bool WaitDisappear(IWebDriver driver, By by, double seconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
            return wait.Until(d =>
            {
                try
                {
                    var el = d.FindElement(by);
                    return !el.Displayed;
                }
                catch (NoSuchElementException) { return true; }
                catch (StaleElementReferenceException) { return true; }
            });
        }
    }
}