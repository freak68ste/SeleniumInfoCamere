using SeleniumForInfoCamere.Models.InfoCamere;
using System;
using System.Collections.Generic;
using System.Text;
using static SeleniumForInfoCamere.Iterations.Infocamere.InfoCamereEnum;

namespace SeleniumForInfoCamere.Iterations.Interface
{
    public interface IServiceInfoCamere
    {
        void Login(InfoCamereFascicoloStoricoModel model);
        bool NavigaImprese();
        bool IsLoginPage();
        void StartSearchByCodiceRea(InfoCamereFascicoloStoricoModel model);
        void StartBrowser();
        TipoImpresa CheckPaginaImpresa();
        void LogOut();
        void CloseBrowser();
        byte[] DownloadFascicoloSocieta();
        byte[] DownloadFascicoloCooperative();
        byte[] DownloadFascicoloDittaIndividuale();
    }
}
