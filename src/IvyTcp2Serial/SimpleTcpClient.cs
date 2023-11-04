using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Invary.IvyTcp2Serial
{

	class ReceivedEventArgs : EventArgs
	{
		public byte[] Data { get; private set; }
		public int Offset { get; private set; }
		public int Length { get; private set; }

		public ReceivedEventArgs(byte[] buffer, int offset, int length)
		{
			Data = buffer;
			Offset = offset;
			Length = length;
		}
	}


	class SimpleTcpClient : IDisposable
	{
		string _strServer = "";
		int _nPort;


		TcpClient? _client;
		NetworkStream? _stream;

		CancellationToken _token;

		bool _bExit = false;


		public EventHandler<ReceivedEventArgs>? Received { get; set; }
		public EventHandler<EventArgs>? Connected { get; set; }
		public EventHandler<EventArgs>? Disconnected { get; set; }




		public SimpleTcpClient()
		{
		}


		public void Dispose()
		{
			Disconnect();
		}



		public void Disconnect()
		{
			bool notify = (_stream != null);

			_bExit = true;
			try
			{
				_stream?.Dispose();
			}
			catch (Exception)
			{
			}
			try
			{
				_client?.Dispose();
			}
			catch(Exception)
			{
			}
			_client = null;
			_stream = null;

			if (notify)
			{
				Disconnected?.Invoke(this, new EventArgs());
			}
		}



		public Task<bool> ConnectAsync(string server, int port, CancellationToken token)
		{
			if (_client != null && _client.Connected)
				throw new Exception("Already connected.");

			_client = new TcpClient();
			_strServer = server;
			_nPort = port;
			_token = token;
			_bExit = false;

			bool done = false;


			Task.Run(() =>
			{
				try
				{
					_client.Connect(_strServer, _nPort);

					if (_client.Connected)
					{
						_stream = _client.GetStream();
						Connected?.Invoke(this, new EventArgs());
					}
				}
				catch(Exception)
				{
				}

				done = true;

				if (_stream == null)
					return;

				while (_bExit == false)
				{
					try
					{
						if (_token.IsCancellationRequested)
							break;

						if (_client == null || _client.Connected == false)
							break;

						var buffer = new byte[10240];
						int len = _stream.Read(buffer, 0, 10240);

						if (len <= 0)
							continue;

						Received?.Invoke(this, new ReceivedEventArgs(buffer, 0, len));
					}
					catch(Exception)
					{
						break;
					}
				}
				Disconnect();
			});


			return Task.Run(() =>
			{
				while (done == false)
				{
					Thread.Sleep(0);
				}
				return (_client.Connected);
			});
		}





		public Task SendAsync(string message)
		{
			if (_client == null || _client.Connected == false || _stream == null)
				throw new Exception("Not connected.");

			var data = Encoding.UTF8.GetBytes(message);
			return _stream.WriteAsync(data, 0, data.Length, _token);
		}

		public Task SendAsync(byte[] data, int offset, int length)
		{
			if (_client == null || _client.Connected == false || _stream == null)
				throw new Exception("Not connected.");

			return _stream.WriteAsync(data, offset, length, _token);
		}
	}
}
