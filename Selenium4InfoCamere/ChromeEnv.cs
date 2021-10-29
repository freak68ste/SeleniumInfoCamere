using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumForInfoCamere
{
    public abstract class ChromeEnv
    {
        private IWebDriver driver;
        private string _startUrl;

        public ChromeEnv(string startUrl)
        {
            this._startUrl = startUrl;
            ChromeOptions opt = new ChromeOptions();
            opt.AddUserProfilePreference("disable-popup-blocking", "true");
            opt.AddArgument("headless");
            
            driver = new ChromeDriver(@"C:\WebDriver\", opt);
        }

        //Open browser Chrome
        public void StartBrowser()
        {
            driver.Url = _startUrl;
        }

        //GetElement by Id
        public IWebElement GetElementById(string idName)
        {
            return driver.FindElement(By.Id(idName));
        }

        //GetElement by CLassName
        public IWebElement GetElementByClass(string className)
        {
            return driver.FindElement(By.ClassName(className));
        }

        //GetByPath
        public IWebElement GetElementByPath(string pathString)
        {
            return driver.FindElement(By.XPath(pathString));
        }

        //Close Browser
        public void CloseBrowser()
        {
            driver.Close();
        }
    }
}
