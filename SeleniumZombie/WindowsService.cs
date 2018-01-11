using System;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using NLog;
using NUnit.Framework.Internal;
using SeleniumZombie.Service;
using SeleniumZombie.Service.Configuration;
using Logger = NLog.Logger;

namespace SeleniumZombie
{
    internal partial class WindowsService : ServiceBase
    {
        private bool _isRunning;
        private SeleniumZombieService _seleniumZombieService;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public WindowsService()
        {
            InitializeComponent();
        }

        public void RunOnce()
        {
            CreateService();
            Run();
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            CreateService();

            var timer = new Timer
            {
                Interval = 1000
            };
            timer.Elapsed += OnTimer;
            timer.Start();
        }

        protected override void OnStop()
        {
            if (_seleniumZombieService.IsRunning())
                _seleniumZombieService.Stop();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            if (_isRunning)
                return;

            _isRunning = true;
            Run();
            _isRunning = false;
        }

        private void CreateService()
        {
            _logger.Info("Starting Windows service...");
            _seleniumZombieService = new SeleniumZombieService(ConfigurationFactory.Create());
        }

        private void Run()
        {
            try
            {
                if (!_seleniumZombieService.IsRunning() && _seleniumZombieService.ShouldStart())
                {
                    _logger.Info("Initializing selenium node...");
                    _seleniumZombieService.Clean();
                    _seleniumZombieService.Update();
                    _seleniumZombieService.Start();
                    _logger.Info("Selenium node initialized...");
                }

                if (_seleniumZombieService.IsRunning() && _seleniumZombieService.ShouldStop())
                {
                    _logger.Info("Stopping selenium node...");
                    _seleniumZombieService.Stop();
                    _logger.Info("Selenium node stopped.");
                }
            }
            catch (Exception e)
            {
                var message = new StringBuilder();
                message.AppendLine(ExceptionHelper.BuildMessage(e));
                message.AppendLine(ExceptionHelper.BuildStackTrace(e));
                _logger.Fatal(message);
            }
        }


    }
}
