using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerManagment.Tools
{
    public class Log
    {
        private static readonly ILog logger =
           LogManager.GetLogger(typeof(Log));
        static Log()
        {
            XmlConfigurator.Configure();
        }

        public static void WriteLine(string txt, int i = 0)
        {
            switch (i)
            {
                // Lors du debug
                case 1:
                    logger.Debug(txt);
                    break;

                // Exception ou Erreur
                case 2:
                    logger.Warn(txt);
                    break;

                // Cas ordinaire
                default:
                    logger.Info(txt);
                    break;
            }
        }

    }
}