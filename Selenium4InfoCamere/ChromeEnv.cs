using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Chromium;
using SeleniumForInfoCamere.ExtensionClass;

namespace SeleniumForInfoCamere
{
    public abstract class ChromeEnv
    {
        private IWebDriver driver;
        private string _startUrl;
        public ChromeEnv()
        {
            ChromeOptions opt = new ChromeOptions();
            opt.AddUserProfilePreference("disable-popup-blocking", "true");
            opt.AddArguments(new List<string>() { "--enable-javascript", "--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36" });
            opt.AddArguments("--window-size=1920,1080");
            opt.AddArguments("--start-maximized");
            opt.AddArguments("--headless");
            //"headless",
            //opt.AddArgument("headless");
            //opt.AddArgument("--enable-javascript");
            driver = new ChromeDriver(@"C:\WebDriver\", opt);
            _startUrl = "https://portaleargo.infocamere.it"; /* prendere da file file di configurazione */
        }


        //Open browser Chrome
        public void StartBrowser()
        {
            driver.Url = _startUrl;
        }

        //GetElement by Id
        public IWebElement GetElementById(string idName)
        {
            string tmp = driver.Url;
            return driver.FindElementAsync(By.Id(idName));
        }

        //GetElement by CLassName
        public IWebElement GetElementByClass(string className)
        {
            return driver.FindElement(By.ClassName(className));
        }

        //GetElement by Id
        public ICollection<IWebElement> GetElementsById(string idName)
        {
            return driver.FindElements(By.Id(idName));
        }

        //GetElement by CLassName
        public ICollection<IWebElement> GetElementsByClass(string className)
        {
            return driver.FindElements(By.ClassName(className));
        }

        //GetElement by Name
        public IWebElement GetElementByName(string name)
        {
            return driver.FindElement(By.Name(name));
        }

        //GetElements By Name
        public ICollection<IWebElement> GetElementsByName(string name)
        {
            return driver.FindElements(By.Name(name));
        }

        //GetByPath
        public IWebElement GetElementByPath(string pathString)
        {
            return driver.FindElement(By.XPath(pathString));
        }

        //GetByPath
        public ICollection<IWebElement> GetElementsByPath(string pathString)
        {
            return driver.FindElements(By.XPath(pathString));
        }

        //GetElement By CssSelector
        public IWebElement GetElementByCssSelector(string cssSelect)
        {
            return driver.FindElement(By.CssSelector(cssSelect));
        }

        //GetElements By CssSelector
        public ICollection<IWebElement> GetElementsByCssSelector(string cssSelect)
        {
            return driver.FindElements(By.CssSelector(cssSelect));
        }

        //Close Browser
        public void CloseBrowser()
        {
            driver.Close();
        }

        public IWebDriver ChangeContext(string handler)
        {
            return driver.SwitchTo().Window(handler);
        }

        public List<string> GetListHandlersWindow()
        {
            return driver.WindowHandles.ToList();
        }

        public string GetCurrentHandlerWindow()
        {
            return driver.CurrentWindowHandle;
        }

        public void GenerateUrlFileDownloadSocieta()
        {   
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(JSMethodGetUrl());
        }
        
        public void NavigateToUrl(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        private string JSMethodGetUrl()
        {
            string text = 
            "window.callDownloadUrl = function(downloadInfo){" +
            "var pathFile = downloadInfo.split(\"#\")[0];" +
            "var documentoRich = downloadInfo.split(\"#\")[1];" +
            "var dettaglioRich = downloadInfo.split(\"#\")[2];" +
            "var inquiry = downloadInfo.split(\"#\")[3];" +
            "var fkInteInquiry = downloadInfo.split(\"#\")[4];" +
            "var listaPathFigli = downloadInfo.split(\"#\")[5];" +
            "var resourceUrl = Liferay.PortletURL.createResourceURL();" +
            "resourceUrl.setParameter('type', 'singolo');" +
            "resourceUrl.setParameter('path', pathFile);" +
            "resourceUrl.setParameter('documentoRich', documentoRich);" +
            "resourceUrl.setParameter('dettaglioRich', dettaglioRich);" +
            "resourceUrl.setParameter('inquiry', inquiry);" +
            "resourceUrl.setParameter('fkInteInquiry', fkInteInquiry);" +
            "resourceUrl.setParameter('listaPathFigli', listaPathFigli);" +
            "resourceUrl.setPortletId(\"1_WAR_prospettiportlet\");" +
            "resourceUrl.setPortletMode(\"view\");" +
            "resourceUrl.setWindowState(\"normal\"); " +
            "console.log(window.location.origin + resourceUrl.toString());" +
            "window.testVariabile =  window.location.origin + resourceUrl.toString();" +
            "}";
            return text;
        }

        public string ReadVariabileFileUrl()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var obj = (object)js.ExecuteScript("return window.testVariabile;");
            return obj.ToString();
        }
        public byte[] DownloadByApiCall(string apiCall)
        {
            var uri = new Uri(driver.Url);
            var path = apiCall;

            byte[] data = null;
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var webRequest = (HttpWebRequest)WebRequest.Create(path);

                webRequest.CookieContainer = new CookieContainer();
                foreach (var cookie in driver.Manage().Cookies.AllCookies)
                    webRequest.CookieContainer.Add(new System.Net.Cookie(cookie.Name, cookie.Value, cookie.Path, string.IsNullOrWhiteSpace(cookie.Domain) ? uri.Host : cookie.Domain));

                using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (var ms = new MemoryStream())
                    {
                        var responseStream = webResponse.GetResponseStream();
                        responseStream.CopyTo(ms);
                        data = ms.ToArray();
                    }
                }
            }
            catch (WebException webex)
            {
                var errResp = webex.Response;
                using (var respStream = errResp.GetResponseStream())
                {
                    var reader = new StreamReader(respStream);
                    //Assert.Fail($"Error getting file from the server({webex.Status} - {webex.Message}): {reader.ReadToEnd()}.");
                }
            }

            return data;
        }

        public string GetPageSource()
        {
            return driver.PageSource;
        }

        public void ExecuteJSScript(string scriptJS)
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                var obj = (object)js.ExecuteScript(scriptJS);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GetRootPathUrl()
        {
            return _startUrl;
        }

        public void RefreshPage()
        {
            driver.Navigate().Refresh();
        }


    }
}
