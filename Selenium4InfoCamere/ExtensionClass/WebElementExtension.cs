using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumForInfoCamere.ExtensionClass
{
    public static class WebElementExtension
    {
        public static bool SetText(this IWebElement webElement, string text)
        {

            try
            {
                webElement.Clear();
                webElement.SendKeys(text);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        } 
    }

    public static class helper
    {
        public static IWebElement FindElementAsync(this IWebDriver driver, By by)
        {

            return waiterElement(driver, by);
        }
        private static IWebElement waiterElement(IWebDriver driver, By by)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(50));
            //IWebElement SearchResult = wait.Until((ExpectedConditions.)wd-> ((JavascriptExecutor)wd).executeScript("return document.readyState").equals("complete"));
            IWebElement SearchResult = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(by));
            //SearchResult = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
            return SearchResult;
        }
    }
}
