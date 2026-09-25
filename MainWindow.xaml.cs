using System;
using System.Collections.Generic;
using STARGBA.Device_Controller;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace STARGBA
{
    public partial class MainWindow : Window
    {
        private DeviceController? controller;

        private string? selectedDeviceSerial;
        private readonly DeviceManager manager;
        private readonly DispatcherTimer connectionTimer;

        public MainWindow()
        {
            InitializeComponent();

            manager = new DeviceManager();

            connectionTimer = new DispatcherTimer();
            connectionTimer.Interval = TimeSpan.FromSeconds(1);
            connectionTimer.Tick += ConnectionTimer_Tick;

            UpdateDevices();

            connectionTimer.Start();
        }


        private void ConnectionTimer_Tick(object? sender, EventArgs e)
        {
            UpdateDevices();
        }


        private void UpdateDevices()
        {
            var devices = manager.GetDevices();

            UpdateConnectionStatus(devices);
            UpdateDeviceList(devices);
            UpdateDeviceController(devices);
        }


        private void UpdateConnectionStatus(List<Device> devices)
        {
            if (devices.Count > 0)
            {
                ConnectionIndicator.Fill = Brushes.LimeGreen;
                ConnectionStatusText.Text = "Connected";
            }
            else
            {
                ConnectionIndicator.Fill = Brushes.Red;
                ConnectionStatusText.Text = "Disconnected";
            }
        }


        private void UpdateDeviceList(List<Device> devices)
        {
            DevicesPanel.Children.Clear();

            if (devices.Count == 0)
            {
                NoDevicesText.Visibility = Visibility.Visible;
                DevicesPanel.Children.Add(NoDevicesText);

                return;
            }

            NoDevicesText.Visibility = Visibility.Collapsed;

            foreach (var device in devices)
            {
                var devicePanel = new Border
                {
                    Background = new SolidColorBrush(
                        Color.FromRgb(34, 38, 46)),

                    BorderBrush = new SolidColorBrush(
                        Color.FromRgb(52, 58, 69)),

                    BorderThickness = new Thickness(1),

                    CornerRadius = new CornerRadius(8),

                    Padding = new Thickness(14),

                    Margin = new Thickness(0, 0, 0, 10)
                };


                var panel = new Grid();

                panel.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });

                panel.ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = GridLength.Auto
                    });


                var deviceText = new TextBlock
                {
                    Text = $"Device {device.Number}\n{device.Serial}",
                    FontSize = 15,
                    FontWeight = FontWeights.SemiBold,
                    VerticalAlignment = VerticalAlignment.Center
                };


                Grid.SetColumn(deviceText, 0);

                panel.Children.Add(deviceText);


                var switchButton = new Button
                {
                    Content = device.Serial == selectedDeviceSerial
                        ? "●"
                        : "○",

                    FontSize = 22,

                    Width = 45,
                    Height = 45,

                    Background = Brushes.Transparent,
                    Foreground = device.Serial == selectedDeviceSerial
                        ? Brushes.LimeGreen
                        : Brushes.Gray,

                    BorderThickness = new Thickness(0),

                    Tag = device
                };


                switchButton.Click += DeviceSwitch_Click;

                Grid.SetColumn(switchButton, 1);

                panel.Children.Add(switchButton);


                devicePanel.Child = panel;

                DevicesPanel.Children.Add(devicePanel);
            }
        }

        private void DeviceSwitch_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not Device device)
                return;

            selectedDeviceSerial = device.Serial;

            controller = new TabletController(device);

            ScreenOnButton.IsEnabled = true;
            ScreenOffButton.IsEnabled = true;

            var devices = manager.GetDevices();

            UpdateDeviceList(devices);
        }

        private void UpdateDeviceController(List<Device> devices)
        {
            if (devices.Count == 0)
            {
                controller = null;
                selectedDeviceSerial = null;

                ScreenOnButton.IsEnabled = false;
                ScreenOffButton.IsEnabled = false;

                return;
            }

            Device? selectedDevice = null;

            if (selectedDeviceSerial != null)
            {
                foreach (var device in devices)
                {
                    if (device.Serial == selectedDeviceSerial)
                    {
                        selectedDevice = device;
                        break;
                    }
                }
            }

            if (selectedDevice == null)
            {
                selectedDevice = devices[0];
                selectedDeviceSerial = selectedDevice.Serial;
            }

            controller = new TabletController(selectedDevice);

            ScreenOnButton.IsEnabled = true;
            ScreenOffButton.IsEnabled = true;
        }


        private void ScreenOn_Click(object sender, RoutedEventArgs e)
        {
            controller?.TurnScreenOn();
        }


        private void ScreenOff_Click(object sender, RoutedEventArgs e)
        {
            controller?.TurnScreenOff();
        }
    }
}