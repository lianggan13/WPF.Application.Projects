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
    //Nuget���룺NLog.Web.AspNetCore
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
    //���û���ҵ��û���Ϣ--->��Ȩʧ��-->��Ȩʧ��--->����ת��ָ����  [HttpGet] Action
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
        // ���� ��Ȩ��֤ ����
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
            ValidateIssuer = true,//�Ƿ���֤Issuer
            ValidateAudience = true,//�Ƿ���֤Audience
            ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
            ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
            ValidAudience = tokenOptions.Audience,//
            ValidIssuer = tokenOptions.Issuer,//Issuer���������ǰ��ǩ��jwt������һ��
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))//�õ�SecurityKey 
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
    options.Filters.Add<CustomAllActionResultFilterAttribute>(); // ȫ��ע�� Filter
}).AddNewtonsoftJson(options =>
{
    // Use the default property (Pascal) casing
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();

    // Configure a custom converter
    options.SerializerSettings.Converters.Add(new DateTimeJsonConverter());
    options.SerializerSettings.Converters.Add(new DateTimeOffsetJsonConverter());
});
//.AddJsonOptions( ����Դ� Json
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
    //  ΪSwagger JSON and UI����xml�ĵ�ע��·�� 
    //string basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
    string basePath = AppContext.BaseDirectory;
    string xmlName = typeof(Program).Assembly.GetName().Name + ".xml";
    // ��Ŀ���� -> �ĵ��ļ� -> [��]���ɰ��� API �ĵ����ļ�
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
// NutGet: Autofac��Castle.Core��Autofac.Extras.DynamicProx
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()); // ͨ�������滻����Autofac���Ͻ���
builder.Host.ConfigureContainer<ContainerBuilder>((containerBuilder) =>
{
    containerBuilder.RegisterType<CusotmInterceptor>();
    containerBuilder.RegisterType<CusotmLogInterceptor>();
    containerBuilder.RegisterType<CusotmCacheInterceptor>();

    containerBuilder.RegisterType<Microphone>().As<IMicrophone>();
    containerBuilder.RegisterType<Power>().As<IPower>();
    containerBuilder.RegisterType<Headphone>().As<IHeadphone>();

    containerBuilder.RegisterType<ApplePhone>().As<IPhone>()
        //.EnableClassInterceptors(); //��ǰ������ Ҫ֧��AOP��չ--ͨ������֧�� virtual ����
        //.EnableInterfaceInterceptors() // ��ǰ������ Ҫ֧��AOP��չ--ͨ�� �ӿ� ��֧��
        .EnableInterfaceInterceptors(new Castle.DynamicProxy.ProxyGenerationOptions()
        {
            // use InterceptorSelector
            Selector = new CustomInterceptorSelector()
        })
        ;

    // ��ָ�������� ָ�� ������
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
        .InterceptedBy(typeof(CusotmLogInterceptor)) // ָ��������
        ;


    // ע�� ÿ�� Controller, ����������ע�빦��
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

// ֧�ֿ�������ʵ�� �� IOC����--Autofac ���д���
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
    app.UseDeveloperExceptionPage(options); // �쳣ҳ��
}
else
{
    app.UseExceptionHandler("/Error"); // UseExceptionHandler ����ӵ��ܵ��еĵ�һ���м���������ˣ��쳣��������м���Ჶ���Ժ�����з������κ��쳣
    app.UseHsts(); // https ���
}

app.UseCors("CorsPolicy");
app.UseRouting();        // ·�� �м��
app.UseAuthentication(); // �����֤ �м�� �������û����ʰ�ȫ��Դ֮ǰ���Զ��û����������֤
app.UseAuthorization();  // �����Ȩ �м�� ��Ȩ�û����ʰ�ȫ��Դ
app.UseDefaultFiles();   // Ĭ���ļ��м��
app.UseStaticFiles();    // ��̬�ļ��м��
app.UseFileServer(enableDirectoryBrowsing: true); // �ļ����
app.UseHttpsRedirection();
//app.UseEndpoints(options =>
//{
//   
//   
//});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=user}/{action=index}/{id?}");

// �Զ����м��
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
//app.UseStatusCodePagesWithReExecute("/Error/{0}");//ֻҪ����200 ���ܽ���

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
