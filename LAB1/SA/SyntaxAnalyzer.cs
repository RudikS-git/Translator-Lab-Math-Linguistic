using System;
using System.Collections.Generic;
using System.Linq;

using LAB1.Exceptions;

namespace LAB1.SA
{
    // генерирует SynAnException или LexAnException.
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

        private void S(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("S");

            if (lexAn.Token.Type == TokenKind.SqrLeftParen)
            {
                lexAn.RecognizeNextToken();
                
                if (lexAn.Token.Type == TokenKind.SecondWord)
                {
                    SyntaxTreeNode nodeA = new SyntaxTreeNode("A");
                    nodeA.AddSubNode(new SyntaxTreeNode("["));
                    A(nodeA);
                    node.AddSubNode(nodeA);
                }
                else
                {
                    node.AddSubNode(new SyntaxTreeNode("]"));
                    S(out SyntaxTreeNode nodeS);
                    node.AddSubNode(nodeS);
                    Match(TokenKind.SqrRightParen);
                    node.AddSubNode(new SyntaxTreeNode("]"));
                }
            }
            else
            {
                SyntaxError("Ожидалось [");
            }
        }

        private void A(SyntaxTreeNode node)
        {
            node.AddSubNode(new SyntaxTreeNode(lexAn.Token.Value));
            lexAn.RecognizeNextToken();

            B(out SyntaxTreeNode nodeB);
            node.AddSubNode(nodeB);
            Match(TokenKind.SqrRightParen);
            node.AddSubNode(new SyntaxTreeNode("]"));
        }

        private void B(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("B");
            
            if (lexAn.Token.Type == TokenKind.SqrLeftParen)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token.Value));
                lexAn.RecognizeNextToken();
                C(out SyntaxTreeNode nodeC);
                node.AddSubNode(nodeC);
                Match(TokenKind.SqrRightParen);
                node.AddSubNode(new SyntaxTreeNode("]"));
            }
            else
            {
                SyntaxError("Ожидалось [");
            }
        }

        private void C(out SyntaxTreeNode node)
        {
            node = new SyntaxTreeNode("C");
            
            if (lexAn.Token.Type == TokenKind.FirstWord)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token.Value));
                lexAn.RecognizeNextToken();
                _C(out SyntaxTreeNode node_C);
                node.AddSubNode(node_C);
            }
            else
            {
                SyntaxError("Ожидалось первое слово");
            }
        }

        private void _C(out SyntaxTreeNode node) // C'
        {
            node = new SyntaxTreeNode("C'");
            
            if (lexAn.Token.Type == TokenKind.FirstWord)
            {
                node.AddSubNode(new SyntaxTreeNode(lexAn.Token.Value));
                lexAn.RecognizeNextToken();
                _C(out SyntaxTreeNode node_C);
                node.AddSubNode(node_C);
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
        private void Match(TokenKind tkn)
        {
            if (lexAn.Token.Type == tkn) // Сравниваем.
            {
                lexAn.RecognizeNextToken(); // Распознаем следующий токен.
            }
            else
            {
                SyntaxError("Ожидалось " + tkn.ToString()); // Обнаружена синтаксическая ошибка.
            }
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
