using System;
using System.Collections.Generic;
using System.Linq;
using LAB1.Exceptions;
using LAB1.SA;

namespace LAB1.Output
{
    public class Generator
    {
        private readonly SyntaxTreeNode treeRoot;
        private readonly List<string> outputText;

        public Generator(SyntaxTreeNode treeRoot)
        {
            this.treeRoot = treeRoot;
            outputText = new List<string>();
        }

        public List<string> OutputText
        {
            get { return outputText; }
        }

        /// <summary>
        /// Генерирует стуктурированный текст
        /// </summary>
        public void GenerateStructuredText()
        {
            outputText.Clear();
            RecurTraverseTree(treeRoot, 0); // Рекурcивный обход
        }

        /// <summary>
        /// Рекурсивно обойти дерево, формируя выходной текст.
        /// </summary>
        /// <param name="node">узел дерева</param>
        /// <param name="indent">отступ</param>
        private void RecurTraverseTree(SyntaxTreeNode node, int indent)
        {
            if (node.SubNodes.Count() > 0 || node.Token == null) // Если текущий узел - нетерминал.
            {
                foreach (SyntaxTreeNode item in node.SubNodes)
                {
                    if ((node != treeRoot) && (node.Name == "S"))
                        RecurTraverseTree(item, indent + 10); //  с дополнительным отступом.
                    else
                        RecurTraverseTree(item, indent); // с текущим отступом.
                }
            }
            else
            {
                int k = 0;
                string name = node.Name;

                if (node.Token.Type == TokenKind.FirstWord || 
                    node.Token.Type == TokenKind.SecondWord ||
                    node.Token.Type == TokenKind.Comma)
                {
                    k = 5;
                }
             
                if (node.Token.Type == TokenKind.FirstWord)
                {
                    int d = 0;
                    int n = 0;
                    var binaryToDecimalTranslation = new BinaryToDecimalTranslation(name);
                    binaryToDecimalTranslation.Start(ref d, ref n);
                    name = d.ToString();
                    //name = Convert.ToInt32(name, 2).ToString();
                }
 
                outputText.Add(new string(' ', indent + k) + name);
            }
        }
    }
}
