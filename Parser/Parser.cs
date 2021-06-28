using System;
using System.Collections.Generic;
using System.Text;
using Lexical_analizer.src;

namespace Lexical_analizer.src
{
    class Parser
    {
        private List<Token> Lexemes;
        private Token CurrentToken;

        public Parser(List<Token> Lexemes)
        {
            this.Lexemes = Lexemes;
            this.CurrentToken = Lexemes[0];
        }

        private void GetNextToken()
        {
            CurrentToken = Lexemes[Lexemes.IndexOf(CurrentToken) + 1];
        }

        private Node GetNode(Token token)
        {
            Node node = new Node(token);
            return node;
        }

        private dynamic ParseExpression()
        {
            //Token token = CurrentToken;

            dynamic left = ParseTerm();
            Token operation = CurrentToken;
            while ((operation.value == "+") || (operation.value == "-"))
            {
                GetNextToken();
                dynamic right = ParseTerm();
                left = new BinOpNode(operation, left, right);
                operation = CurrentToken;
            }
            return left;
        }

        private dynamic ParseTerm()
        {
            dynamic left = ParseFactor();
            Token operation = CurrentToken;

            while ((operation.value == "*") || (operation.value == "/"))
            {
                GetNextToken();
                Node right = ParseFactor();
                left = new BinOpNode(operation, left, right);
                operation = CurrentToken;
            }
            return left;
        }

        private Node ParseFactor()
        {
            Token token = CurrentToken;
            GetNextToken();

            if ((token.type == "identifier") || (token.type.Contains("integer")) || (token.type.Contains("real"))) return new Node(token);
            else if (token.type == "(")
            {
                Node left = ParseExpression();
                token = CurrentToken;
                GetNextToken();
                return left;
            }
            else throw new Exception("Errrrrr");
        }
        
    }
}
