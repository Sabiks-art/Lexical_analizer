using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace Lexical_analizer.src
{
    public class Lexer
    {
        public List<Token> Lexemes = new List<Token>();
        private enum States { S, ID, NUM, NUM2, NUM8, NUM10, NUM16, FNUM, CH, STR, OP, COM, ER, FIN };
        private States state;
        private enum Key_words { @as,      @bool,     @break,   @byte,   @case,   @catch,  @char,       @checked,   @const,  @continue,
                                 @decimal, @default,  @do,      @double, @else,   @enum,   @false,      @finally,   @float,  @for,
                                 @foreach, @goto,     @if,      @in,     @int,    @is,     @long,       @namespace, @new,    @null,
                                 @out,     @true,     @try,     @typeof, @params, @ref,    @return,     @sbyte,     @short,  @sizeof,
                                 @static,  @string,   @switch,  @throw,  @uint,   @ulong,  @unchecked,  @unsafe,    @ushort, @void,
                                 @while };

        private enum Type_lex
        {
            key_word, identifier, integer_literal_int, real_literal_double, char_literal,
            string_literal, delimiter, Operator, integer_literal_uint,
            integer_literal_long, integer_literal_ulong, real_literal_float, real_literal_decimal
        };

        private readonly string[] control_sequence = { "\\\'", "\\\"", "\\\\", "\\0", "\\a", "\\b", "\\f", "\\n", "\\r", "\\t", "\\v" };
        private readonly string[] control_sequence_value = { "\'", "\"", "\\", "U+0000", "U+0007", "U+0008", "U+000C", "U+000A", "U+000D", "\t", "U+000B" };


        private readonly char[] delimiter = { '(', ')', '[', ']', '{', '}', '.', ',', ';' };

        private readonly string[] operators = { "+", "-", "*", "/", "%", "&",
                                                "|", "^", "!", "~", "=", "<",
                                                ">", "++", "--", "&&",
                                                "||", "==", "!=", "<=", ">=",
                                                "+=", "-=", "*=", "/=", "%=", "&=",
                                                "|=", "^=", "<<", "<<=", "=>", ">>", ">>=",};

        private StreamReader sr;
        private string buf = "";
        private readonly char[] sm = new char[1];

        private int NumOfString = 1;
        private int NumOfColumn = 1;

        private void GetNext()
        {
            if (sr.Read(sm, 0, 1) == 0) sm[0] = '\0';
        }

        private void ClearBuf()
        {
            buf = "";
        }

        private void AddBuf(char symb)
        {
            buf += symb;
        }

        private void AddLex(List<Token> lexes, int string_num, int column_num, string type, string source, dynamic val)
        {
            lexes.Add(new Token(string_num, column_num, type, source, val));
        }

        private bool IsDelimiter(char symb)  
        {
            if (Array.IndexOf(delimiter,symb) != (-1)) return true;
            else return false;
        }

        private bool IsOperator(string str)
        {
            if (Array.IndexOf(operators, str) != (-1)) return true;
            else return false;
        }

        private bool IsDelimiterOrOperator(char symb)
        {
            if (IsDelimiter(symb) || IsOperator(Convert.ToString(symb))) return true;
            else return false;
        }

        private bool IsHexadecimal(char symb)
        {
            if (Char.IsDigit(symb) || (symb >= 'a' && symb <= 'f') || (symb >= 'A' && symb <= 'F')) return true;
            else return false;
        }
 
        private bool IsInsignChar(char symb)
        {
            if (symb == ' ' || symb == '\n' || symb == '\t' || symb == '\r' || symb == '\v') return true;
            else return false;
        }

        private (string, string) SufDefinition(string str, string suf)
        {
            if (str.Contains("lu")) str = str.Replace("lu", "ul");

            if (str.Contains("ul") || str.Contains("l") || str.Contains("u"))
            {
                if (!IsCorrectSuf(str)) state = States.ER;


                if (str.Contains("ul"))
                {
                    suf = "ul";
                    str = str[..^2];
                }
                else
                {
                    suf = Convert.ToString(str[^1]);
                    str = str[..^1];
                }

            }
            return (str, suf);
        }

        private bool IsCorrectSuf(string str)
        {
            
            if (str.Contains("ul"))
            {
                if (str.IndexOf("ul") != (str.Length - 2)) return false;
            }
            else if (str.Contains("u"))
            {
                if (str.IndexOf("u") != (str.Length - 1)) return false;
            }
            else if (str.Contains("l"))
            {
                if (str.IndexOf("l") != (str.Length - 1)) return false;
            }
            else if (str.Contains("f"))
            {
                if (str.IndexOf("f") != (str.Length - 1)) return false;
            }
            else if (str.Contains("d"))
            {
                if (str.IndexOf("d") != (str.Length - 1)) return false;
            }
            else if (str.Contains("m"))
            {
                if (str.IndexOf("m") != (str.Length - 1)) return false;
            }


            return true;
        }

        private void Processing_conditions_num(States st, int basis)
        {
            AddBuf(sm[0]);
            GetNext();
            
            if ((basis == 10) && ((sm[0] == '.') || (Char.ToLower(sm[0]) == 'e'))) state = States.FNUM;
            else if (!IsInsignChar(sm[0]) && !IsDelimiterOrOperator(sm[0]) && !sr.EndOfStream) state = st;
            else
            {
                if ((sr.EndOfStream) && (!IsInsignChar(sm[0]) && !IsDelimiterOrOperator(sm[0]))) AddBuf(sm[0]);

                TypeDefinitionAndAddLex(basis);

                if (state == States.ER) return;
                else if (IsInsignChar(sm[0]) || IsDelimiterOrOperator(sm[0])) state = States.S;
                else if (sr.EndOfStream) state = States.FIN;
                else state = States.S;
            }
        }

        private void TypeDefinitionAndAddLex(int basis)
        {
            string str;
            string suf = "";

            if ((basis == 2) || (basis == 16)) str = buf[2..].ToLower();
            else if (basis == 8) str = buf[1..].ToLower();
            else str = buf.ToLower();

            var tuple = SufDefinition(str, suf);
            str = tuple.Item1;
            suf = tuple.Item2;

            try { str = Convert.ToString(Convert.ToUInt64(str,basis)); }
            catch 
            { 
                state = States.ER;
                return;
            };

            switch (suf)
            {
                case "ul":
                    if (ulong.TryParse(str, out ulong result))              AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_ulong), buf, result);
                    else state = States.ER;
                    break;

                case "u":
                    if (uint.TryParse(str, out uint result1))               AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_uint), buf, result1);
                    else
                    {
                        if (ulong.TryParse(str, out ulong result2))         AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_ulong), buf, result2);
                        else state = States.ER;
                    }
                    break;

                case "l":
                    if (long.TryParse(str, out long result3))               AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_long), buf, result3);
                    else
                    {
                        if (ulong.TryParse(str, out ulong result4))         AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_ulong), buf, result4);
                        else state = States.ER;
                    }
                    break;

                default:
                    if (int.TryParse(str, out int result5))                 AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_int), buf, result5);
                    else
                    {
                        if (uint.TryParse(str, out uint result6))           AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_uint), buf, result6);
                        else
                        {
                            if (long.TryParse(str, out long result7))       AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_long), buf, result7);
                            else
                            {
                                if (ulong.TryParse(str, out ulong result8)) AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_ulong), buf, result8);
                                else state = States.ER;
                            }
                        }
                    }
                    break;
            }
        }

        public void Analysis(string text)
        {
            sr = new StreamReader(text);
            GetNext();

            while (state != States.FIN)
            {
                switch (state)
                {

                    case States.S:

                        NumOfColumn += buf.Length;
                        if (IsInsignChar(sm[0]))
                        {

                            if (sm[0] == '\n')
                            {
                                NumOfColumn = 1;
                                NumOfString++;
                            }
                            else NumOfColumn++;

                            ClearBuf();


                            if (sr.EndOfStream) state = States.FIN;
                            else
                            {
                                GetNext();
                                state = States.S;
                            }
                        }
                        else if (Char.IsLetter(sm[0]) || sm[0] == '_')
                        {

                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();

                            state = States.ID;
                        }
                        else if (char.IsDigit(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();

                            state = States.NUM;
                        }
                        else if (sm[0] == '\'')
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();

                            state = States.CH;
                        }
                        else if (sm[0] == '"')
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();

                            state = States.STR;
                        }
                        else if (IsDelimiter(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);

                            AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.delimiter), buf, buf);

                            if (sr.EndOfStream) state = States.FIN;
                            else
                            {
                                GetNext();
                                state = States.S;
                            }
                        }
                        else if (IsOperator(Convert.ToString(sm[0])))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();
                            state = States.OP;
                        }
                        else if (sm[0] == '@')
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            GetNext();

                            if (sm[0] == '"')
                            {
                                AddBuf(sm[0]);
                                GetNext();
                                state = States.STR;
                            }
                            else if (Char.IsLetter(sm[0]) || sm[0] == '_')
                            {
                                AddBuf(sm[0]);
                                GetNext();
                                state = States.ID;
                            }
                            else state = States.ER;
                        }
                        else
                        {
                            GetNext();
                            state = States.ER;
                        }
                        break;

                    case States.ID:

                        if ((Char.IsLetterOrDigit(sm[0]) || sm[0] == '_') && !sr.EndOfStream)
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = States.ID;
                        }
                        else if (IsInsignChar(sm[0]) || IsDelimiterOrOperator(sm[0]) || sr.EndOfStream)
                        {
                            if ((sr.EndOfStream) && (Char.IsLetterOrDigit(sm[0]) || sm[0] == '_')) AddBuf(sm[0]);

                            bool srch = false;
                            foreach (Key_words key_word in Enum.GetValues(typeof(Key_words))) if (Convert.ToString(key_word) == buf) srch = true;

                            if (srch) AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.key_word), buf, "KWName");
                            else AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.identifier), buf, "IDName");

                            if (IsInsignChar(sm[0]) || IsDelimiterOrOperator(sm[0])) state = States.S;
                            else if (sr.EndOfStream) state = States.FIN;
                            else state = States.S;
                        }
                        else state = States.ER;
                        break;

                    case States.NUM:
                        if ((sr.EndOfStream && Char.IsDigit(sm[0])) || (sr.EndOfStream && sm[0] == '\0'))
                        {
                            AddBuf(sm[0]);
                            AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_int), buf, Convert.ToInt32(buf));
                            state = States.FIN;
                        }
                        else if (char.IsDigit(sm[0]) && buf == "0")             state = States.NUM8;
                        else if (char.IsDigit(sm[0]))                           state = States.NUM10;
                        else if ((Char.ToLower(sm[0]) == 'b') && buf == "0")                   state = States.NUM2;
                        else if ((Char.ToLower(sm[0]) == 'x') && buf == "0")                   state = States.NUM16;
                        else if (sm[0] == '.' || Char.ToLower(sm[0]) == 'e')    state = States.FNUM;
                        else if ((IsInsignChar(sm[0]) || IsDelimiterOrOperator(sm[0])) && Int32.TryParse(buf, out int result))
                        {
                            AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.integer_literal_int), buf, result);
                            state = States.S;
                        }
                        else state = States.NUM10;
                        break;

                    case States.NUM2:
                        Processing_conditions_num(States.NUM2, 2);
                        break;

                    case States.NUM8:
                        Processing_conditions_num(States.NUM8, 8);
                        break;

                    case States.NUM16:
                        Processing_conditions_num(States.NUM16, 16);
                        break;

                    case States.NUM10:
                        Processing_conditions_num(States.NUM10, 10);
                        break;

                    case States.FNUM:
                        
                        AddBuf(sm[0]);
                        GetNext();

                        if ((!IsInsignChar(sm[0]) && !IsDelimiterOrOperator(sm[0]) && !sr.EndOfStream) || sm[0] == '.' || sm[0] == '-') state = States.FNUM;
                        else
                        {

                            if ((sr.EndOfStream) && (!IsInsignChar(sm[0]) && !IsDelimiterOrOperator(sm[0]))) AddBuf(sm[0]);

                            string sup_str = buf.Replace(".", ",");
                            sup_str = sup_str.ToLower();

                            if ((sup_str.Contains("f") || sup_str.Contains("d") || sup_str.Contains("m")) && !IsCorrectSuf(sup_str)) state = States.ER;

                            if (sup_str.Contains("f") && float.TryParse(sup_str[..^1], out float result))                                                                AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.real_literal_float), buf, result);
                            else if (sup_str.Contains("d") && double.TryParse(sup_str[..^1], out double result1))                                                        AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.real_literal_double), buf, result1);
                            else if (sup_str.Contains("m") && decimal.TryParse(sup_str[..^1], NumberStyles.Float, CultureInfo.CurrentUICulture, out decimal result3))    AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.real_literal_decimal), buf, result3);
                            else if (double.TryParse(sup_str, out double result4))                                                                                       AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.real_literal_double), buf, result4);
                            else state = States.ER;

                            if (state == States.ER) break;
                            else if (IsInsignChar(sm[0]) || IsDelimiterOrOperator(sm[0])) state = States.S;
                            else if (sr.EndOfStream) state = States.FIN;
                            else state = States.S;
                        }
                        break;

                    case States.CH:

                        if (sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\v') state = States.ER;
                        else if ((sm[0] != '\'' || (sm[0] == '\'' && buf[^1] == '\\' && buf.Length == 2)) && buf.Length < 8)
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = States.CH;
                        }
                        else
                        {

                            if (buf.Length < 2) state = States.ER;

                            string sup_str = buf.Remove(0, 1);
                            buf += "\'";

                            int srch = Array.IndexOf(control_sequence, sup_str);

                            if (srch != -1)
                            {
                                AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.char_literal), buf, control_sequence_value[srch]);
                            }
                            else if ((buf.Contains("\\u") || buf.Contains("\\U")) && buf.Length == 8)
                            {
                                try { AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.char_literal), buf, Convert.ToChar(Convert.ToInt32(buf[3..^1], 16))); }
                                catch { state = States.ER; };
                            }
                            else if (buf.Length == 3)
                            {
                                AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.char_literal), buf, Convert.ToChar(buf.Replace("'", "")));
                            }
                            else state = States.ER;

                            if (state == States.ER) GetNext();
                            else if (sr.EndOfStream) state = States.FIN;
                            else
                            {
                                GetNext();
                                state = States.S;
                            }
                        }
                        break;

                    case States.STR:

                        if (sm[0] == '"')
                        {

                            buf += "\"";
                            string base_buf = buf;
                            if (buf[0] == '@')
                            {
                                buf = buf.Replace("@", "");

                                AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.string_literal), "@" + buf, buf.Replace("\"", ""));
                                NumOfColumn++;

                                if (sr.EndOfStream) state = States.FIN;
                                else
                                {
                                    GetNext();
                                    state = States.S;
                                }
                                break;
                            }

                            for (int i = 0; i < 11; i++) if (buf.Contains(control_sequence[i])) buf = buf.Replace(control_sequence[i], control_sequence_value[i]);

                            if (buf.Contains("\\u") || buf.Contains("\\U"))
                            {
                                if (buf.Contains("\\U")) buf = buf.Replace("\\U", "\\u");

                                int srch = buf.IndexOf("\\u");
                                
                                try { buf = buf.Replace(buf[srch..(srch + 6)], Convert.ToString(Convert.ToChar(Convert.ToInt32(buf[(srch + 2)..(srch + 6)], 16)))); }
                                catch { state = States.ER; };
                            }

                            if (state != States.ER) AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.string_literal), base_buf, buf.Replace("\"", ""));

                            buf = base_buf;

                            if (state == States.ER) GetNext();
                            else if (sr.EndOfStream) state = States.FIN;
                            else
                            {
                                GetNext();
                                state = States.S;
                            }
                        }
                        else if (sm[0] == '\n' || sr.EndOfStream) state = States.ER;
                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = States.STR;
                        }
                        break;

                    case States.OP:

                        if (buf[0] == '/' && (sm[0] == '/' || sm[0] == '*'))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = States.COM;
                        }
                        else if (IsOperator(Convert.ToString(sm[0])) && !sr.EndOfStream)
                        {

                            AddBuf(sm[0]);
                            GetNext();
                            state = States.OP;
                        }
                        else
                        {
                            if (sr.EndOfStream && IsOperator(Convert.ToString(sm[0]))) AddBuf(sm[0]);

                            if (IsOperator(buf)) AddLex(Lexemes, NumOfString, NumOfColumn, nameof(Type_lex.Operator), buf, buf);
                            else state = States.ER;

                            
                            if (state == States.ER) break;
                            else if (sm[0] != buf[^1]) state = States.S;
                            else if (sr.EndOfStream) state = States.FIN;
                            else state = States.S;
                        }
                        break;

                    case States.COM:

                        if (buf[^1] == '/') while (sm[0] != '\n' && !sr.EndOfStream) GetNext();
                        else 
                            while (buf[^2] != '*' || buf[^1] != '/')
                            {
                                AddBuf(sm[0]);
                                GetNext();

                                if (sr.EndOfStream && (buf[^2] != '*' || buf[^1] != '/')) AddBuf(sm[0]);
                                if (sm[0] == '\n') NumOfString++;

                                if (sr.EndOfStream && (buf[^2] != '*' || buf[^1] != '/')) 
                                {
                                    state = States.ER;
                                    break;
                                }
                            }

                        if ((buf[^2] == '*' || buf[^1] == '/') && (buf.Contains('\n'))) NumOfColumn -= buf.LastIndexOf('/') + 1;

                        if (state == States.ER) break;
                        else if (sr.EndOfStream) state = States.FIN;
                        else state = States.S;
                        
                        break;

                    case States.ER:
                        AddLex(Lexemes, NumOfString, NumOfColumn, "Ошибка ввода", "", "");

                        if (sr.EndOfStream) state = States.FIN;
                        else state = States.S;

                        break;
                        
                    case States.FIN:
                        break;
                }
            }
        }
    }
}
