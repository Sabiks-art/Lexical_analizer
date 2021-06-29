using System;
using System.Collections.Generic;

namespace Lexical_analizer.src
{
    public class Parser
    {
        private readonly List<Token> Lexemes;
        private Token CurrentToken;
        private Token Er;

        private enum Type_lex
        {
            identifier,             integer_literal_int,        integer_literal_uint,    integer_literal_long,   integer_literal_ulong,
            real_literal_float,     real_literal_decimal,       real_literal_double,
        };

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

        private bool IsOperation(string str)
        {
            if ((str == "+") || (str == "-") || (str == "*") || (str == "/")) return true;
            else return false;
        }

        private void OutputER(Token token)
        {
            Er = new Token(token.string_num, token.column_num, "Syntax error", "Expected Identificator or Number", "");
        }

        private BinOpNode ParseExpression()
        {
            Token token = CurrentToken;

            if (Lexemes.Count == 1)
            {
                Er = new Token(token.string_num, token.column_num + token.source.Length, "Syntax Error", "Expected Expression", "");
                return new BinOpNode(token, "", "");
            }

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
                OutputER(operation);
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
            if (Enum.TryParse(typeof(Type_lex), token.type, out object _)) return new StandartNode(token);
            else if (Convert.ToString(token.value) == "(")
            {
                dynamic left = ParseExpression();
                token = CurrentToken;

                GetNextToken();

                if (Convert.ToString(token.value) != ")") Er = new Token(token.string_num, token.column_num + token.source.Length, "Syntax Error", "Expected \")\"", "");
                return left;
            }
            else
            {
                OutputER(token);
                return 0;
            }
        }
        
    }
}
