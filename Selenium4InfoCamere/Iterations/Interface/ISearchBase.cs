using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumForInfoCamere.Iterations.Interface
{
    public interface ISearchBase
    {
        public string SearchElementText { get; set; }
        public TypeSearch TypeSearch { get; set; }
    }

    public enum TypeSearch
    {
        ById = 1,
        ByClass = 2,
        ByXPath = 3
    }

}
