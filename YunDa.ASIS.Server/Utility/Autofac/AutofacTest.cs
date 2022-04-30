using Advanced.NET6.Business.Interfaces;
using Advanced.NET6.Business.Services;
using Autofac;
using System.Reflection;

namespace YunDa.ASIS.Server.Utility.Autofac
{
    public static class AutofacTest
    {
        public static void Show()
        {
            //1.Nuget引入程序包
            //2.得到容器的建造者
            //3.配置抽象和具体类之间的关系
            //4.Build一下得到容器实例
            //5.基于容器来获取对象的实例了
            {
                ContainerBuilder containerBuilder = new ContainerBuilder();
                containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                IContainer container = containerBuilder.Build();
                IMicrophone microphone = container.Resolve<IMicrophone>();

            }

            //关于Autofac容器的多种注册
            {

                //注册抽象和具体普通类 RegisterType
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    IContainer container = containerBuilder.Build();
                    IMicrophone microphone = container.Resolve<IMicrophone>();
                }

                //注册一个具体的实例 RegisterInstance
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterInstance(new Microphone());
                    IContainer container = containerBuilder.Build();
                    IMicrophone microphone = container.Resolve<Microphone>();
                }

                {
                    ////注册一段业务逻辑 Register
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    containerBuilder.Register<IPower>(context =>
                    {
                        //这里的业务逻辑负责创建出IPower 的实例---可以给一个入口，我们自己来创建对象的实例
                        IMicrophone microphone = context.Resolve<IMicrophone>();
                        IPower power = new Power(microphone);

                        return power;
                    });
                    IContainer container = containerBuilder.Build();
                    IPower power = container.Resolve<IPower>();
                }

                //注册泛型 RegisterGeneric
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterGeneric(typeof(List<>)).As(typeof(IList<>));
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    IContainer container = containerBuilder.Build();
                    IList<IMicrophone> microphonelist = container.Resolve<IList<IMicrophone>>();
                }

                //注册程序集 RegisterAssemblyTypes
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    Assembly interfaceAssembly = Assembly.LoadFrom("Advanced.NET6.Business.Interfaces.dll");
                    Assembly serviceAssembly = Assembly.LoadFrom("Advanced.NET6.Business.Services.dll");
                    containerBuilder.RegisterAssemblyTypes(interfaceAssembly, serviceAssembly).AsImplementedInterfaces();
                    IContainer container = containerBuilder.Build();
                    IEnumerable<IMicrophone> microphonelist = container.Resolve<IEnumerable<IMicrophone>>();
                }

            }

            //Autofac支持构造函数注入
            {
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    containerBuilder.RegisterType<Power>().As<IPower>();
                    IContainer container = containerBuilder.Build();
                    IPower power = container.Resolve<IPower>();
                }

                //如果有多个构造函数，默认选择构造函数参数最多构造函数了
                //如果希望选择其中只有一个构造函数参数的构造函数 UsingConstructor
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>()
                        .As<IMicrophone>();
                    containerBuilder.RegisterType<Power>()
                         .UsingConstructor(typeof(IMicrophone), typeof(IMicrophone)) //选择类型为IMicrophone 且参数的数量只有一个的构造函数
                        .As<IPower>();
                    IContainer container = containerBuilder.Build();
                    IPower power = container.Resolve<IPower>();
                }

                //属性注入 PropertiesAutowired
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    containerBuilder.RegisterType<Power>().As<IPower>();
                    containerBuilder.RegisterType<Headphone>().As<IHeadphone>();
                    containerBuilder.RegisterType<ApplePhone>().As<IPhone>()
                        .PropertiesAutowired(); //表示要支持属性注入： 在对象创建出来以后，自动给属性创建实例，赋值上去

                    IContainer container = containerBuilder.Build();
                    IPhone iPhone = container.Resolve<IPhone>();
                }

                //属性注入--支持 PropertySelector
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    containerBuilder.RegisterType<Power>().As<IPower>();
                    containerBuilder.RegisterType<Headphone>().As<IHeadphone>();
                    containerBuilder.RegisterType<ApplePhone>().As<IPhone>()
                        .PropertiesAutowired(new CusotmPropertySelector()); //表示要支持属性注入： 在对象创建出来以后，自动给属性创建实例，赋值上去

                    IContainer container = containerBuilder.Build();
                    IPhone iPhone = container.Resolve<IPhone>();
                }

                //方法注入 OnActivated
                {
                    ContainerBuilder containerBuilder = new ContainerBuilder();
                    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                    containerBuilder.RegisterType<Power>().As<IPower>();
                    containerBuilder.RegisterType<Headphone>().As<IHeadphone>();
                    containerBuilder.RegisterType<ApplePhone>().As<IPhone>()
                        .OnActivated(activa =>
                        {
                            IPower power = activa.Context.Resolve<IPower>();
                            activa.Instance.Init123456678890(power);
                        });

                    IContainer container = containerBuilder.Build();
                    IPhone iPhone = container.Resolve<IPhone>();
                }


            }

            //单抽象多实现
            //1.默认创建出的对象是后面注册的这个Service Keyed
            //2.需要在注册的时候，给定一个标识，然后在获取的时候，也把标识指定，就会按照标识来匹配创建对象
            {
                ContainerBuilder containerBuilder = new ContainerBuilder();
                //containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
                //containerBuilder.RegisterType<MicrophoneNew>().As<IMicrophone>();

                containerBuilder.RegisterType<Microphone>().Keyed<IMicrophone>("Microphone");
                containerBuilder.RegisterType<MicrophoneNew>().Keyed<IMicrophone>("MicrophoneNew");

                IContainer container = containerBuilder.Build();
                //IMicrophone microphone = container.Resolve<IMicrophone>();
                IEnumerable<IMicrophone> microphonelist = container.Resolve<IEnumerable<IMicrophone>>();

                IMicrophone microphone2 = container.ResolveKeyed<IMicrophone>("Microphone");
                IMicrophone microphone3 = container.ResolveKeyed<IMicrophone>("MicrophoneNew");
            }

        }
    }
}
