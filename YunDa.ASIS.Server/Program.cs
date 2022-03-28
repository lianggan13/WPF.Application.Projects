using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Security.Claims;
using YunDa.ASIS.Server.Filters;
using YunDa.ASIS.Server.Filters.AuthorizeAttr;
using YunDa.ASIS.Server.Middleware;
using YunDa.ASIS.Server.MinimalApis;
using YunDa.ASIS.Server.Providers;
using YunDa.ASIS.Server.Services;
using YunDa.ASIS.Server.Services.JWT;
using YunDa.ASIS.Server.Utility.JsonTypeConverter;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.Configure<IISOptions>(options =>
//{
//    options.ForwardClientCertificate = true;
//});

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


// ע��
builder.Services.AddTransient<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
// ѡ����ַ�ʽ ��Ȩ(�����֤) ע�� cookie ��֤����

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
builder.Services.AddControllers(options =>
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



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler("/Error"); // UseExceptionHandler ����ӵ��ܵ��еĵ�һ���м���������ˣ��쳣��������м���Ჶ���Ժ�����з������κ��쳣
    app.UseHsts(); // https ���
}

app.UseRouting();        // ·�� �м��
app.UseAuthentication(); // �����֤ �м�� �������û����ʰ�ȫ��Դ֮ǰ���Զ��û����������֤
app.UseAuthorization();  // �����Ȩ �м�� ��Ȩ�û����ʰ�ȫ��Դ
app.UseStaticFiles();
app.UseFileServer(enableDirectoryBrowsing: true); // �ļ����
app.UseServiceLocator();
//app.UseEndpoints(options =>
//{
//    options.MapDefaultControllerRoute(); // {controller=Home}/{action=Index}/{id?}
//    options.MapControllers();
//});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=user}/{action=index}/{id?}");

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

app.UseMiddleware<MinimalApiMiddleware>();

app.MapControllers();
app.UseUserApiExt();

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

app.Run();
//app.Run(async context =>
//{
//    await context.Response.WriteAsync("Append response end.");
//});
