namespace YunDa.ASIS.Server.Services
{
    public static class ServiceLocator
    {
        private static IServiceProvider? provider;

        /// <summary>
        /// 服务定位器
        /// </summary>
        /// <param name="app"></param>
        public static void UseServiceLocator(this IApplicationBuilder app)
        {
            provider = app.ApplicationServices;
        }

        /// <summary>
        /// 手动获取注入的对象实例
        /// (注: 仅对注入方式 AddTransient 和 AddSingleton 有效)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T? GetService<T>()
        {
            if (provider == null)
                return default(T);
            return provider.GetService<T>();
        }
    }
}
