using Lexical_analizer.src;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexical_analizer.src
{
    abstract class qwe { }
    class Node : qwe
    {
        private Token token;
        public Node(Token token) => this.token = token;

        public dynamic GetValue()
        {
            return token.value;
        }

        public new string GetType()
        {
            return token.type;
        }
    }

    class BinOpNode : qwe
    {
        private Token Operation;
        private Node LeftOperand;
        private Node RightOperand;

        public BinOpNode(Token Operation, Node LeftOperand, Node RightOperand)
        {
            this.Operation = Operation;
            this.LeftOperand = LeftOperand;
            this.RightOperand = RightOperand;
        }
    }

}
