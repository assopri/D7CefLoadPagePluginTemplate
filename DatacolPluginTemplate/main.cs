using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Collections.Specialized;
using System.IO;
using System.Data;
using System.Net;
using System.Threading;
using CefBrowserWrapper;
using System.Diagnostics;
using System.Windows.Forms;

namespace Plugin
{
    /// <summary>
    /// Пример простого плагина загрузки страницы. Плагин загружает страницу по ссылке и возвращает исходный код либо ошибку.
    /// </summary>
    public class HandlerClass : PluginInterface.IPlugin
    {
        /// <summary>
        /// Обработчик плагина
        /// </summary>
        /// <param name="parameters">Словарь параметров: ключ - имя параметра (string), 
        /// значение - содержимое параметра (object, который в зависимости от типа плагина (задается в parameters["type"])
        /// и ключа приводится к тому или иному типу) </param>
        /// <param name="error">Переменная (string), в которую возвращается ошибка работы плагина, 
        /// если таковая произошла. Если ошибки не произошло, данная переменная должна оставаться пустой строкой</param>
        /// <returns>Возвращаемое значение - это объект, который может иметь тот или иной тип,
        /// в зависимости от типа плагина (задается в  parameters["type"])</returns>
        public object pluginHandler(Dictionary<string, object> parameters, out string error)
        {
            string retVal = "";
            //try
            //{
            error = "";

            //Проверяем правильно ли подключен плагин в программе
            if (String.Compare(parameters["type"].ToString(), "after_load_page_plugin") != 0)
            {
                throw new Exception("Вы используете неверный тип плагина");
            }
            //throw new TimeoutException();
            //параметр URL текущей страницы
            string url = parameters["url"].ToString();
            CancellationToken ct = (CancellationToken)parameters["cancellation_token"];
            CefBrowserWrapperBase cefBrowserWrapper = (CefBrowserWrapperBase)parameters["cef_browser_wrapper"];
            bool devMode = parameters.ContainsKey("dev");

            #region ВАШ КОД

            // cefBrowserWrapper.ChangeWindowState(FormWindowState.Maximized);
            cefBrowserWrapper.Scroll(100);
            if(devMode) cefBrowserWrapper.EvaluateScript("alert('Push Enter to continue');"); 

            cefBrowserWrapper.ScrollToElement("//input[@name='search_name']");
            if (devMode) cefBrowserWrapper.EvaluateScript("alert('Push Enter to continue');");
            
            //Console.WriteLine("Do smth or just see what is going on in browser and click any button to click");
            //Console.ReadKey();

            cefBrowserWrapper.SetValue("//input[@name='search_name']", "test1");
            //the same as set value, but imitating real user (real event option in Datacol scenarios)
            cefBrowserWrapper.SendTextToElement("//input[@name='search_price_from']", "test2"); 

            cefBrowserWrapper.SendMouseClickToElement("//input[@id='ctrl-prd-cmp-3942']");

            cefBrowserWrapper.Click("//input[@name='advanced_search_in_category']");

            //cefBrowserWrapper.Scroll("//span[contains(text(),'Показать телефон')]");
            //Thread.Sleep(2000);
            //document.evaluate('//span[contains(text(),"Показать телефон")]', document, null, XPathResult.ANY_TYPE, null).iterateNext().getBoundingClientRect();
            //Console.WriteLine(
            //    cefBrowserWrapper.GetHtmlElementClientRect("//a[@class='submit']"));
            //cefBrowserWrapper.GetHtmlElementClientRect("//span[contains(text(),'Показать телефон')]"));

            // cefBrowserWrapper.SendMouseClickToElement("//span[contains(text(),'Показать телефон')]");
            //cefBrowserWrapper.SendMouseClickToElement("//a[@class='submit']");

            cefBrowserWrapper.GetHtml();
            // cefBrowserWrapper.u
            #endregion

            return retVal;
        }

        #region Методы и свойства необходимые, для соответствия PluginInterface (обычно не используются при создании плагина)

        public void Init()
        {
            //инициализация пока не нужна
        }

        public void Destroy()
        {
            //это тоже пока не надо
        }

        public string Name
        {
            get { return "PluginName"; }
        }

        public string Description
        {
            get { return "Описание текущего плагина"; }
        }

        #endregion
    }
}
