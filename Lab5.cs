using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab5
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            // можно None, но Eager безопаснее для препода

            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }

        [Test]
        public void Test13_ModalDialogs()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Alerts, Frame & Windows']")));
            driver.FindElement(By.XPath("//h5[text()='Alerts, Frame & Windows']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Modal Dialogs']")));
            driver.FindElement(By.XPath("//span[text()='Modal Dialogs']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("showSmallModal")));
            driver.FindElement(By.Id("showSmallModal")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-content")));
            Assert.That(driver.FindElement(By.ClassName("modal-title")).Text.Contains("Small Modal"), Is.True);

            string modalText = driver.FindElement(By.ClassName("modal-body")).Text;
            Assert.That(modalText.Contains("small modal"), Is.True);

            driver.FindElement(By.Id("closeSmallModal")).Click();
        }

        [Test]
        public void Test14_Accordian()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Widgets']")));
            driver.FindElement(By.XPath("//h5[text()='Widgets']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Accordian']")));
            driver.FindElement(By.XPath("//span[text()='Accordian']")).Click();

            Thread.Sleep(1000);
            
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("section1Heading")));
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("section1Content")));
            Assert.That(driver.FindElement(By.Id("section1Content")).Text.Contains("Lorem Ipsum"), Is.True);

            IWebElement section2Heading = driver.FindElement(By.Id("section2Heading"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", section2Heading);
            Thread.Sleep(500);
            section2Heading.Click();

            wait.Until(driver =>
            {
                var text = driver.FindElement(By.Id("section2Content")).Text;
                return !string.IsNullOrWhiteSpace(text);
            });

            Assert.That(driver.FindElement(By.Id("section2Content")).Text.Length > 0, Is.True);

            IWebElement section3Heading = driver.FindElement(By.Id("section3Heading"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", section3Heading);
            Thread.Sleep(500);
            section3Heading.Click();

            wait.Until(driver =>
            {
                var text = driver.FindElement(By.Id("section3Content")).Text;
                return !string.IsNullOrWhiteSpace(text);
            });

            Assert.That(driver.FindElement(By.Id("section3Content")).Text.Length > 0, Is.True);
        }

        [Test]
        public void Test15_AutoComplete()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Widgets']")));
            driver.FindElement(By.XPath("//h5[text()='Widgets']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Auto Complete']")));
            driver.FindElement(By.XPath("//span[text()='Auto Complete']")).Click();

            Thread.Sleep(1000);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("autoCompleteMultipleInput")));
            IWebElement multiInput = driver.FindElement(By.Id("autoCompleteMultipleInput"));
            
            multiInput.Click();
            multiInput.SendKeys("Bl");
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Black']")));
            driver.FindElement(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Black']")).Click();
            Thread.Sleep(500);
            
            multiInput = driver.FindElement(By.Id("autoCompleteMultipleInput"));
            multiInput.Click();
            multiInput.SendKeys("Re");
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Red']")));
            driver.FindElement(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Red']")).Click();
            Thread.Sleep(500);
            
            multiInput = driver.FindElement(By.Id("autoCompleteMultipleInput"));
            multiInput.Click();
            multiInput.SendKeys("Ma");
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Magenta']")));
            driver.FindElement(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Magenta']")).Click();
            Thread.Sleep(500);

            IWebElement singleInput = driver.FindElement(By.Id("autoCompleteSingleInput"));
            singleInput.Click();
            singleInput.SendKeys("Bl");
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Black']")));
            driver.FindElement(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Black']")).Click();
            Thread.Sleep(500);

            singleInput = driver.FindElement(By.Id("autoCompleteSingleInput"));
            singleInput.Click();
            singleInput.SendKeys(Keys.Control + "a");
            singleInput.SendKeys(Keys.Delete);
            Thread.Sleep(300);
            singleInput.SendKeys("Re");
            Thread.Sleep(500);
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Red']")));
            driver.FindElement(By.XPath("//div[contains(@class,'auto-complete__option') and text()='Red']")).Click();
        }
    }
}