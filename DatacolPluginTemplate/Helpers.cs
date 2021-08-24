using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DatacolPluginTemplate
{
    public static class Helpers
    {
        
        public static Dictionary<string, object> GetDictionaryParams(string filename, string encoding = "")
        {
            Encoding encode = null;

            if (encoding == "")
            {
                encode = Encoding.Default;
            }
            else
            {
                encode = Encoding.GetEncoding(encoding);
            }
            string filecontent = string.Empty;
            Dictionary<string, object> dictParams = new Dictionary<string, object>();
            if (!File.Exists(filename))
            {
                return dictParams;
            }
            else
            {
                filecontent = File.ReadAllText(filename, encode);

                MatchCollection parameters = Regex.Matches(filecontent, "<dc5par[^<>]*?type=\"([^<>]*?)\"[^<>]*?name=\"([^<>]*?)\"[^<>]*?>(.*?)</dc5par>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

                string type = string.Empty;
                string name = string.Empty;
                string value = string.Empty;

                int paramInt = -1;

                List<string> paramList = new List<string>();

                foreach (Match param1 in parameters)
                {


                    type = param1.Groups[1].Value.Trim();
                    name = param1.Groups[2].Value.Trim();
                    value = param1.Groups[3].Value.Trim();
                    if (type == "list-string")
                    {

                        paramList = GetAllLines(value, true);

                        dictParams.Add(name, paramList);
                    }
                    else if (type == "string")
                    {
                        dictParams.Add(name, value);
                    }
                    else if (type == "int")
                    {
                        paramInt = Convert.ToInt32(value);
                        dictParams.Add(name, paramInt);
                    }
                    else
                    {
                        throw new Exception("Тип параметра " + type + " в файле конфигурации " + filename + "не поддерживается");
                    }

                }

            }
            return dictParams;
        }

        /// <summary>
        /// Получение списка строк из текста
        /// </summary>
        /// <param name="completeStr">текст</param>
        /// <param name="unique">флаг уникальности получаемых строк относительно друг друга</param>
        /// <returns>Список строк текста</returns>
        public static List<string> GetAllLines(string completeStr, bool unique = false)
        {
            List<string> SCollection = new List<string>();

            try
            {
                string[] oneStrings = completeStr.Split('\n');

                //для каждой строки, содержащей информацию о прокси сервере (максимум адрес:порт:логин:пароль)
                foreach (string oneString in oneStrings)
                {
                    string trimmed = oneString.Trim();
                    if (String.IsNullOrEmpty(trimmed)) continue;
                    if (unique)
                    {
                        if (SCollection.Contains(trimmed)) continue;
                    }

                    SCollection.Add(trimmed);

                }
                return SCollection;
            }
            catch
            {
                return SCollection;
            }


        }
    }
}
