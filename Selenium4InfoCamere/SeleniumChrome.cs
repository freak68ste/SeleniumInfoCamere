using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace SeleniumForInfoCamere
{
    public class SeleniumChrome : ChromeEnv, ISeleniumChrome
    {
        public SeleniumChrome()
        {

        }
    }

    public interface ISeleniumChrome
    {
        IWebElement GetElementById(string idName);
        IWebElement GetElementByName(string name);
        IWebElement GetElementByCssSelector(string cssSelect);
        IWebElement GetElementByClass(string className);
        ICollection<IWebElement> GetElementsById(string idName);
        ICollection<IWebElement> GetElementsByClass(string className);
        ICollection<IWebElement> GetElementsByName(string name);
        IWebElement GetElementByPath(string pathString);
        ICollection<IWebElement> GetElementsByPath(string pathString);
        ICollection<IWebElement> GetElementsByCssSelector(string cssSelect);
        IWebDriver ChangeContext(string handler);
        List<string> GetListHandlersWindow();
        string GetCurrentHandlerWindow();
        void GenerateUrlFileDownloadSocieta();
        string ReadVariabileFileUrl();
        byte[] DownloadByApiCall(string apiCall);
        void NavigateToUrl(string url);
        string GetPageSource();
        void ExecuteJSScript(string scriptJS);
        void RefreshPage();

        void StartBrowser();
        void CloseBrowser();
        string GetRootPathUrl();
    }
}
