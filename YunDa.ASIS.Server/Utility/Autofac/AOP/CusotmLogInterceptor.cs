using Castle.DynamicProxy;
using Newtonsoft.Json;

namespace Advanced.NET6.Framework.AutofaExt.AOP
{
    public class CusotmLogInterceptor : IInterceptor
    {
        private readonly ILogger<CusotmLogInterceptor> _ILogger;
        public CusotmLogInterceptor(ILogger<CusotmLogInterceptor> logger)
        {
            this._ILogger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            _ILogger.LogInformation($"{invocation.Method.Name} Arguments:{JsonConvert.SerializeObject(invocation.Arguments)}");
            invocation.Proceed(); //这句话的执行就是要去执行真实的方法
            _ILogger.LogInformation($"{invocation.Method.Name} ReturnValue:{JsonConvert.SerializeObject(invocation.ReturnValue)}");
        }
    }
}
