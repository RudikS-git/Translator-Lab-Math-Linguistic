using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB1.Exceptions
{
    public class ContextAnalyzerException : AnalyzerException
    {
        public ContextAnalyzerException(string message, int lineIndex, int symIndex)
            : base(message, lineIndex, symIndex)
        {

        }

        public override string ToString()
        {
            return String.Format("Контекстная ошибка ({0},{1}): {2}", LineIndex + 1, SymIndex + 1, Message);
        }
    }
}
