using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

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

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("section1Heading")));
            
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("section1Content")));
            Assert.That(driver.FindElement(By.Id("section1Content")).Text.Contains("Lorem Ipsum"), Is.True);

            driver.FindElement(By.Id("section2Heading")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("section2Content")));
            Assert.That(driver.FindElement(By.Id("section2Content")).Text.Length > 0, Is.True);

            driver.FindElement(By.Id("section3Heading")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("section3Content")));
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

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("autoCompleteMultipleInput")));
            IWebElement multiInput = driver.FindElement(By.Id("autoCompleteMultipleInput"));
            
            multiInput.SendKeys("Black");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Black']")));
            driver.FindElement(By.XPath("//div[text()='Black']")).Click();
            
            multiInput.SendKeys("Red");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Red']")));
            driver.FindElement(By.XPath("//div[text()='Red']")).Click();
            
            multiInput.SendKeys("Magenta");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Magenta']")));
            driver.FindElement(By.XPath("//div[text()='Magenta']")).Click();

            IWebElement singleInput = driver.FindElement(By.Id("autoCompleteSingleInput"));
            singleInput.SendKeys("Black");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Black']")));
            driver.FindElement(By.XPath("//div[text()='Black']")).Click();

            singleInput = driver.FindElement(By.Id("autoCompleteSingleInput"));
            singleInput.Clear();
            singleInput.SendKeys("Red");
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[text()='Red']")));
            driver.FindElement(By.XPath("//div[text()='Red']")).Click();
        }
    }
}
