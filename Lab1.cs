using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab1
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.PageLoadStrategy = PageLoadStrategy.Eager;

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
        public void Test1_TextBox()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Elements']")));
            driver.FindElement(By.XPath("//h5[text()='Elements']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Text Box']")));
            driver.FindElement(By.XPath("//span[text()='Text Box']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("userName")));
            driver.FindElement(By.Id("userName")).SendKeys("Test User");
            driver.FindElement(By.Id("userEmail")).SendKeys("testuser@example.com");
            driver.FindElement(By.Id("currentAddress")).SendKeys("123 Test Street");
            driver.FindElement(By.Id("permanentAddress")).SendKeys("456 Permanent Avenue");

            driver.FindElement(By.Id("submit")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("output")));
            Assert.That(driver.FindElement(By.Id("name")).Text.Contains("Test User"), Is.True);
            Assert.That(driver.FindElement(By.Id("email")).Text.Contains("testuser@example.com"), Is.True);
            Assert.That(driver.FindElement(By.XPath("//p[@id='currentAddress']")).Text.Contains("123 Test Street"), Is.True);
            Assert.That(driver.FindElement(By.XPath("//p[@id='permanentAddress']")).Text.Contains("456 Permanent Avenue"), Is.True);
        }

        [Test]
        public void Test2_CheckBox()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//h5[text()='Elements']"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[text()='Check Box']"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.CssSelector(".rct-option-expand-all"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[text()='Notes']"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[text()='Veu']"))).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//span[text()='Private']"))).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(
                By.Id("result")));

            string resultText = driver.FindElement(By.Id("result")).Text;

            Assert.That(resultText.Contains("notes"), Is.True);
            Assert.That(resultText.Contains("veu"), Is.True);
            Assert.That(resultText.Contains("private"), Is.True);
        }


        [Test]
        public void Test3_RadioButton()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Elements']")));
            driver.FindElement(By.XPath("//h5[text()='Elements']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Radio Button']")));
            driver.FindElement(By.XPath("//span[text()='Radio Button']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//label[@for='yesRadio']")));
            driver.FindElement(By.XPath("//label[@for='yesRadio']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".text-success")));
            Assert.That(driver.FindElement(By.CssSelector(".text-success")).Text.Contains("Yes"), Is.True);

            driver.FindElement(By.XPath("//label[@for='impressiveRadio']")).Click();

            wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector(".text-success"), "Impressive"));
            Assert.That(driver.FindElement(By.CssSelector(".text-success")).Text.Contains("Impressive"), Is.True);

            Assert.That(driver.FindElement(By.Id("noRadio")).Enabled, Is.False);
        }
    }
}
