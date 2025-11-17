using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Zhaoxi.DigtialScreen.Base;
using Zhaoxi.DigtialScreen.Models;

namespace Zhaoxi.DigtialScreen.ViewModels
{
    public class MainViewModel : NotifyBase
    {
        public SeriesCollection StateSeries { get; set; }
        public ChartValues<ObservableValue> YeildValues1 { get; set; }
        public ChartValues<ObservableValue> YeildValues2 { get; set; }

        public List<CompareItemModel> WorkerCompareList { get; set; }

        public List<CompareItemModel> QualityList { get; set; }

        public ObservableCollection<string> Alarms { get; set; }

        //public string CurrentYeild { get; set; } = "123456";
        private string _currentYeild;
        public string CurrentYeild
        {
            get { return _currentYeild; }
            set { SetProperty(ref _currentYeild, value); }
        }
        public int FinishRate { get; set; } = 80;

        public List<BadItemModel> BadScatter { get; set; }

        Random random = new Random();
        CancellationTokenSource cts = new CancellationTokenSource();
        Task task = null;

        public MainViewModel()
        {
            #region 环状图数据初始化
            StateSeries = new SeriesCollection();
            StateSeries.Add(new PieSeries()
            {
                Title = "普货",
                Values = new ChartValues<double>(new double[] { 0.533 }),
                Fill = new SolidColorBrush(Color.FromArgb(255, 43, 182, 254))
            });
            StateSeries.Add(new PieSeries()
            {
                Title = "普货",
                Values = new ChartValues<double>(new double[] { 0.2 }),
                Fill = Brushes.Red
            });
            StateSeries.Add(new PieSeries()
            {
                Title = "普货",
                Values = new ChartValues<double>(new double[] { 0.167 }),
                Fill = new SolidColorBrush(Color.FromArgb(255, 144, 150, 191))
            });
            StateSeries.Add(new PieSeries()
            {
                Title = "普货",
                Values = new ChartValues<double>(new double[] { 0.1 }),
                Fill = Brushes.DimGray
            });
            #endregion

            #region 人员绩效初始化
            string[] Empolys = new string[] { "赵XX", "钱XX", "孙XX", "李XX", "周XX" };
            WorkerCompareList = new List<CompareItemModel>();
            foreach (var e in Empolys)
            {
                WorkerCompareList.Add(new CompareItemModel()
                {
                    Name = e,
                    PlanValue = random.Next(100, 200),
                    FinishedValue = random.Next(10, 150),
                });
            }

            #endregion

            #region 报警数据初始化
            Alarms = new ObservableCollection<string>();
            //Alarms.Add("【H338->厂务冷却水入水温度[℃]】 34->10:00");
            //Alarms.Add("【H338->厂务冷却水入水温度[℃]】 34->10:00");
            //Alarms.Add("【H338->厂务冷却水入水温度[℃]】 34->10:00");
            //Alarms.Add("【H338->厂务冷却水入水温度[℃]】 34->10:00");
            #endregion

            YeildValues1 = new ChartValues<ObservableValue>();
            YeildValues2 = new ChartValues<ObservableValue>();
            for (int i = 0; i < 12; i++)
            {
                YeildValues1.Add(new ObservableValue(random.Next(20, 380)));
                YeildValues2.Add(new ObservableValue(random.Next(20, 300)));
            }

            #region 不良分布初始化
            BadScatter = new List<BadItemModel>();
            string[] BadNames = new string[] { "缺角A", "缺角B", "缺角C", "缺角D", "缺角E", "缺角E", "缺角F", "缺角G" };
            for (int i = 0; i < BadNames.Length; i++)
            {
                BadScatter.Add(new BadItemModel() { Title = BadNames[i], Size = 180 - 20 * i, Value = 0.9 - 0.1 * i });
            }
            #endregion

            #region 质量控制
            string[] quality = new string[] { "机床-1", "机床-2", "机床-3", "机床-4",
                "机床-5", "机床-6", "机床-7", "机床-8", "机床-9", "机床-10" };
            QualityList = new List<CompareItemModel>();
            foreach (var q in quality)
            {
                QualityList.Add(new CompareItemModel()
                {
                    Name = q,
                    PlanValue = random.Next(100, 200),
                    FinishedValue = random.Next(10, 150),
                });
            }
            #endregion


            task = Task.Run(async () =>
            {
                while (!cts.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    // 持续获取现场数据
                    CurrentYeild = new Random().Next(0, 100).ToString("000000");
                    // 添加报警
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Alarms.Insert(0, "【H338->厂务冷却水入水温度[℃]】 34->20:00");
                        if (Alarms.Count > 6)
                            Alarms.RemoveAt(Alarms.Count - 1);// 滚动   停留
                    });
                }

                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect("127.0.0.1", 502);
                Modbus.Device.ModbusIpMaster master = Modbus.Device.ModbusIpMaster.CreateIp(tcpClient);

                while (!cts.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    // 持续获取现场数据
                    ushort[] values = master.ReadHoldingRegisters(1, 0, 1);
                    CurrentYeild = values[0].ToString("000000");

                    // 添加报警
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Alarms.Insert(0, "【H338->厂务冷却水入水温度[℃]】 34->20:00");
                        if (Alarms.Count > 6)
                            Alarms.RemoveAt(Alarms.Count - 1);// 滚动   停留
                    });
                }
            }, cts.Token);
        }

        public void Dispose()
        {
            cts.Cancel();
            Task.WaitAny(task);
        }
    }
}
