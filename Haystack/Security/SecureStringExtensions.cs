using System.Security;

namespace Haystack.Security;

public static class SecureStringExtensions
{
    public static SecureString ToSecureString(this string input)
    {
        var secureString = new SecureString();
        var passwordCharacters = input.ToCharArray();

        foreach (var character in passwordCharacters)
        {
            secureString.AppendChar(character);
        }

        return secureString;
    }
}