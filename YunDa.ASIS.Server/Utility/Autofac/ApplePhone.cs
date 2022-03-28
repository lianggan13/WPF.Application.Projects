using Advanced.NET6.Business.Interfaces;


namespace Advanced.NET6.Business.Services
{
    //[Intercept(typeof(CusotmInterceptor))]
    public class ApplePhone : IPhone
    {
        public IMicrophone Microphone { get; set; }
        public IHeadphone Headphone { get; set; }
        public IPower Power { get; set; }

        public ApplePhone(IHeadphone iHeadphone)
        {
            this.Headphone = iHeadphone;
            Console.WriteLine("{0}带参数构造函数", this.GetType().Name);
        }

        public virtual void Call()
        {
            Console.WriteLine("{0}打电话", this.GetType().Name); ;
        }

        public void Text()
        {
            Console.WriteLine("{0}发信息", this.GetType().Name); ;
        }


        public object QueryUser(object opara)
        {
            return new
            {
                Id = 123,
                Name = "Richard",
                DateTiem = DateTime.Now.ToString()
            };
        }


        public void Init123456678890(IPower iPower)
        {
            this.Power = iPower;
        }
    }
}
