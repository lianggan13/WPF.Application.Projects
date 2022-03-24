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
    ////Nuget �����룺
    ////1.Log4Net
    ////2.Microsoft.Extensions.Logging.Log4Net.AspNetCore
    builder.Services.AddLogging(cfg =>
    {
        //Ĭ�ϵ������ļ�·�����ڸ�Ŀ¼�����ļ���Ϊlog4net.config
        //����ļ�·���������б仯����Ҫ����������·��������
        //��������Ŀ��Ŀ¼�´���һ����Ϊcfg���ļ��У���log4net.config�ļ��������У�������Ϊlog.config
        //����Ҫʹ������Ĵ�������������
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
    //Nuget���룺NLog.Web.AspNetCore
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
// ע��
builder.Services.AddTransient<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
// ѡ����ַ�ʽ ��Ȩ(�����֤)
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
    }); // ע�� cookie ��֤����
builder.Services.AddAuthorization(options =>
{
    // ��֤����(���)
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
    options.Filters.Add<CustomGlobalActionFilterAttribute>(); // ȫ��ע�� Filter
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

    app.UseExceptionHandler("/Error"); // UseExceptionHandler ����ӵ��ܵ��еĵ�һ���м���������ˣ��쳣��������м���Ჶ���Ժ�����з������κ��쳣
    app.UseHsts();
}

app.UseRouting();        // ·�� �м��
app.UseAuthentication(); // �����֤ �м�� �������û����ʰ�ȫ��Դ֮ǰ���Զ��û����������֤
app.UseAuthorization();  // �����Ȩ �м�� ��Ȩ�û����ʰ�ȫ��Դ
app.UseStaticFiles();

app.UseServiceLocator();

// �Զ����м��
//app.Use(async (context, next) =>
//{
//    // Do work that doesn't write to the Response.
//    Console.WriteLine("Do work that doesn't write to the Response");
//    await next.Invoke();
//    // Do loggin or other work that does't write to the Response.
//    Console.WriteLine("Do loggin or other work that does't write to the Response.");
//});
//app.UseMiddleware<LogsMiddleware>(); //�����־��¼�м��

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
