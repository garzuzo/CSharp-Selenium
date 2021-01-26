using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ShoppingCart
{
    public class Tests
    {
        private readonly string test_url = "http://automationpractice.com";
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
        }

        [Test, Order(1)]
        public void AddTenUniqueItems()
        {
            _driver.Url = test_url;


            IReadOnlyCollection<IWebElement> homeItems = _driver.FindElements(By.CssSelector("a[title='Add to cart']"));

            var itemsOnCart = 0;
            foreach (var currentItem in homeItems)
            {
                if (itemsOnCart == 10)
                {
                    return;
                }
                currentItem.Click();
                System.Threading.Thread.Sleep(2000);
                var continueShopping = _driver.FindElement(By.ClassName("continue"));
                continueShopping.Click();

                itemsOnCart++;
            }

            System.Threading.Thread.Sleep(2000);
            var womenSection = _driver.FindElement(By.CssSelector("a[title='Women']"));
            womenSection.Click();

            IReadOnlyCollection<IWebElement> womenItems = _driver.FindElements(By.CssSelector("a[data-id-product]"));
            foreach (var currentItem in womenItems)
            {
                if (itemsOnCart == 10)
                {
                    return;
                }
                currentItem.Click();
                System.Threading.Thread.Sleep(2000);
                var continueShopping = _driver.FindElement(By.ClassName("continue"));
                continueShopping.Click();

                itemsOnCart++;
            }
        }

        [Test, Order(2)]
        public void NavigateToShoppingCart()
        {
            IWebElement cartButton = _driver.FindElement(By.CssSelector("a[title='View my shopping cart']"));

            cartButton.Click();
            var countSpan = _driver.FindElement(By.Id("summary_products_quantity"));
            Assert.Equals("10 Products", countSpan.Text);
        }

        [Test, Order(3)]
        public void DeleteShoppingCartElements()
        {
            IReadOnlyCollection<IWebElement> itemsToDelete = _driver.FindElements(By.CssSelector("a[title='delete']"));

            foreach (var itemToDelete in itemsToDelete)
            {
                itemToDelete.Click();
            }

            var emptyCart = _driver.FindElement(By.ClassName("alert"));
            Assert.Equals("Your shopping cart is empty.", emptyCart.Text);
        }

        [OneTimeTearDown]
        public void CloseBrowser()
        {
            _driver.Quit();
        }
    }
}