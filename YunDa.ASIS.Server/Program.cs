using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Serialization;
using YunDa.ASIS.Server.Filters;
using YunDa.ASIS.Server.Providers;
using YunDa.ASIS.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.Configure<IISOptions>(options =>
//{
//    options.ForwardClientCertificate = true;
//});

#region Log4Net
{
    ////Nuget 包引入：
    ////1.Log4Net
    ////2.Microsoft.Extensions.Logging.Log4Net.AspNetCore
    builder.Services.AddLogging(cfg =>
    {
        //默认的配置文件路径是在根目录，且文件名为log4net.config
        //如果文件路径或名称有变化，需要重新设置其路径或名称
        //比如在项目根目录下创建一个名为cfg的文件夹，将log4net.config文件移入其中，并改名为log.config
        //则需要使用下面的代码来进行配置
        cfg.AddLog4Net(new Log4NetProviderOptions()
        {
            Log4NetConfigFileName = "ConfigFiles/log4net.config",
            Watch = true
        });

    });
}

#endregion

#region NLogin
{
    //Nuget引入：NLog.Web.AspNetCore
    //builder.Logging.AddNLog("ConfigFiles/NLog.config");
}
#endregion

#region Session
//builder.Services.AddSession();
#endregion

#region Database
builder.Services.Configure<MongoDbSettings>(
builder.Configuration.GetSection("MongoDbSettings")
);
#endregion

builder.Services.AddSingleton<BooksService>();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddSingleton<LoggerService>();


//builder.Services.adds
//builder.Services.AddAuthentication(options=>options.AddScheme())
// 注册
builder.Services.AddTransient<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
// 选择何种方式 鉴权(身份验证)
builder.Services.AddAuthentication(
    options =>
    {
        options.RequireAuthenticatedSignIn = false;
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "";
    }); // 注册 cookie 认证服务
builder.Services.AddAuthorization(options =>
{
    // 验证策略(多个)
    options.AddPolicy("testpolicy", builder =>
    {
        builder.RequireClaim("user", "delete");
    });
    options.AddPolicy("testpolicy1", builder =>
    {
        builder.RequireRole("admin");
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<CustomGlobalActionFilterAttribute>(); // 全局注册 Filter
}).AddNewtonsoftJson(options =>
{
    // Use the default property (Pascal) casing
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

    // Configure a custom converter
    //options.SerializerSettings.Converters.Add(new MyCustomJsonConverter());
});
//.AddJsonOptions(
//        options => /*options.JsonSerializerOptions.PropertyNamingPolicy = null */);


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});


var app = builder.Build();



var db = app.Services.GetService<MongoDbService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Error"); // UseExceptionHandler 是添加到管道中的第一个中间件组件。因此，异常处理程序中间件会捕获以后调用中发生的任何异常
    app.UseHsts();
}

app.UseRouting();        // 路由 中间件
app.UseAuthentication(); // 身份验证 中间件 在允许用户访问安全资源之前尝试对用户进行身份验证
app.UseAuthorization();  // 身份授权 中间件 授权用户访问安全资源
app.UseStaticFiles();

app.UseServiceLocator();

// 自定义中间件
//app.Use(async (context, next) =>
//{
//    // Do work that doesn't write to the Response.
//    Console.WriteLine("Do work that doesn't write to the Response");
//    await next.Invoke();
//    // Do loggin or other work that does't write to the Response.
//    Console.WriteLine("Do loggin or other work that does't write to the Response.");
//});
//app.UseMiddleware<LogsMiddleware>(); //添加日志记录中间件

app.MapControllers();


// no use controller
app.MapGet("/api/heart", () =>
{
    return "ok";
})
.WithName("Heart");



app.Run();
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Append response end.");
//});
