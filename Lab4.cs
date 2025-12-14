using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab4
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
        public void Test10_Alerts()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Alerts, Frame & Windows']")));
            driver.FindElement(By.XPath("//h5[text()='Alerts, Frame & Windows']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Alerts']")));
            driver.FindElement(By.XPath("//span[text()='Alerts']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("alertButton")));
            driver.FindElement(By.Id("alertButton")).Click();

            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert1 = driver.SwitchTo().Alert();
            Assert.That(alert1.Text, Is.EqualTo("You clicked a button"));

            alert1.Accept();

            driver.FindElement(By.Id("timerAlertButton")).Click();

            WebDriverWait alertWait = new WebDriverWait(driver, TimeSpan.FromSeconds(6));
            alertWait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert2 = driver.SwitchTo().Alert();
            Assert.That(alert2.Text, Is.EqualTo("This alert appeared after 5 seconds"));

            alert2.Accept();

            driver.FindElement(By.Id("confirmButton")).Click();

            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert3 = driver.SwitchTo().Alert();
            Assert.That(alert3.Text, Is.EqualTo("Do you confirm action?"));

            alert3.Dismiss();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("confirmResult")));
            Assert.That(driver.FindElement(By.Id("confirmResult")).Text.Contains("Cancel"), Is.True);

            driver.FindElement(By.Id("promtButton")).Click();

            wait.Until(ExpectedConditions.AlertIsPresent());
            IAlert alert4 = driver.SwitchTo().Alert();
            alert4.SendKeys("Test Name");

            alert4.Accept();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("promptResult")));
            Assert.That(driver.FindElement(By.Id("promptResult")).Text.Contains("Test Name"), Is.True);
        }

        [Test]
        public void Test11_Frames()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Alerts, Frame & Windows']")));
            driver.FindElement(By.XPath("//h5[text()='Alerts, Frame & Windows']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Frames']")));
            driver.FindElement(By.XPath("//span[text()='Frames']")).Click();

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("frame1")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sampleHeading")));
            Assert.That(driver.FindElement(By.Id("sampleHeading")).Text.Contains("This is a sample page"), Is.True);
            driver.SwitchTo().DefaultContent();

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("frame2")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("sampleHeading")));
            Assert.That(driver.FindElement(By.Id("sampleHeading")).Text.Contains("This is a sample page"), Is.True);
            driver.SwitchTo().DefaultContent();
        }

        [Test]
        public void Test12_NestedFrames()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Alerts, Frame & Windows']")));
            driver.FindElement(By.XPath("//h5[text()='Alerts, Frame & Windows']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Nested Frames']")));
            driver.FindElement(By.XPath("//span[text()='Nested Frames']")).Click();

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("frame1")));
            
            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("p")));
            Assert.That(driver.FindElement(By.TagName("p")).Text.Contains("Child Iframe"), Is.True);
            driver.SwitchTo().DefaultContent();

            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("frame1")));
            wait.Until(ExpectedConditions.ElementIsVisible(By.TagName("body")));
            Assert.That(driver.FindElement(By.TagName("body")).Text.Contains("Parent frame"), Is.True);
            driver.SwitchTo().DefaultContent();
        }
    }
}
