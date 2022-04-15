using Microsoft.AspNetCore.Http.Connections;
using SignalRDemo1.Hubs;
using SignalRNotify;


{

    var builder1 = WebApplication.CreateBuilder(args);
    #region SignalR
    builder1.Services.AddSignalR(hubOptions =>
    {
        hubOptions.EnableDetailedErrors = true;
        hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(13);
    });
    #endregion
    // Add services to the container.
    builder1.Services.AddRazorPages();

    var app1 = builder1.Build();

    // Configure the HTTP request pipeline.
    if (!app1.Environment.IsDevelopment())
    {
        app1.UseExceptionHandler("/Error");
    }
    app1.UseStaticFiles();

    app1.UseRouting();

    app1.UseAuthorization();

    app1.MapRazorPages();

    app1.UseEndpoints(endpoints =>
    {
        //endpoints.MapRazorPages();
        //endpoints.MapControllers();
        //endpoints.MapDefaultControllerRoute(); // {controller=Home}/{action=Index}/{id?}

        endpoints.MapHub<ChatHub>("/chatHub", options =>
        {
            options.Transports =
                HttpTransportType.WebSockets |
                HttpTransportType.LongPolling |
                HttpTransportType.ServerSentEvents;
        });
        endpoints.MapHub<NotificationHub>("/notificationHub");
    });


    app1.Run();

}



// Add services to the container.

//builder.Services.Configure<IISOptions>(options =>
//{
//    options.ForwardClientCertificate = true;
//});
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

#region Log4Net
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    //logging.AddConsole();
});
builder.Services.AddLogging(cfg =>
{
    cfg.AddLog4Net(new Log4NetProviderOptions()
    {
        Log4NetConfigFileName = "ConfigFiles/log4net.config",
        Watch = true,
    });
});
builder.Services.AddSingleton<LoggerService>();
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
    builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<MongoDbService>();
#endregion

builder.Services.AddSingleton<BooksService>();

#region Config Authentication/AddAuthorization
// Config Authentication
builder.Services.AddAuthentication(option =>
{
    //options.RequireAuthenticatedSignIn = false;
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, option =>
{
    //如果没有找到用户信息--->鉴权失败-->授权失败--->就跳转到指定的  [HttpGet] Action
    option.LoginPath = "/api/user/login";
    option.AccessDeniedPath = "/api/exception/unauthorized";
});


IConfigurationSection jwtSection = builder.Configuration.GetSection("JWTTokenOptions");
//JWTTokenOptions tokenOptions = new JWTTokenOptions();
//builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
JWTTokenOptions tokenOptions = jwtSection.Get<JWTTokenOptions>();

builder.Services.Configure<JWTTokenOptions>(jwtSection);
builder.Services.AddTransient<IJWTAuthorizeService, JWTAuthorizHSService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // 配置 鉴权验证 规则
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //JWT有一些默认的属性，就是给鉴权时就可以筛选了
            ValidateIssuer = true,//是否验证Issuer
            ValidateAudience = true,//是否验证Audience
            ValidateLifetime = true,//是否验证失效时间
            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            ValidAudience = tokenOptions.Audience,//
            ValidIssuer = tokenOptions.Issuer,//Issuer，这两项和前面签发jwt的设置一致
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//拿到SecurityKey 
        };
    }
);


// Config Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizePolicy.UserPolicy, policyBuilder =>
    {
        policyBuilder.RequireRole("Admin");
        policyBuilder.RequireClaim("Account", "Administrator");
        policyBuilder.AddRequirements(new AuthKeyRequirement());

        policyBuilder.RequireAssertion(context =>
        {
            bool pass1 = context.User.HasClaim(c => c.Type == ClaimTypes.Role);
            bool pass2 = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Admin";
            bool pass3 = context.User.Claims.Any(c => c.Type == ClaimTypes.Name);
            bool pass = pass1 && pass2 && pass3;
            return pass;
        });
    });
});
builder.Services.AddTransient<IAuthorizationHandler, AuthKeyHander>();

#endregion

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
});

#region Controller Filter Json
//builder.Services.AddControllers(options =>
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomAllActionResultFilterAttribute>(); // 全局注册 Filter
}).AddNewtonsoftJson(options =>
{
    // Use the default property (Pascal) casing
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

    // Configure a custom converter
    options.SerializerSettings.Converters.Add(new DateTimeJsonConverter());
    options.SerializerSettings.Converters.Add(new DateTimeOffsetJsonConverter());
});
//.AddJsonOptions( 框架自带 Json
//        options => /*options.JsonSerializerOptions.PropertyNamingPolicy = null */);

#endregion

#region SwaggerGen
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(t => t.FullName);
    //[ApiExplorerSettings(GroupName = "V1"))]
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Shen12DP",
        Version = "v1",
        Description = "Swagger for Api Test",
    });
    //  为Swagger JSON and UI设置xml文档注释路径 
    //string basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
    string basePath = AppContext.BaseDirectory;
    string xmlName = typeof(Program).Assembly.GetName().Name + ".xml";
    // 项目属性 -> 文档文件 -> [√]生成包含 API 文档的文件
    var filePath = Path.Combine(basePath, xmlName);
    c.IncludeXmlComments(filePath);
});

#endregion

#region Cors
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("X-Pagination"));
});
#endregion

#region Autofac AOP
// NutGet: Autofac、Castle.Core、Autofac.Extras.DynamicProx
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); // 通过工厂替换，把Autofac整合进来
builder.Host.ConfigureContainer<ContainerBuilder>((containerBuilder) =>
{
    containerBuilder.RegisterType<CusotmInterceptor>();
    containerBuilder.RegisterType<CusotmLogInterceptor>();
    containerBuilder.RegisterType<CusotmCacheInterceptor>();

    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
    containerBuilder.RegisterType<Power>().As<IPower>();
    containerBuilder.RegisterType<Headphone>().As<IHeadphone>();

    containerBuilder.RegisterType<ApplePhone>().As<IPhone>()
        //.EnableClassInterceptors(); //当前的配置 要支持AOP扩展--通过类来支持 virtual 方法
        //.EnableInterfaceInterceptors() // 当前的配置 要支持AOP扩展--通过 接口 来支持
        .EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions()
        {
            // use InterceptorSelector
            Selector = new CustomInterceptorSelector()
        })
        ;

    // 对指定的类型 指定 拦截器
    //containerBuilder.RegisterAssemblyTypes(Assembly.Load("YunDa.ASIS.Server"))
    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .Where(s =>
        {
            var pass = s.Name.Contains("Power");
            return pass;
        })
        .PropertiesAutowired()
        .AsImplementedInterfaces()
        .EnableInterfaceInterceptors()
        .InterceptedBy(typeof(CusotmLogInterceptor)) // 指定拦截器
        ;


    // 注册 每个 Controller, 并开启属性注入功能
    var ctrllerBaseType = typeof(ControllerBase);
    containerBuilder.RegisterAssemblyTypes(typeof(Program).Assembly)
        .Where(t => ctrllerBaseType.IsAssignableFrom(t) && t != ctrllerBaseType)
        .PropertiesAutowired(new CusotmPropertySelector());
    #region another way
    //var ctrllersTypesInDll = typeof(Program).Assembly.GetExportedTypes()
    //        .Where(t => ctrllerBaseType.IsAssignableFrom(t)).ToArray();

    //containerBuilder.RegisterTypes(ctrllersTypesInDll)
    //    .PropertiesAutowired(new CusotmPropertySelector());
    #endregion
});

// 支持控制器的实例 由 IOC容器--Autofac 进行创建
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
#endregion

#region SignalR
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(13);
});
#endregion


builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    app.UseSwagger();
    app.UseSwaggerUI();

    var options = new DeveloperExceptionPageOptions();
    options.SourceCodeLineCount = 13;
    app.UseDeveloperExceptionPage(options); // 异常页面
}
else
{
    app.UseExceptionHandler("/Error"); // UseExceptionHandler 是添加到管道中的第一个中间件组件。因此，异常处理程序中间件会捕获以后调用中发生的任何异常
    app.UseHsts(); // https 相关
}

app.UseCors("CorsPolicy");
app.UseRouting();        // 路由 中间件
app.UseAuthentication(); // 身份验证 中间件 在允许用户访问安全资源之前尝试对用户进行身份验证
app.UseAuthorization();  // 身份授权 中间件 授权用户访问安全资源
app.UseDefaultFiles();   // 默认文件中间件
app.UseStaticFiles();    // 静态文件中间件
app.UseFileServer(enableDirectoryBrowsing: true); // 文件浏览
app.UseHttpsRedirection();
//app.UseEndpoints(options =>
//{
//   
//   
//});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=user}/{action=index}/{id?}");

// 自定义中间件
app.UseMiddleware<MinimalApiMiddleware>();

app.MapControllers();
app.MapRazorPages();

#region Exception Handler
//app.UseStatusCodePages(Application.Json, "Status Code Page: {0}");
//app.UseStatusCodePages(async statusCodeContext =>
//{
//    // using static System.Net.Mime.MediaTypeNames;
//    statusCodeContext.HttpContext.Response.ContentType = Application.Json;

//    await statusCodeContext.HttpContext.Response.WriteAsync(
//        $"Status Code Page: {statusCodeContext.HttpContext.Response.StatusCode}");
//});
//app.UseStatusCodePagesWithRedirects("/StatusCode/{0}");
//app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
//app.UseStatusCodePagesWithReExecute("/StatusCode", "?statusCode={0}");
//app.UseStatusCodePagesWithReExecute("/Error/{0}");//只要不是200 都能进来

app.UseExceptionHandler(config =>
{
    config.Run(async context =>
    {
        //context.Response.StatusCode = 500;
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = Application.Json; //  Text.Plain

        LoggerService.Error((context.Request.Method).PadLeft(5) + context.Request.Path);

        string url = string.Empty;
        var statusCodeReExecuteFeature =
                context.Features.Get<IStatusCodeReExecuteFeature>();
        if (statusCodeReExecuteFeature != null)
        {
            url = string.Join(
               statusCodeReExecuteFeature.OriginalPathBase,
               statusCodeReExecuteFeature.OriginalPath,
               statusCodeReExecuteFeature.OriginalQueryString);
        }

        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            var ex = error.Error;
            var errorStr = JsonConvert.SerializeObject(ex);
            await context.Response.WriteAsync(errorStr);
            LoggerService.Error(url, ex);
        }
    });
});

#endregion

////app.MapRazorPages();
//app.Run();

//app.Use(async (context, next) =>
//{
//    context.Response.ContentType = $"{Text.Plain};chartset=utf-8";
//    // beofre ...
//    LoggerService.Info("await next() before...");
//    await next(); // call next middleware
//    // after ...
//    LoggerService.Info("await next() after...");
//});

app.UseEndpoints(endpoints =>
{
    //endpoints.MapRazorPages();
    //endpoints.MapControllers();
    //endpoints.MapDefaultControllerRoute(); // {controller=Home}/{action=Index}/{id?}

    endpoints.MapHub<ChatHub>("/chatHub", options =>
    {
        options.Transports =
            HttpTransportType.WebSockets |
            HttpTransportType.LongPolling |
            HttpTransportType.ServerSentEvents;
    });
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.UseUserMiniApi();
app.UsePhoneMiniApi();
app.UseServiceLocator();

app.Run();
