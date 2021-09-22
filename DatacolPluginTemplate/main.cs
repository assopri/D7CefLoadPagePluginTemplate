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
using CefSharp.Fluent;
using DatacolPluginTemplate;
using System.Runtime.InteropServices;

namespace Plugin
{
    /// <summary>
    /// Пример простого плагина загрузки страницы. Плагин загружает страницу по ссылке и возвращает исходный код либо ошибку.
    /// </summary>
    public class HandlerClass : PluginInterface.IPlugin
    {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);



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
          
            error = "";

            //Проверяем правильно ли подключен плагин в программе
            if (String.Compare(parameters["type"].ToString(), "after_load_page_plugin") != 0)
            {
                throw new Exception("Вы используете неверный тип плагина");
            }
            
            // URL текущей страницы
            string url = parameters["url"].ToString();
            // токен позволяет отследить, если пользователь остановил работу кампании с помощью ct.IsCancellationRequested
            CancellationToken ct = (CancellationToken)parameters["cancellation_token"];
            // Обертка для доступа к объекту браузера, в том числе командам вроде Click и т.п.
            CefBrowserWrapperBase cefBrowserWrapper = (CefBrowserWrapperBase)parameters["cef_browser_wrapper"];
            // Переменная позволяет добавлять в сценарий элементы для отладки (например сообщения в виде диалогового окна) для случая,
            // если запуск плагина произведен из тестового приложения, а не из программы Datacol
            bool devMode = parameters.ContainsKey("dev");

            BasicScenario(devMode, cefBrowserWrapper);

            // DownloadByClickScenario(cefBrowserWrapper, ct, 50, "//a[@id='click_to_download']");//TODO: замените последний параметр на реальный xpath

            return retVal;
        }

        #region Examples
        private void BasicScenario(bool devMode, CefBrowserWrapperBase cefBrowserWrapper)
        {
            // cefBrowserWrapper.ChangeWindowState(FormWindowState.Maximized);
            cefBrowserWrapper.Scroll(100);
            if (devMode) cefBrowserWrapper.EvaluateScript("alert('Push Enter to continue');");

            cefBrowserWrapper.ScrollToElement("//input[@name='search_name']");
            if (devMode) cefBrowserWrapper.EvaluateScript("alert('Push Enter to continue');");


            cefBrowserWrapper.SetValue("//input[@name='search_name']", "test1");
            //the same as set value, but imitating real user (real event option in Datacol scenarios)
            cefBrowserWrapper.SendTextToElement("//input[@name='search_price_from']", "test2");

            cefBrowserWrapper.SendMouseClickToElement("//input[@id='ctrl-prd-cmp-3942']");

            cefBrowserWrapper.Click("//input[@name='advanced_search_in_category']");

            cefBrowserWrapper.GetHtml();

        }

        private void DownloadByClickScenario(
            CefBrowserWrapperBase cefBrowserWrapper, 
            CancellationToken ct,
            int maxSecondsToWaitForDownload,
            string xpathOfElementToClickOn)
        {
            ManualResetEventSlim downloadReadyEvent = new ManualResetEventSlim(false);

            // To Stop showing download form
            cefBrowserWrapper.DownloadHandler =
                DownloadHandler.UseFolder(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                    (chromiumBrowser, browser, downloadItem, callback) =>
                    {
                        if (downloadItem.IsComplete || downloadItem.IsCancelled)
                        {
                            if (browser.IsPopup && !browser.HasDocument)
                            {
                                browser.GetHost().CloseBrowser(true);
                                downloadReadyEvent.Set();
                            }
                        }
            //TODO: You may wish to customise this condition to better suite your
            //requirements. 
            else if (downloadItem.ReceivedBytes < 100)
                        {
                            var popupHwnd = browser.GetHost().GetWindowHandle();

                            var visible = IsWindowVisible(popupHwnd);
                            if (visible)
                            {
                                const int SW_HIDE = 0;
                                ShowWindow(popupHwnd, SW_HIDE);
                            }
                        }
                    });

            cefBrowserWrapper.Click(xpathOfElementToClickOn);

            downloadReadyEvent.Wait(maxSecondsToWaitForDownload * 1000, ct);
        }
       
        #endregion

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
