
namespace Lexical_analizer.src
{
    public class Token
    {
        public readonly int string_num;
        public readonly int column_num;
        public readonly string type;
        public readonly string source;
        public readonly dynamic value;

        public Token(int string_num, int column_num, string type, string source, dynamic value)
        {
            this.string_num = string_num;
            this.column_num = column_num;
            this.type = type;
            this.source = source;
            this.value = value;
        }
    }
}
