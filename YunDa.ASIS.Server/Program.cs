using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using YunDa.ASIS.Server.Models;
using YunDa.ASIS.Server.Providers;
using YunDa.ASIS.Server.Services;
using YunDa.ASIS.Server.Test;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var ss = builder.Configuration.GetSection("BookStoreDatabase");

MongoDBTest dBTest = new MongoDBTest();
dBTest.TestJoin();
dBTest.TestAdd();
dBTest.TestMatch();
dBTest.TestFind();
dBTest.TestProjection();


builder.Services.Configure<BookStoreDatabaseSettings>(
   builder.Configuration.GetSection("BookStoreDatabase")
);

builder.Services.AddSingleton<BooksService>();
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
//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error"); // UseExceptionHandler 是添加到管道中的第一个中间件组件。因此，异常处理程序中间件会捕获以后调用中发生的任何异常
    app.UseHsts();
}

app.UseRouting();        // 路由 中间件
app.UseAuthentication(); // 身份验证 中间件 在允许用户访问安全资源之前尝试对用户进行身份验证
app.UseAuthorization();  // 身份授权 中间件 授权用户访问安全资源


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
