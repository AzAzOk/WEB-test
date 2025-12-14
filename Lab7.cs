using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab7
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
        public void Test19_SelectMenu()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            SafeClick(By.XPath("//h5[text()='Widgets']"));

            SafeClick(By.XPath("//span[text()='Select Menu']"));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("withOptGroup")));
            driver.FindElement(By.Id("withOptGroup")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[text()='A root option']")));
            driver.FindElement(By.XPath("//div[text()='A root option']")).Click();

            driver.FindElement(By.Id("selectOne")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[text()='Ms.']")));
            driver.FindElement(By.XPath("//div[text()='Ms.']")).Click();

            IWebElement oldStyleSelect = driver.FindElement(By.Id("oldSelectMenu"));
            SelectElement selectElement = new SelectElement(oldStyleSelect);
            selectElement.SelectByText("Black");

            driver.FindElement(By.XPath("//div[contains(text(),'Select...')]")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[text()='Black']")));
            driver.FindElement(By.XPath("//div[text()='Black']")).Click();
            driver.FindElement(By.XPath("//div[text()='Red']")).Click();
            driver.FindElement(By.TagName("body")).Click();

            IWebElement standardMultiSelect = driver.FindElement(By.Id("cars"));
            SelectElement multiSelectElement = new SelectElement(standardMultiSelect);
            multiSelectElement.SelectByValue("opel");
        }

        [Test]
        public void Test20_Sortable()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            SafeClick(By.XPath("//h5[text()='Interactions']"));

            SafeClick(By.XPath("//span[text()='Sortable']"));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("demo-tab-list")));
            driver.FindElement(By.Id("demo-tab-list")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".vertical-list-container")));
            Actions actions = new Actions(driver);

            var listItems = driver.FindElements(By.CssSelector(".vertical-list-container .list-group-item")).ToList();
            
            for (int i = listItems.Count - 1; i > 0; i--)
            {
                IWebElement sourceElement = driver.FindElements(By.CssSelector(".vertical-list-container .list-group-item"))[i];
                IWebElement targetElement = driver.FindElements(By.CssSelector(".vertical-list-container .list-group-item"))[0];
                
                actions.DragAndDrop(sourceElement, targetElement).Perform();
                System.Threading.Thread.Sleep(200);
            }
        }

        [Test]
        public void Test21_Selectable()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            SafeClick(By.XPath("//h5[text()='Interactions']"));

            SafeClick(By.XPath("//span[text()='Selectable']"));

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("demo-tab-grid")));
            driver.FindElement(By.Id("demo-tab-grid")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("row1")));
            var gridItems = driver.FindElements(By.CssSelector("#row1 .list-group-item, #row2 .list-group-item, #row3 .list-group-item"));
            
            foreach (var item in gridItems)
            {
                if (!item.GetAttribute("class").Contains("active"))
                {
                    item.Click();
                }
            }

            gridItems = driver.FindElements(By.CssSelector("#row1 .list-group-item, #row2 .list-group-item, #row3 .list-group-item"));
            foreach (var item in gridItems)
            {
                if (item.GetAttribute("class").Contains("active"))
                {
                    item.Click();
                }
            }

            driver.FindElement(By.XPath("//li[text()='Five']")).Click();
            
            var selectedItems = driver.FindElements(By.CssSelector(".list-group-item.active"));
            Assert.That(selectedItems.Count, Is.EqualTo(1));
            Assert.That(selectedItems[0].Text, Is.EqualTo("Five"));
        }
    }
}