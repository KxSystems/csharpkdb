using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace kx
{
	/// <summary>
	/// An extended class of <see cref="TcpClient"/> that serves as a connector class for 
	/// interfacing with a KDB+ process.
	/// </summary>
	/// <remarks>
	/// This class is essentially a serializer/deserializer of .NET types 
	/// to/from the KDB+ IPC wire format, enabling remote method invocation in KDB+ via TCP/IP.
	/// </remarks>
	public class c : TcpClient
	{
		private const int DefaultMaxBufferSize = 65536;

		private const long Year2000Ticks = 630822816000000000L;

		private const int KMinInt32 = int.MinValue;

		private const long KMinInt64 = long.MinValue;

		private const double NullDouble = double.NaN;

		private static readonly DateTime KMinDateTime = DateTime.MinValue.AddTicks(1L);

		private static readonly DateTime KMaxDateTime = DateTime.MaxValue;

		private static readonly object[] KNullValues = new object[20]
		{
			null,
			false,
			default(Guid),
			null,
			(byte)0,
			short.MinValue,
			KMinInt32,
			KMinInt64,
			(float)NullDouble,
			NullDouble,
			' ',
			"",
			new DateTime(0L),
			new Month(KMinInt32),
			new Date(KMinInt32),
			new DateTime(0L),
			new KTimespan(KMinInt64),
			new Minute(KMinInt32),
			new Second(KMinInt32),
			new TimeSpan(KMinInt64)
		};

		private static readonly char[] KNullCharIds = " bg xhijefcspmdznuvt"
			.ToCharArray();

		private static readonly int[] NumberOfBytesForType = new int[20]
		{
			0,
			1,
			16,
			0,
			1,
			2,
			4,
			8,
			4,
			8,
			1,
			0,
			8,
			4,
			4,
			8,
			8,
			4,
			4,
			4
		};

		private readonly Stream _clientStream;

		private readonly int _maxBufferSize;

		private readonly int _versionNumber;

		/// <summary>
		/// Used to convert a .NET <see cref="Guid"/> into a KDB+ compatible id.
		/// </summary>
		private readonly byte[] _guidInterProcess = new byte[16]
		{
			3,
			2,
			1,
			0,
			5,
			4,
			7,
			6,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15
		};

		/// <summary>
		/// The buffer used to store the incoming message bytes from the remote prior to de-serialization.
		/// </summary>
		private byte[] _readBuffer;

		/// <summary>
		/// The buffer used to store the outgoing message bytes when serializing an object.
		/// </summary>
		private byte[] _writeBuffer;

		/// <summary>
		/// The current position of the de-serializer within the read buffer.
		/// </summary>
		private int _readPosition;

		/// <summary>
		/// The current position of the serialiser within the write buffer.
		/// </summary>
		private int _writePosition;

		/// <summary>
		/// A boolean flag indicating the Endianness of a message.
		/// </summary>
		private bool _isLittleEndian;

		/// <summary>
		/// Initialises a new instance of <see cref="c" /> with a specified host and port 
		/// to connect to.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port to connect to.</param>
		/// <exception cref="ArgumentNullException"><paramref name="host" /> was null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="port" /> must be between <see cref="System.Net.IPEndPoint.MinPort" /> and <see cref="System.Net.IPEndPoint.MaxPort" /></exception>
		/// <exception cref="KException">Unable to connect to KDB+ process, access denied or process unavailable.</exception>
		public c(string host,
			int port)
			: this(host, port, Environment.UserName)
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="c" /> with a specified host and port 
		/// to connect to, a username/password for authentication, an optional maximum buffersize and
		/// an optional flag indicating whether to use TLS.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port to connect to.</param>
		/// <param name="userPassword">The username and passsword, as "username:password" for remote authorisation.</param>
		/// <param name="maxBufferSize">The maximum buffer size, default is 65536.</param>
		/// <param name="useTLS">A boolean flag indicating whether or not TLS authentication is enabled, default is false.</param>
		/// <exception cref="ArgumentNullException"><paramref name="host" /> or <paramref name="userPassword" /> was null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="port" /> must be between <see cref="System.Net.IPEndPoint.MinPort" /> and <see cref="System.Net.IPEndPoint.MaxPort" /></exception>
		/// <exception cref="KException">Unable to connect to KDB+ process, access denied or process unavailable.</exception>
		public c(string host,
			int port,
			string userPassword,
			int maxBufferSize = DefaultMaxBufferSize,
			bool useTLS = false)
		{
			if (host == null)
			{
				throw new ArgumentNullException(nameof(host),
					$"Unable to initialise c. {nameof(host)} parameter cannot be null");
			}
			if (port < 0 ||
				port > 65535)
			{
				throw new ArgumentOutOfRangeException(nameof(port),
					$"Unable to initialise c. {nameof(port)} parameter must be between MinPort and MaxPort");
			}
			if (userPassword == null)
			{
				throw new ArgumentNullException(nameof(userPassword),
					$"Unable to initialise c. {nameof(userPassword)} parameter cannot be null");
			}

			_maxBufferSize = maxBufferSize;
			Connect(host, port);
			_clientStream = GetStream();
			if (useTLS)
			{
				_clientStream = new SslStream(_clientStream, false);
				((SslStream)_clientStream).AuthenticateAsClient(host);
			}
			_writeBuffer = new byte[2 + userPassword.Length];
			_writePosition = 0;
			w(userPassword + "\u0003");
			_clientStream.Write(_writeBuffer, 0, _writePosition);
			if (_clientStream.Read(_writeBuffer, 0, 1) != 1)
			{
				throw new KException("access");
			}
			_versionNumber = Math.Min(_writeBuffer[0], (byte)3);
		}

		/// <summary>
		/// Initialises a new instance of <see cref="c"/>
		/// </summary>
		/// <remarks>
		/// Parameterless constructor intended for unit-testing only, keep internal.
		/// </remarks>
		internal c()
			: this(3)
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="c"/> with a specified
		/// KDB+ version number.
		/// </summary>
		/// <param name="versionNumber">The KDB+ version number to use for testing.</param>
		/// <remarks>
		/// Test constructor intended for unit-testing only, keep internal.
		/// </remarks>
		internal c(int versionNumber)
		{
			_versionNumber = versionNumber;
		}

		/// <summary>
		/// Gets or sets character encoding for serialising/deserialising strings, 
		/// default is <see cref="Encoding.ASCII"/>.
		/// </summary>
		public static Encoding e { get; set; } = Encoding.ASCII;

		/// <summary>
		/// Disposes this <see cref="kx.c"/> instance and requests that the underlying
		/// stream and TCP connection be closed.
		/// </summary>
		public new void Close()
		{
			if (_clientStream != null)
			{
				_clientStream.Close();
			}
			base.Close();
		}

		/// <summary>
		/// Reads an incoming message from the remote KDB+ process.
		/// </summary>
		/// <returns>
		/// Deserialised response to request.
		/// </returns>
		public object k()
		{
			_readBuffer = new byte[8];
			read(_readBuffer);

			_isLittleEndian = _readBuffer[0] == 1;
			bool num = _readBuffer[2] == 1;
			_readPosition = 4;

			_readBuffer = new byte[ri() - 8];
			read(_readBuffer);

			if (num)
			{
				u();
			}
			else
			{
				_readPosition = 0;
			}
			if (_readBuffer[0] == 128)
			{
				_readPosition = 1;
				throw new KException(rs());
			}
			return r();
		}

		/// <summary>
		/// Sends a sync message request to the remote KDB+ process.
		/// </summary>
		/// <param name="x">The object parameter to send.</param>
		/// <returns>
		/// Deserialised response to request.
		/// </returns>
		public object k(object x)
		{
			w(1, x);
			return k();
		}

		/// <summary>
		/// Sends a sync message request to the remote KDB+ process with a 
		/// specified expression.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		/// <returns>
		/// Deserialised response to request.
		/// </returns>
		public object k(string s)
		{
			return k(s.ToCharArray());
		}

		/// <summary>
		/// Sends a sync message request to the remote KDB+ process with a 
		/// specified expression and request object.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		/// <param name="x">The object parameter to send.</param>
		/// <returns>
		/// Deserialised response to request.
		/// </returns>
		public object k(string s, object x)
		{
			object[] array = new object[]
			{
				s.ToCharArray(),
				x
			};

			return k(array);
		}

		/// <summary>
		/// Sends a sync message request to the remote KDB+ process with a 
		/// specified expression and request objects.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		/// <param name="x">The first object parameter to send.</param>
		/// <param name="y">The second object parameter to send.</param>
		/// <returns>
		/// Deserialised response to request.
		/// </returns>
		public object k(string s, object x, object y)
		{
			object[] array = new object[]
			{
				s.ToCharArray(),
				x,
				y
			};

			return k(array);
		}

		/// <summary>
		/// Sends a sync message request to the remote KDB+ process with a 
		/// specified expression and request objects.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		/// <param name="x">The first object parameter to send.</param>
		/// <param name="y">The second object parameter to send.</param>
		/// <param name="z">The third object parameter to send.</param>
		/// <returns>
		/// Deserialised response to request.
		/// </returns>
		public object k(string s, object x, object y, object z)
		{
			object[] array = new object[]
			{
				s.ToCharArray(),
				x,
				y,
				z
			};

			return k(array);
		}

		/// <summary>
		/// Sends an async message to the remote KDB+ process with a specified expression.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		public void ks(string s)
		{
			w(0, s.ToCharArray());
		}

		/// <summary>
		/// Sends an async message to the remote KDB+ process with a specified expression 
		/// and object parameter.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		/// <param name="x">The object parameter to send.</param>
		public void ks(string s, object x)
		{
			object[] array = new object[]
			{
				s.ToCharArray(),
				x
			};

			w(0, array);
		}

		/// <summary>
		/// Sends an async message to the remote KDB+ process with a specified expression 
		/// and object parameters.
		/// </summary>
		/// <param name="s">The expression to send.</param>
		/// <param name="x">The first object parameter to send.</param>
		/// <param name="y">The second object parameter to send.</param>
		public void ks(string s, object x, object y)
		{
			object[] array = new object[]
			{
				s.ToCharArray(),
				x,
				y
			};

			w(0, array);
		}

		/// <summary>
		/// Serialises a specified object as a byte-array
		/// </summary>
		/// <param name="messageType">The type of object to be serialised.</param>
		/// <param name="x">The object to be serialised.</param>
		/// <returns>
		/// A byte-array containing the serialised object data.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="x"/> parameter was null.</exception>
		public byte[] Serialize(int messageType, object x)
		{
			if (x == null)
			{
				throw new ArgumentNullException(nameof(x),
					$"Unable to serialize data. {nameof(x)} parameter cannot be null");
			}

			int length = nx(x) + 8;
			_writeBuffer = new byte[length];
			_writeBuffer[0] = 1;
			_writeBuffer[1] = (byte)messageType;
			_writePosition = 4;
			w(length);
			w(x);

			return _writeBuffer;
		}

		/// <summary>
		/// Deserialises a specified byte-array to an object.
		/// </summary>
		/// <param name="buffer">The byte-array to be deserialised.</param>
		/// <returns>
		/// The deserialised object.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="buffer"/> parameter was null.</exception>
		/// <exception cref="KException">Error occurred during de-serialisation.</exception>
		public object Deserialize(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException(nameof(buffer),
					$"Unable to deserialize data. {nameof(buffer)} parameter cannot be null");
			}

			_readBuffer = buffer;
			_isLittleEndian = _readBuffer[0] == 1;

			bool isCompressed = _readBuffer[2] == 1;

			int responseLength = buffer.Length - 8;
			_readBuffer = new byte[responseLength];

			Array.Copy(buffer, 8, _readBuffer, 0, responseLength);

			if (isCompressed)
			{
				u();
			}
			else
			{
				_readPosition = 0;
			}
			if (_readBuffer[0] == 128)
			{
				_readPosition = 1;
				throw new KException(rs());
			}
			return r();
		}

		/// <summary>
		/// Gets the null object for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="t">The .NET type.</param>
		/// <returns>
		/// Instance of null object of specified KDB+ type.
		/// </returns>
		public static object NULL(Type t)
		{
			for (int i = 0; i < KNullValues.Length; i++)
			{
				if (KNullValues[i] != null &&
					t == KNullValues[i].GetType())
				{
					return KNullValues[i];
				}
			}
			return null;
		}

		/// <summary>
		/// Gest the null object for the specified <see cref="char"/> id.
		/// </summary>
		/// <param name="c">The character id.</param>
		/// <returns>
		/// Instance of null object of specified KDB+ type.
		/// </returns>
		/// <exception cref="ArgumentException"><paramref name="c"/> character is not recognised.</exception>
		public static object NULL(char c)
		{
			int index = Array.IndexOf(KNullCharIds, c);

			if (index == -1)
			{
				throw new ArgumentException($"Unable to find KDB+ null for character {c}, not recognised",
					nameof(c));
			}

			return KNullValues[index];
		}

		/// <summary>
		/// Tests whether an object is a null object of that type.
		/// </summary>
		/// <param name="x">The object to be tested for null.</param>
		/// <returns>
		/// <c>true</c> if the <paramref name="x"/> is KDB+ null; otherwise returns <c>false</c>.
		/// </returns>
		/// <remarks>
		/// qn(NULL('j')) should return <c>true</c>.
		/// </remarks>
		public static bool qn(object x)
		{
			int t = -c.t(x);
			if (t == 2 || t > 4)
			{
				return x.Equals(KNullValues[t]);
			}
			return false;
		}

		/// <summary>
		/// Removes the key from a keyed table. 
		/// </summary>
		/// <param name="X">The original object to convert to a <see cref="Flip"/>.</param>
		/// <returns>
		/// A simple table.
		/// </returns>
		public static Flip td(object X)
		{
			if (t(X) == 98)
			{
				return (Flip)X;
			}
			Dict obj = (Dict)X;
			Flip a = (Flip)obj.x;
			Flip obj2 = (Flip)obj.y;
			int j = n(a.x);
			int i = n(obj2.x);
			string[] x = new string[j + i];
			Array.Copy(a.x, 0, x, 0, j);
			Array.Copy(obj2.x, 0, x, j, i);
			object[] y = new object[j + i];
			Array.Copy(a.y, 0, y, 0, j);
			Array.Copy(obj2.y, 0, y, j, i);

			return new Flip(new Dict(x, y));
		}

		/// <summary>
		/// Returns the number of elements in a specified object.
		/// </summary>
		/// <param name="x">The object to be serialised.</param>
		/// <returns>
		/// The number of elements in an object.
		/// </returns>
		public static int n(object x)
		{
			Dict dict = x as Dict;
			if (dict != null)
			{
				return n(dict.x);
			}
			Flip flip = x as Flip;
			if (flip != null)
			{
				return n(flip.y[0]);
			}

			if (x is char[])
			{
				return e.GetBytes((char[])x).Length;
			}
			return ((Array)x).Length;
		}

		/// <summary>
		/// Gets the object at an index of an array.
		/// </summary>
		/// <param name="x">The array to index.</param>
		/// <param name="i">The offset to index at.</param>
		/// <returns>
		/// The object at the index, or null if the object value represents 
		/// a KDB+ null value for its' type.
		/// </returns>
		public static object at(object x, int i)
		{
			object r = ((Array)x).GetValue(i);
			if (!qn(r))
			{
				return r;
			}
			return null;
		}

		private void u()
		{
			int j = 0;
			int r = 0;
			int f = 0;
			int s = 8;
			int p = s;
			short k = 0;
			_readPosition = 0;
			byte[] dst = new byte[ri()];
			int d = _readPosition;
			int[] aa = new int[256];
			while (s < dst.Length)
			{
				if (k == 0)
				{
					f = (0xFF & _readBuffer[d++]);
					k = 1;
				}
				if ((f & k) != 0)
				{
					r = aa[0xFF & _readBuffer[d++]];
					dst[s++] = dst[r++];
					dst[s++] = dst[r++];
					j = (0xFF & _readBuffer[d++]);
					for (int i = 0; i < j; i++)
					{
						dst[s + i] = dst[r + i];
					}
				}
				else
				{
					dst[s++] = _readBuffer[d++];
				}
				while (p < s - 1)
				{
					aa[(0xFF & dst[p]) ^ (0xFF & dst[p + 1])] = p++;
				}
				if ((f & k) != 0)
				{
					p = (s += j);
				}
				k = (short)(k * 2);
				if (k == 256)
				{
					k = 0;
				}
			}
			_readBuffer = dst;
			_readPosition = 8;
		}

		private void w(bool x)
		{
			_writeBuffer[_writePosition++] = (byte)(x ? 1 : 0);
		}

		private bool rb()
		{
			return 1 == _readBuffer[_readPosition++];
		}

		private void w(byte x)
		{
			_writeBuffer[_writePosition++] = x;
		}

		private byte rx()
		{
			return _readBuffer[_readPosition++];
		}

		private void w(short h)
		{
			_writeBuffer[_writePosition++] = (byte)h;
			_writeBuffer[_writePosition++] = (byte)(h >> 8);
		}

		private short rh()
		{
			int x = _readBuffer[_readPosition++];
			int y = _readBuffer[_readPosition++];
			return (short)(_isLittleEndian ? ((x & 0xFF) | (y << 8)) : ((x << 8) | (y & 0xFF)));
		}

		private void w(int i)
		{
			w((short)i);
			w((short)(i >> 16));
		}

		private int ri()
		{
			int x = rh();
			int y = rh();
			if (!_isLittleEndian)
			{
				return (x << 16) | (y & 0xFFFF);
			}
			return (x & 0xFFFF) | (y << 16);
		}

		private void w(Guid g)
		{
			byte[] b = g.ToByteArray();
			if (_versionNumber < 3)
			{
				throw new KException("Guid not valid pre kdb+3.0");
			}
			for (int i = 0; i < b.Length; i++)
			{
				w(b[_guidInterProcess[i]]);
			}
		}

		private Guid rg()
		{
			bool oa = _isLittleEndian;
			_isLittleEndian = false;
			int j = ri();
			short h3 = rh();
			short h2 = rh();
			_isLittleEndian = oa;
			byte[] b = new byte[8];
			for (int i = 0; i < 8; i++)
			{
				b[i] = rx();
			}
			return new Guid(j, h3, h2, b);
		}

		private void w(long j)
		{
			w((int)j);
			w((int)(j >> 32));
		}

		private long rj()
		{
			int x = ri();
			int y = ri();
			if (!_isLittleEndian)
			{
				return ((long)x << 32) | (y & uint.MaxValue);
			}
			return (x & uint.MaxValue) | ((long)y << 32);
		}

		private void w(float e)
		{
			byte[] bytes = BitConverter.GetBytes(e);
			foreach (byte i in bytes)
			{
				w(i);
			}
		}

		private float re()
		{
			if (!_isLittleEndian)
			{
				byte c2 = _readBuffer[_readPosition];
				_readBuffer[_readPosition] = _readBuffer[_readPosition + 3];
				_readBuffer[_readPosition + 3] = c2;
				c2 = _readBuffer[_readPosition + 1];
				_readBuffer[_readPosition + 1] = _readBuffer[_readPosition + 2];
				_readBuffer[_readPosition + 2] = c2;
			}
			float result = BitConverter.ToSingle(_readBuffer, _readPosition);
			_readPosition += 4;
			return result;
		}

		private void w(double f)
		{
			w(BitConverter.DoubleToInt64Bits(f));
		}

		private double rf()
		{
			return BitConverter.Int64BitsToDouble(rj());
		}

		private void w(char c)
		{
			w((byte)c);
		}

		private char rc()
		{
			return (char)(_readBuffer[_readPosition++] & 0xFF);
		}

		private void w(string s)
		{
			byte[] bytes = e.GetBytes(s);
			foreach (byte i in bytes)
			{
				w(i);
			}
			_writeBuffer[_writePosition++] = 0;
		}

		private string rs()
		{
			int i = _readPosition;
			while (_readBuffer[_readPosition] != 0)
			{
				_readPosition++;
			}
			string @string = e.GetString(_readBuffer, i, _readPosition - i);
			_readPosition++;
			return @string;
		}

		private void w(Date d)
		{
			w(d.i);
		}

		private Date rd()
		{
			return new Date(ri());
		}

		private void w(Minute u)
		{
			w(u.i);
		}

		private Minute ru()
		{
			return new Minute(ri());
		}

		private void w(Month m)
		{
			w(m.i);
		}

		private Month rm()
		{
			return new Month(ri());
		}

		private void w(Second v)
		{
			w(v.i);
		}

		private Second rv()
		{
			return new Second(ri());
		}

		private void w(TimeSpan t)
		{
			if (_versionNumber < 1)
			{
				throw new KException("Timespan not valid pre kdb+2.6");
			}
			w((int)(qn(t) ? KMinInt32 : (t.Ticks / 10000)));
		}

		private TimeSpan rt()
		{
			int i = ri();
			return new TimeSpan(qn(i) ? KMinInt64 : (10000L * (long)i));
		}

		private void w(DateTime p)
		{
			if (_versionNumber < 1)
			{
				throw new KException("Timestamp not valid pre kdb+2.6");
			}
			w(qn(p) ? KMinInt64 : (100 * (p.Ticks - Year2000Ticks)));
		}

		private DateTime rz()
		{
			double f = rf();
			if (!double.IsInfinity(f))
			{
				return new DateTime(qn(f) ? 0 : clampDT(10000 * (long)Math.Round(86400000.0 * f) + Year2000Ticks));
			}
			if (f >= 0.0)
			{
				return KMaxDateTime;
			}
			return KMinDateTime;
		}

		private void w(KTimespan t)
		{
			w(qn(t) ? KMinInt64 : (t.t.Ticks * 100));
		}

		private KTimespan rn()
		{
			return new KTimespan(rj());
		}

		private DateTime rp()
		{
			long i = rj();
			long d = (i < 0) ? ((i + 1) / 100 - 1) : (i / 100);
			return new DateTime((i == KMinInt64) ? 0 : (Year2000Ticks + d));
		}

		private void w(object x)
		{
			int t = c.t(x);
			w((byte)t);
			switch (t)
			{
				case -1:
					w((bool)x);
					return;
				case -2:
					w((Guid)x);
					return;
				case -4:
					w((byte)x);
					return;
				case -5:
					w((short)x);
					return;
				case -6:
					w((int)x);
					return;
				case -7:
					w((long)x);
					return;
				case -8:
					w((float)x);
					return;
				case -9:
					w((double)x);
					return;
				case -10:
					w((char)x);
					return;
				case -11:
					w((string)x);
					return;
				case -12:
					w((DateTime)x);
					return;
				case -13:
					w((Month)x);
					return;
				case -14:
					w((Date)x);
					return;
				case -15:
					w((DateTime)x);
					return;
				case -16:
					w((KTimespan)x);
					return;
				case -17:
					w((Minute)x);
					return;
				case -18:
					w((Second)x);
					return;
				case -19:
					w((TimeSpan)x);
					return;
			}
			if (t == 99)
			{
				Dict r2 = (Dict)x;
				w(r2.x);
				w(r2.y);
				return;
			}
			_writeBuffer[_writePosition++] = 0;
			if (t == 98)
			{
				Flip r = (Flip)x;
				_writeBuffer[_writePosition++] = 99;
				w(r.x);
				w(r.y);
				return;
			}
			w(n(x));
			switch (t)
			{
				case 3:
					break;
				case 0:
					{
						foreach (object obj in (object[])x)
						{
							w(obj);
						}
						break;
					}
				case 1:
					{
						foreach (bool obj in (bool[])x)
						{
							w(obj);
						}
						break;
					}
				case 2:
					{
						foreach (Guid obj in (Guid[])x)
						{
							w(obj);
						}
						break;
					}
				case 4:
					{
						foreach (byte obj in (byte[])x)
						{
							w(obj);
						}
						break;
					}
				case 5:
					{
						foreach (short obj in (short[])x)
						{
							w(obj);
						}
						break;
					}
				case 6:
					{
						foreach (int obj in (int[])x)
						{
							w(obj);
						}
						break;
					}
				case 7:
					{
						foreach (long obj in (long[])x)
						{
							w(obj);
						}
						break;
					}
				case 8:
					{
						foreach (float obj in (float[])x)
						{
							w(obj);
						}
						break;
					}
				case 9:
					{
						foreach (double obj in (double[])x)
						{
							w(obj);
						}
						break;
					}
				case 10:
					{
						byte[] byteArray = e.GetBytes((char[])x);
						foreach (byte obj in byteArray)
						{
							w(obj);
						}
						break;
					}
				case 11:
					{
						foreach (string obj in (string[])x)
						{
							w(obj);
						}
						break;
					}
				case 12:
					{
						foreach (DateTime obj in (DateTime[])x)
						{
							w(obj);
						}
						break;
					}
				case 13:
					{
						foreach (Month obj in (Month[])x)
						{
							w(obj);
						}
						break;
					}
				case 14:
					{
						foreach (Date obj in (Date[])x)
						{
							w(obj);
						}
						break;
					}
				case 15:
					{
						foreach (DateTime obj in (DateTime[])x)
						{
							w(obj);
						}
						break;
					}
				case 16:
					{
						foreach (KTimespan obj in (KTimespan[])x)
						{
							w(obj);
						}
						break;
					}
				case 17:
					{
						foreach (Minute obj in (Minute[])x)
						{
							w(obj);
						}
						break;
					}
				case 18:
					{
						foreach (Second obj in (Second[])x)
						{
							w(obj);
						}
						break;
					}
				case 19:
					{
						foreach (TimeSpan obj in (TimeSpan[])x)
						{
							w(obj);
						}
						break;
					}
			}
		}

		private object r()
		{
			int i = 0;
			int t = (sbyte)_readBuffer[_readPosition++];
			switch (t)
			{
				case -1:
					return rb();
				case -2:
					return rg();
				case -4:
					return _readBuffer[_readPosition++];
				case -5:
					return rh();
				case -6:
					return ri();
				case -7:
					return rj();
				case -8:
					return re();
				case -9:
					return rf();
				case -10:
					return rc();
				case -11:
					return rs();
				case -12:
					return rp();
				case -13:
					return rm();
				case -14:
					return rd();
				case -15:
					return rz();
				case -16:
					return rn();
				case -17:
					return ru();
				case -18:
					return rv();
				case -19:
					return rt();
				default:
					{
						if (t > 99)
						{
							if (t == 101 && _readBuffer[_readPosition++] == 0)
							{
								return null;
							}
							throw new KException("func");
						}
						if (t == 99)
						{
							return new Dict(r(), r());
						}
						_readPosition++;
						if (t == 98)
						{
							return new Flip((Dict)r());
						}
						int j = ri();
						switch (t)
						{
							case 0:
								{
									object[] L = new object[j];
									for (; i < j; i++)
									{
										L[i] = r();
									}
									return L;
								}
							case 1:
								{
									bool[] B = new bool[j];
									for (; i < j; i++)
									{
										B[i] = rb();
									}
									return B;
								}
							case 2:
								{
									Guid[] G = new Guid[j];
									for (; i < j; i++)
									{
										G[i] = rg();
									}
									return G;
								}
							case 4:
								{
									byte[] G2 = new byte[j];
									for (; i < j; i++)
									{
										G2[i] = _readBuffer[_readPosition++];
									}
									return G2;
								}
							case 5:
								{
									short[] H = new short[j];
									for (; i < j; i++)
									{
										H[i] = rh();
									}
									return H;
								}
							case 6:
								{
									int[] I = new int[j];
									for (; i < j; i++)
									{
										I[i] = ri();
									}
									return I;
								}
							case 7:
								{
									long[] J = new long[j];
									for (; i < j; i++)
									{
										J[i] = rj();
									}
									return J;
								}
							case 8:
								{
									float[] E = new float[j];
									for (; i < j; i++)
									{
										E[i] = re();
									}
									return E;
								}
							case 9:
								{
									double[] F = new double[j];
									for (; i < j; i++)
									{
										F[i] = rf();
									}
									return F;
								}
							case 10:
								{
									char[] chars = e.GetChars(_readBuffer, _readPosition, j);
									_readPosition += j;
									return chars;
								}
							case 11:
								{
									string[] S = new string[j];
									for (; i < j; i++)
									{
										S[i] = rs();
									}
									return S;
								}
							case 12:
								{
									DateTime[] P = new DateTime[j];
									for (; i < j; i++)
									{
										P[i] = rp();
									}
									return P;
								}
							case 13:
								{
									Month[] M = new Month[j];
									for (; i < j; i++)
									{
										M[i] = rm();
									}
									return M;
								}
							case 14:
								{
									Date[] D = new Date[j];
									for (; i < j; i++)
									{
										D[i] = rd();
									}
									return D;
								}
							case 15:
								{
									DateTime[] Z = new DateTime[j];
									for (; i < j; i++)
									{
										Z[i] = rz();
									}
									return Z;
								}
							case 16:
								{
									KTimespan[] N = new KTimespan[j];
									for (; i < j; i++)
									{
										N[i] = rn();
									}
									return N;
								}
							case 17:
								{
									Minute[] U = new Minute[j];
									for (; i < j; i++)
									{
										U[i] = ru();
									}
									return U;
								}
							case 18:
								{
									Second[] V = new Second[j];
									for (; i < j; i++)
									{
										V[i] = rv();
									}
									return V;
								}
							case 19:
								{
									TimeSpan[] T = new TimeSpan[j];
									for (; i < j; i++)
									{
										T[i] = rt();
									}
									return T;
								}
							default:
								return null;
						}
					}
			}
		}

		private void w(int i, object x)
		{
			int j = nx(x) + 8;
			_writeBuffer = new byte[j];
			_writeBuffer[0] = 1;
			_writeBuffer[1] = (byte)i;
			_writePosition = 4;
			w(j);
			w(x);
			_clientStream.Write(_writeBuffer, 0, j);
		}

		private void read(byte[] b)
		{
			int k = 0;
			int j = b.Length;
			while (true)
			{
				if (k < j)
				{
					int i;
					if ((i = _clientStream.Read(b, k, Math.Min(_maxBufferSize, j - k))) == 0)
					{
						break;
					}
					k += i;
					continue;
				}
				return;
			}
			throw new Exception("read");
		}

		private static int ns(string s)
		{
			int j = s.IndexOf('\0');
			j = ((-1 < j) ? j : s.Length);
			return e.GetBytes(s.Substring(0, j)).Length;
		}

		private static string i2(int i)
		{
			return $"{i:00}";
		}

		private static long clampDT(long j)
		{
			return Math.Min(Math.Max(j, KMinDateTime.Ticks), KMaxDateTime.Ticks);
		}

		private static int t(object x)
		{
			if (x is bool)
			{
				return -1;
			}
			if (x is Guid)
			{
				return -2;
			}
			if (x is byte)
			{
				return -4;
			}
			if (x is short)
			{
				return -5;
			}
			if (x is int)
			{
				return -6;
			}
			if (x is long)
			{
				return -7;
			}
			if (x is float)
			{
				return -8;
			}
			if (x is double)
			{
				return -9;
			}
			if (x is char)
			{
				return -10;
			}
			if (x is string)
			{
				return -11;
			}
			if (x is DateTime)
			{
				return -12;
			}
			if (x is Month)
			{
				return -13;
			}
			if (x is Date)
			{
				return -14;
			}
			if (x is DateTime)
			{
				return -15;
			}
			if (x is KTimespan)
			{
				return -16;
			}
			if (x is Minute)
			{
				return -17;
			}
			if (x is Second)
			{
				return -18;
			}
			if (x is TimeSpan)
			{
				return -19;
			}
			if (x is bool[])
			{
				return 1;
			}
			if (x is Guid[])
			{
				return 2;
			}
			if (x is byte[])
			{
				return 4;
			}
			if (x is short[])
			{
				return 5;
			}
			if (x is int[])
			{
				return 6;
			}
			if (x is long[])
			{
				return 7;
			}
			if (x is float[])
			{
				return 8;
			}
			if (x is double[])
			{
				return 9;
			}
			if (x is char[])
			{
				return 10;
			}
			if (x is string[])
			{
				return 11;
			}
			if (x is DateTime[])
			{
				return 12;
			}
			if (x is Month[])
			{
				return 13;
			}
			if (x is Date[])
			{
				return 14;
			}

			if (x is KTimespan[])
			{
				return 16;
			}
			if (x is Minute[])
			{
				return 17;
			}
			if (x is Second[])
			{
				return 18;
			}

			if (x is TimeSpan[])
			{
				return 19;
			}
			if (x is Flip)
			{
				return 98;
			}
			if (x is Dict)
			{
				return 99;
			}
			return 0;
		}

		private static int nx(object x)
		{
			int k = 0;
			int t = c.t(x);
			if (t == 99)
			{
				return 1 + nx(((Dict)x).x) + nx(((Dict)x).y);
			}
			if (t == 98)
			{
				return 3 + nx(((Flip)x).x) + nx(((Flip)x).y);
			}
			if (t < 0)
			{
				if (t != -11)
				{
					return 1 + NumberOfBytesForType[-t];
				}
				return 2 + ns((string)x);
			}
			int j = 6;
			int i = n(x);
			if (t == 0 || t == 11)
			{
				for (; k < i; k++)
				{
					j += ((t == 0) ? nx(((object[])x)[k]) : (1 + ns(((string[])x)[k])));
				}
			}
			else
			{
				j += i * NumberOfBytesForType[t];
			}
			return j;
		}

		/// <summary>
		/// Represents a KDB+ Date type.
		/// </summary>
		[Serializable]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1716: Allow class name c.Date for backwards compatability")]
		public class Date : IComparable, IComparable<Date>, IEquatable<Date>
		{
			/// <summary>
			/// Initialises a new instance of <see cref="Date"/> using 
			/// a specified int that respresents the value.
			/// </summary>
			/// <param name="x">The value.</param>
			public Date(int x)
			{
				i = x;
			}

			/// <summary>
			/// Initialises a new instance of <see cref="Date"/> using 
			/// a specified long that respresents the value.
			/// </summary>
			/// <param name="x">The value.</param>
			public Date(long x)
			{
				i = (x == 0L) ? KMinInt32 : ((int)(x / 864000000000L) - 730119);
			}

			/// <summary>
			/// Initialises a new instance of <see cref="Date"/> using 
			/// a specified <see cref="DateTime"/> that respresents the value.
			/// </summary>
			/// <param name="z">The value.</param>
			public Date(DateTime z)
				: this(z.Ticks)
			{
			}

			/// <summary>
			/// Gets or sets the value of this KDB+ Date.
			/// </summary>
			public int i
			{
				get;
				set;
			}

			/// <summary>
			/// Converts this KDB+ <see cref="Date"/> into an equivalent 
			/// .NET <see cref="DateTime"/>
			/// </summary>
			/// <returns>
			/// A <see cref="DateTime"/> that is equivalent to the KDB+ Date.
			/// </returns>
			public DateTime DateTime()
			{
				if (i != -2147483647)
				{
					if (i != int.MaxValue)
					{
						return new DateTime((i == KMinInt32) ? 0 : clampDT(864000000000L * i + Year2000Ticks));
					}
					return KMaxDateTime;
				}
				return KMinDateTime;
			}

			#region Object Overrides
			/// <inheritdoc />
			[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "S4136: Equals to remain in Object Override and IEquatable<T> region")]
			public override bool Equals(object obj)
			{
				return Equals(obj as Date);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return i;
			}

			/// <inheritdoc />
			public override string ToString()
			{
				if (i != KMinInt32)
				{
					return DateTime().ToString("d", System.Globalization.CultureInfo.InvariantCulture);
				}
				return string.Empty;
			}
			#endregion Object Overrides

			#region IComparable Members
			/// <inheritdoc />
			public int CompareTo(object obj)
			{
				return CompareTo(obj as Date);
			}
			#endregion IComparable Members

			#region IComparable<Date> Members
			/// <inheritdoc />
			public int CompareTo(Date other)
			{
				if (other is null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
			#endregion IComparable<Date> Members

			#region IEquatable<Date> Members
			/// <inheritdoc />
			public virtual bool Equals(Date other)
			{
				if (other is null)
				{
					return false;
				}
				if (ReferenceEquals(other, this))
				{
					return true;
				}
				return i == other.i;
			}
			#endregion IEquatable<Date> Members

			/// <summary>
			/// Determines whether two specified instances of <see cref="Date"/> are equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator ==(Date left, Date right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.Equals(right);
			}

			/// <summary>
			/// Determines whether two specified instances of <see cref="Date"/> are not equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are not equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator !=(Date left, Date right)
			{
				return !(left == right);
			}

			/// <summary>
			/// Determines whether one specified <see cref="Date"/> is less than another specified 
			/// <see cref="Date"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <(Date left, Date right)
			{
				if (left is null)
				{
					return !(right is null);
				}
				return left.CompareTo(right) == -1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Date"/> is greater than another specified 
			/// <see cref="Date"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >(Date left, Date right)
			{
				if (left is null)
				{
					return false;
				}
				return left.CompareTo(right) == 1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Date"/> is less than or equal to 
			/// another specified 
			/// <see cref="Date"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <=(Date left, Date right)
			{
				if (left is null)
				{
					return true;
				}
				return left.CompareTo(right) <= 0;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Date"/> is greater than or equal to 
			/// another specified 
			/// <see cref="Date"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >=(Date left, Date right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.CompareTo(right) >= 0;
			}
		}

		/// <summary>
		/// Represents a KDB+ Month type.
		/// </summary>
		[Serializable]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		public class Month : IComparable, IComparable<Month>, IEquatable<Month>
		{
			/// <summary>
			/// Initialises a new instance of <see cref="Month"/> using a specified 
			/// number of months.
			/// </summary>
			/// <param name="x">The number of months since Jan 2000.</param>
			public Month(int x)
			{
				i = x;
			}

			/// <summary>
			/// Gets or sets the number of months since Jan 2000
			/// </summary>
			/// <remarks>
			/// Post-millennium is positive and pre is negative.
			/// </remarks>
			public int i
			{
				get;
				set;
			}

			#region Object Overrides
			/// <inheritdoc />
			[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "S4136: Equals to remain in Object Override and IEquatable<T> region")]
			public override bool Equals(object obj)
			{
				return Equals(obj as Month);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return i;
			}

			/// <inheritdoc />
			public override string ToString()
			{
				int value = 24000 + i;
				int y = value / 12;
				if (i != KMinInt32)
				{
					return i2(y / 100) + i2(y % 100) + "-" + i2(1 + value % 12);
				}
				return "";
			}
			#endregion Object Overrides

			#region IComparable Members
			/// <inheritdoc />
			public int CompareTo(object obj)
			{
				return CompareTo(obj as Month);
			}
			#endregion IComparable Members

			#region IComparable<Month> Members
			/// <inheritdoc />
			public int CompareTo(Month other)
			{
				if (other is null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
			#endregion IComparable<Month> Members

			#region IEquatable<Month> Members
			/// <inheritdoc />
			public virtual bool Equals(Month other)
			{
				if (other is null)
				{
					return false;
				}
				if (ReferenceEquals(other, this))
				{
					return true;
				}
				return other.i == i;
			}
			#endregion IEquatable<Month> Members

			/// <summary>
			/// Determines whether two specified instances of <see cref="Month"/> are equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator ==(Month left, Month right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.Equals(right);
			}

			/// <summary>
			/// Determines whether two specified instances of <see cref="Month"/> are not equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are not equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator !=(Month left, Month right)
			{
				return !(left == right);
			}

			/// <summary>
			/// Determines whether one specified <see cref="Month"/> is less than another specified 
			/// <see cref="Month"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <(Month left, Month right)
			{
				if (left is null)
				{
					return !(right is null);
				}
				return left.CompareTo(right) == -1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Month"/> is greater than another specified 
			/// <see cref="Month"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >(Month left, Month right)
			{
				if (left is null)
				{
					return false;
				}
				return left.CompareTo(right) == 1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Month"/> is less than or equal to 
			/// another specified 
			/// <see cref="Month"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <=(Month left, Month right)
			{
				if (left is null)
				{
					return true;
				}
				return left.CompareTo(right) <= 0;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Month"/> is greater than or equal to 
			/// another specified 
			/// <see cref="Month"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >=(Month left, Month right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.CompareTo(right) >= 0;
			}
		}

		/// <summary>
		/// Represents a KDB+ Minute type.
		/// </summary>
		[Serializable]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		public class Minute : IComparable, IComparable<Minute>, IEquatable<Minute>
		{
			/// <summary>
			/// Initialises a new instance of <see cref="Month"/> using a specified 
			/// number of minutes since midnight.
			/// </summary>
			/// <param name="x">Number of minutes since midnight.</param>
			public Minute(int x)
			{
				i = x;
			}

			/// <summary>
			/// Gets or sets the number of minutes since midnight.
			/// </summary>
			public int i
			{
				get;
				set;
			}

			#region Object Overrides
			/// <inheritdoc />
			[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "S4136: Equals to remain in Object Override and IEquatable<T> region")]
			public override bool Equals(object obj)
			{
				return Equals(obj as Minute);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return i;
			}

			/// <inheritdoc />
			public override string ToString()
			{
				if (i != KMinInt32)
				{
					return i2(i / 60) + ":" + i2(i % 60);
				}
				return "";
			}
			#endregion Object Overrides

			#region IComparable Members
			///<inheritdoc />
			public int CompareTo(object obj)
			{
				return CompareTo(obj as Minute);
			}
			#endregion IComparable Members

			#region IComparable<Minute> Members
			///<inheritdoc />
			public int CompareTo(Minute other)
			{
				if (other is null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
			#endregion IComparable<Minute> Members

			#region IEquatable<Minute> Members
			///<inheritdoc />
			public virtual bool Equals(Minute other)
			{
				if (other is null)
				{
					return false;
				}
				if (ReferenceEquals(other, this))
				{
					return true;
				}
				return i == other.i;
			}
			#endregion IEquatable<Minute> Members

			/// <summary>
			/// Determines whether two specified instances of <see cref="Minute"/> are equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator ==(Minute left, Minute right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.Equals(right);
			}

			/// <summary>
			/// Determines whether two specified instances of <see cref="Minute"/> are not equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are not equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator !=(Minute left, Minute right)
			{
				return !(left == right);
			}

			/// <summary>
			/// Determines whether one specified <see cref="Minute"/> is less than another specified 
			/// <see cref="Minute"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <(Minute left, Minute right)
			{
				if (left is null)
				{
					return !(right is null);
				}
				return left.CompareTo(right) == -1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Minute"/> is greater than another specified 
			/// <see cref="Minute"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >(Minute left, Minute right)
			{
				if (left is null)
				{
					return false;
				}
				return left.CompareTo(right) == 1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Minute"/> is less than or equal to 
			/// another specified <see cref="Minute"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <=(Minute left, Minute right)
			{
				if (left is null)
				{
					return true;
				}
				return left.CompareTo(right) <= 0;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Minute"/> is greater than or equal to 
			/// another specified <see cref="Minute"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >=(Minute left, Minute right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.CompareTo(right) >= 0;
			}
		}

		/// <summary>
		/// Represents a KDB+ Second type.
		/// </summary>
		[Serializable]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		public class Second : IComparable, IComparable<Second>, IEquatable<Second>
		{
			/// <summary>
			/// Initialises a new instance of <see cref="Month"/> using a specified 
			/// number of seconds.
			/// </summary>
			/// <param name="x">The number of seconds since midnight.</param>
			public Second(int x)
			{
				i = x;
			}

			/// <summary>
			/// Gets or sets the number of seconds since midnight.
			/// </summary>
			public int i
			{
				get;
				set;
			}

			#region Object Overrides
			///<inheritdoc />
			[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "S4136: Equals to remain in Object Override and IEquatable<T> region")]
			public override bool Equals(object obj)
			{
				return Equals(obj as Second);
			}

			///<inheritdoc />
			public override int GetHashCode()
			{
				return i;
			}

			///<inheritdoc />
			public override string ToString()
			{
				if (i != KMinInt32)
				{
					return new Minute(i / 60).ToString() + ":" + i2(i % 60);
				}
				return "";
			}
			#endregion Object Overrides

			#region IComparable Members
			/// <inheritdoc />
			public int CompareTo(object obj)
			{
				return CompareTo(obj as Second);
			}
			#endregion IComparable Members

			#region IComparable<Second> Members
			/// <inheritdoc />
			public int CompareTo(Second other)
			{
				if (other is null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
			#endregion IComparable<Second> Members

			#region IEquatable<Second> Members
			/// <inheritdoc />
			public virtual bool Equals(Second other)
			{
				if (other is null)
				{
					return false;
				}
				if (ReferenceEquals(other, this))
				{
					return true;
				}
				return i == other.i;
			}
			#endregion IEquatable<Second> Members

			/// <summary>
			/// Determines whether two specified instances of <see cref="Second"/> are equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator ==(Second left, Second right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.Equals(right);
			}

			/// <summary>
			/// Determines whether two specified instances of <see cref="Second"/> are not equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are not equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator !=(Second left, Second right)
			{
				return !(left == right);
			}

			/// <summary>
			/// Determines whether one specified <see cref="Second"/> is less than another specified 
			/// <see cref="Second"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <(Second left, Second right)
			{
				if (left is null)
				{
					return !(right is null);
				}
				return left.CompareTo(right) == -1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Second"/> is greater than another specified 
			/// <see cref="Second"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >(Second left, Second right)
			{
				if (left is null)
				{
					return false;
				}
				return left.CompareTo(right) == 1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Second"/> is less than or equal to 
			/// another specified <see cref="Second"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <=(Second left, Second right)
			{
				if (left is null)
				{
					return true;
				}
				return left.CompareTo(right) <= 0;
			}

			/// <summary>
			/// Determines whether one specified <see cref="Second"/> is greater than or equal to 
			/// another specified <see cref="Second"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >=(Second left, Second right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.CompareTo(right) >= 0;
			}
		}

		/// <summary>
		/// Represents a KDB+ TimeSpan type.
		/// </summary>
		[Serializable]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		public class KTimespan : IComparable, IComparable<KTimespan>, IEquatable<KTimespan>
		{
			/// <summary>
			/// Initialises a new instance of <see cref="KTimespan"/> using a specified 
			/// number of nanoseconds.
			/// </summary>
			/// <param name="x">Number of nanoseconds since midnight.</param>
			public KTimespan(long x)
			{
				t = new TimeSpan((x == KMinInt64) ? KMinInt64 : (x / 100));
			}

			/// <summary>
			/// Gets or sets the .NET <see cref="TimeSpan"/>.
			/// </summary>
			public TimeSpan t
			{
				get;
				set;
			}

			#region Object Overrides
			/// <inheritdoc />
			[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "S4136: Equals to remain in Object Override and IEquatable<T> region")]
			public override bool Equals(object obj)
			{
				return Equals(obj as KTimespan);
			}

			/// <inheritdoc />
			public override int GetHashCode()
			{
				return t.GetHashCode();
			}

			/// <inheritdoc />
			public override string ToString()
			{
				if (!qn(t))
				{
					return t.ToString();
				}
				return "";
			}
			#endregion Object Overrides

			#region IComparable Members
			/// <inheritdoc />
			public int CompareTo(object obj)
			{
				return CompareTo(obj as KTimespan);
			}
			#endregion IComparable Members

			#region IComparable<KTimespan> Members
			/// <inheritdoc />
			public int CompareTo(KTimespan other)
			{
				if (other is null)
				{
					return 1;
				}
				return t.CompareTo(other.t);
			}
			#endregion IComparable<KTimespan> Members

			#region IEquatable<KTimespan> Members
			///<inheritdoc />
			public virtual bool Equals(KTimespan other)
			{
				if (other is null)
				{
					return false;
				}
				if (ReferenceEquals(other, this))
				{
					return true;
				}
				return t.Ticks == other.t.Ticks;
			}
			#endregion IEquatable<KTimespan> Members

			/// <summary>
			/// Determines whether two specified instances of <see cref="KTimespan"/> are equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator ==(KTimespan left, KTimespan right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.Equals(right);
			}

			/// <summary>
			/// Determines whether two specified instances of <see cref="KTimespan"/> are not equal.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if instances are not equal; otherwise <c>false</c>.
			/// </returns>
			public static bool operator !=(KTimespan left, KTimespan right)
			{
				return !(left == right);
			}

			/// <summary>
			/// Determines whether one specified <see cref="KTimespan"/> is less than another specified 
			/// <see cref="KTimespan"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <(KTimespan left, KTimespan right)
			{
				if (left is null)
				{
					return !(right is null);
				}
				return left.CompareTo(right) == -1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="KTimespan"/> is greater than another specified 
			/// <see cref="KTimespan"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >(KTimespan left, KTimespan right)
			{
				if (left is null)
				{
					return false;
				}
				return left.CompareTo(right) == 1;
			}

			/// <summary>
			/// Determines whether one specified <see cref="KTimespan"/> is less than or equal to 
			/// another specified <see cref="KTimespan"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is less than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator <=(KTimespan left, KTimespan right)
			{
				if (left is null)
				{
					return true;
				}
				return left.CompareTo(right) <= 0;
			}

			/// <summary>
			/// Determines whether one specified <see cref="KTimespan"/> is greater than or equal to 
			/// another specified <see cref="KTimespan"/>.
			/// </summary>
			/// <param name="left">The first instance.</param>
			/// <param name="right">The second instance.</param>
			/// <returns>
			/// <c>true</c> if left instance is greater than or equal to right instance; otherwise <c>false</c>.
			/// </returns>
			public static bool operator >=(KTimespan left, KTimespan right)
			{
				if (left is null)
				{
					return right is null;
				}
				return left.CompareTo(right) >= 0;
			}
		}

		/// <summary>
		/// Represents a KDB+ dictionary type.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		public class Dict
		{
			/// <summary>
			/// Initialises a new instance of <see cref="Dict"/>
			/// </summary>
			/// <param name="X">Keys to store. Should be an array type when using multiple values.</param>
			/// <param name="Y">
			/// Values to store. Index of each value should match the corresponding associated key.
			/// Should be an array type when using multiple values.
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="X"/> or <paramref name="Y"/> was null.</exception>
			public Dict(object X, object Y)
			{
				if (X == null)
				{
					throw new ArgumentNullException(nameof(X));
				}

				if (Y == null)
				{
					throw new ArgumentNullException(nameof(Y));
				}

				x = X;
				y = Y;
			}

			/// <summary>
			/// Gets or sets the <see cref="Dict"/> keys.
			/// </summary>
			public object x { get; set; }

			/// <summary>
			/// Gets or sets the <see cref="Dict"/> values.
			/// </summary>
			public object y { get; set; }
		}

		/// <summary>
		/// Represents a KDB+ table type.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("csharpsquid", "CA1034: Allow nested class for backwards compatability")]
		public class Flip
		{
			/// <summary>
			/// Initialises a new instance of <see cref="c.Flip"/> using a specified 
			/// <see cref="c.Dict"/>.
			/// </summary>
			/// <param name="X">The <see cref="c.Dict"/>.</param>
			/// <exception cref="ArgumentNullException"><paramref name="X"/> was null.</exception>
			public Flip(Dict X)
			{
				if (X == null)
				{
					throw new ArgumentNullException(nameof(X));
				}

				x = (string[])X.x;
				y = (object[])X.y;
			}

			/// <summary>
			/// Gets or sets the array of column names
			/// </summary>
			public string[] x { get; set; }

			/// <summary>
			/// Gets or sets the array of column values.
			/// </summary>
			public object[] y { get; set; }

			/// <summary>
			/// Gets the column values given the column name.
			/// </summary>
			/// <param name="s">The column name.</param>
			/// <returns>
			/// The value(s) associated with the column name which can be casted to an array of objects.
			/// </returns>
			/// <exception cref="IndexOutOfRangeException"><paramref name="s"/> column name was not found.</exception>
			public object at(string s)
			{
				return y[Array.IndexOf(x, s)];
			}
		}

	}
}
