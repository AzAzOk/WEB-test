using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace Selenium.LaboratoryWorks
{
    [TestFixture]
    public class Lab2
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
        public void Test4_Buttons()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Elements']")));
            driver.FindElement(By.XPath("//h5[text()='Elements']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Buttons']")));
            driver.FindElement(By.XPath("//span[text()='Buttons']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[text()='Click Me']")));
            driver.FindElement(By.XPath("//button[text()='Click Me']")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("dynamicClickMessage")));
            Assert.That(driver.FindElement(By.Id("dynamicClickMessage")).Text.Contains("You have done a dynamic click"), Is.True);

            Actions actions = new Actions(driver);
            IWebElement doubleClickBtn = driver.FindElement(By.Id("doubleClickBtn"));
            actions.DoubleClick(doubleClickBtn).Perform();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("doubleClickMessage")));
            Assert.That(driver.FindElement(By.Id("doubleClickMessage")).Text.Contains("You have done a double click"), Is.True);

            IWebElement rightClickBtn = driver.FindElement(By.Id("rightClickBtn"));
            actions.ContextClick(rightClickBtn).Perform();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("rightClickMessage")));
            Assert.That(driver.FindElement(By.Id("rightClickMessage")).Text.Contains("You have done a right click"), Is.True);
        }

        [Test]
        public void Test5_Links()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Elements']")));
            driver.FindElement(By.XPath("//h5[text()='Elements']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Links']")));
            driver.FindElement(By.XPath("//span[text()='Links']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("simpleLink")));
            string originalWindow = driver.CurrentWindowHandle;
            driver.FindElement(By.Id("simpleLink")).Click();

            wait.Until(d => d.WindowHandles.Count > 1);
            foreach (string windowHandle in driver.WindowHandles)
            {
                if (windowHandle != originalWindow)
                {
                    driver.SwitchTo().Window(windowHandle);
                    break;
                }
            }

            wait.Until(d => d.Url.Contains("demoqa.com"));
            Assert.That(driver.Url.Contains("https://demoqa.com/"), Is.True);

            driver.Close();
            driver.SwitchTo().Window(originalWindow);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("moved")));
            driver.FindElement(By.Id("moved")).Click();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("linkResponse")));
            string responseText = driver.FindElement(By.Id("linkResponse")).Text;
            Assert.That(responseText.Contains("301") && responseText.Contains("Moved Permanently"), Is.True);
        }

        [Test]
        public void Test6_UploadAndDownload()
        {
            driver.Navigate().GoToUrl("https://demoqa.com/");

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//h5[text()='Elements']")));
            driver.FindElement(By.XPath("//h5[text()='Elements']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[text()='Upload and Download']")));
            driver.FindElement(By.XPath("//span[text()='Upload and Download']")).Click();

            string testFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "test_file.txt");
            System.IO.File.WriteAllText(testFilePath, "Test file content");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("uploadFile")));
            driver.FindElement(By.Id("uploadFile")).SendKeys(testFilePath);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("uploadedFilePath")));
            string uploadedFileName = driver.FindElement(By.Id("uploadedFilePath")).Text;
            Assert.That(uploadedFileName.Contains("test_file.txt"), Is.True);

            Assert.That(uploadedFileName.Contains("fakepath") || uploadedFileName.Contains("test_file.txt"), Is.True);

            System.IO.File.Delete(testFilePath);
        }
    }
}
