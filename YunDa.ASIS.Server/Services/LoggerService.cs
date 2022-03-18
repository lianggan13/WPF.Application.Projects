namespace YunDa.ASIS.Server.Services
{
    public class LoggerService
    {
        private readonly ILogger<LoggerService> logger;
        private static LoggerService? instance;

        public static LoggerService? Instance
        {
            get
            {
                if (instance == null)
                    instance = ServiceLocator.GetService<LoggerService>();
                return instance;
            }
        }

        public LoggerService(ILogger<LoggerService> logger, ILoggerFactory loggerFactory)
        {
            this.logger = logger;
            //this._Logger.LogInformation($"{this.GetType().Name} 被构造了。。。_Logger");
            //ILogger<LoggerService> _Logger2 = loggerFactory.CreateLogger<LoggerService>();
            //_Logger2.LogInformation($"{this.GetType().Name} 被构造了。。。_Logger2");
        }

        public static void Info(string? text, Exception? ex = null)
        {
            Instance?.logger.LogInformation(ex, text);
        }

        public static void Debug(string? text, Exception? ex = null)
        {
            Instance?.logger.LogDebug(ex, text);
        }

        public static void Warn(string? text, Exception? ex = null)
        {
            Instance?.logger.LogWarning(ex, text);
        }

        public static void Error(string? text, Exception? ex = null)
        {
            Instance?.logger.LogError(ex, text);
        }

        public static void Critical(string? text, Exception? ex = null)
        {
            Instance?.logger.LogCritical(ex, text);
        }

    }
}
