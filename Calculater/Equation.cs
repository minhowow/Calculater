using System;
using System.Collections.Generic;
using System.Text;

namespace Calculater
{
    enum EqualType
    {
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
        Equal,
        NotEqual,
    }
    class Equation
    {
        public readonly Block a;
        public readonly Block b;
        public readonly EqualType type;

        private static Dictionary<EqualType, string> parseName = new Dictionary<EqualType, string>() {
            { EqualType.Less, "<" },
            { EqualType.LessOrEqual, "<=" },
            { EqualType.Greater, ">" },
            { EqualType.GreaterOrEqual, ">=" },
            { EqualType.Equal, "=" },
            { EqualType.NotEqual, "!=" },
        };
        private static Dictionary<EqualType, string> name = new Dictionary<EqualType, string>() {
            { EqualType.Less, "＜" },
            { EqualType.LessOrEqual, "≤" },
            { EqualType.Greater, "＞" },
            { EqualType.GreaterOrEqual, "≥" },
            { EqualType.Equal, "＝" },
            { EqualType.NotEqual, "≠" },
        };
        private Equation(Block a, Block b, EqualType type)
        {
            this.a = a;
            this.b = b;
            this.type = type;
        }

        public static bool TryParse(string str, out Equation equation)
        {
            try
            {
                equation = Parse(str);
                return true;
            }
            catch (Exception)
            {
                equation = null;
                return false;
            }
        }

        public static bool IsEquation(string str)
        {
            foreach (var item in parseName)
            {
                if (str.Contains(item.Value))
                {
                    var a = str.Split(new string[] { item.Value }, StringSplitOptions.None);
                    if (a.Length == 2)
                    {
                        return Block.IsBlock(a[0]) && Block.IsBlock(a[1]);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public static Equation Parse(string str)
        {
            foreach (var item in parseName)
            {
                if (str.Contains(item.Value))
                {
                    var a = str.Split(new string[] { item.Value }, StringSplitOptions.None);
                    if (a.Length == 2)
                    {
                        return new Equation(Block.Parse(a[0]), Block.Parse(a[1]), item.Key);
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            throw new ArgumentException();
        }

        public static string ToString(EqualType type)
        {
            return name[type];
        }
        public static Equation Create(Block a, Block b, EqualType type)
        {
            return new Equation(a, b, type);
        }

        public override string ToString()
        {
            return $"{a.ToString()} {ToString(type)} {b.ToString()}";
        }
    }
}
