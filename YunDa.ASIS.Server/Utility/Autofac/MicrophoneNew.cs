﻿using Advanced.NET6.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced.NET6.Business.Services
{
    public class MicrophoneNew : IMicrophone
    {
        public MicrophoneNew()
        {
            Console.WriteLine($"{this.GetType().Name}被构造。。");
        } 
    }
}
