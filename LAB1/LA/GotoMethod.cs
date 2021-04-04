namespace LAB1
{
    public class GotoMethod : LexicalAnalyzer
    {
        public GotoMethod(string [] inputLines) : base(inputLines) { }

        protected override void RecognizeSecondWord() // Конечный автомат для идентификатора.
        {
            char buffer;
            
            A: // начальное состояние
            if (curSymKind == SymbolKind.Letter)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == 'a' || buffer == 'b' || buffer == 'c')
                {
                    goto B_Fin;
                }
                else if(buffer == 'd')
                {
                    goto C_Fin;
                }
            }

            LexicalError("Ожидались буквы a, b, c, d");

            B_Fin:
            if (curSymKind == SymbolKind.Letter)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();
                
                if (buffer == 'a' || buffer == 'b' || buffer == 'c' || buffer == 'd')
                {
                    goto B_Fin;
                }
            }
            else
            {
                goto quit;
            }

            LexicalError("Ожидались буквы a, b, c, d");

            C_Fin:
            if (curSymKind == SymbolKind.Letter)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer != 'b' && buffer != 'c' && buffer != 'd')
                {
                    LexicalError("Ожидались буквы b, c, d");
                }
                
                goto C_Fin;
            }

            quit:
            Token.Type = TokenKind.SecondWord;
        }

        protected override void RecognizeFirstWord() // Конечный автомат для числа.
        {
            char buffer;
            
            A:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '0')
                {
                    goto B;
                }
                else if(buffer == '1')
                {
                    goto D;
                }
            }
            
            LexicalError("Ожидалось 0, 1");

            B:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '1')
                {
                    goto C;
                }

                LexicalError("Ожидалась единица");
            }

            LexicalError("Ожидалось 0, 1");

            C:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '1')
                {
                    goto E;
                }

                LexicalError("Ожидалась единица");
            }
            
            LexicalError("Ожидалось 0, 1");

            D:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '0')
                {
                    goto F;
                }

                LexicalError("Ожидался ноль");
            }
            
            LexicalError("Ожидалось 0, 1");

            E:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '0')
                {
                    goto B;
                }
                else if(buffer == '1')
                {
                    goto D;
                }
            }
            
            LexicalError("Ожидалось 0, 1");

            F:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '1')
                {
                    goto G_Fin;
                }

                LexicalError("Ожидалась единица");
            }
            
            LexicalError("Ожидалось 0, 1");
            
            G_Fin:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '1')
                {
                    goto H;
                }
                else
                {
                    LexicalError("Ожидалось 1");
                }

            }

            goto quit;

            H:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '1')
                {
                    goto I;
                }

                LexicalError("Ожидалась единица");
            }

            LexicalError("Ожидалась цифра");

            I:
            if (curSymKind == SymbolKind.Digit)
            {
                buffer = curSym;
                Token.Value += curSym;
                ReadNextSymbol();

                if (buffer == '0')
                {
                    goto G_Fin;
                }

                LexicalError("Ожидался ноль");
            }
            
            LexicalError("Ожидалось 0, 1");

            quit:
            Token.Type = TokenKind.FirstWord;
        }

        protected override void SkipComment() // Конечный автомат для комментария
        {
            char buffer;
            A:
            buffer = curSym;
            ReadNextSymbol();
            if (buffer == '<')
            {
                goto B;
            }

            LexicalError("Ожидалось <");

            B:
            buffer = curSym;
            ReadNextSymbol();
            if (buffer == '!')
            {
                goto C;
            }

            LexicalError("Ожидалось !");

            C:
            buffer = curSym;
            ReadNextSymbol();
            if (buffer == '-')
            {
                goto D;
            }

            LexicalError("Ожидалось -");

            D:
            buffer = curSym;
            ReadNextSymbol();
            if (buffer == '-')
            {
                goto E;
            }

            LexicalError("Ожидалось -");

            E:
            if (curSym == '-')
            {
                ReadNextSymbol();
                goto F;
            }
            else if (curSymKind == SymbolKind.EndOfText)
            {
                LexicalError("Незаконченный комментарий");
            }
            else
            {
                ReadNextSymbol();
                goto E;
            }

            F:
            buffer = curSym;
            ReadNextSymbol();
            if (buffer == '-')
            {
                goto G;
            }
            else
            {
                goto E;
            }

            G:
            buffer = curSym;
            ReadNextSymbol();

            if (buffer == '-')
            {
                goto G;
            }
            else if (buffer != '>')
            {
                goto E;
            }
        }
    }
}
