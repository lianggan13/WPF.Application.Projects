using Microsoft.AspNetCore.Http.Connections;
using SignalRDemo1.Hubs;
using SignalRNotify;


var builder1 = WebApplication.CreateBuilder(args);

// Add services to the container.
builder1.Services.AddRazorPages();

#region SignalR
builder1.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(13);
});
#endregion


var app = builder1.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

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

app.Run();
