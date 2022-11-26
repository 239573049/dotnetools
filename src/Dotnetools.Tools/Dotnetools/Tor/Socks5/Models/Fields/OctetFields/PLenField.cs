using Dotnetools.Helpers;
using Dotnetools.Tor.Socks5.Models.Bases;
using Dotnetools.Tor.Socks5.Models.Fields.ByteArrayFields;

namespace Dotnetools.Tor.Socks5.Models.Fields.OctetFields;

public class PLenField : OctetSerializableBase
{
	public PLenField(byte byteValue)
	{
		ByteValue = byteValue;
	}

	public PLenField(PasswdField passwd)
	{
		Guard.NotNull(nameof(passwd), passwd);

		ByteValue = (byte)passwd.ToBytes().Length;
	}

	public int Value => ByteValue;
}
