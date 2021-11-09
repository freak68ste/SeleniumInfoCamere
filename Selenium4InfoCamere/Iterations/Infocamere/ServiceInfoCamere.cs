using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumForInfoCamere.ExtensionClass;
using SeleniumForInfoCamere.Iterations.Interface;
using SeleniumForInfoCamere.Models.InfoCamere;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.IO;
using System.Timers;
using System.Threading.Tasks;

namespace SeleniumForInfoCamere.Iterations.Infocamere
{
    public class ServiceInfoCamere : IServiceInfoCamere
    {
        private ISeleniumChrome icDriver;
        private static System.Timers.Timer aTimer;
        private static bool stopTime = false;


        public ServiceInfoCamere(ISeleniumChrome driver)
        {
            icDriver = driver;
        }


        #region public method
        public void Login(InfoCamereFascicoloStoricoModel model)
        {
            var elem = icDriver.GetElementByName("userid");
            elem.SetText(model.UsernameLogin);
            elem = icDriver.GetElementByName("password");
            elem.SetText(model.PasswordLogin);
            //click del caiser
            IWebElement itemSubmit = icDriver.GetElementByCssSelector(".button");
            itemSubmit.Click();
        }

        public bool NavigaImprese()
        {
            try
            {
                icDriver.NavigateToUrl("https://portaleargo.infocamere.it/group/argo/imprese");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool IsLoginPage()
        {
            string idPassPage = "passwordHref";

            var elemLogin = icDriver.GetElementById(idPassPage);

            if (elemLogin != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void StartSearchByCodiceRea(InfoCamereFascicoloStoricoModel model)
        {
            try
            {   
                //pulizia dei campi
                var elemDenom = icDriver.GetElementById("inputDenominazione");
                var elemCF = icDriver.GetElementById("inputDenominazione");
                var elemNumRea = icDriver.GetElementById("inputNumeroREA");
                var elemProvincia = icDriver.GetElementById("selezioneInputProvinciaRea");
                var btnAddFiltro = icDriver.GetElementByCssSelector(".btnAggiungiFiltriDatiImpresa.btnAggiungiFiltri");
                var avviaRicerca = icDriver.GetElementById("_1_WAR_ricercaimpreseportlet_INSTANCE_mQtqIvfcL3VT_info-prezzo");

                SelectElement provSelect = new SelectElement(elemProvincia);
                var page = icDriver.GetPageSource();
                //settaggio del numero Rea e della provincia
                elemDenom.Clear();
                elemCF.Clear();
                elemNumRea.SetText(model.CodiceRea);
                provSelect.SelectByValue(model.Provincia);

                //emulo il click da javascript del button
                string textJSAddFiltro = "$(\".btnAggiungiFiltriDatiImpresa\").click();";
                icDriver.ExecuteJSScript(textJSAddFiltro);
                string textJSCerca = "cerca();";
                icDriver.ExecuteJSScript(textJSCerca);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public void StartBrowser()
        {
            icDriver.StartBrowser();
        }

        public InfoCamereEnum.TipoImpresa CheckPaginaImpresa()
        {
            try
            {
                bool isDittaIndividuale = true;
                string pageUrl = icDriver.GetPageSource();
                var lstElem = icDriver.GetElementsByCssSelector("a.linkRisultatiRicerca");
                foreach (var item in lstElem)
                {
                    string innerText = item.GetAttribute("innerText").ToLower();
                    if (innerText.Contains("fascicolo storico"))
                    {
                        isDittaIndividuale = false;
                        break;
                    }
                }
                if (isDittaIndividuale)
                {
                    return InfoCamereEnum.TipoImpresa.DITTA_INDIVIDUALE;
                }

                //prendo la forma giuridica per procesarla
                var formaGiuridica = icDriver.GetElementByCssSelector(".ddFormaG");

                string testoTmp = formaGiuridica.GetAttribute("innerText").ToLower(); 
                
                if (testoTmp.Contains("cooperativa"))
                {
                    return InfoCamereEnum.TipoImpresa.COOPERATIVA;
                }

                return InfoCamereEnum.TipoImpresa.SOCIETA;
                
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        private static void SetTimer(int milliseconds)
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(milliseconds);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            stopTime = true;
        }

        private static void CloseTimer()
        {
            aTimer.Stop();
            aTimer.Dispose();
            stopTime = false;
        }

        public byte[] DownloadFascicoloSocieta()
        {
            try
            {
                bool stopWait = false;
                
                NavigaFascicoloStorico("fascicolo storico");
                GoToPageDownloadSocieta();
                byte[] result = null;
                SetTimer(40000);


                while (!stopWait && !stopTime)
                {
                    if (CheckAvailabilityFileDownload())
                    {
                        stopWait = true;
                        result = DownloadFileSocieta();
                    }
                    else
                    {
                        RefreshPageDownloadFileSocieta();
                        Task.Delay(2000);
                    }
                }

                if (stopTime)
                {
                    throw new TimeoutException();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                CloseTimer();
            }
        }

        public byte[] DownloadFascicoloCooperative()
        {
            NavigaFascicoloStorico("fascicolo storico");
            GoToPageDownLoadCooperativa();
            return DownloadFileCooperativaDittaIndividuale();
        }

        public byte[] DownloadFascicoloDittaIndividuale()
        {
            NavigaFascicoloStorico("visura storica");
            GoToPageDownLoadDittaIndividuale();
            return DownloadFileCooperativaDittaIndividuale();
        }


        public void LogOut()
        {
            string rootPath = icDriver.GetRootPathUrl();
            string pathUrl = string.Concat(rootPath, "/c/portal/logout");
            icDriver.NavigateToUrl(pathUrl);
            ///c/portal/logout
        }

        public void CloseBrowser()
        {
            icDriver.CloseBrowser();
        }
        #endregion

        #region private method

        private void RefreshPageDownloadFileSocieta()
        {
            var itemButton = icDriver.GetElementsByCssSelector("button.btn.btn-primary").FirstOrDefault(x => x.GetAttribute("innerText").ToLower().Contains("cerca"));
            if (itemButton != null)
            {
                itemButton.Click();
            }
            //foreach (var item in lstButton)
            //{
            //    string text = item.GetAttribute("innerText").ToLower();
            //    if (text.Contains("cerca"))
            //    {
            //        item.Submit();
            //        break;
            //    }
            //}

            //string scriptJS = "var btnClick = $(\"button.btn.btn-primary\"); " +
            //    "btnClick[0].click();";
            //icDriver.ExecuteJSScript(scriptJS);
        }
        private bool CheckAvailabilityFileDownload()
        {
            var rowsElement = icDriver.GetElementByCssSelector(".table.tableProspettiDoc").FindElements(By.TagName("tr"));
            var itemCheck = rowsElement[1].FindElement(By.ClassName("icon-circle"));
            
            string nameTest = itemCheck.GetAttribute("className").ToLower();
            if (nameTest.Contains("text-success"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private byte[] DownloadFileSocieta()
        {
            try
            {
                //preparare la generazione dell'url del file
                icDriver.GenerateUrlFileDownloadSocieta();
                //prendo la tabella dei file
                var rowsElement = icDriver.GetElementByCssSelector(".table.tableProspettiDoc").FindElements(By.TagName("tr"));
                rowsElement[1].FindElement(By.TagName("img")).Click();
                //chiamo il get Url
                var urlFile = icDriver.ReadVariabileFileUrl();
                //prendo i byte del file
                if (urlFile == default)
                    throw new FileNotFoundException();

                var byteFile = icDriver.DownloadByApiCall(urlFile);
                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private byte[] DownloadFileCooperativaDittaIndividuale()
        {
            try
            {
                //recupero l'href del 
                var linkDownload = icDriver.GetElementById("mostraPdf");
                string urlApi = linkDownload.GetAttribute("href");
                var byteFile = icDriver.DownloadByApiCall(urlApi);

                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        private void GoToPageDownloadSocieta()
        {
            try
            {
                var pageCurrent = icDriver.GetPageSource();
                //var driv = icDriver.ChangeContext(lstrHandler[1]);
                var areaLstocument = icDriver.GetElementsByCssSelector(".bilancio-link>a");

                foreach (var itemDoc in areaLstocument)
                {
                    if (itemDoc.Text.ToLower().Trim() == "area documenti")
                    {
                        if (itemDoc.TagName.ToLower() == "a")
                        {
                            itemDoc.Click();
                            break;
                        }
                    }
                }
                var lstrHandler = icDriver.GetListHandlersWindow();

                icDriver.ChangeContext(lstrHandler[1]);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void NavigaFascicoloStorico(string linkText)
        {
            try
            {
                //prendo gli elementi di classe
                var lstElem = icDriver.GetElementsByCssSelector("a.linkRisultatiRicerca");
                var previousPageHandler = icDriver.GetCurrentHandlerWindow();
                var previousPageSource = icDriver.GetPageSource();
                foreach (var item in lstElem)
                {
                    string innerText = item.GetAttribute("innerText").ToLower();
                    if (innerText.Contains(linkText))
                    {
                        //prendo l'href e faccio il navigate url
                        string href = item.GetAttribute("href");
                        icDriver.NavigateToUrl(href);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GoToPageDownLoadCooperativa()
        {
            try
            {
                var selectTypeFile = icDriver.GetElementByCssSelector("select");
                SelectElement provSelect = new SelectElement(selectTypeFile);

                provSelect.SelectByValue("FS_SCAP_FS1");

                string textJSScript = "$(\"#btnPdf\").click();";
                icDriver.ExecuteJSScript(textJSScript);
                var lstHandler = icDriver.GetListHandlersWindow();
                icDriver.ChangeContext(lstHandler[1]);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void GoToPageDownLoadDittaIndividuale()
        {
            //prendo l'elemento a
            var linkDownload = icDriver.GetElementByCssSelector("a.stampaU");
            linkDownload.Click();
            var lstHandler = icDriver.GetListHandlersWindow();
            icDriver.ChangeContext(lstHandler[1]);
        }
        #endregion

    }
}
