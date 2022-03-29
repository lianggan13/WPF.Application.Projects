using Advanced.NET6.Business.Interfaces;
using YunDa.ASIS.Server.Services;


namespace YunDa.ASIS.Server.MinimalApis
{
    public static class PhoneMiniApi
    {
        public static void UsePhoneMiniApi(this WebApplication app)
        {
            app.MapGet("/api/phone/apple_phone",
            async (IPhone _IPhone, MongoDbService dbService) =>
                    {
                        _IPhone.Text();
                        _IPhone.Call();

                        return await Task.FromResult("ok");
                    })
                   .WithTags("Phone");

            app.MapGet("/api/phone/power",
              async (IPower ipower, MongoDbService dbService) =>
              {
                  ipower.Open();
                  ipower.Close();

                  return await Task.FromResult("ok");
              })
                 .WithTags("Phone");

        }
    }

}




