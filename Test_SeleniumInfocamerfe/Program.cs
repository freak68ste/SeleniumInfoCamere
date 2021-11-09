using System;
using SeleniumForInfoCamere;
using SeleniumForInfoCamere.Iterations.Infocamere;
using SeleniumForInfoCamere.Iterations.Interface;
using SeleniumForInfoCamere.Models.InfoCamere;

namespace Test_SeleniumInfocamerfe
{
    class Program
    {
        static void Main(string[] args)
        {
            StartUp.Initialize();


            ////var driver = new SeleniumChrome("file:///C:/Users/freak/PatrimonioIllecitiDoc/Html%20Argo/pagina1%20-%20Home.html");
            //var driver = new SeleniumChrome("https://portaleargo.infocamere.it");
            //InfoCamereFascicoloStoricoModel model = new InfoCamereFascicoloStoricoModel();
            
            //driver.StartBrowser();
            //var test = new ServiceInfoCamere(driver);
            
            //if (test.IsLoginPage())
            //{
            //    model.PasswordLogin = "Marialinda01!";
            //    model.UsernameLogin = "lmpp0008";
            //    test.Login(model);
            //}

            //test.NavigaImprese();
           
            ////model.
            ////driver.CloseBrowser();

        }



        /**
         * 
         * Passi Selenium IDE
         * 
         * 
         * 
         * */

       
    }

    public class SpiderService : ISpiderService
    {
        private IServiceInfoCamere _service;
        //private ISeleniumChrome _driver;
        public SpiderService(IServiceInfoCamere service) //, ISeleniumChrome driver
        {
            _service = service;
            //_driver = driver;
        }
        public void Run()
        {
            try
            {
                byte[] fileDownloaded;
                //var driver = new SeleniumChrome("file:///C:/Users/freak/PatrimonioIllecitiDoc/Html%20Argo/pagina1%20-%20Home.html");
                //var _driver = new SeleniumChrome("https://portaleargo.infocamere.it");
                InfoCamereFascicoloStoricoModel model = new InfoCamereFascicoloStoricoModel();
                //societa
                model.CodiceRea = "114792";
                model.Provincia = "RC";
                //cooperativa
                //model.CodiceRea = "427027";
                //model.Provincia = "VR";
                //ditta individuale
                //model.CodiceRea = "155697";
                //model.Provincia = "KR";
                _service.StartBrowser();
                //_driver.StartBrowser();
                //var test = new ServiceInfoCamere(driver);
                //_service.LogOut();
                if (_service.IsLoginPage())
                {
                    model.PasswordLogin = "Marialinda01!";
                    model.UsernameLogin = "lmpp0008";
                    _service.Login(model);
                }
                _service.NavigaImprese();
                _service.StartSearchByCodiceRea(model);

                var checkPage = _service.CheckPaginaImpresa();
                //var checkPage = InfoCamereEnum.TipoImpresa.DITTA_INDIVIDUALE;
                switch (checkPage)
                {
                    case InfoCamereEnum.TipoImpresa.COOPERATIVA:
                        fileDownloaded = _service.DownloadFascicoloCooperative();
                        break;
                    case InfoCamereEnum.TipoImpresa.DITTA_INDIVIDUALE:
                        fileDownloaded = _service.DownloadFascicoloDittaIndividuale();
                        break;
                    case InfoCamereEnum.TipoImpresa.SOCIETA:
                        fileDownloaded = _service.DownloadFascicoloSocieta();
                        break;
                }
            }
            catch (Exception ex)
            {   
                throw;
            }
            finally
            {
                _service.LogOut();
                _service.CloseBrowser();
            }
        }
    }

    public interface ISpiderService
    {
        void Run();
    }
}
