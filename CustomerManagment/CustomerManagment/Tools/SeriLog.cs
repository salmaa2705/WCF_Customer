//using Serilog;
//using Serilog.Events;
//using Serilog.Formatting.Compact;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Web;

//namespace CustomerManagment.Tools.DTO
//{
//    public static class SeriLog
//    {
     
//        public static void ConfigureLogger()
//        {
//            Serilog.Log.Logger = new LoggerConfiguration()
//                        .MinimumLevel.Debug()
//                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information) 
//                        .Enrich.FromLogContext()
//                        .WriteTo.Console(new CompactJsonFormatter()) 
//                        .WriteTo.File("C:\\Users\\Salma.Mouelhi\\Desktop\\CustomerRepo\\CustomerManagment\\CustomerManagment\\Serilog.txt", rollingInterval: RollingInterval.Day) 
                                                                                       
//                        .CreateLogger();
//        }
        

//    }
//}