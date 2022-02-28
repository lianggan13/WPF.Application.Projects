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
    app.UseExceptionHandler("/Error"); // UseExceptionHandler ����ӵ��ܵ��еĵ�һ���м���������ˣ��쳣��������м���Ჶ���Ժ�����з������κ��쳣
    app.UseHsts();
}

app.UseRouting();        // ·�� �м��
app.UseAuthentication(); // �����֤ �м�� �������û����ʰ�ȫ��Դ֮ǰ���Զ��û����������֤
app.UseAuthorization();  // �����Ȩ �м�� ��Ȩ�û����ʰ�ȫ��Դ


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
