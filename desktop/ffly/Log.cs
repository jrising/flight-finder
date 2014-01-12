using System;
using log4net;
using log4net.Config;

namespace ffly
{
	public class Log
	{
		static Log()
		{
			log4net.Config.BasicConfigurator.Configure();
		}
		
		public static void Debug(string txt) {
			log4net.ILog log = log4net.LogManager.GetLogger("ffly");
			log.Debug(txt);
		}
		
		public static void Info(string txt) {
			log4net.ILog log = log4net.LogManager.GetLogger("ffly");
			log.Info(txt);			
		}
		
		public static void Error(string txt) {
			log4net.ILog log = log4net.LogManager.GetLogger("ffly");
			log.Error(txt);			
		}
	}
}

