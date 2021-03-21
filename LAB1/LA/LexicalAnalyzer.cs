using LAB1.Exceptions;
using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LAB1
{
    /// <summary>
    /// <exception cref="LexicalAnalyzerException"></exception>
    /// </summary>
    public abstract class LexicalAnalyzer
    {
        protected enum SymbolKind // Тип символа
        {
            Letter,    // Буква.
            Digit,     // Цифра.
            Space,     // Пробел.
            Reserved,  // Зарезервированный.
            Other,     // Другой.
            EndOfLine, // Конец строки.
            EndOfText  // Конец текста.
        };

        protected const char commentSymbol1 = '<';
        protected const char commentSymbol2 = '!';
        protected const char commentSymbol3 = '-';
        protected const char commentSymbol4 = '>';

        protected string[] inputLines; // Входной текст - массив строк.
        protected int curLineIndex = 0;    // Индекс текущей строки.
        protected int curSymIndex = -1;     // Индекс текущего символа в текущей строке.

        protected char curSym = (char)0;           // Текущий символ.
        protected SymbolKind curSymKind; // Тип текущего символа.

        public int CurLineIndex { get { return curLineIndex; } }
        public int CurSymIndex { get { return curSymIndex; } }

        // Токен, распознанный при последнем вызове метода RecognizeNextToken() - свойство только для чтения.
        public Token Token { get; protected set; } = null;

        public LexicalAnalyzer(string[] inputLines)  // В качестве параметра передается исходный текст
        {
            this.inputLines = inputLines;

            ReadNextSymbol(); // Считываем первый символ входного текста
        }

        protected abstract void SkipComment();
        protected abstract void RecognizeFirstWord();
        protected abstract void RecognizeSecondWord();

        protected void ReadNextSymbol() // Считать следующий символ
        {
            if (curLineIndex >= inputLines.Count()) // Если индекс текущей строки выходит за пределы текстового поля.
            {
                curSym = (char)0; // Обнуляем значение текущего символа.
                curSymKind = SymbolKind.EndOfText; // Тип текущего символа - конец текста.
                return;
            }

            curSymIndex++; // Увеличиваем индекс текущего символа.

            if (curSymIndex >= inputLines[curLineIndex].Count()) // Если индекс текущего символа выходит за пределы текущей строки.
            {
                curLineIndex++; // Увеличиваем индекс текущей строки.
                
                if (curLineIndex < inputLines.Count()) // Если индекс текущей строки находится в пределах текстового поля.
                {
                    curSym = (char)0; // Обнуляем значение текущего символа.
                    curSymIndex = -1; // Переносим индекс текущего символа в начало строки.
                    curSymKind = SymbolKind.EndOfLine; // Тип текущего символа - конец строки.
                }
                else
                {
                    curSym = (char)0; // Обнуляем значение текущего символа.
                    curSymKind = SymbolKind.EndOfText; // Тип текущего символа - конец текста.
                }

                return;
            }

            curSym = inputLines[curLineIndex][curSymIndex]; // Считываем текущий символ.

            ClassifyCurrentSymbol();
        }

        protected void ClassifyCurrentSymbol() // Классифицировать текущий символ
        {
            if (((curSym >= 'A') && (curSym <= 'Z')) ||
                ((curSym >= 'a') && (curSym <= 'z')) ||
                ((curSym >= 'А') && (curSym <= 'Я')) ||
                ((curSym >= 'а') && (curSym <= 'я')))
            {
                curSymKind = SymbolKind.Letter;
                return;
            }
            else if ((curSym >= '0') && (curSym <= '9'))
            {
                curSymKind = SymbolKind.Digit;
                return;
            }
            
            switch (curSym)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                    curSymKind = SymbolKind.Letter;
                    return;

                case ' ':
                    curSymKind = SymbolKind.Space;
                    return;

                case '.':
                case commentSymbol1:
                case commentSymbol2:
                case commentSymbol3:
                case commentSymbol4:
                case '_':
                    
                // резервные символы для кс
                case '[':
                case ']':
                    curSymKind = SymbolKind.Reserved;
                    return;

                default:
                    curSymKind = SymbolKind.Other;
                    return;
            }
        }

        public void RecognizeNextToken() // Распознать следующий токен в тексте
        {
            // На данный момент уже прочитан символ, следующий за токеном, распознанным в прошлом вызове этого метода.
            // Если же это первый вызов, то на данный момент уже прочитан первый символ текста (в конструкторе).

            while ( (curSymKind == SymbolKind.Space) || 
                    (curSymKind == SymbolKind.EndOfLine) ||
                    (curSym == commentSymbol1) )
            {
                if (curSym == commentSymbol1) // первый символ комментария.
                    SkipComment();
                else
                    ReadNextSymbol(); // Пропускаем пробел или переход на новую строку.
            }

            Token = new Token();
            Token.LineIndex = curLineIndex;
            Token.SymStartIndex = curSymIndex;

            switch (curSymKind)
            {
                case SymbolKind.Letter:
                    RecognizeSecondWord();         
                    break;

                case SymbolKind.Digit:
                    RecognizeFirstWord();
                    break;

                case SymbolKind.Reserved:
                    RecognizeReservedSymbol();
                    break;

                case SymbolKind.EndOfText:
                    Token.Type = TokenKind.EndOfText;
                    break;

                default:
                    LexicalError("Недопустимый символ");
                    break;
            }
        }

        // Распознать зарезервированный символ.
        private void RecognizeReservedSymbol()
        {
            switch (curSym)
            {
                case '[':
                    Token.Type = TokenKind.SqrLeftParen;
                    break;

                case ']':
                    Token.Type = TokenKind.SqrRightParen;
                    break;

                default:
                    LexicalError("Неизвестный зарезервированный символ '" + curSym + "'");
                    break;
            }

            Token.Value += curSym;
            ReadNextSymbol();
        }
        
        protected void LexicalError(string msg) // msg - описание ошибки.
        {
            throw new LexicalAnalyzerException(msg, curLineIndex, curSymIndex);
        }
    }
}
