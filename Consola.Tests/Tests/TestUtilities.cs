﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy.Hosting.Self;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using Consola.SelfHost;
using System.Collections.ObjectModel;
using System.Text;

namespace Consola.Tests
{
    [TestClass]
    public partial class Tests
    {
        private static PhantomJSDriver browser;
        private static Uri hostLocation = new Uri("http://localhost:80");

        [ClassInitialize]
        public static void initialize(TestContext context)
        {
            startServer();
            startBrowser();
        }

        [ClassCleanup]
        public static void tearDown()
        {
            Host.stop();
            browser.Close();
        }

        [TestInitialize]
        public void test_start()
        {
            browser.Navigate().GoToUrl(hostLocation + "Console");
        }

        private ReadOnlyCollection<LogEntry> jsErrors
        {
            get { return getErrors(LogType.Browser); }
        }

        private ReadOnlyCollection<LogEntry> header
        {
            get { return getErrors("har"); }
        }

        private ReadOnlyCollection<LogEntry> getErrors(string type)
        {
            ILogs logs = browser.Manage().Logs;
            return logs.GetLog(type);
        }

        private string genMessage(string message)
        {
            return String.Format("{0}. {1}", message, getJSErrors());
        }

        protected string getJSErrors()
        {
            if (jsErrors.Count > 0)
            {
                StringBuilder messageBuilder = new StringBuilder("JS Log: ");
                foreach (LogEntry entry in jsErrors)
                {
                    messageBuilder.Append(entry.Message + Environment.NewLine);
                }
                return messageBuilder.ToString();
            }
            return String.Empty;
        }

        private string getLineClass(int lineNumber)
        {
            return String.Format("LN{0}", lineNumber);
        }

        private static void startServer()
        {
            Host.start();           
        }

        private static void startBrowser()
        {
            browser = new PhantomJSDriver();
            browser.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
        }

        private void cout(string text)
        {
            browser.FindElementByClassName("consolaCommand").SendKeys(text);
        }

        private string getLineText(int lineNumber)
        {
            return browser.FindElementByClassName(getLineClass(lineNumber)).Text;
        }
    }
}