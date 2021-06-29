using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lexical_analizer.src;
using System.IO;

namespace UnitTestProject2
{
    [TestClass]
    public class Lexer_test
    {
        private void Abc(string test_name)
        {
            Lexical_analizer.src.Lexer Lexer1 = new Lexical_analizer.src.Lexer();

            Lexer1.Analysis(@"C:\Users\Danil\source\repos\Lexical_analizer\UnitTestProject2\Lexer_Tests\" + test_name + ".txt");
            string result;
            string expectation;
            StreamReader sr = new StreamReader(@"C:\Users\Danil\source\repos\Lexical_analizer\UnitTestProject2\Lexer_Tests_expectation\" + test_name + ".txt");
            for (int i = 0; i < Lexer1.Lexemes.Count; i++)
            {
                result = Lexer1.Lexemes[i].string_num + "\t" + Lexer1.Lexemes[i].column_num + "\t" + Lexer1.Lexemes[i].type + "\t" + Lexer1.Lexemes[i].source + "\t" + Lexer1.Lexemes[i].value;
                expectation = sr.ReadLine();
                Assert.AreEqual(result, expectation);
            }
        }
        [TestMethod]
        public void Id() { Abc("id"); }

        [TestMethod]
        public void Id_() { Abc("id_"); }

        [TestMethod]
        public void Id_dog() { Abc("id_@"); }

        [TestMethod]
        public void Id_dog_num() { Abc("id_@_num"); }

        [TestMethod]
        public void Id_dog_num_() { Abc("id_@_num_"); }

        [TestMethod]
        public void Id_num() { Abc("id_num"); }

        [TestMethod]
        public void Unrecognized_char_in_id_and_num() { Abc("unrecognized_char_in_id_and_num"); }

        [TestMethod]
        public void Int_num2() { Abc("int_num2"); }

        [TestMethod]
        public void Int_num8() { Abc("int_num8"); }

        [TestMethod]
        public void Int_num10() { Abc("int_num10"); }

        [TestMethod]
        public void Int_num16() { Abc("int_num16"); }

        [TestMethod]
        public void Long_num2() { Abc("long_num2"); }

        [TestMethod]
        public void Long_num8() { Abc("long_num8"); }

        [TestMethod]
        public void Long_num10() { Abc("long_num10"); }

        [TestMethod]
        public void Long_num16() { Abc("long_num16"); }

        [TestMethod]
        public void Uint_num2() { Abc("uint_num2"); }

        [TestMethod]
        public void Uint_num8() { Abc("uint_num8"); }

        [TestMethod]
        public void Uint_num10() { Abc("uint_num10"); }

        [TestMethod]
        public void Uint_num16() { Abc("uint_num16"); }

        [TestMethod]
        public void Ulong_num2() { Abc("ulong_num2"); }

        [TestMethod]
        public void Ulong_num8() { Abc("ulong_num8"); }


        [TestMethod]
        public void Ulong_num10() { Abc("ulong_num10"); }

        [TestMethod]
        public void Ulong_num16() { Abc("ulong_num16"); }

        [TestMethod]
        public void Num2L() { Abc("num2L"); }

        [TestMethod]
        public void Num8L() { Abc("num8L"); }

        [TestMethod]
        public void Num10L() { Abc("num10L"); }

        [TestMethod]
        public void Num16L() { Abc("num16L"); }

        [TestMethod]
        public void Num2uint() { Abc("num2uint"); }

        [TestMethod]
        public void Num8uint() { Abc("num8uint"); }

        [TestMethod]
        public void Num10uint() { Abc("num10uint"); }

        [TestMethod]
        public void Num16uint() { Abc("num16uint"); }

        [TestMethod]
        public void Num2ulong() { Abc("num2ulong"); }

        [TestMethod]
        public void Num8ulong() { Abc("num2ulong"); }

        [TestMethod]
        public void Num10ulong() { Abc("num10ulong"); }

        [TestMethod]
        public void Num16ulong() { Abc("num16ulong"); }

        [TestMethod]
        public void Real() { Abc("real"); }

        [TestMethod]
        public void Real_dot_exp() { Abc("real_._exp"); }

        [TestMethod]
        public void Real_decimal() { Abc("real_decimal"); }

        [TestMethod]
        public void Real_decimal_dot_exp() { Abc("real_decimal_._exp"); }

        [TestMethod]
        public void Real_decimal_exp() { Abc("real_decimal_exp"); }

        [TestMethod]
        public void Real_double() { Abc("real_double"); }

        [TestMethod]
        public void Real_double_dot_exp() { Abc("real_double_._exp"); }

        [TestMethod]
        public void Real_double_exp() { Abc("real_double_exp"); }

        [TestMethod]
        public void Real_exp() { Abc("real_exp"); }

        [TestMethod]
        public void Real_float() { Abc("real_float"); }

        [TestMethod]
        public void Real_float_dot_exp() { Abc("real_float_._exp"); }

        [TestMethod]
        public void Real_float_exp() { Abc("real_float_exp"); }

        [TestMethod]
        public void Char() { Abc("char"); }

        [TestMethod]
        public void Char_error() { Abc("char_error"); }

        [TestMethod]
        public void Char_error1() { Abc("char_error1"); }

        [TestMethod]
        public void Char_error2() { Abc("char_error2"); }

        [TestMethod]
        public void Char_control_sequence() { Abc("char_control_sequence"); }

        [TestMethod]
        public void Char_unicode() { Abc("char_unicode"); }

        [TestMethod]
        public void Delimiters() { Abc("delimiters"); }

        [TestMethod]
        public void Arithmetic_operators() { Abc("arithmetic_operators"); }

        [TestMethod]
        public void Compair_operators() { Abc("compair_operators"); }

        [TestMethod]
        public void Logic_operators() { Abc("logic_operators"); }

        [TestMethod]
        public void Others_operators() { Abc("others_operators"); }

        [TestMethod]
        public void Single_string_comment() { Abc("single_string_comment"); }

        [TestMethod]
        public void Single_string_comment1() { Abc("single_string_comment1"); }

        [TestMethod]
        public void Multi_string_comment1() { Abc("multi_string_comment1"); }

        [TestMethod]
        public void Multi_string_comment() { Abc("multi_string_comment"); }

        [TestMethod]
        public void Multi_string_comment2() { Abc("multi_string_comment2"); }

        [TestMethod]
        public void String () { Abc("string"); }

        [TestMethod]
        public void string_error() { Abc("string_error"); }

        [TestMethod]
        public void operator_error() { Abc("operator_error"); }

        [TestMethod]
        public void comment_error() { Abc("comment_error"); }

        [TestMethod]
        public void string_error1() { Abc("string_error1"); }

        [TestMethod]
        public void String_with_control_sequence() { Abc("string_with_control_sequence"); }

        [TestMethod]
        public void String_with_unicode_symbol() { Abc("string_with_unicode_symbol"); }

        [TestMethod]
        public void Literal_string() { Abc("literal_string"); }

        [TestMethod]
        public void Key_words() { Abc("key_words"); }

        [TestMethod]
        public void Func_declaration() { Abc("func_declaration"); }

        [TestMethod]
        public void Array_declaration_and_initialization() { Abc("array_declaration_and_initialization"); }

        [TestMethod]
        public void If_else_operator() { Abc("if_else_operator"); }

        [TestMethod]
        public void Char_declaration_and_initialization() { Abc("char_declaration_and_initialization"); }

        [TestMethod]
        public void String_declaration_and_initialization() { Abc("string_declaration_and_initialization"); }

        [TestMethod]
        public void Number_declaration_and_initialization() { Abc("number_declaration_and_initialization"); }

        [TestMethod]
        public void Unrecognized_char() { Abc("unrecognized_char"); }

    }

    [TestClass]
    public class Parser_tests
    {
        private void Abc(string test_name)
        {
            Lexical_analizer.src.Lexer Lexer1 = new Lexical_analizer.src.Lexer();

            Lexer1.Analysis(@"C:\Users\Danil\source\repos\Lexical_analizer\UnitTestProject2\Parser_Tests\" + test_name + ".txt");
            string result = "";
            string expectation = "";
            Parser pars = new Parser(Lexer1.Lexemes);
           
            StreamReader sr = new StreamReader(@"C:\Users\Danil\source\repos\Lexical_analizer\UnitTestProject2\Parser_Tests_expectation\" + test_name + ".txt");

            result = pars.GetTree();
            expectation = sr.ReadToEnd();

            Assert.AreEqual(result, expectation);
        }

        [TestMethod]
        public void Sum() { Abc("sum"); }

        [TestMethod]
        public void Sum_comp() { Abc("sum_comp"); }

        [TestMethod]
        public void Sum_parentheses() { Abc("sum_parentheses"); }

        [TestMethod]
        public void Sum_comp_parentheses() { Abc("sum_comp_parentheses"); }

        [TestMethod]
        public void Double_parentheses_sum() { Abc("double_parentheses_sum"); }

        [TestMethod]
        public void Double_parentheses_sum_comp_div() { Abc("double_parentheses_sum_comp_div"); }

        [TestMethod]
        public void Wrong_parentheses1() { Abc("wrong_parentheses1"); }

        [TestMethod]
        public void Wrong_parentheses2() { Abc("wrong_parentheses2"); }

        [TestMethod]
        public void Error1() { Abc("error1"); }

        [TestMethod]
        public void Diff() { Abc("diff"); }

        [TestMethod]
        public void Error2() { Abc("error2"); }

        [TestMethod]
        public void Semicolon() { Abc("semicolon"); }

        [TestMethod]
        public void Nothing() { Abc("nothing"); }
    }
}
