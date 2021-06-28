using System;
using System.Collections.Generic;

namespace Lexical_analizer.src
{
    public class Parser
    {
        private readonly List<Token> Lexemes;
        private Token CurrentToken;
        private Token Er;
        private readonly string Tree;
        public Parser(List<Token> Lexemes)
        {
            this.Lexemes = Lexemes;
            this.CurrentToken = Lexemes[0];
            this.Tree = ParseExpression().OutputTree(Er);
        }

        private void GetNextToken()
        {
            if ((Lexemes.IndexOf(CurrentToken) + 1) != Lexemes.Count) CurrentToken = Lexemes[Lexemes.IndexOf(CurrentToken) + 1];
        }

        public string GetTree()
        {
            return Tree;
        }

        private BinOpNode ParseExpression()
        {
            dynamic left = ParseTerm();
            Token operation = CurrentToken;
            
            while ((Convert.ToString(operation.value) == "+") || (Convert.ToString(operation.value) == "-"))
            {
                GetNextToken();
                dynamic right = ParseTerm();
                left = new BinOpNode(operation, left, right);
                operation = CurrentToken;
                
            }

            if (Convert.ToString(left.GetType()).Contains("BinOpNode")) return left;
            else
            {
                Er = new Token(operation.string_num, operation.column_num, "Syntax error", "", "");
                return new BinOpNode(operation,"","");
            }
        }

        private dynamic ParseTerm()
        {
            dynamic left = ParseFactor();
            Token operation = CurrentToken;

            while ((Convert.ToString(operation.value) == "*") || (Convert.ToString(operation.value) == "/"))
            {
                GetNextToken();

                dynamic right = ParseFactor();
                left = new BinOpNode(operation, left, right);
                operation = CurrentToken;
            }

            return left;
        }

        private dynamic ParseFactor()
        {
            Token token = CurrentToken;
            GetNextToken();

            if ((token.type == "identifier") || (token.type.Contains("integer")) || (token.type.Contains("real"))) return new Node(token);
            else if (Convert.ToString(token.value) == "(")
            {
                dynamic left = ParseExpression();
                token = CurrentToken;

                GetNextToken();

                if (Convert.ToString(token.value) != ")") Er = new Token(token.string_num, token.column_num + token.source.Length, "Expected \")\"", "", "");
                return left;
            }
            else
            {
                Er = new Token(token.string_num, token.column_num, "Syntax error", "", "");
                return 0;
            }
        }
        
    }
}
