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

		private readonly int _maxBufferSize;

		public static Encoding e = Encoding.ASCII;

		private byte[] b;

		private byte[] B;

		private int j;

		private int J;

		private int vt;

		private bool a;

		private Stream s;

		private static TimeSpan v;

		private static int ni = int.MinValue;

		private static long nj = long.MinValue;

		private static long o = 630822816000000000L;

		private static double nf = double.NaN;

		private static object[] NU = new object[20]
		{
			null,
			false,
			default(Guid),
			null,
			(byte)0,
			short.MinValue,
			ni,
			nj,
			(float)nf,
			nf,
			' ',
			"",
			new DateTime(0L),
			new Month(ni),
			new Date(ni),
			new DateTime(0L),
			new KTimespan(nj),
			new Minute(ni),
			new Second(ni),
			new TimeSpan(nj)
		};

		private static DateTime za = DateTime.MinValue.AddTicks(1L);

		private static DateTime zw = DateTime.MaxValue;

		private static int[] nt = new int[20]
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

		private byte[] gip = new byte[16]
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
		/// Initialises a new instance of <see cref="c"/> with a specified host and port 
		/// to connect to.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port to connect to.</param>
		/// <exception cref="KException">Unable to connect to KDB+ process, access denied or process unavailable.</exception>
		public c(string host, int port)
			: this(host, port, Environment.UserName)
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="c"/> with a specified host and port 
		/// to connect to and a username/password for authentication.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port to connect to.</param>
		/// <param name="userPassword">The username and passsword, as "username:password" for remote authorisation.</param>
		/// <exception cref="KException">Unable to connect to KDB+ process, access denied or process unavailable.</exception>
		public c(string host, int port, string userPassword)
			: this(host, port, userPassword, DefaultMaxBufferSize)
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="c"/> with a specified host and port 
		/// to connect to, a username/password for authentication and a maximum buffersize.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port to connect to.</param>
		/// <param name="userPassword">The username and passsword, as "username:password" for remote authorisation.</param>
		/// <param name="maxBufferSize">The maximum buffer size.</param>
		/// <exception cref="KException">Unable to connect to KDB+ process, access denied or process unavailable.</exception>
		public c(string host, int port, string userPassword, int maxBufferSize)
			: this(host, port, userPassword, maxBufferSize, false)
		{
		}

		/// <summary>
		/// Initialises a new instance of <see cref="c"/> with a specified host and port 
		/// to connect to, a username/password for authentication, a maximum buffersize and
		/// a flag indicating whether to use TLS.
		/// </summary>
		/// <param name="host">The host to connect to.</param>
		/// <param name="port">The port to connect to.</param>
		/// <param name="userPassword">The username and passsword, as "username:password" for remote authorisation.</param>
		/// <param name="maxBufferSize">The maximum buffer size.</param>
		/// <param name="useTLS">A boolean flag indicating whether or not TLS authentication is enabled.</param>
		/// <exception cref="KException">Unable to connect to KDB+ process, access denied or process unavailable.</exception>
		public c(string host, int port, string userPassword, int maxBufferSize, bool useTLS)
		{
			_maxBufferSize = maxBufferSize;
			Connect(host, port);
			s = GetStream();
			if (useTLS)
			{
				s = new SslStream(s, false);
				((SslStream)s).AuthenticateAsClient(host);
			}
			B = new byte[2 + userPassword.Length];
			J = 0;
			w(userPassword + "\u0003");
			s.Write(B, 0, J);
			if (1 != s.Read(B, 0, 1))
			{
				throw new KException("access");
			}
			vt = Math.Min(B[0], (byte)3);
		}

		/// <summary>
		/// Disposes this <see cref="kx.c"/> instance and requests that the underlying
		/// stream and TCP connection be closed.
		/// </summary>
		public new void Close()
		{
			if (s != null)
			{
				s.Close();
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
			read(b = new byte[8]);
			a = (b[0] == 1);
			bool num = b[2] == 1;
			j = 4;
			read(b = new byte[ri() - 8]);
			if (num)
			{
				u();
			}
			else
			{
				j = 0;
			}
			if (b[0] == 128)
			{
				j = 1;
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
		/// Gets the null object for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="t">The .NET type.</param>
		/// <returns>
		/// Instance of null object of specified KDB+ type.
		/// </returns>
		public static object NULL(Type t)
		{
			for (int i = 0; i < NU.Length; i++)
			{
				if (NU[i] != null &&
					t == NU[i].GetType())
				{
					return NU[i];
				}
			}
			return null;
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
				return x.Equals(NU[t]);
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
			this.j = 0;
			byte[] dst = new byte[ri()];
			int d = this.j;
			int[] aa = new int[256];
			while (s < dst.Length)
			{
				if (k == 0)
				{
					f = (0xFF & b[d++]);
					k = 1;
				}
				if ((f & k) != 0)
				{
					r = aa[0xFF & b[d++]];
					dst[s++] = dst[r++];
					dst[s++] = dst[r++];
					j = (0xFF & b[d++]);
					for (int i = 0; i < j; i++)
					{
						dst[s + i] = dst[r + i];
					}
				}
				else
				{
					dst[s++] = b[d++];
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
			b = dst;
			this.j = 8;
		}

		private void w(bool x)
		{
			B[J++] = (byte)(x ? 1 : 0);
		}

		private bool rb()
		{
			return 1 == b[j++];
		}

		private void w(byte x)
		{
			B[J++] = x;
		}

		private byte rx()
		{
			return b[j++];
		}

		private void w(short h)
		{
			B[J++] = (byte)h;
			B[J++] = (byte)(h >> 8);
		}

		private short rh()
		{
			int x = b[j++];
			int y = b[j++];
			return (short)(a ? ((x & 0xFF) | (y << 8)) : ((x << 8) | (y & 0xFF)));
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
			if (!a)
			{
				return (x << 16) | (y & 0xFFFF);
			}
			return (x & 0xFFFF) | (y << 16);
		}

		private void w(Guid g)
		{
			byte[] b = g.ToByteArray();
			if (vt < 3)
			{
				throw new KException("Guid not valid pre kdb+3.0");
			}
			for (int i = 0; i < b.Length; i++)
			{
				w(b[gip[i]]);
			}
		}

		private Guid rg()
		{
			bool oa = a;
			a = false;
			int j = ri();
			short h3 = rh();
			short h2 = rh();
			a = oa;
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
			if (!a)
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
			if (!a)
			{
				byte c2 = b[j];
				b[j] = b[j + 3];
				b[j + 3] = c2;
				c2 = b[j + 1];
				b[j + 1] = b[j + 2];
				b[j + 2] = c2;
			}
			float result = BitConverter.ToSingle(b, j);
			j += 4;
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
			return (char)(b[j++] & 0xFF);
		}

		private void w(string s)
		{
			byte[] bytes = e.GetBytes(s);
			foreach (byte i in bytes)
			{
				w(i);
			}
			B[J++] = 0;
		}

		private string rs()
		{
			int i = j;
			while (b[j] != 0)
			{
				j++;
			}
			string @string = e.GetString(b, i, j - i);
			j++;
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
			if (vt < 1)
			{
				throw new KException("Timespan not valid pre kdb+2.6");
			}
			w((int)(qn(t) ? ni : (t.Ticks / 10000)));
		}

		private TimeSpan rt()
		{
			int i = ri();
			return new TimeSpan(qn(i) ? nj : (10000L * (long)i));
		}

		private void w(DateTime p)
		{
			if (vt < 1)
			{
				throw new KException("Timestamp not valid pre kdb+2.6");
			}
			w(qn(p) ? nj : (100 * (p.Ticks - o)));
		}

		private DateTime rz()
		{
			double f = rf();
			if (!double.IsInfinity(f))
			{
				return new DateTime(qn(f) ? 0 : clampDT(10000 * (long)Math.Round(86400000.0 * f) + o));
			}
			if (f >= 0.0)
			{
				return zw;
			}
			return za;
		}

		private void w(KTimespan t)
		{
			w(qn(t) ? nj : (t.t.Ticks * 100));
		}

		private KTimespan rn()
		{
			return new KTimespan(rj());
		}

		private DateTime rp()
		{
			long i = rj();
			long d = (i < 0) ? ((i + 1) / 100 - 1) : (i / 100);
			return new DateTime((i == nj) ? 0 : (o + d));
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
			B[J++] = 0;
			if (t == 98)
			{
				Flip r = (Flip)x;
				B[J++] = 99;
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
			int t = (sbyte)b[this.j++];
			switch (t)
			{
				case -1:
					return rb();
				case -2:
					return rg();
				case -4:
					return b[this.j++];
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
							if (t == 101 && b[this.j++] == 0)
							{
								return null;
							}
							throw new KException("func");
						}
						if (t == 99)
						{
							return new Dict(r(), r());
						}
						this.j++;
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
										G2[i] = b[this.j++];
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
									char[] chars = e.GetChars(b, this.j, j);
									this.j += j;
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
			B = new byte[j];
			B[0] = 1;
			B[1] = (byte)i;
			J = 4;
			w(j);
			w(x);
			s.Write(B, 0, j);
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
					if ((i = s.Read(b, k, Math.Min(_maxBufferSize, j - k))) == 0)
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

		private static TimeSpan t()
		{
			return DateTime.Now.TimeOfDay;
		}

		private static void tm()
		{
			TimeSpan u = v;
			v = t();
			O(v - u);
		}

		private static void O(object x)
		{
			Console.WriteLine(x);
		}

		private static string i2(int i)
		{
			return $"{i:00}";
		}

		private static object NULL(char c)
		{
			return NU[" bg xhijefcspmdznuvt".IndexOf(c)];
		}

		private static long clampDT(long j)
		{
			return Math.Min(Math.Max(j, za.Ticks), zw.Ticks);
		}

		private static int find(string[] x, string y)
		{
			int i;
			for (i = 0; i < x.Length && !x[i].Equals(y); i++)
			{
			}
			return i;
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
			if (x is KTimespan[])
			{
				return 16;
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
					return 1 + nt[-t];
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
				j += i * nt[t];
			}
			return j;
		}

		/// <summary>
		/// Represents a KDB+ Date type.
		/// </summary>
		[Serializable]
		public class Date : IComparable
		{
			public int i;

			private Date()
			{
			}

			public Date(int x)
			{
				i = x;
			}

			public DateTime DateTime()
			{
				if (i != -2147483647)
				{
					if (i != int.MaxValue)
					{
						return new DateTime((i == ni) ? 0 : clampDT(864000000000L * i + o));
					}
					return zw;
				}
				return za;
			}

			public Date(long x)
			{
				i = ((x == 0L) ? ni : ((int)(x / 864000000000L) - 730119));
			}

			public Date(DateTime z)
				: this(z.Ticks)
			{
			}

			public override string ToString()
			{
				if (i != ni)
				{
					return DateTime().ToString("d");
				}
				return "";
			}

			public override bool Equals(object o)
			{
				if (o == null)
				{
					return false;
				}
				if (GetType() != o.GetType())
				{
					return false;
				}
				Date d = (Date)o;
				return i == d.i;
			}

			public override int GetHashCode()
			{
				return i;
			}

			public int CompareTo(object o)
			{
				if (o == null)
				{
					return 1;
				}
				Date other = o as Date;
				if (other == null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
		}

		/// <summary>
		/// Represents a KDB+ Month type.
		/// </summary>
		[Serializable]
		public class Month : IComparable
		{
			public int i;

			private Month()
			{
			}

			public Month(int x)
			{
				i = x;
			}

			public override string ToString()
			{
				int i = 24000 + this.i;
				int y = i / 12;
				if (this.i != ni)
				{
					return i2(y / 100) + i2(y % 100) + "-" + i2(1 + i % 12);
				}
				return "";
			}

			public override bool Equals(object o)
			{
				if (o == null)
				{
					return false;
				}
				if (GetType() != o.GetType())
				{
					return false;
				}
				Month i = (Month)o;
				return this.i == i.i;
			}

			public override int GetHashCode()
			{
				return i;
			}

			public int CompareTo(object o)
			{
				if (o == null)
				{
					return 1;
				}
				Month other = o as Month;
				if (other == null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
		}

		/// <summary>
		/// Represents a KDB+ Minute type.
		/// </summary>
		[Serializable]
		public class Minute : IComparable
		{
			public int i;

			private Minute()
			{
			}

			public Minute(int x)
			{
				i = x;
			}

			public override string ToString()
			{
				if (i != ni)
				{
					return i2(i / 60) + ":" + i2(i % 60);
				}
				return "";
			}

			public override bool Equals(object o)
			{
				if (o == null)
				{
					return false;
				}
				if (GetType() != o.GetType())
				{
					return false;
				}
				Minute i = (Minute)o;
				return this.i == i.i;
			}

			public override int GetHashCode()
			{
				return i;
			}

			public int CompareTo(object o)
			{
				if (o == null)
				{
					return 1;
				}
				Minute other = o as Minute;
				if (other == null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
		}

		/// <summary>
		/// Represents a KDB+ Second type.
		/// </summary>
		[Serializable]
		public class Second : IComparable
		{
			public int i;

			private Second()
			{
			}

			public Second(int x)
			{
				i = x;
			}

			public override string ToString()
			{
				if (i != ni)
				{
					return new Minute(i / 60).ToString() + ":" + i2(i % 60);
				}
				return "";
			}

			public override bool Equals(object o)
			{
				if (o == null)
				{
					return false;
				}
				if (GetType() != o.GetType())
				{
					return false;
				}
				Second s = (Second)o;
				return i == s.i;
			}

			public override int GetHashCode()
			{
				return i;
			}

			public int CompareTo(object o)
			{
				if (o == null)
				{
					return 1;
				}
				Second other = o as Second;
				if (other == null)
				{
					return 1;
				}
				return i.CompareTo(other.i);
			}
		}

		/// <summary>
		/// Represents a KDB+ TimeSpan type.
		/// </summary>
		[Serializable]
		public class KTimespan : IComparable
		{
			public TimeSpan t;

			private KTimespan()
			{
			}

			public KTimespan(long x)
			{
				t = new TimeSpan((x == nj) ? nj : (x / 100));
			}

			public override string ToString()
			{
				if (!qn(t))
				{
					return t.ToString();
				}
				return "";
			}

			public override bool Equals(object o)
			{
				if (o == null)
				{
					return false;
				}
				if (GetType() != o.GetType())
				{
					return false;
				}
				KTimespan i = (KTimespan)o;
				return t.Ticks == i.t.Ticks;
			}

			public override int GetHashCode()
			{
				return t.GetHashCode();
			}

			public int CompareTo(object o)
			{
				if (o == null)
				{
					return 1;
				}
				KTimespan other = o as KTimespan;
				if (other == null)
				{
					return 1;
				}
				return t.CompareTo(other.t);
			}
		}

		/// <summary>
		/// Represents a KDB+ dictionary type.
		/// </summary>
		public class Dict
		{
			public object x;

			public object y;

			public Dict(object X, object Y)
			{
				x = X;
				y = Y;
			}
		}

		/// <summary>
		/// Represents a KDB+ table type.
		/// </summary>
		public class Flip
		{
			public string[] x;

			public object[] y;

			public Flip(Dict X)
			{
				x = (string[])X.x;
				y = (object[])X.y;
			}

			public object at(string s)
			{
				return y[find(x, s)];
			}
		}
	}
}
