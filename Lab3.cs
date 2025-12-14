using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab3
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }

        [Test]
        public void Test7_DynamicProperties()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Elements']")));
            driver.FindElement(By.XPath("//h5[text()='Elements']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Dynamic Properties']")));
            driver.FindElement(By.XPath("//span[text()='Dynamic Properties']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("colorChange")));
            string initialColor = driver.FindElement(By.Id("colorChange")).GetCssValue("color");
            
            WebDriverWait longWait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            longWait.Until(d =>
            {
                string currentColor = d.FindElement(By.Id("colorChange")).GetCssValue("color");
                return currentColor != initialColor;
            });

            driver.Navigate().Refresh();

            WebDriverWait visibilityWait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            visibilityWait.Until(ExpectedConditions.ElementIsVisible(By.Id("visibleAfter")));
            Assert.That(driver.FindElement(By.Id("visibleAfter")).Displayed, Is.True);
        }

        [Test]
        public void Test8_PracticeForm()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Forms']")));
            driver.FindElement(By.XPath("//h5[text()='Forms']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Practice Form']")));
            driver.FindElement(By.XPath("//span[text()='Practice Form']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("firstName")));
            driver.FindElement(By.Id("firstName")).SendKeys("John");
            driver.FindElement(By.Id("lastName")).SendKeys("Doe");

            driver.FindElement(By.Id("userEmail")).SendKeys("john.doe@example.com");

            driver.FindElement(By.XPath("//label[@for='gender-radio-1']")).Click();

            driver.FindElement(By.Id("userNumber")).SendKeys("1234567890");

            driver.FindElement(By.Id("dateOfBirthInput")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("react-datepicker")));
            
            IWebElement yearDropdown = driver.FindElement(By.ClassName("react-datepicker__year-select"));
            SelectElement yearSelect = new SelectElement(yearDropdown);
            yearSelect.SelectByValue("1990");
            
            IWebElement monthDropdown = driver.FindElement(By.ClassName("react-datepicker__month-select"));
            SelectElement monthSelect = new SelectElement(monthDropdown);
            monthSelect.SelectByValue("5");
            
            driver.FindElement(By.XPath("//div[contains(@class,'react-datepicker__day') and text()='15']")).Click();

            IWebElement subjectsInput = driver.FindElement(By.Id("subjectsInput"));
            subjectsInput.SendKeys("Maths");
            subjectsInput.SendKeys(Keys.Enter);
            subjectsInput.SendKeys("English");
            subjectsInput.SendKeys(Keys.Enter);
            subjectsInput.SendKeys("Physics");
            subjectsInput.SendKeys(Keys.Enter);

            driver.FindElement(By.XPath("//label[@for='hobbies-checkbox-1']")).Click();
            driver.FindElement(By.XPath("//label[@for='hobbies-checkbox-2']")).Click();

            driver.FindElement(By.Id("currentAddress")).SendKeys("123 Main Street, City");

            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", driver.FindElement(By.Id("state")));
            driver.FindElement(By.Id("state")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[text()='NCR']")));
            driver.FindElement(By.XPath("//div[text()='NCR']")).Click();
            
            driver.FindElement(By.Id("city")).Click();
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[text()='Delhi']")));
            driver.FindElement(By.XPath("//div[text()='Delhi']")).Click();

            driver.FindElement(By.Id("submit")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("modal-content")));
            string modalText = driver.FindElement(By.ClassName("modal-body")).Text;
            Assert.That(modalText.Contains("John Doe"), Is.True);
            Assert.That(modalText.Contains("john.doe@example.com"), Is.True);
            Assert.That(modalText.Contains("1234567890"), Is.True);

            driver.FindElement(By.Id("closeLargeModal")).Click();
        }

        [Test]
        public void Test9_BrowserWindows()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Alerts, Frame & Windows']")));
            driver.FindElement(By.XPath("//h5[text()='Alerts, Frame & Windows']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Browser Windows']")));
            driver.FindElement(By.XPath("//span[text()='Browser Windows']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("tabButton")));
            string originalWindow = driver.CurrentWindowHandle;
            driver.FindElement(By.Id("tabButton")).Click();

            wait.Until(d => d.WindowHandles.Count > 1);
            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != originalWindow)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }

            wait.Until(d => d.Url.Contains("sample"));
            Assert.That(driver.Url.Contains("https://demoqa.com/sample"), Is.True);

            driver.Close();
            driver.SwitchTo().Window(originalWindow);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("windowButton")));
            driver.FindElement(By.Id("windowButton")).Click();

            wait.Until(d => d.WindowHandles.Count > 1);
            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != originalWindow)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }

            wait.Until(d => d.Url.Contains("sample"));
            Assert.That(driver.Url.Contains("https://demoqa.com/sample"), Is.True);

            driver.Close();
            driver.SwitchTo().Window(originalWindow);
        }
    }
}
