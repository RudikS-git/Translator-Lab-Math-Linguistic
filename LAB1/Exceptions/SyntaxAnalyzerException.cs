using System;

namespace LAB1.Exceptions
{
    public class SyntaxAnalyzerException : AnalyzerException
    {
        public SyntaxAnalyzerException(string message, int lineIndex, int symIndex)
            : base(message, lineIndex, symIndex)
        {

        }

        public override string ToString()
        {
            return String.Format("Синтаксическая ошибка ({0},{1}): {2}", LineIndex + 1, SymIndex + 1, Message);
        }
    }
}
