using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumForInfoCamere.Iterations.Interface;
using SeleniumForInfoCamere.Models.InfoCamere;

namespace SeleniumForInfoCamere.Iterations.Infocamere
{
    public class ServiceInfoCamere : AbstractBaseInfoCamere
    {

        public ServiceInfoCamere(SeleniumChrome icDriver)
        {
            base.icDriver = icDriver;
        }

        public bool StartElaborazioneDownloadFascicolo(InfoCamereFascicoloStoricoModel model)
        {
            bool ret = false;
            //fase di login 
            Login(model);

            return ret;
        }

        public void Login(InfoCamereFascicoloStoricoModel model)
        {
            SearchMode src = new SearchMode();
            
        }

        

       
    }
}
