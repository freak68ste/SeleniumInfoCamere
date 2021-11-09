using SeleniumForInfoCamere.Iterations.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumForInfoCamere.Models.InfoCamere
{
    public class InfoCamereFascicoloStoricoModel : ISeleniumBase, ISearchBase
    {
        public string UsernameLogin { get; set; }
        public string PasswordLogin { get; set; }
        public TypeSearch SearchMode { get; set; }
        public string SearchElementText { get; set; }

        public string CodiceRea { get; set; }
        public string CodiceFiscale { get; set; }
        public string Provincia { get; set; }
        public TypeSearch TypeSearch { get; set; }
    }
}
