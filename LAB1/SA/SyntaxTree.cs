using System;
using System.Collections.Generic;

namespace LAB1.SA
{
    public class SyntaxTreeNode
    {
        private string name;
        private List<SyntaxTreeNode> subNodes;
        private Token token;

        public SyntaxTreeNode(string name)
        {
            this.name = name;
            this.token = null;
            subNodes = new List<SyntaxTreeNode>();
        }

        public SyntaxTreeNode(Token token)
        {
            this.token = token;
            this.name = token.Value;
            subNodes = new List<SyntaxTreeNode>();
        }

        public void AddSubNode(SyntaxTreeNode subNode)
        {
            this.subNodes.Add(subNode);
        }
        
        public string Name
        {
            get { return name; }
        }

        public Token Token
        {
            get { return token; }
        }

        public List<SyntaxTreeNode> SubNodes
        {
            get { return subNodes; }
        }
    }
}
