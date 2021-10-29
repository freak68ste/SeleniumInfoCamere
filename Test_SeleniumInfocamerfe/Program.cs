using System;
using SeleniumForInfoCamere;

namespace Test_SeleniumInfocamerfe
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = new SeleniumChrome("http://www.google.it");

            driver.StartBrowser();

            driver.CloseBrowser();

            Console.WriteLine("Hello World!");
        }
    }
}
