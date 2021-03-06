﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EMS.Core;
using EMS.Core.Interfaces;
using EMS.Core.Interfaces.Providers;
using EMS.Core.Providers;
using EMS.Infrastructure.Common.Configurations;
using EMS.Infrastructure.Common.Utils;
using EMS.Infrastructure.DependencyInjection;
using EMS.Infrastructure.DependencyInjection.Interfaces;
using Fiddler;
using Microsoft.Practices.Unity;
using AForge.Video;
using AForge;
using AForge.Video.DirectShow;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave;
using System.Speech.Recognition;
using System.Threading;

namespace EMS.CLI
{
    class Program
    {
        static Bitmap image;
        static WaveFileWriter waveWriter;

        static void Main(string[] args)
        {
            // NETWORK SNIFFER
            // ivp4 ips only
            //StartNetworkSniffer();

            // PROCESS API
            //IWin32ApiProvider win32Provider = new Win32ApiProvider();
            //IProcessAPI processApi = new ProcessAPI(win32Provider, new ProcessAPIConfig());
            //var disabledProcesses = new HashSet<string>()
            //{
            //    "Skype"
            //};

            //processApi.OnForegroundProcessChanged += (sender, process) =>
            // {
            //     Console.WriteLine($"{process.MachineName} - {process.ProcessName}");
            // };

            //processApi.OnActiveProcessesChanged += (sender, processes) =>
            //{
            //    foreach (var process in processes)
            //    {
            //        if (disabledProcesses.Contains(process.ProcessName))
            //        {
            //            Console.WriteLine(process.ProcessName);
            //            process.Kill();
            //            Console.WriteLine(process.HasExited);
            //        }
            //    }
            //};

            // KeyboardAPI
            //IWin32ApiProvider win32Provider = new Win32ApiProvider();
            //IKeyboardAPI keyboardApi = new KeyboardApi(win32Provider, new KeyboardApiConfig { SleepIntervalInMilliseconds = 20 });
            //keyboardApi.OnKeyPressed += (sender, key) =>
            //{
            //    Console.WriteLine($"{DateTime.Now.ToShortTimeString()} => {key}");
            //};

            //keyboardApi.StartListeningToKeyboard();

            // DisplayAPI
            //IDisplayApi dapi = new DisplayApi(new DisplayApiConfig { DisplayWatcherSleepIntervalInMilliseconds = 1000 });

            //var counter = 0;
            //dapi.OnDisplaySnapshotTaken += (sender, payload) =>
            // {
            //     var image = payload.ToImage();
            //     image.Save($"{counter++}.jpg");
            // };
            //dapi.StartWatchingDisplay();

            //Console.ReadLine();
            // mark fbcdn, facebook, fb, 
            // CURRENTLY FOCUSED WINDOW
            //var activeForegroundWindowChangeEventHandle = Win32ApiWrapper.HookActiveForegroundWindowChangedEvent();

            //Win32ApiWrapper.UnhookActiveForegroundWindowChangedEvent(activeForegroundWindowChangeEventHandle);

            // CAMERA API
            //var counter = 0;
            //ICameraApi camApi = new CameraApi(new CameraApiConfig
            //{
            //    SnapshotConfig = new TimerConfig
            //    {
            //        DueTime = 1000,
            //        Period = 500
            //    }
            //});
            //camApi.OnWebcamSnapshotTaken += (sender, payload) =>
            //  {
            //      payload.ToImage().Save($"Cam{counter++}.jpg");
            //      Console.WriteLine($"Taken image {counter}");
            //  };
            //camApi.StartCamera();

            //Task.Delay(10000).Wait();

            //camApi.StopCamera();

            // MicrophoneAPi
            //List<WaveInCapabilities> sources = new List<WaveInCapabilities>();

            //for (int i = 0; i < WaveIn.DeviceCount; i++)
            //{
            //    sources.Add(WaveIn.GetCapabilities(i));
            //}

            //foreach(var source in sources)
            //{
            //    var sourceStream = new WaveInEvent();
            //    sourceStream.DeviceNumber = 0;
            //    sourceStream.WaveFormat = new WaveFormat(44100, source.Channels);

            //    sourceStream.DataAvailable += (sender, e) =>
            //    {
            //        if (waveWriter == null)
            //        {
            //            return;
            //        }

            //        waveWriter.Write(e.Buffer, 0, e.BytesRecorded);
            //        waveWriter.Flush();
            //    };

            //    waveWriter = new WaveFileWriter(new FileStream("Recording.wav",FileMode.CreateNew), sourceStream.WaveFormat);

            //    sourceStream.StartRecording();
            //    Console.WriteLine("Start");

            //    Console.ReadLine();

            //    if (sourceStream != null)
            //    {
            //        sourceStream.StopRecording();
            //        Console.WriteLine("Stop");
            //        sourceStream.Dispose();
            //        sourceStream = null;
            //    }
            //    if (waveWriter != null)
            //    {
            //        waveWriter.Dispose();
            //        waveWriter = null;
            //    }
            //}
            //SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            //Choices choiceList = new Choices();
            //choiceList.Add(new string[] { "nanan" });

            //GrammarBuilder builder = new GrammarBuilder();
            //builder.Append(choiceList);
            //Grammar grammar = new Grammar(new GrammarBuilder(builder, 0, 10));

            //sre.SpeechRecognized += (sender, srArgs) =>
            //{
            //    Console.WriteLine("Speech recognized");
            //};

            //sre.SpeechDetected += (sender, sdArgs) =>
            //{
            //    Console.WriteLine("Speech detected");
            //};

            //sre.RecognizeCompleted += (sender, rcArgs) =>
            //{
            //    Console.WriteLine("Recognize completed");
            //};

            //sre.InitialSilenceTimeout = TimeSpan.FromSeconds(0);
            //sre.BabbleTimeout = TimeSpan.FromSeconds(0);
            //sre.EndSilenceTimeout = TimeSpan.FromSeconds(0);
            //sre.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(0);
            //sre.SetInputToDefaultAudioDevice();
            //sre.LoadGrammar(grammar);

            //while (true)
            //{
            //    sre.Recognize();
            //}

            var keyboardApi = new KeyboardApi(new Win32ApiProvider(), new KeyboardApiConfig
            {
                SleepIntervalInMilliseconds = 10
            });

            var random = new Random();
            keyboardApi.OnKeyPressed += (sender, e) =>

              {
                  var sleepPeriod = random.Next(500, 1000);
                  Thread.Sleep(sleepPeriod);
                  Console.WriteLine($"Event handle for {e} with sleep period: {sleepPeriod}");
              };

            keyboardApi.StartListeningToKeyboard();
        }

        private static void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            image = (Bitmap)eventArgs.Frame.Clone();
        }

        private static void StartNetworkSniffer()
        {
            INetworkApi networkApi = new NetworkApi();

            networkApi.OnPacketSniffed += NetworkApi_OnPacketSniffedHandler;
            networkApi.StartSniffingAllAddresses();

            Console.Read();
        }

        private static void NetworkApi_OnPacketSniffedHandler(object sender, byte[] buffer)
        {
            var protocol = buffer.Skip(9).First().ToProtocolString();
            var localAddress = new IPAddress(BitConverter.ToUInt32(buffer, 12)).ToString();
            var localAddressPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 20))).ToString();
            var destinationAddress = new IPAddress(BitConverter.ToUInt32(buffer, 16)).ToString();
            var destinationAddressPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 22))).ToString();

            try
            {
                var dnsName = Dns.GetHostEntry(destinationAddress);
                Console.WriteLine($"DNS NAME: {dnsName.HostName} -> RESOLVED FOR {destinationAddress}");
            }
            catch (Exception)
            {
                Console.WriteLine($"CANNOT RESOLVE DNS NAME FOR: {destinationAddress}");
            }

            Console.WriteLine($"{protocol} - {localAddress}:{localAddressPort} -> {destinationAddress}:{destinationAddressPort}");
        }

        // Should be moved to web api handler
        static void Sniff(IPAddress ip)
        {
            // Setup the socket to listen on, we are listening just to IPv4 Addresses
            var socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Raw,
                ProtocolType.IP);

            var ipEndpoint = new IPEndPoint(ip, port: 0);

            socket.Bind(ipEndpoint);
            socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.HeaderIncluded,
                optionValue: true);
            socket.IOControl(
                IOControlCode.ReceiveAll,
                optionInValue: new byte[4] { 1, 0, 0, 0 },
                optionOutValue: null);

            // byte array to hold the packet data we want to examine.
            //  we are assuming default (20byte) IP header size + 4 bytes for TCP header to get ports
            var buffer = new byte[24];

            // Async methods for recieving and processing data
            Action<IAsyncResult> OnReceive = null;
            OnReceive = (ar) =>
            {
                // More details about the IPV4 packet structure at: http://en.wikipedia.org/wiki/IPv4_packet#Packet_structure
                var protocol = buffer.Skip(9).First().ToProtocolString();
                var localAddress = new IPAddress(BitConverter.ToUInt32(buffer, 12)).ToString();
                var localAddressPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 20))).ToString();
                var destinationAddress = new IPAddress(BitConverter.ToUInt32(buffer, 16)).ToString();
                var destinationAddressPort = ((ushort)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, 22))).ToString();


                try
                {
                    var dnsName = Dns.GetHostByAddress(destinationAddress);
                    Console.WriteLine($"DNS NAME: {dnsName.HostName} -> {destinationAddress}");
                    Console.WriteLine(dnsName.Aliases.Aggregate(string.Empty, (accumulate, x) => accumulate + $"{x} "));
                }
                catch (Exception)
                {
                }

                Console.WriteLine($"{protocol} - {localAddress}:{localAddressPort} -> {destinationAddress}:{destinationAddressPort}");

                buffer = new byte[24];
                socket.BeginReceive(
                    buffer,
                    offset: 0,
                    size: 24,
                    socketFlags: SocketFlags.None,
                    callback: new AsyncCallback(OnReceive),
                    state: null);
            };

            // begin listening to the socket
            socket.BeginReceive(
                buffer: buffer,
                offset: 0,
                size: buffer.Length,
                socketFlags: SocketFlags.None,
                callback: new AsyncCallback(OnReceive),
                state: null);
        }
    }

    public static class Extensions
    {
        public static string ToProtocolString(this byte protocolNumberByte)
        {
            switch (protocolNumberByte)
            {
                case 1:
                    return "ICMP";
                case 2:
                    return "IGMP";
                case 6:
                    return "TCP";
                case 17:
                    return "UDP";
                default:
                    return "#" + protocolNumberByte.ToString();
            }
        }
    }

    public static class Win32ApiWrapper
    {
        private const string User32DLL = "user32.dll";
        private const string UnknownWindow = "UnknownWindow";

        // Need to ensure delegate is not collected while we're using it,
        // storing it in a class field is simplest way to do this.
        private static WinEventDelegate procDelegate = new WinEventDelegate(WinEventProc);

        private delegate void WinEventDelegate(
            IntPtr hWinEventHook,
            uint eventType,
            IntPtr hwnd,
            int idObject,
            int idChild,
            uint dwEventThread,
            uint dwmsEventTime);

        private static void WinEventProc(IntPtr hWinEventHook, uint eventType,
            IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            Console.WriteLine("Foreground changed to {0:x8}", hwnd.ToInt32());
            Console.WriteLine("ObjectID changed to {0:x8}", idObject);
            Console.WriteLine("ChildID changed to {0:x8}", idChild);
            Console.WriteLine(GetForegroundProcessName());
        }

        [DllImport(User32DLL)]
        private static extern IntPtr SetWinEventHook(
            uint eventMin,
            uint eventMax,
            IntPtr hmodWinEventProc,
            WinEventDelegate lpfnWinEventProc,
            uint idProcess,
            uint idThread,
            uint dwFlags);

        [DllImport(User32DLL)]
        private static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        /// <summary>
        /// The "GetForegroundWindow" function gets the window 
        /// with which the user is currently working.
        /// </summary>
        /// <returns>Returns a handle to the currently active foreground window.</returns>
        [DllImport(User32DLL)]
        private static extern IntPtr GetForegroundWindow();


        /// <summary>
        /// The "GetWindowThreadProcessId" function retrieves the identifier of the thread
        /// that created the specified window and, optionally, the identifier of the
        /// process that created the window.
        /// </summary>
        /// <returns></returns>
        [DllImport(User32DLL)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        /// <summary>
        /// Returns the name of the process owning the foreground window.
        /// </summary>
        /// <returns>
        /// The name of the currently active process or "Unknown" 
        /// if there is no active window at this exact moment.
        /// </returns>
        public static string GetForegroundProcessName()
        {
            var currentlyActiveWindowHandle = GetForegroundWindow();

            // This check must be done because
            // the foreground window can be NULL in certain cases, 
            // such as when a window is losing activation.
            if (currentlyActiveWindowHandle == null)
            {
                return UnknownWindow;
            }

            var processId = default(uint);
            GetWindowThreadProcessId(currentlyActiveWindowHandle, out processId);

            var process = Process.GetProcessById((int)processId);
            var processName = process == null ? UnknownWindow : process.ProcessName;

            return processName;
        }

        public static IntPtr HookActiveForegroundWindowChangedEvent()
        {
            var activeForegroundWindowChangedHandle = SetWinEventHook(
                eventMin: 3,
                eventMax: 3,
                hmodWinEventProc: IntPtr.Zero,
                lpfnWinEventProc: procDelegate,
                idProcess: 0,
                idThread: 0,
                dwFlags: 0);

            return activeForegroundWindowChangedHandle;
        }

        public static void UnhookActiveForegroundWindowChangedEvent(IntPtr handle)
        {
            UnhookWinEvent(handle);
        }
    }
}
