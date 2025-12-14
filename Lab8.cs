using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab8
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            options.AddUserProfilePreference("profile.default_content_settings.popups", 0);
            
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.Manage().Window.Maximize();
            
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }

        private void SafeClick(By locator)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "var banner = document.getElementById('fixedban'); " +
                    "if(banner) banner.remove();"
                );
            }
            catch { }

            var element = wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
            System.Threading.Thread.Sleep(500);
            element.Click();
        }

        [Test]
        public void Test22_BookStoreLogin()
        {
            int retries = 3;
            bool pageLoaded = false;
            
            for (int i = 0; i < retries && !pageLoaded; i++)
            {
                try
                {
                    Console.WriteLine($"Попытка {i + 1} загрузить страницу...");
                    driver.Navigate().GoToUrl("https://demoqa.com/");
                    wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
                    pageLoaded = true;
                    Console.WriteLine("Страница загружена успешно");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Попытка {i + 1} не удалась: {ex.Message}");
                    if (i == retries - 1) throw;
                    System.Threading.Thread.Sleep(5000);
                }
            }

            SafeClick(By.XPath("//h5[text()='Book Store Application']"));

            SafeClick(By.XPath("//span[text()='Login']"));

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("userName")));
            driver.FindElement(By.Id("userName")).SendKeys("testuser");

            driver.FindElement(By.Id("password")).SendKeys("testpass");

            driver.FindElement(By.Id("login")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("name")));
            Assert.That(driver.FindElement(By.Id("name")).Text.Contains("Invalid username or password!"), Is.True);

            driver.FindElement(By.Id("newUser")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("firstname")));
            driver.FindElement(By.Id("firstname")).SendKeys("John");

            driver.FindElement(By.Id("lastname")).SendKeys("Doe");

            driver.FindElement(By.Id("userName")).SendKeys("johndoe123");

            driver.FindElement(By.Id("password")).SendKeys("1");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.Id("register")));
            driver.FindElement(By.Id("register")).Click();

            System.Threading.Thread.Sleep(2000);
            
            bool recaptchaMessageFound = false;
            try
            {
                var message = driver.FindElement(By.Id("recaptcha-verify-message"));
                recaptchaMessageFound = message.Displayed;
            }
            catch
            {
                recaptchaMessageFound = driver.PageSource.Contains("Please verify reCaptcha") ||
                                       driver.PageSource.Contains("reCAPTCHA") ||
                                       driver.PageSource.Contains("captcha");
            }
            
            if (!recaptchaMessageFound)
            {
                Assert.That(!driver.Url.Contains("profile"), Is.True, "Регистрация не должна пройти с неправильным паролем");
            }

            try
            {
                var recaptchaFrame = driver.FindElements(By.CssSelector("iframe[title*='reCAPTCHA']"));
                if (recaptchaFrame.Count > 0)
                {
                    Console.WriteLine("reCAPTCHA обнаружена на странице");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"reCAPTCHA не найдена: {ex.Message}");
            }
            finally
            {
                try { driver.SwitchTo().DefaultContent(); } catch { }
            }

            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("weak");
            driver.FindElement(By.Id("register")).Click();
            
            System.Threading.Thread.Sleep(1000);
            
            driver.FindElement(By.Id("password")).Clear();
            driver.FindElement(By.Id("password")).SendKeys("Password@123");

            try
            {
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = driver.SwitchTo().Alert();
                alert.Accept();
            }
            catch (Exception)
            {
            }

        }
    }
}