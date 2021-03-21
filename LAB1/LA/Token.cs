using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB1
{
    public class Token
    {
        public string Value { get; set; } // Значение токена (само слово).
        public TokenKind Type { get; set; } // Тип токена.

        // Индекс строки в исходном тексте, на которой расположен токен.
        public int LineIndex { get; set; }

        // Индекс символа в строке LineIndex в исходном тексте, с которого начинается токен.
        public int SymStartIndex { get; set; }

        public Token()
        {
            Reset(); // Сбрасываем значения полей токена.
        }
        
        // Сбросить значения полей токена.
        public void Reset()
        {
            Value = "";
            Type = TokenKind.Unknown;
            LineIndex = -1;
            SymStartIndex = -1;
        }
    }
}
