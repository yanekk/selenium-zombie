﻿using System;
using System.Diagnostics;
using NLog;
using SeleniumZombie.ChromeDriver;
using SeleniumZombie.Common;
using SeleniumZombie.Selenium;
using SeleniumZombie.Service.Update;

namespace SeleniumZombie.Service
{
    public class SeleniumZombieService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TimeSpan _startTime;
        private readonly TimeSpan _endTime;
        private readonly string _hubAddress;

        private readonly ModuleUpdater _moduleUpdater;

        private bool _isRunning;
        private Process _nodeProcess;

        public SeleniumZombieService(TimeSpan startTime, TimeSpan endTime, string hubAddress)
        {
            _startTime = startTime;
            _endTime = endTime;
            _hubAddress = hubAddress;
            _moduleUpdater = new ModuleUpdater(
                new ChromeDriverDownloader(),
                new SeleniumDownloader());
        }

        public void Update()
        {
            _moduleUpdater.Update();
        }

        public void Start()
        {
            var arguments = string.Join(" ", 
                @"-Dwebdriver.chrome.driver="".\chromedriver.exe""", 
                "-jar selenium-server-standalone.jar", 
                "-role node", 
                $"-hub http://{_hubAddress}/grid/register", 
                "-maxSession 8", 
                "-browser browserName=chrome,maxInstances=8");

            _logger.Info("Executing java with following parameters: " + arguments);

            _nodeProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "java",
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = CurrentDirectory.ActualPath
                }
            };
            _nodeProcess.OutputDataReceived += (sender, args) =>
            {
                _logger.Info("Selenium Node: " + args.Data);
            };
            _nodeProcess.ErrorDataReceived += (sender, args) =>
            {
                _logger.Info("Selenium Node: " + args.Data);
            };
            _nodeProcess.Start();
            _nodeProcess.BeginOutputReadLine();
            _nodeProcess.BeginErrorReadLine();
            _isRunning = true;
        }

        public bool IsRunning()
        {
            return _isRunning;
        }

        public bool ShouldStart()
        {
            return IsDateWithinBoundaries(DateTime.Now);
        }

        public bool ShouldStop()
        {
            return !IsDateWithinBoundaries(DateTime.Now);
        }

        public void Stop()
        {
            _logger.Info("Stopping selenium node...");
            if (_nodeProcess != null)
            {
                if (!_nodeProcess.HasExited)
                {
                    _nodeProcess.Kill();
                    _nodeProcess.WaitForExit(Convert.ToInt32(TimeSpan.FromSeconds(10).TotalMilliseconds));
                }
                _nodeProcess = null;
                _isRunning = false;
                GC.Collect();
            }
            _logger.Info("Selenium node stopped.");
        }

        public void Clean()
        {
            var driverProcesses = Process.GetProcessesByName("chromedriver.exe");
            _logger.Info($"Killing {driverProcesses.Length} chromedriver processes...");

            foreach (var driverProcess in driverProcesses)   
                driverProcess.Kill();
        }

        private bool IsDateWithinBoundaries(DateTime date)
        {
            var startDate = DateTime.Today + _startTime;
            var endDate = DateTime.Today + _endTime;

            return date >= startDate && endDate >= date;
        }
    }
}