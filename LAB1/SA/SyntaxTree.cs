using System;
using System.Collections.Generic;

namespace LAB1.SA
{
    public class SyntaxTreeNode
    {
        private string name;
        private List<SyntaxTreeNode> subNodes;

        public SyntaxTreeNode(string name)
        {
            this.name = name;
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

        public List<SyntaxTreeNode> SubNodes
        {
            get { return subNodes; }
        }
    }
}
