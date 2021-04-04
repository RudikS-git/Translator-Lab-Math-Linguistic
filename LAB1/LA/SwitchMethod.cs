using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LAB1
{
    public class SwitchMethod : LexicalAnalyzer
    {
        public SwitchMethod(string [] inputLines) : base(inputLines) { }

        enum StateComment { A, B, C, D, E, F, G, Fin }

        enum StateFirstWord { A, B, C, D, E, F, G, H, I }

        enum StateSecondWord { A, B_Fin, C_Fin }

        protected override void SkipComment()
        {
            StateComment currentState = StateComment.A;

            while (true)
            {
                switch (currentState)
                {
                    case StateComment.A:

                        if (curSym == '<')
                        {
                            currentState = StateComment.B;
                        }
                        else
                        {
                            LexicalError("Ожидалось <");
                        }

                        ReadNextSymbol();

                        break;

                    case StateComment.B:
                        
                        if (curSym == '!')
                        {
                            currentState = StateComment.C;
                        }
                        else
                        {
                            LexicalError("Ожидалось !");
                        }

                        ReadNextSymbol();

                        break;

                    case StateComment.C:

                        if (curSym == '-')
                        {
                            currentState = StateComment.D;
                        }
                        else
                        {
                            LexicalError("Ожидалось -");
                        }

                        ReadNextSymbol();

                        break;

                    case StateComment.D:

                        if (curSym == '-')
                        {
                            currentState = StateComment.E;
                        }
                        else
                        {
                            LexicalError("Ожидалось -");
                        }

                        ReadNextSymbol();

                        break;

                    case StateComment.E:
                        
                        if (curSym == '-')
                        {
                            ReadNextSymbol();
                            currentState = StateComment.F;
                        }
                        else if (curSymKind == SymbolKind.EndOfText)
                        {
                            LexicalError("Незаконченный комментарий");
                        }
                        else
                        {
                            ReadNextSymbol();
                            currentState = StateComment.E;
                        }
                        
                        break;

                    case StateComment.F:

                        if (curSym == '-')
                        {
                            currentState = StateComment.G;
                        }
                        else
                        {
                            currentState = StateComment.E;
                        }

                        ReadNextSymbol();
                        break;

                    case StateComment.G:

                        if (curSym == '-')
                        {
                            currentState = StateComment.G;
                            break;
                        }
                        else if (curSym != '>')
                        {
                            currentState = StateComment.E;
                            break;
                        }

                        ReadNextSymbol();
                        
                        return;
                }
            }
        }

        protected override void RecognizeFirstWord()
        {
            StateFirstWord currentState = StateFirstWord.A;

            while (true)
            {
                switch (currentState)
                {
                    case StateFirstWord.A:
                        if (curSym == '0')
                        {
                            currentState = StateFirstWord.B;
                            Token.Value += curSym;
                        }
                        else if (curSym == '1')
                        {
                            currentState = StateFirstWord.D;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 0, 1");
                        }

                        ReadNextSymbol();
                        break;

                    case StateFirstWord.B:
                        if (curSym == '1')
                        {
                            currentState = StateFirstWord.C;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 1");
                        }

                        ReadNextSymbol();
                        break;

                    case StateFirstWord.C:
                        if (curSym == '1')
                        {
                            currentState = StateFirstWord.E;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 1");
                        }
                        
                        ReadNextSymbol();
                        break;

                    case StateFirstWord.D:
                        if (curSym == '0')
                        {
                            currentState = StateFirstWord.F;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 0");
                        }
                        
                        ReadNextSymbol();
                        break;

                    case StateFirstWord.E:
                        if (curSym == '1')
                        {
                            currentState = StateFirstWord.D;
                            Token.Value += curSym;
                        }
                        else if(curSym == '0')
                        {
                            currentState = StateFirstWord.B;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 0, 1");
                        }

                        ReadNextSymbol();
                        break;

                    case StateFirstWord.F:
                        if (curSym == '1')
                        {
                            currentState = StateFirstWord.G;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 1");
                        }
                        
                        ReadNextSymbol();
                        break;

                    case StateFirstWord.G:

                        if (curSym == '1')
                        {
                            currentState = StateFirstWord.H;
                            Token.Value += curSym;
                        }
                        else
                        {
                            Token.Type = TokenKind.FirstWord;
                            return;
                        }

                        ReadNextSymbol();
                        break;

                    case StateFirstWord.H:
                        if (curSym == '1')
                        {
                            currentState = StateFirstWord.I;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 1");
                        }

                        ReadNextSymbol();
                        break;

                    case StateFirstWord.I:
                        if (curSym == '0')
                        {
                            currentState = StateFirstWord.G;
                            Token.Value += curSym;
                        }
                        else
                        {
                            LexicalError("Ожидалось 0");
                        }

                        ReadNextSymbol();
                        break;

                }
            }
        }

        protected override void RecognizeSecondWord()
        {
            StateSecondWord currentState = StateSecondWord.A;
            char buffer;
            
            while (true)
            {
                switch (currentState)
                {
                    case StateSecondWord.A:
                        buffer = curSym;
                        Token.Value += curSym;
                        ReadNextSymbol();

                        if (buffer == 'a' || buffer == 'b' || buffer == 'c')
                        {
                            currentState = StateSecondWord.B_Fin;
                        }
                        else if (buffer == 'd')
                        {
                            currentState = StateSecondWord.C_Fin;
                        }
                        else
                        {
                            LexicalError("Ожидалось a, b, c, d");
                        }

                        break;

                    case StateSecondWord.B_Fin:

                        if (SymbolKind.Letter != curSymKind)
                        {
                            Token.Type = TokenKind.SecondWord;
                            return;
                        }
                        
                        buffer = curSym;
                        Token.Value += curSym;
                        ReadNextSymbol();

                        if (buffer == 'a' || buffer == 'b' || buffer == 'c' || buffer == 'd')
                        {
                            currentState = StateSecondWord.B_Fin;
                        }
                        else
                        {
                            LexicalError("Ожидалось a, b, c, d");
                        }
                        
                        break;

                    case StateSecondWord.C_Fin:
                        
                        if (SymbolKind.Letter != curSymKind)
                        {
                            Token.Type = TokenKind.SecondWord;
                            return;
                        }

                        buffer = curSym;
                        Token.Value += curSym;
                        ReadNextSymbol();

                        if (buffer == 'b' || buffer == 'c' || buffer == 'd')
                        {
                            currentState = StateSecondWord.B_Fin;
                        }
                        else
                        {
                            LexicalError("Ожидалось b, c, d");
                        }

                        break;
                }
            }
        }
    }
}
