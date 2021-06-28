using System;

namespace Lexical_analizer.src
{
    class StandartNode
    {
        private readonly Token token;
        public StandartNode(Token token) => this.token = token;
        public string GetSource()
        {
            return token.source;
        }

    }

    class BinOpNode
    {
        private readonly Token Operation;
        private readonly dynamic LeftOperand;
        private readonly dynamic RightOperand;

        public BinOpNode(Token Operation, dynamic LeftOperand, dynamic RightOperand)
        {
            this.Operation = Operation;
            this.LeftOperand = LeftOperand;
            this.RightOperand = RightOperand;
        }

        public string OutputTree(Token Er, int priority = 0)
        {
            string tree = "";

            if (Er != null) return tree += Er.string_num + "\t" + Er.column_num + "\t" + Er.type + "\t" + Er.source;

            tree += Operation.value + "\n";
            for (int i = 0; i < (priority + 1); i++) tree += "\t";

            if (Convert.ToString(LeftOperand.GetType()).Contains("BinOpNode"))
            {
                tree += LeftOperand.OutputTree(Er, priority + 1) + "\n";
            }
            else
            {
                tree += LeftOperand.GetSource() + "\n";
            }
            for (int i = 0; i < (priority + 1); i++) tree += "\t";

            if (Convert.ToString(RightOperand.GetType()).Contains("BinOpNode"))
            {
                tree += RightOperand.OutputTree(Er, priority + 1) + "\n";
            }
            else
            {
                tree += RightOperand.GetSource() + "\n";
            }
            for (int i = 0; i < (priority + 1); i++) tree += "\t";

            return tree;
        }
    }

}
