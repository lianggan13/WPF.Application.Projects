using Advanced.NET6.Business.Interfaces;
using Castle.DynamicProxy;
using System.Reflection;
using YunDa.ASIS.Server.Services;

namespace Advanced.NET6.Framework.AutofaExt.AOP
{
    public class CustomInterceptorSelector : IInterceptorSelector
    {
        //private CusotmLogInterceptor _CusotmLogInterceptor;
        //public CustomInterceptorSelector(CusotmLogInterceptor cusotmLogInterceptor)
        //{
        //    this._CusotmLogInterceptor = cusotmLogInterceptor;
        //}

        /// <summary>
        /// 让我们选择使用哪个IInterceptor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="interceptors"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            if (type == typeof(IPower))
            {
                return new IInterceptor[] {
                               //new CusotmCacheInterceptor(),
                               //new CusotmInterceptor()
                                 ServiceLocator.GetService<CusotmLogInterceptor>(),
                                };
            }
            else
            {
                return new IInterceptor[] {
                               new CusotmCacheInterceptor(),
                               new CusotmInterceptor()
                                 //_CusotmLogInterceptor
                                };
            }
        }
    }
}
