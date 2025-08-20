using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using yt_watchlist_Selenium.Utils;

namespace yt_watchlist_Selenium.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        public LoginPage(IWebDriver driver) => _driver = driver;

        public void NavigateHome()
        {
            _driver.Navigate().GoToUrl("https://www.youtube.com");
        }

        private By AcceptCookiesButton => By.XPath("//button[contains(., 'Accept all') or contains(., 'Zgadzam się')]");

        public void AcceptCookies()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                var button = wait.Until(ExpectedConditions.ElementToBeClickable(AcceptCookiesButton));
                button.Click();
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Cookies popup not shown.");
            }
        }

        public void ClickSignIn()
        {
            try
            {
                var signIn = new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                    .Until(ExpectedConditions.ElementToBeClickable(
                        By.XPath("//yt-button-renderer[@id='sign-in-button']")));
                signIn.Click();
            }
            catch
            {
                var anySignIn = _driver.FindElements(By.CssSelector("a[href*='ServiceLogin']"));
                if (anySignIn.Count > 0) anySignIn[0].Click();
                else throw new Exception("Sign in button not found");
            }
        }

        public void LoginWithGoogle(string email, string password)
        {
            var emailInput = Wait.WaitVisible(_driver, By.Id("identifierId"));
            emailInput.Clear();
            emailInput.SendKeys(email);
            _driver.FindElement(By.Id("identifierNext")).Click();

            var passwordInput = new WebDriverWait(_driver, TimeSpan.FromSeconds(20))
                .Until(ExpectedConditions.ElementIsVisible(By.CssSelector("input[type='password']")));
            passwordInput.Clear();
            passwordInput.SendKeys(password);
            _driver.FindElement(By.Id("passwordNext")).Click();

            new WebDriverWait(_driver, TimeSpan.FromSeconds(10))
                .Until(ExpectedConditions.ElementExists(By.CssSelector("ytd-app")));
        }
    }
}