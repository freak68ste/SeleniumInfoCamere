using OpenQA.Selenium;
using SeleniumForInfoCamere.Iterations.Interface;
using SeleniumForInfoCamere.Models.InfoCamere;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumForInfoCamere.Iterations.Infocamere
{
    public abstract class AbstractBaseInfoCamere
    {
        protected SeleniumChrome icDriver;

        public bool DoClick(IWebElement item)
        {
            try
            {
                item.Click();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IWebElement GetElement(ISearchBase itemSearch)
        {
            IWebElement retItem;
            switch (itemSearch.TypeSearch)
            {
                case TypeSearch.ByClass:
                    retItem = icDriver.GetElementByClass(itemSearch.SearchElementText);
                    break;
                case TypeSearch.ById:
                    retItem = icDriver.GetElementById(itemSearch.SearchElementText);
                    break;
                case TypeSearch.ByXPath:
                    retItem = icDriver.GetElementByPath(itemSearch.SearchElementText);
                    break;
                default:
                    retItem = null;
                    break;
            }
            return retItem;
        }

        public bool SetText(IWebElement item, string text)
        {
            try
            {
                item.SendKeys(text);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ClearText(IWebElement item)
        {
            try
            {
                item.Clear();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
