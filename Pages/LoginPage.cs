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

        public void AcceptCookies()
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

                var btn = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id='content']/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button")));
                btn.Click();
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine("Cookies popup not shown.");
            }
            catch (ElementClickInterceptedException)
            {
                var btn = _driver.FindElement(By.XPath("//*[@id='content']/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
            }
        }

        public void ClickSignIn()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            try
            {
                var signIn = wait.Until(ExpectedConditions.ElementToBeClickable(
                    By.XPath("//*[@id='buttons']/ytd-button-renderer/yt-button-shape/a")
                ));
                signIn.Click();
            }
            catch (WebDriverTimeoutException)
            {
                // If you delete THIS comment line the script won't work (idk why) 
                var alt = _driver.FindElements(By.XPath("//a[@aria-label='Sign in' and contains(@href,'ServiceLogin')]"));
                if (alt.Count > 0)
                {
                    alt[0].Click();
                }
                else
                {
                    throw new Exception("Sign in button not found on the page.");
                }
            }
            catch (StaleElementReferenceException)
            {
                var retry = _driver.FindElements(By.XPath("//*[@id='buttons']/ytd-button-renderer/yt-button-shape/a"));
                if (retry.Count > 0)
                {
                    retry[0].Click();
                }
                else
                {
                    throw new Exception("Sign in button not found after retry.");
                }
            }
        }

        public void LoginWithGoogle(string email, string password)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));

            var emailInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id='identifierId']")));
            emailInput.Clear();
            emailInput.SendKeys(email);

            _driver.FindElement(By.XPath("//*[@id='identifierNext']/div/button")).Click();

            var passInput = wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//*[@id='password']/div[1]/div/div[1]/input")));
            passInput.Clear();
            passInput.SendKeys(password);

            _driver.FindElement(By.XPath("//*[@id='passwordNext']/div/button")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//ytd-app")));
        }
    }
}
