namespace Advanced.NET6.Business.Interfaces
{
    //[Intercept(typeof(CusotmLogInterceptor))]
    public interface IPower
    {
        void Open();
        void Close();
    }
}
