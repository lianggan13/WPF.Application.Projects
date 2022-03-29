using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced.NET6.Framework.AutofaExt.AOP
{

    //1.Nuget引入：Castle.Core
    //2.扩展一个IInterceptor 实现方法
    //3.注册对象和具体之间的关系的时候，需要执行要支持AOP扩展EnableClassInterceptors
    //4.把要扩展aop的方法定义为  virtual 方法
    //5.把扩展的IInterceptor 也要注册到容器中去


    //一、通过EnableClassInterceptors 来支持的时候
    //1.需要把 Intercept标记到具体的而实现类上--扩展IInterceptor也要引用进来
    //2.特点：必须要是虚方法才会进入到  扩展IInterceptor 来---才能支持aop扩展


    //二、通过EnableInterfaceInterceptors来支持的时候
    //1.需要把 Intercept标记到抽象--接口--扩展IInterceptor也要引用到抽象这
    //2.特点：只要是实现了这接口，无论是否是虚方法，都可以进入到IInterceptor 中来，也就是都可以支持AOP扩展
    public class CusotmInterceptor : IInterceptor
    {
        /// <summary>
        /// 切入者逻辑
        /// 
        /// 使用了Intercept 方法把 要调用的Call方法给包裹起来了
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            {
                Console.WriteLine("Before");
            }
            invocation.Proceed(); //这句话的执行就是要去执行真实的方法
            {
                Console.WriteLine("After");
            }

        }
    }
}
