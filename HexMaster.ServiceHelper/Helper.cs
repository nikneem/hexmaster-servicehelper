﻿using HexMaster.ServiceHelper;
using HexMaster.Threading;
using HexMaster.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace HexMaster
{
    public static class Helper
    {
        public static void Run(this IEnumerable<ServiceBase> services)
        {
            if (Debugger.IsAttached)
            {
                var t = Task.Factory.StartNew
                (
                    () =>
                    {
                        var app = new App();
                        app.InitializeComponent();
                        app.Startup += (o, e) =>
                        {
                            var window = new Main
                            {
                                Width = 300,
                                Height = 200,
                            };
                            window.Services = services;
                            window.Show();
                        };
                        app.Run();
                    },
                    CancellationToken.None,
                    TaskCreationOptions.PreferFairness,
                    new StaticThreadTaskScheduler(25)
                );
                t.Wait();
            }
            else
            {
                ServiceBase.Run(services.ToArray());
            }
        }
    }
}
