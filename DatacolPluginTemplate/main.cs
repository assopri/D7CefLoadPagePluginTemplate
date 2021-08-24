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
            try
            {
                error = "";

                //Проверяем правильно ли подключен плагин в программе
                if (String.Compare(parameters["type"].ToString(), "load_page_plugin") != 0)
                {
                    throw new Exception("Вы используете неверный тип плагина");
                }

                //параметр URL текущей страницы
                string url = parameters["url"].ToString();
                
                #region Дополнительные возможные параметры (можно использовать при необходимости)
                                
                ////параметр уровень вложенности загружаемой страницы
                //int nestinglevel = Convert.ToInt32(parameters["nestinglevel"].ToString());
                ////параметр реферер для загружаемой страницы
                //string referer = parameters["referer"].ToString();
                ////параметр флаг использования прокси при загрузке
                //bool useproxy = Convert.ToBoolean(parameters["useproxy"].ToString());
                ////параметр объект Загрузчик Datacol
                //DatacolHttp http = (DatacolHttp)parameters["datacolhttp"];
                ////параметр имя прокси чекера
                //string checkername = parameters["checkername"].ToString();
                ////параметр режим использования прокси (список или из прокси чекера)
                //string proxymode = parameters["proxymode"].ToString();
                ////параметр предопределенный прокси для загрузки страницы
                //WebProxy webproxy = (WebProxy)parameters["webproxy"];
                ////параметр предопределенная кодировка загружаемой страницы
                //string encoding = parameters["encoding"].ToString();
                //WebProxy usedProxy = new WebProxy();

                #endregion

                #region ВАШ КОД

                retVal = "loaded url pagecode";

                #endregion

                return retVal;
            }
            catch (Exception exp)
            {
                error = exp.Message;
                return "";
            }
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
