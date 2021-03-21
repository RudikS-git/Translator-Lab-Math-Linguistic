using System;

namespace LAB1.Exceptions
{
    public class AnalyzerException : Exception
    {
        // Индекс строки, где возникла исключительная ситуация - свойство только для чтения.
        public int LineIndex { get; }

        // Индекс символа, на котором возникла исключительная ситуация - свойство только для чтения.
        public int SymIndex { get; }

        // message - описание исключительной ситуации.
        // lineIndex и symIndex - позиция возникновения исключительной ситуации в анализируемом тексте.
        public AnalyzerException(string message, int lineIndex, int symIndex) : base(message)
        {
            LineIndex = lineIndex;
            SymIndex = symIndex;
        }
    }
}
