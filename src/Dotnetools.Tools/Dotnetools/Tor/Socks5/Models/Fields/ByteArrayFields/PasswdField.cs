using System.Text;
using Dotnetools.Helpers;
using Dotnetools.Tor.Socks5.Models.Bases;

namespace Dotnetools.Tor.Socks5.Models.Fields.ByteArrayFields;

public class PasswdField : ByteArraySerializableBase
{
	public PasswdField(byte[] bytes)
	{
		Bytes = Guard.NotNullOrEmpty(nameof(bytes), bytes);
	}

	public PasswdField(string password)
		: this(Encoding.UTF8.GetBytes(password))
	{
	}

	private byte[] Bytes { get; }

	public override byte[] ToBytes() => Bytes;
}
