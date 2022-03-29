using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced.NET6.Framework.AutofaExt.AOP
{
    public class CusotmCacheInterceptor : IInterceptor
    {
        //缓存：
        //1.key
        //2.value

        private static Dictionary<string, object> _cacheDictionary = new Dictionary<string, object>();
        public void Intercept(IInvocation invocation)
        {
            //使用当前的方法名称作为key
            string cacheKey = invocation.Method.Name;
            {
                //应该在方法计算前就要检查缓存；如果有缓存，就返回结果了，不结算了
            }
            if (_cacheDictionary.ContainsKey(cacheKey))
            {
                invocation.ReturnValue = _cacheDictionary[cacheKey];
            }
            else
            {
                invocation.Proceed(); //这句话的执行就是要去执行真实的方法计算 
                //计算完毕，应该把计算的结果保存到缓存中去
                _cacheDictionary[cacheKey] = invocation.ReturnValue;
            }







        }
    }
}
