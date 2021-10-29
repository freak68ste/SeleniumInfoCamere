using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumForInfoCamere.Iterations.Interface
{   
    public interface ISeleniumBase
    {
        public string UsernameLogin { get; set; }
        public string PasswordLogin { get; set; }
    }

}
