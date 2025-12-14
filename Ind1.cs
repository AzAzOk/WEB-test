using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace Selenium.IndependentWork
{
    public class BasePage
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        protected IWebElement WaitAndFind(By locator)
        {
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        protected bool IsElementPresent(By locator)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected int GetElementsCount(By locator)
        {
            try
            {
                wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                return driver.FindElements(locator).Count;
            }
            catch
            {
                return 0;
            }
        }

        protected bool IsElementPresentMultiple(params By[] locators)
        {
            foreach (var locator in locators)
            {
                if (IsElementPresent(locator))
                    return true;
            }
            return false;
        }

        protected IWebElement WaitAndFindMultiple(params By[] locators)
        {
            foreach (var locator in locators)
            {
                try
                {
                    return WaitAndFind(locator);
                }
                catch
                {
                    continue;
                }
            }
            throw new NoSuchElementException("Элемент не найден");
        }
    }

    public class GenericMainPage : BasePage
    {

        private readonly string siteUrl = "https://www.skillbox.ru/";         

        private By acceptCookiesButton = By.XPath("//button[contains(text(),'Принять') or contains(text(),'Согласен')]");
        
        private By[] logoLocators = new By[]
        {
            By.XPath("//img[contains(@src, 'logo')]"),              
            By.XPath("//a[@href='/']/img"),                          
            By.XPath("//img[contains(@alt, 'logo')]"),               
            By.CssSelector("img[class*='logo']"),                    
            By.TagName("img")                                        
        };
        
        private By[] searchInputLocators = new By[]
        {
            By.XPath("//input[contains(@placeholder, 'поиск') or contains(@placeholder, 'Поиск')]"),
            By.XPath("//input[contains(@class, 'search')]"),
            By.XPath("//input[@type='search']"),
            By.CssSelector("input[type='text']"),
            By.XPath("//input[@name='q']")                           
        };
        
        private By[] catalogButtonLocators = new By[]
        {
            By.XPath("//button[contains(text(), 'Каталог')]"),
            By.XPath("//a[contains(text(), 'Каталог')]"),
            By.XPath("//nav//a[1]"),                                 
            By.CssSelector("nav a"),                                 
            By.XPath("//a[contains(@class, 'menu')]")
        };

        public GenericMainPage(IWebDriver driver) : base(driver) { }

        public void AcceptCookiesIfPresent()
        {
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(acceptCookiesButton)).Click();
            }
            catch
            {
            }
        }

        public void Open()
        {
            driver.Navigate().GoToUrl(siteUrl);
            System.Threading.Thread.Sleep(3000);
        }

        public bool IsLogoDisplayed()
        {
            return IsElementPresentMultiple(logoLocators);
        }

        public bool IsSearchDisplayed()
        {
            return IsElementPresentMultiple(searchInputLocators);
        }

        public bool IsCatalogButtonDisplayed()
        {
            return IsElementPresentMultiple(catalogButtonLocators);
        }

        public void PerformSearch(string searchText)
        {
            var input = WaitAndFindMultiple(searchInputLocators);
            input.Clear();
            input.SendKeys(searchText);
            input.SendKeys(Keys.Enter);
            System.Threading.Thread.Sleep(3000);
        }
    }

    public class GenericSearchResultsPage : BasePage
    {
        private By[] searchResultsLocators = new By[]
        {
            By.XPath("//a[@data-product-id]"),
            By.XPath("//div[@data-product-id]"),
            By.XPath("//div[contains(@class, 'product')]"),
            By.XPath("//div[contains(@class, 'item')]"),
            By.XPath("//div[contains(@class, 'card')]"),
            By.XPath("//article"),
            By.CssSelector("[class*='result'], [class*='item']")
        };

        public GenericSearchResultsPage(IWebDriver driver) : base(driver) { }

        public bool HasSearchResults()
        {
            foreach (var locator in searchResultsLocators)
            {
                if (GetElementsCount(locator) > 0)
                    return true;
            }
            return false;
        }

        public int GetResultsCount()
        {
            foreach (var locator in searchResultsLocators)
            {
                int count = GetElementsCount(locator);
                if (count > 0)
                    return count;
            }
            return 0;
        }
    }

    [TestFixture]
    public class Ind1_GenericTests
    {
        private IWebDriver driver;
        private GenericMainPage mainPage;
        private GenericSearchResultsPage searchResultsPage;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            mainPage = new GenericMainPage(driver);
            searchResultsPage = new GenericSearchResultsPage(driver);
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }


        [Test]
        public void Test1_MainPageOpens()
        {
            mainPage.Open();
            mainPage.AcceptCookiesIfPresent();

            Assert.That(mainPage.IsLogoDisplayed(), Is.True,
                "Логотип не отображается на главной странице");
        }


        [Test]
        public void Test2_LogoIsDisplayed()
        {
            mainPage.Open();
            mainPage.AcceptCookiesIfPresent();

            Assert.That(mainPage.IsLogoDisplayed(), Is.True,
                "Логотип не найден на странице");
        }


        [Test]
        public void Test3_SearchFieldIsDisplayed()
        {
            mainPage.Open();
            mainPage.AcceptCookiesIfPresent();

            Assert.That(mainPage.IsSearchDisplayed(), Is.True,
                "Поле поиска не отображается на странице");
        }


        [Test]
        public void Test4_CatalogButtonIsDisplayed()
        {
            mainPage.Open();
            mainPage.AcceptCookiesIfPresent();

            Assert.That(mainPage.IsCatalogButtonDisplayed(), Is.True,
                "Кнопка каталога не отображается на странице");
        }


        [Test]
        public void Test5_SearchReturnsResults()
        {
            mainPage.Open();
            mainPage.AcceptCookiesIfPresent();
            
            mainPage.PerformSearch("Python"); 

            Assert.That(searchResultsPage.HasSearchResults(), Is.True,
                "Результаты поиска не найдены");

            int resultsCount = searchResultsPage.GetResultsCount();
            Assert.That(resultsCount, Is.GreaterThan(0),
                $"Ожидалось более 0 результатов, получено: {resultsCount}");
        }
    }
}