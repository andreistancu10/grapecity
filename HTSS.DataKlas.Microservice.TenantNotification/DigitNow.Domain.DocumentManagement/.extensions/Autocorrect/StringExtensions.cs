using System;

namespace Domain.Autocorrect
{
    public static class StringExtensions
    {
        public static string ToUpperFirstCharacter(this string source)
        {
            return source switch
            {
                null => throw new ArgumentNullException(nameof(source)),
                "" => source,
                _ => source[..1].ToUpper() + source[1..]
            };
        }
    }
}