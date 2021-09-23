using CefBrowserWrapper;
using Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://webasyst.synoparser.ru/index.php?categoryID=723";
            HandlerClass hc = new HandlerClass();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string error = string.Empty;
            parameters.Add("campaignname", "Test");
            parameters.Add("dev", "");
            parameters.Add("type", "after_load_page_plugin");
            parameters.Add("url", url);
            parameters.Add("cancellation_token", CancellationToken.None);
            
            CefBrowserWrapperFactoryBase factory = new UniCefBrowserWrapperFactory(true);
            CefBrowserWrapperBase cefBrowserWrapper = factory.Create(true, true, 10000, false, new SingleBrowserInfo("", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36"));
            parameters.Add("cef_browser_wrapper", cefBrowserWrapper);

            try
            {
                
                cefBrowserWrapper.LoadUrl(url);

                hc.pluginHandler(parameters, out error);

                Console.WriteLine("Click any button to finish");
                Console.ReadKey();

                cefBrowserWrapper.Dispose();
                factory.Dispose();

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
