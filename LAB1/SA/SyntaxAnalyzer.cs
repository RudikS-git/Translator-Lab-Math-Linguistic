using System;
using System.Collections.Generic;
using System.Linq;

using LAB1.Exceptions;

namespace LAB1.SA
{
    public class SyntaxAnalyzer
    {
        private LexicalAnalyzer lexAn;

        public SyntaxAnalyzer(LexicalAnalyzer lexicalAnalyzer)
        {
            lexAn = lexicalAnalyzer;
        }

        private void SyntaxError(string msg)
        {
            throw new SyntaxAnalyzerException(msg, lexAn.CurLineIndex, lexAn.CurSymIndex);
        }

        private void ContextError(string msg)
        {
            throw new ContextAnalyzerException(msg, lexAn.CurLineIndex, lexAn.CurSymIndex);
        }

        private void S(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("S");

            if (lexAn.Token.Type == TokenKind.SqrLeftParen)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                lexAn.RecognizeNextToken();
                
                if (lexAn.Token.Type == TokenKind.SecondWord)
                {
                    node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                    lexAn.RecognizeNextToken();

                    B(out SyntaxTreeNode nodeB);
                    node.AddSubNode(nodeB);
                }
                else
                {
                    S(out SyntaxTreeNode nodeS);
                    node.AddSubNode(nodeS);
                }

                node.AddSubNode(new SyntaxTreeNode(Match(TokenKind.SqrRightParen)));
            }
            else
            {
                SyntaxError("Ожидалось [");
            }
        }

        private void A(SyntaxTreeNode node)
        {
            node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
            lexAn.RecognizeNextToken();

            B(out SyntaxTreeNode nodeB);
            node.AddSubNode(nodeB);
            Match(TokenKind.SqrRightParen);
            node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
        }

        private void B(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("B");
            
            if (lexAn.Token.Type == TokenKind.SqrLeftParen)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                lexAn.RecognizeNextToken();
                C(out SyntaxTreeNode nodeC);
                node.AddSubNode(nodeC);
                Match(TokenKind.SqrRightParen);
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
            }
            else
            {
                SyntaxError("Ожидалось [");
            }
        }

        private void C(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("C");
            var numbers = new List<string>();
            
            if (lexAn.Token.Type == TokenKind.FirstWord)
            {
                numbers.Add(lexAn.Token.Value);
                
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                lexAn.RecognizeNextToken();
                _C(out SyntaxTreeNode node_C, numbers);
                node.AddSubNode(node_C);
            }
            else
            {
                SyntaxError("Ожидалось первое слово");
            }
        }

        // numbers - наследуемый атрибут терминала C'
        private void _C(out SyntaxTreeNode node, List<string> numbers) // C'
        {
            node = new SyntaxTreeNode("C'");

            if (lexAn.Token.Value == ",")
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                lexAn.RecognizeNextToken();
                if (lexAn.Token.Type == TokenKind.FirstWord)
                {
                    if (numbers.Exists(it => it == lexAn.Token.Value))
                    {
                        ContextError($"{lexAn.Token.Value} данное число уже встречалось");
                    }
                    
                    numbers.Add(lexAn.Token.Value);
                    node.AddSubNode(new SyntaxTreeNode(lexAn.Token));
                    lexAn.RecognizeNextToken();
                    _C(out SyntaxTreeNode node_C, numbers);
                    node.AddSubNode(node_C);
                }
            }
        }

        // Провести синтаксический анализ текста.
        public void ParseText(out SyntaxTreeNode treeRoot)
        {
            lexAn.RecognizeNextToken(); // Распознаем первый токен в тексте.

            S(out treeRoot);

            if (lexAn.Token.Type != TokenKind.EndOfText) // Если текущий токен не является концом текста.
            {
                SyntaxError("После выражения идет еще какой-то текст"); // Обнаружена синтаксическая ошибка.
            }
        }

        // Проверить, что тип текущего распознанного токена совпадает с заданным.
        // Если совпадает, то распознать следующий токен, иначе синтаксическая ошибка.
        private Token Match(TokenKind tkn)
        {
            if (lexAn.Token.Type == tkn) // Сравниваем.
            {
                var token = lexAn.Token;
                lexAn.RecognizeNextToken(); // Распознаем следующий токен.
                return token;
            }
            else
            {
                SyntaxError("Ожидалось " + tkn.ToString()); // Обнаружена синтаксическая ошибка.
            }

            return null;
        }

        // Проверить, что текущий распознанный токен совпадает с заданным (сравнение производится в нижнем регистре).
        // Если совпадает, то распознать следующий токен, иначе синтаксическая ошибка.
        private void Match(string tkn)
        {
            if (lexAn.Token.Value.ToLower() == tkn.ToLower()) // Сравниваем.
            {
                lexAn.RecognizeNextToken(); // Распознаем следующий токен.
            }
            else
            {
                SyntaxError("Ожидалось " + tkn); // Обнаружена синтаксическая ошибка.
            }
        }
    }
}
