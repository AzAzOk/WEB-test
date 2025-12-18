using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Threading;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab6
    {
        private IWebDriver? driver;
        private WebDriverWait? wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddArgument("--start-maximized");
            
            driver = new ChromeDriver(options);
            
            // Отключаем PageLoad timeout - полагаемся на явные WebDriverWait
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            if (driver != null)
            {
                try
                {
                    driver.Quit();
                }
                catch
                {
                    // Игнорируем ошибки при закрытии
                }
            }
            driver?.Dispose();
        }

        [Test]
        public void Test16_DatePicker()
        {
            driver!.Navigate().GoToUrl("https://demoqa.com/");
            Thread.Sleep(3000); // Даём странице время на полную загрузку

            wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Widgets']")));
            driver.FindElement(By.XPath("//h5[text()='Widgets']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Date Picker']")));
            driver.FindElement(By.XPath("//span[text()='Date Picker']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("datePickerMonthYearInput")));
            driver.FindElement(By.Id("datePickerMonthYearInput")).Click();
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("react-datepicker")));
            
            IWebElement yearDropdown = driver.FindElement(By.ClassName("react-datepicker__year-select"));
            SelectElement yearSelect = new SelectElement(yearDropdown);
            yearSelect.SelectByValue("2023");
            
            IWebElement monthDropdown = driver.FindElement(By.ClassName("react-datepicker__month-select"));
            SelectElement monthSelect = new SelectElement(monthDropdown);
            monthSelect.SelectByValue("11");
            
            driver.FindElement(By.XPath("//div[contains(@class,'react-datepicker__day') and text()='1' and not(contains(@class,'outside-month'))]")).Click();

            Thread.Sleep(500);
            driver.FindElement(By.Id("dateAndTimePickerInput")).Click();
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("react-datepicker")));
            
            driver.FindElement(By.ClassName("react-datepicker__year-read-view")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'react-datepicker__year-option') and text()='2022']")));
            driver.FindElement(By.XPath("//div[contains(@class,'react-datepicker__year-option') and text()='2022']")).Click();
            
            driver.FindElement(By.ClassName("react-datepicker__month-read-view")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class,'react-datepicker__month-option') and text()='November']")));
            driver.FindElement(By.XPath("//div[contains(@class,'react-datepicker__month-option') and text()='November']")).Click();
            
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'react-datepicker__day') and text()='2' and not(contains(@class,'outside-month'))]")));
            driver.FindElement(By.XPath("//div[contains(@class,'react-datepicker__day') and text()='2' and not(contains(@class,'outside-month'))]")).Click();
            
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//li[contains(@class,'react-datepicker__time-list-item') and text()='20:00']")));
            driver.FindElement(By.XPath("//li[contains(@class,'react-datepicker__time-list-item') and text()='20:00']")).Click();
        }

        [Test]
        public void Test17_Slider()
        {
            driver!.Navigate().GoToUrl("https://demoqa.com/");
            Thread.Sleep(3000);

            wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Widgets']")));
            driver.FindElement(By.XPath("//h5[text()='Widgets']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Slider']")));
            driver.FindElement(By.XPath("//span[text()='Slider']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".range-slider")));
            IWebElement slider = driver.FindElement(By.CssSelector("input[type='range']"));
            
            int sliderWidth = slider.Size.Width;
            
            int targetValue = 50;
            
            string minStr = slider.GetAttribute("min");
            string maxStr = slider.GetAttribute("max");
            int min = string.IsNullOrEmpty(minStr) ? 0 : int.Parse(minStr);
            int max = string.IsNullOrEmpty(maxStr) ? 100 : int.Parse(maxStr);
            
            double percentage = (double)(targetValue - min) / (max - min);
            int xOffset = (int)(sliderWidth * percentage) - (sliderWidth / 2);
            
            Actions actions = new Actions(driver);
            actions.ClickAndHold(slider)
                   .MoveByOffset(xOffset, 0)
                   .Release()
                   .Perform();
            
            Thread.Sleep(500);
            
            string actualValue = driver.FindElement(By.Id("sliderValue")).GetAttribute("value");
            int actualValueInt = int.Parse(actualValue);
            
            Assert.That(actualValueInt, Is.InRange(45, 55), 
                $"Ожидалось значение около 50, получено: {actualValueInt}");
            
            if (actualValueInt < 45 || actualValueInt > 55)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript($"arguments[0].value = {targetValue}", slider);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].dispatchEvent(new Event('input', { bubbles: true }))", slider);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].dispatchEvent(new Event('change', { bubbles: true }))", slider);
                
                Thread.Sleep(500);
                
                actualValue = driver.FindElement(By.Id("sliderValue")).GetAttribute("value");
                Assert.That(actualValue, Is.EqualTo("50"));
            }
        }

        [Test]
        public void Test18_Tabs()
        {
            driver!.Navigate().GoToUrl("https://demoqa.com/");
            Thread.Sleep(3000);

            wait!.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Widgets']")));
            driver.FindElement(By.XPath("//h5[text()='Widgets']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Tabs']")));
            driver.FindElement(By.XPath("//span[text()='Tabs']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("demo-tab-what")));
            driver.FindElement(By.Id("demo-tab-what")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("demo-tabpane-what")));
            Assert.That(driver.FindElement(By.Id("demo-tabpane-what")).Text.Length > 0, Is.True);

            driver.FindElement(By.Id("demo-tab-origin")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("demo-tabpane-origin")));
            Assert.That(driver.FindElement(By.Id("demo-tabpane-origin")).Text.Length > 0, Is.True);

            driver.FindElement(By.Id("demo-tab-use")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("demo-tabpane-use")));
            Assert.That(driver.FindElement(By.Id("demo-tabpane-use")).Text.Length > 0, Is.True);

            IWebElement moreTab = driver.FindElement(By.Id("demo-tab-more"));
            string ariaDisabled = moreTab.GetAttribute("aria-disabled");
            Assert.That(ariaDisabled, Is.EqualTo("true"));
        }
    }
}