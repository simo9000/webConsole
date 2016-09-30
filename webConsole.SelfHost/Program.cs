﻿using System.Web.Http;
using Microsoft.Owin.Hosting;
using System;

namespace webConsole.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://+:8080";
            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
