using System;
using System.IO;
using Lexical_analizer.src;

namespace Lexical_analizer
{
    public class Program
    {

        public static void Main()
        {

            Lexer Lexer1 = new Lexer();
            
           
            Lexer1.Analysis(@"..\..\..\src\Tests\1.txt");
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            Console.WriteLine("{0,-18} {1,-18} {2,-26} {3,-15} {4}", "Номер строки", "Номер столбца", "Тип", "Исходный код", "Значение\n");

            for (int i = 0; i < Lexer1.Lexemes.Count; i++)
                Console.WriteLine("{0,-18} {1,-18} {2,-26} {3,-15} {4}", Lexer1.Lexemes[i].string_num, Lexer1.Lexemes[i].column_num, Lexer1.Lexemes[i].type, Lexer1.Lexemes[i].source, Lexer1.Lexemes[i].value);
            Console.WriteLine("Лексический анализ закончен");
            Console.ReadLine();
        }
    }
}
