using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LAB1
{
    public class TableMethod : LexicalAnalyzer
    {
        public TableMethod(string [] inputLines) : base(inputLines) { }

        private byte[][] CommentTable = new byte[8][] // A-0, B-1, C-2, D-3, E-4, F-5, G-6, Fin-7
        {
            // 0-'<', 1-'!', 2-'-', 3-'>', 4-'с', 5-Fin
            new byte[] { 1, 255, 255, 255, 255,   0 }, // A 0
            new byte[] { 255, 2, 255, 255, 255,   0 }, // B 1
            new byte[] { 255, 255, 3, 255, 255,   0 }, // C 2
            new byte[] { 255, 255, 4, 255, 255,   0 }, // D 3
            new byte[] { 255, 255, 5, 255, 4,     0 }, // E 4
            new byte[] { 255, 255, 6, 255, 4,     0 }, // F 5
            new byte[] { 255, 255, 6, 7, 4,   0 }, // G 6
            new byte[] { 255, 255, 255, 255, 255, 1 }  // Fin 7
        };

        private byte[][] FirstWordTable = new byte[9][] // {A}-0, {B}-1, {C}-2, {E}-3, {A, D}-4, {F}-5, {G, Fin}-6, {H}-7, {I}-8
        {
            // 0-0, 1-1, Fin-2
            new byte[] { 1,   3, 0 }, // {A}0
            new byte[] { 255, 2, 0 }, // {B}1
            new byte[] { 255, 4, 0 }, // {C}2
            new byte[] { 5, 255, 0 }, // {E}3
            new byte[] { 1,   3, 0 }, // {A, D}4
            new byte[] { 255, 6, 0 }, // {F}5
            new byte[] { 255, 7, 0 }, // {G, Fin}6
            new byte[] { 255, 8, 1 }, // {H}7
            new byte[] { 6, 255, 0 }  // {I}8
        };

        private byte[][] SecondWordTable = new byte[3][] // {A}-0, {B, Fin}-1, {C, Fin}-2
        {
            // 0-'a', 1-'b', 2-'c', 3-'d', 4-Fin
            new byte[] { 1,   1, 1, 2, 0 }, // {A}
            new byte[] { 1,   1, 1, 1, 1 }, // {B, Fin}
            new byte[] { 255, 2, 2, 2, 1 }, // {C, Fin}
        };

        protected override void SkipComment()
        {
            Func<char, int> GetColumnForSymbol = (currentSymbol) =>
            {
                switch (currentSymbol)
                {
                    case '<':
                        return 0;

                    case '!':
                        return 1;
                    
                    case '-':
                        return 2;
                    
                    case '>':
                        return 3;
                    
                    default:
                        return 4;
                }
            };

            int currentState = 0;

            while (true)
            {
                int currentColumn = GetColumnForSymbol(curSym);

                currentState = CommentTable[currentState][currentColumn];
                
                if (currentState == 255)
                {
                    LexicalError("Ожидалось <, !, -, >");
                }
                else if (currentState == 7)
                {
                    ReadNextSymbol();
                    return;
                }

                ReadNextSymbol();
            }
        }

        protected override void RecognizeFirstWord()
        {
            Func<char, int> GetColumnForSymbol = (currentSymbol) =>
            {
                switch (currentSymbol)
                {
                    case '0':
                        return 0;

                    case '1':
                        return 1;
                }

                return -1;
            };

            byte currentState = 0;

            while (true)
            {
                int currentColumn = GetColumnForSymbol(curSym);

                if (currentColumn == -1)
                {
                    if (currentState == 6)
                    {
                        Token.Type = TokenKind.FirstWord;
                        return;
                    }
                    
                    LexicalError($"Ожидалось 0, 1");
                }

                currentState = FirstWordTable[currentState][currentColumn];

                if (currentState == 255)
                {
                    LexicalError($"Ожидалось 0, 1");
                }
                else if(curSym == '0' || curSym == '1')
                {
                    Token.Value += curSym;
                }

                ReadNextSymbol();
            }
        }

        protected override void RecognizeSecondWord()
        {
            Func<char, int> GetColumnForSymbol = (currentSymbol) =>
            {
                switch (currentSymbol)
                {
                    case 'a':
                        return 0;

                    case 'b':
                        return 1;

                    case 'c':
                        return 2;

                    case 'd':
                        return 3;
                }

                LexicalError("Ожидалось a, b, c, d");
                return -1;
            };

            int currentState = 0;

            while (true)
            {
                if (currentState == 1 || currentState == 2)
                {
                    if (curSymKind != SymbolKind.Letter)
                    {
                        Token.Type = TokenKind.SecondWord;
                        return;
                    }
                }

                int currentColumn = GetColumnForSymbol(curSym);

                currentState = SecondWordTable[currentState][currentColumn];

                if (currentState == 255)
                {
                    LexicalError("Ожидалось b, c, d");
                }
                else
                {
                    Token.Value += curSym;
                    ReadNextSymbol();
                }
            }
        }
    }
}
