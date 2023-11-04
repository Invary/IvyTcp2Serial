using System;
using System.IO.Ports;
using System.Text;
using Invary.Utility;

namespace Invary.IvyTcp2Serial
{
	internal class Program
	{
		static	SerialPort? _serial;

		static bool _bAbort = false;

		static string _strTcpServer = "";
		static int _nTcpPort = 80;

		static string _strComPort = "";
		static int _nComBaudRate = 9800;
		static Parity _Parity = Parity.None;
		static StopBits _StopBits = StopBits.One;


		static void Main(string[] args)
		{
			ReadCommandLineOptions();

			//_strTcpServer = "192.168.100.101";
			//_nTcpPort = 23;
			//_strComPort = "COM19";
			//_nComBaudRate = 115200;
			//_Parity = Parity.None;
			//_StopBits = StopBits.One;
			////_bTcpReconnect = true;



			Console.WriteLine("IvyTcp2Serial Ver100");
			Console.WriteLine("   https://github.com/Invary/IvyTcp2Serial/");
			Console.WriteLine("");
			Console.WriteLine($"Server IP: {_strTcpServer}");
			Console.WriteLine($"Server Port: {_nTcpPort}");
			Console.WriteLine($"ComPort: {_strComPort}");
			Console.WriteLine($"BaudRate: {_nComBaudRate}");
			Console.WriteLine($"Parity: {_Parity}");
			Console.WriteLine($"StopBits: {_StopBits}");
			Console.WriteLine("");

			if (string.IsNullOrEmpty(_strTcpServer))
			{
				Console.WriteLine("Server IP is invalid.");
				return;
			}
			if (string.IsNullOrEmpty(_strComPort))
			{
				Console.WriteLine("ComPort name is invalid.");
				return;
			}
			if (_nComBaudRate <= 0)
			{
				Console.WriteLine("BaudRate is invalid.");
				return;
			}

			var cancellationSource = new CancellationTokenSource();



			var client = new SimpleTcpClient();


			client.Received += (sender, e) =>
			{
				//send to serial
				if (_serial != null && _serial.IsOpen)
				{
					try
					{
						_serial.Write(e.Data, e.Offset, e.Length);
					}
					catch(Exception)
					{
					}
				}
			};

			client.Disconnected += delegate
			{
				Console.WriteLine("disconnected");
				cancellationSource.Cancel();
				_bAbort = true;
			};

			client.Connected += delegate
			{
				Console.WriteLine("Connected");
			};

			bool ret = client.ConnectAsync(_strTcpServer, _nTcpPort, cancellationSource.Token).Result;

			if (ret == false)
			{
				Console.WriteLine("Cannot connected to Tcp server.");
				return;
			}




			_serial = new SerialPort(_strComPort, _nComBaudRate, _Parity, 8, _StopBits);


			//timeout 5000msec
			_serial.WriteTimeout = 5000;


			_serial.DataReceived += delegate
			{
				try
				{
					var buffer = new byte[1024 * 10];

					var len = _serial.Read(buffer, 0, 1024 * 10);
					if (len == 0)
						return;

					//var message = Encoding.UTF8.GetString(buffer, 0, len);
					client.SendAsync(buffer, 0, len).Wait();
				}
				catch (Exception)
				{
				}
			};

			
			try
			{
				_serial.Open();
			}
			catch (Exception)
			{
				Console.WriteLine("Cannot open ComPort");
				return;
			}

			Console.CancelKeyPress += (sender, e) =>
			{
				e.Cancel = true;
				_bAbort = true;
				cancellationSource.Cancel();
			};

			Console.WriteLine("Press [Ctrl]+[C] to exit.");
			while (true)
			{
				if (_bAbort)
				{
					Console.WriteLine("abort...");
					break;
				}

				Thread.Sleep(100);
			}

			client.Disconnect();
			try
			{
				_serial.Close();
			}
			catch (Exception)
			{
			}
		}




		static void ReadCommandLineOptions()
		{
			var options = new List<CommandLineOption>();
			{
				var option = new CommandLineOption("comport", 1, true);
				option.Help = "/comport=[name]\n  Serial port name. ex:\"/comport=COM1\"";
				option.OnApply += delegate
				{
					if (option.Values.Count > 0)
						_strComPort = option.Values[0];
				};
				options.Add(option);
			}
			{
				var option = new CommandLineOption("baudrate", 1, true);
				option.Help = "/baudrate=[speed]\n  Serial port baud rate value. ex:\"/baudrate=115200\"";
				option.OnApply += delegate
				{
					try
					{
						if (option.Values.Count > 0)
							_nComBaudRate = int.Parse(option.Values[0]);
					}
					catch (Exception)
					{
					}
				};
				options.Add(option);
			}
			{
				var option = new CommandLineOption("parity", 1, true);
				option.Help = "/parity=[none|odd|even|space|mark]\n  Serial port parity value. ex:\"/parity=none\"";
				option.OnApply += delegate
				{
					if (option.Values.Count > 0)
					{
						var value = option.Values[0].ToLower();
						if (value == "none")
							_Parity = Parity.None;
						if (value == "odd")
							_Parity = Parity.Odd;
						if (value == "even")
							_Parity = Parity.Even;
						if (value == "space")
							_Parity = Parity.Space;
						if (value == "mark")
							_Parity = Parity.Mark;
					}
				};
				options.Add(option);
			}
			{
				var option = new CommandLineOption("stopbits", 1, true);
				option.Help = "/stopbits=[none|1|1.5|2]\n  Serial port stop bits value. ex:\"/stopbits=1\"";
				option.OnApply += delegate
				{
					if (option.Values.Count > 0)
					{
						var value = option.Values[0].ToLower();
						if (value == "none" || value == "0")
							_StopBits = StopBits.None;
						if (value == "one" || value == "1")
							_StopBits = StopBits.One;
						if (value == "onepointfive" || value == "1.5")
							_StopBits = StopBits.OnePointFive;
						if (value == "two" || value == "2")
							_StopBits = StopBits.Two;
					}
				};
				options.Add(option);
			}
			{
				var option = new CommandLineOption("server", 1, true);
				option.Help = "/server=[name]\n  Server IP address. ex:\"/server=192.168.100.101\"";
				option.OnApply += delegate
				{
					if (option.Values.Count > 0)
						_strTcpServer = option.Values[0];
				};
				options.Add(option);
			}
			{
				var option = new CommandLineOption("port", 1, true);
				option.Help = "/port=[number]\n  Server port. ex:\"/port=80\"";
				option.OnApply += delegate
				{
					try
					{
						if (option.Values.Count > 0)
							_nTcpPort = int.Parse(option.Values[0]);
					}
					catch (Exception)
					{
					}
				};
				options.Add(option);
			}
			{
				var option = new CommandLineOption("?", 0, true);
				option.Help = "/?\n  Show supported command line options.";
				option.OnApply += delegate
				{
					Console.WriteLine("Help");
					Console.WriteLine("");
					foreach(var item in options)
					{
						Console.WriteLine($"{item.Help}");
					}
					Console.WriteLine("example: /server=192.168.100.101 /port=80 /comport=COM19 /baudrate=115200 /parity=none /stopbits=1");
					Console.WriteLine("");
					Console.WriteLine("");
				};
				options.Add(option);
			}
			CommandLineOption.ArgTypeSpaceSeparated = false;
			CommandLineOption.AnalyzeCommandLine(options);

			foreach(var item in options)
			{
				item.Apply();
			}
		}



	}
}