﻿using System.ServiceProcess;

namespace SeleniumZombie
{
    public class Program
    {
        static void Main()
        {
            //ServiceBase.Run(new WindowsService());
            var service = new WindowsService();
            service.RunOnce();
        }
    }
}
