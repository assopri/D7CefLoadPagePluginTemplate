using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plugin;
using System.Data;
using System.Collections.Generic;

namespace PluginUnitTest
{
    [TestClass]
    public class UnitTest
    {
        
        [TestMethod]
        public void TestPageLoadPlugin()
        {
            HandlerClass hc = new HandlerClass();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            string error = string.Empty;
            parameters.Add("campaignname", "Test");
            parameters.Add("type", "load_page_plugin");
            parameters.Add("url", "https://google.com");

            hc.pluginHandler(parameters, out error);

            Assert.AreEqual("", error);
        }


    }
}
