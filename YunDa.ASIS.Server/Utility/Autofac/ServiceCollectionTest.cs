using Advanced.NET6.Business.Interfaces;
using Advanced.NET6.Business.Services;

namespace YunDa.ASIS.Server.Utility.Autofac
{
    public static class ServiceCollectionTest
    {
        public static void Show()
        {
            {
                ////传统工艺
                //IMicrophone microphone = new Microphone();
            }

            //内置容器：
            {
                ////1.创建一个容器 nuget引入：Microsoft.Extensions.DependencyInjection.Abstractions
                ServiceCollection serviceDescriptors = new ServiceCollection();
                //2.注册抽象和具体普通类之间的关系
                serviceDescriptors.AddTransient<IMicrophone, MicrophoneNew>();
                //3.serviceDescriptors.builder一下 
                ServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
                IMicrophone microphone = serviceProvider.GetService<IMicrophone>();
            }

            //如果后续有新的实现： 
            //1.传统工艺来直接New  ---对象创建在项目中是有多个地方
            //2.可以在全局跟一个ioc容器，配置抽象和具体普通之间的关系的时候，可以修改，这里修改了，获取实例就获取新的实例


            //传统工艺创建对象
            {
                IMicrophone microphone = new Microphone();
                IPower power = new Power(microphone);
                IHeadphone headphone = new Headphone(power);
            }

            //IOC容器支持依赖注入
            {
                ServiceCollection serviceDescriptors = new ServiceCollection();
                serviceDescriptors.AddTransient<IMicrophone, Microphone>();
                serviceDescriptors.AddTransient<IPower, Power>();
                serviceDescriptors.AddTransient<IHeadphone, Headphone>();
                ServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
                IHeadphone headphone = serviceProvider.GetService<IHeadphone>();
            }

            //关于ServiceCollectioon生命周期
            //一、AddTransient：瞬时生命周期,每一次创建都是是一个全新的实例
            {
                ServiceCollection serviceDescriptors = new ServiceCollection();
                serviceDescriptors.AddTransient<IMicrophone, Microphone>();
                ServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
                IMicrophone microphone1 = serviceProvider.GetService<IMicrophone>();
                IMicrophone microphone2 = serviceProvider.GetService<IMicrophone>();
                IMicrophone microphone3 = serviceProvider.GetService<IMicrophone>();
                IMicrophone microphone4 = serviceProvider.GetService<IMicrophone>();

                Console.WriteLine($"microphone1 比较 microphone2 ：{object.ReferenceEquals(microphone1, microphone2)}");
                Console.WriteLine($"microphone1 比较 microphone3 ：{object.ReferenceEquals(microphone1, microphone3)}");
                Console.WriteLine($"microphone2 比较 microphone4 ：{object.ReferenceEquals(microphone2, microphone4)}");
            }

            //二、AddSingleton：单例生命周期：同一个类型，创建出来的是同一个实例
            {
                ServiceCollection serviceDescriptors = new ServiceCollection();
                serviceDescriptors.AddSingleton<IMicrophone, Microphone>();
                ServiceProvider serviceProvider = serviceDescriptors.BuildServiceProvider();
                IMicrophone microphone1 = serviceProvider.GetService<IMicrophone>();
                IMicrophone microphone2 = serviceProvider.GetService<IMicrophone>();
                IMicrophone microphone3 = serviceProvider.GetService<IMicrophone>();
                IMicrophone microphone4 = serviceProvider.GetService<IMicrophone>();

                Console.WriteLine($"microphone1 比较 microphone2 ：{object.ReferenceEquals(microphone1, microphone2)}");
                Console.WriteLine($"microphone1 比较 microphone3 ：{object.ReferenceEquals(microphone1, microphone3)}");
                Console.WriteLine($"microphone2 比较 microphone4 ：{object.ReferenceEquals(microphone2, microphone4)}");
            }

            //三、AddScoped：作用域生命周期： 同一个serviceProvider获取到的是同一个实例
            {
                ServiceCollection serviceDescriptors = new ServiceCollection();
                serviceDescriptors.AddScoped<IMicrophone, Microphone>();
                ServiceProvider serviceProvider1 = serviceDescriptors.BuildServiceProvider();
                IMicrophone microphone1 = serviceProvider1.GetService<IMicrophone>();
                IMicrophone microphone2 = serviceProvider1.GetService<IMicrophone>();

                ServiceProvider serviceProvider2 = serviceDescriptors.BuildServiceProvider();
                IMicrophone microphone3 = serviceProvider2.GetService<IMicrophone>();
                IMicrophone microphone4 = serviceProvider2.GetService<IMicrophone>();

                Console.WriteLine($"microphone1 比较 microphone2 ：{object.ReferenceEquals(microphone1, microphone2)}");
                Console.WriteLine($"microphone3 比较 microphone4 ：{object.ReferenceEquals(microphone3, microphone4)}");
                Console.WriteLine($"microphone1 比较 microphone3 ：{object.ReferenceEquals(microphone1, microphone3)}");
                Console.WriteLine($"microphone1 比较 microphone4 ：{object.ReferenceEquals(microphone1, microphone4)}");
            }

            //ServiceCollection 抽象和具体之间多种注册方式
            {
                {
                    ServiceCollection serviceDescriptors = new ServiceCollection();
                    serviceDescriptors.AddTransient<IMicrophone, Microphone>();
                    ServiceProvider serviceProvider1 = serviceDescriptors.BuildServiceProvider();
                    IMicrophone microphone1 = serviceProvider1.GetService<IMicrophone>();
                }
                {
                    ServiceCollection serviceDescriptors = new ServiceCollection();
                    serviceDescriptors.AddTransient(typeof(IMicrophone), typeof(Microphone));
                    ServiceProvider serviceProvider1 = serviceDescriptors.BuildServiceProvider();
                    IMicrophone microphone1 = serviceProvider1.GetService<IMicrophone>();
                }
                //注册抽象和一段业务逻辑
                {
                    ServiceCollection serviceDescriptors = new ServiceCollection();
                    serviceDescriptors.AddTransient(typeof(IPower), provider =>
                    {
                        //在这里可以我们自己来决定如何创建这个对象的实例；可以对创建出来的这个实例，可以做加工
                        IMicrophone microphone = provider.GetService<IMicrophone>();
                        return new Power(microphone);
                    });

                    serviceDescriptors.AddTransient(typeof(IMicrophone), typeof(Microphone));
                    ServiceProvider serviceProvider1 = serviceDescriptors.BuildServiceProvider();
                    IPower power = serviceProvider1.GetService<IPower>();
                }
                {
                    ServiceCollection serviceDescriptors = new ServiceCollection();
                    serviceDescriptors.AddTransient(typeof(Microphone));
                    ServiceProvider serviceProvider1 = serviceDescriptors.BuildServiceProvider();
                    Microphone microphone1 = serviceProvider1.GetService<Microphone>();
                }

                {
                    ServiceCollection serviceDescriptors = new ServiceCollection();
                    serviceDescriptors.AddTransient<Microphone>();
                    ServiceProvider serviceProvider1 = serviceDescriptors.BuildServiceProvider();
                    Microphone microphone1 = serviceProvider1.GetService<Microphone>();
                }
            }

        }
    }
}
