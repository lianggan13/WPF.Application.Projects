
using Advanced.NET6.Business.Interfaces;

namespace Advanced.NET6.Business.Services
{
    public class Power : IPower
    {
        private IMicrophone Microphone;
        private IMicrophone Microphone2;

        public Power(IMicrophone microphone)
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。");
            this.Microphone = microphone;
        }

        public Power(int microphone)
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。");
        }

        public Power(IMicrophone microphone, IMicrophone microphone2)
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。");
            this.Microphone = microphone;
            this.Microphone2 = microphone2;
        }

        public void Open()
        {
            Console.WriteLine($"{this.GetType().Name} {nameof(Open)}...");
        }

        public void Close()
        {
            Console.WriteLine($"{this.GetType().Name} {nameof(Close)}...");
        }
    }
}
