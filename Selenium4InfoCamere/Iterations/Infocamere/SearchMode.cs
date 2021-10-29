using SeleniumForInfoCamere.Iterations.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumForInfoCamere.Iterations.Infocamere
{
    public class SearchMode : ISearchBase
    {
        public string SearchElementText { get; set; }
        public TypeSearch TypeSearch { get; set; }
    }
}
