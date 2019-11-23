using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculater
{
    static class BlockExtension
    {
        public static Block Plus(this Block block)
        {
            return SignBlock.Create(block, SignType.Plus);
        }
        public static Block Minus(this Block block)
        {
            return SignBlock.Create(block, SignType.Minus);
        }
        public static Block Plus(this Block block, double a)
        {
            return TwoOperatorBlock.Create(block, ConstBlock.Create(a), TwoOperatorType.Plus);
        }
        public static Block Plus(this Block block, Block a)
        {
            return TwoOperatorBlock.Create(block, a, TwoOperatorType.Plus);
        }
        public static Block Minus(this Block block, double a)
        {
            return TwoOperatorBlock.Create(block, ConstBlock.Create(a), TwoOperatorType.Minus);
        }
        public static Block Minus(this Block block, Block a)
        {
            return TwoOperatorBlock.Create(block, a, TwoOperatorType.Minus);
        }
        public static Block Multiply(this Block block, double a)
        {
            return TwoOperatorBlock.Create(block, ConstBlock.Create(a), TwoOperatorType.Multiply);
        }
        public static Block Multiply(this Block block, Block a)
        {
            return TwoOperatorBlock.Create(block, a, TwoOperatorType.Multiply);
        }
        public static Block Divide(this Block block, double a)
        {
            return TwoOperatorBlock.Create(block, ConstBlock.Create(a), TwoOperatorType.Divide);
        }
        public static Block Divide(this Block block, Block a)
        {
            return TwoOperatorBlock.Create(block, a, TwoOperatorType.Divide);
        }
        public static Block Pow(this Block block, double a)
        {
            return TwoOperatorBlock.Create(block, ConstBlock.Create(a), TwoOperatorType.Pow);
        }
        public static Block Pow(this Block block, Block a)
        {
            return TwoOperatorBlock.Create(block, a, TwoOperatorType.Pow);
        }
        public static Block Sin(this Block block)
        {
            return FunctionBlock.Create(block, FunctionType.Sin);
        }
        public static Block Cos(this Block block)
        {
            return FunctionBlock.Create(block, FunctionType.Cos);
        }
        public static Block Tan(this Block block)
        {
            return FunctionBlock.Create(block, FunctionType.Tan);
        }
    }

    abstract class Block
    {

        private static Block ParseConcat(Block a, Block b, char type)
        {
            if (a == null)
            {
                return SignBlock.Create(b, OperatorBlock.ToSignType(type));
            }

            if (b == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                return TwoOperatorBlock.Create(a, b, OperatorBlock.ToTwoOperatorType(type));
            }
        }

        public static bool TryParse(string str, out Block block)
        {
            try
            {
                block = Parse(str);
                if (block != null&& block.IsValid())
                {
                    return true;
                }
            }
            catch (Exception)
            {
                block = null;
            }
            return false;
        }

        public static bool IsBlock(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            str = str.Trim();

            foreach (var item in TwoOperatorBlock.priority)
            {
                if (str.IndexOfAny(item.ToCharArray()) != -1)
                {
                    List<string> subjects = new List<string>();
                    List<char> oper = new List<char>();
                    int lastIndex = 0;
                    int startIndex = 0;
                    int Index = 0;
                    string sub;
                    int bracket = 0;
                    bool success = false;

                    while ((Index = str.IndexOfAny(item.ToCharArray(), startIndex)) != -1)
                    {
                        sub = str.Substring(startIndex, Index - startIndex);

                        //괄호가 있으면
                        bracket += sub.Count(e => e == '(');
                        bracket -= sub.Count(e => e == ')');

                        if (bracket == 0)
                        {
                            subjects.Add(str.Substring(lastIndex, Index - lastIndex));
                            oper.Add(str[Index]);
                            success = true;

                            lastIndex = Index + 1;
                        }
                        startIndex = Index + 1;
                    }
                    sub = str.Substring(startIndex, str.Length - startIndex);
                    //괄호가 있으면
                    bracket += sub.Count(e => e == '(');
                    bracket -= sub.Count(e => e == ')');

                    if (bracket == 0)
                    {
                        subjects.Add(str.Substring(lastIndex, str.Length - lastIndex));
                    }
                    else
                    {
                        return false;
                    }

                    if (success)
                    {
                        return subjects.All((st) =>
                        {
                            return Block.IsBlock(st);
                        });
                    }

                }
            }

            if (str.EndsWith(")"))
            {
                string st = str.ToLower();

                if (st.StartsWith("("))
                {
                    return /*BracketBlock.Create(*/Block.IsBlock(str.Substring(1, str.Length - 2))/*)*/;
                }
                else
                {
                    Dictionary<FunctionType, string> funcName = new Dictionary<FunctionType, string>() {
                        { FunctionType.Sin ,"sin"} ,
                        { FunctionType.Cos ,"cos"} ,
                        { FunctionType.Tan ,"tan"} ,
                        { FunctionType.Root ,"root"} ,
                    };
                    foreach (var item in funcName)
                    {
                        if (st.StartsWith(item.Value + "("))
                        {
                            string a = str.Substring(item.Value.Length + 1, str.Length - (2 + item.Value.Length));
                            FunctionType type = item.Key;
                            return Block.IsBlock(a);
                        }
                    }
                }
                return false;

            }

            if (char.IsLetter(str[0]) && !str.Any(e => char.IsWhiteSpace(e)))
            {
                return true;
            }

            return double.TryParse(str, out _);
        }

        public static Block Parse(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            str = str.Trim();

            foreach (var item in TwoOperatorBlock.priority)
            {
                if (str.IndexOfAny(item.ToCharArray()) != -1)
                {
                    List<string> subjects = new List<string>();
                    List<char> oper = new List<char>();
                    int lastIndex = 0;
                    int startIndex = 0;
                    int Index = 0;
                    string sub;
                    int bracket = 0;
                    bool success = false;

                    while ((Index = str.IndexOfAny(item.ToCharArray(), startIndex)) != -1)
                    {
                        sub = str.Substring(startIndex, Index - startIndex);

                        //괄호가 있으면
                        bracket += sub.Count(e => e == '(');
                        bracket -= sub.Count(e => e == ')');

                        if (bracket == 0)
                        {
                            subjects.Add(str.Substring(lastIndex, Index - lastIndex));
                            oper.Add(str[Index]);
                            success = true;

                            lastIndex = Index + 1;
                        }
                        startIndex = Index + 1;
                    }
                    sub = str.Substring(startIndex, str.Length - startIndex);
                    //괄호가 있으면
                    bracket += sub.Count(e => e == '(');
                    bracket -= sub.Count(e => e == ')');

                    if (bracket == 0)
                    {
                        subjects.Add(str.Substring(lastIndex, str.Length - lastIndex));
                    }
                    else
                    {
                        throw new ArgumentException();
                    }

                    if (success)
                    {
                        Block block = Parse(subjects[0]);
                        for (int i = 0; i < oper.Count; i++)
                        {
                            char op = oper[i];
                            block = ParseConcat(block, Parse(subjects[i + 1]), op);
                        }
                        return block;
                    }

                }
            }

            if (str.EndsWith(")"))
            {
                string st = str.ToLower();

                if (st.StartsWith("("))
                {
                    return /*BracketBlock.Create(*/Block.Parse(str.Substring(1, str.Length - 2))/*)*/;
                }
                else
                {
                    Dictionary<FunctionType, string> funcName = new Dictionary<FunctionType, string>() {
                        { FunctionType.Sin ,"sin"} ,
                        { FunctionType.Cos ,"cos"} ,
                        { FunctionType.Tan ,"tan"} ,
                        { FunctionType.Root ,"root"} ,
                    };
                    foreach (var item in funcName)
                    {
                        if (st.StartsWith(item.Value + "("))
                        {
                            string a = str.Substring(item.Value.Length + 1, str.Length - (2 + item.Value.Length));
                            FunctionType type = item.Key;
                            return FunctionBlock.Create(Parse(a), type);
                        }
                    }
                }
                throw new ArgumentException();

            }

            if (char.IsLetter(str[0]) && !str.Any(e => char.IsWhiteSpace(e)))
            {
                return UnknownBlock.Create(str);
            }

            return ConstBlock.Create(Convert.ToDouble(str));
        }


        public abstract bool IsConst();
        public abstract bool IsOperator();
        public abstract bool IsBracket();
        public abstract bool IsValid();

        public double Calculate()
        {
            return Calculate(null);
        }
        public double Calculate(double x)
        {
            return Calculate(new Dictionary<string, double>() { { "x", x } });
        }
        public double Calculate(double x, double y)
        {
            return Calculate(new Dictionary<string, double>() { { "x", x }, { "y", y } });
        }
        public double Calculate(double x, double y, double z)
        {
            return Calculate(new Dictionary<string, double>() { { "x", x }, { "y", y }, { "z", z } });
        }
        public abstract double Calculate(Dictionary<string, double> unknown);
        public Block Reduce()
        {
            return Reduce(null);
        }
        public Block Reduce(double x)
        {
            return Reduce(new Dictionary<string, double>() { { "x", x } });
        }
        public Block Reduce(double x, double y)
        {
            return Reduce(new Dictionary<string, double>() { { "x", x }, { "y", y } });
        }
        public Block Reduce(double x, double y, double z)
        {
            return Reduce(new Dictionary<string, double>() { { "x", x }, { "y", y }, { "z", z } });
        }
        public abstract Block Reduce(Dictionary<string, double> unknown);
        public abstract bool OperatorLowPriority(int p);
        public abstract bool OperatorLowOrEqualPriority(int p);
        public abstract Block Clone();

    }

    class ConstBlock : Block
    {
        public readonly double c;

        private ConstBlock(double c)
        {
            this.c = c;
        }

        internal static Block Create(double c)
        {
            return new ConstBlock(c);
        }

        public override double Calculate(Dictionary<string, double> unknown)
        {
            return c;
        }

        public override Block Clone()
        {
            return Create(c);
        }


        public override bool IsConst()
        {
            return true;
        }

        public override bool IsOperator()
        {
            return false;
        }
        public override bool IsBracket()
        {
            return false;
        }

        public override bool OperatorLowOrEqualPriority(int p)
        {

            return c < 0 ? OperatorBlock.GetPriority(SignType.Minus) <= p : false;
        }

        public override bool OperatorLowPriority(int p)
        {
            return c < 0 ? OperatorBlock.GetPriority(SignType.Minus) < p : false;
        }

        public override Block Reduce(Dictionary<string, double> unknown)
        {
            return this;
        }

        public override string ToString()
        {
            return c.ToString();
        }

        public override bool IsValid()
        {
            return true;
        }
    }
    class BracketBlock : Block
    {
        public readonly Block a;

        private BracketBlock(Block a)
        {
            this.a = a;
        }

        internal static Block Create(Block a)
        {
            return new BracketBlock(a);
        }

        public override double Calculate(Dictionary<string, double> unknown)
        {
            return a.Calculate(unknown);
        }

        public override Block Clone()
        {
            return Create(a.Clone());
        }


        public override bool IsConst()
        {
            return false;
        }

        public override bool IsOperator()
        {
            return false;
        }
        public override bool IsBracket()
        {
            return true;
        }

        public override bool OperatorLowOrEqualPriority(int p)
        {
            return false;
        }

        public override bool OperatorLowPriority(int p)
        {
            return false;
        }

        public override Block Reduce(Dictionary<string, double> unknown)
        {
            return a.Reduce(unknown);
        }

        public override string ToString()
        {
            return $"({a.ToString()})";
        }

        public override bool IsValid()
        {
            return a.IsValid();
        }
    }
    abstract class OperatorBlock : Block
    {
        internal static bool IsSequence(TwoOperatorType v)
        {
            switch (v)
            {
                case TwoOperatorType.Minus:
                case TwoOperatorType.Divide:
                case TwoOperatorType.Pow:
                    return true;
                case TwoOperatorType.Plus:
                case TwoOperatorType.Multiply:
                    return false;
                default:
                    throw new NotImplementedException();
            }
        }
        internal static bool IsSequence(SignType v)
        {
            switch (v)
            {
                case SignType.Plus:
                case SignType.Minus:
                    return true;
                default:
                    throw new NotImplementedException();
            }
        }
        internal static int GetPriority(TwoOperatorType v)
        {
            switch (v)
            {
                case TwoOperatorType.Plus:
                case TwoOperatorType.Minus:
                    return 1;
                case TwoOperatorType.Multiply:
                case TwoOperatorType.Divide:
                    return 2;
                case TwoOperatorType.Pow:
                    return 3;
                default:
                    throw new NotImplementedException();
            }
        }
        internal static int GetPriority(SignType v)
        {
            switch (v)
            {
                case SignType.Plus:
                case SignType.Minus:
                    return 0;
                default:
                    throw new NotImplementedException();
            }
        }
        internal static TwoOperatorType ToTwoOperatorType(char v)
        {
            switch (v)
            {
                case '+':
                    return TwoOperatorType.Plus;
                case '-':
                    return TwoOperatorType.Minus;
                case '*':
                    return TwoOperatorType.Multiply;
                case '/':
                    return TwoOperatorType.Divide;
                case '^':
                    return TwoOperatorType.Pow;
                default:
                    throw new ArgumentException();
            }
        }
        internal static SignType ToSignType(char v)
        {
            switch (v)
            {
                case '+':
                    return SignType.Plus;
                case '-':
                    return SignType.Minus;
                default:
                    throw new ArgumentException();
            }
        }

        internal static char ToChar(TwoOperatorType v)
        {
            switch (v)
            {
                case TwoOperatorType.Plus:
                    return '+';
                case TwoOperatorType.Minus:
                    return '-';
                case TwoOperatorType.Multiply:
                    return '*';
                case TwoOperatorType.Divide:
                    return '/';
                case TwoOperatorType.Pow:
                    return '^';
                default:
                    throw new NotImplementedException();
            }
        }
        internal static char ToChar(SignType v)
        {
            switch (v)
            {
                case SignType.Plus:
                    return '+';
                case SignType.Minus:
                    return '-';
                default:
                    throw new NotImplementedException();
            }
        }
    }

    enum TwoOperatorType
    {
        Plus,
        Minus,
        Multiply,
        Divide,
        Pow,
    }
    class TwoOperatorBlock : OperatorBlock
    {
        public static readonly IReadOnlyList<string> priority = new List<string>() {
            "+-",
            "*/",
            "^",
        };

        public readonly Block a;
        public readonly Block b;
        public readonly TwoOperatorType type;

        private TwoOperatorBlock(Block a, Block b, TwoOperatorType type)
        {
            this.a = a;
            this.b = b;
            this.type = type;
        }

        internal static Block Create(Block a, Block b, TwoOperatorType type)
        {
            return new TwoOperatorBlock(a, b, type);
        }

        public override double Calculate(Dictionary<string, double> unknown)
        {
            switch (type)
            {
                case TwoOperatorType.Plus:
                    return a.Calculate(unknown) + b.Calculate(unknown);
                case TwoOperatorType.Minus:
                    return a.Calculate(unknown) - b.Calculate(unknown);
                case TwoOperatorType.Multiply:
                    return a.Calculate(unknown) * b.Calculate(unknown);
                case TwoOperatorType.Divide:
                    return a.Calculate(unknown) / b.Calculate(unknown);
                case TwoOperatorType.Pow:
                    return Math.Pow(a.Calculate(unknown), b.Calculate(unknown));
                default:
                    throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return ToString(
                a.OperatorLowPriority(GetPriority(type)) ? $"({a.ToString()})" : a.ToString(),
                (IsSequence(type) ? b.OperatorLowOrEqualPriority(GetPriority(type)) : b.OperatorLowPriority(GetPriority(type))) ? $"({b.ToString()})" : b.ToString(), type);

        }
        internal static string ToString(string str1, string str2, TwoOperatorType v)
        {
            switch (v)
            {
                case TwoOperatorType.Plus:
                case TwoOperatorType.Minus:
                    return $"{str1} {ToChar(v)} {str2}";
                case TwoOperatorType.Multiply:
                case TwoOperatorType.Divide:
                case TwoOperatorType.Pow:
                    return $"{str1}{ToChar(v)}{str2}";
                default:
                    throw new NotImplementedException();
            }
        }

        public override Block Clone()
        {
            return Create(a.Clone(), b.Clone(), type);
        }

        public override bool OperatorLowPriority(int p)
        {
            return GetPriority(type) < p;
        }

        public override bool OperatorLowOrEqualPriority(int p)
        {
            return GetPriority(type) <= p;
        }

        public override Block Reduce(Dictionary<string, double> unknown)
        {
            var blockA = a.Reduce(unknown);
            var blockB = b.Reduce(unknown);
            if (blockA.IsConst() && blockB.IsConst())
            {
                return ConstBlock.Create(Calculate(unknown));
            }
            return Create(blockA, blockB, type);
        }

        public override bool IsConst()
        {
            return false;
        }

        public override bool IsOperator()
        {
            return true;
        }

        public override bool IsBracket()
        {
            return false;
        }

        public override bool IsValid()
        {
            return a.IsValid() && b.IsValid();
        }
    }

    enum FunctionType
    {
        Sin,
        Cos,
        Tan,
        Root,
    }

    class FunctionBlock : Block
    {
        public readonly Block a;
        public readonly FunctionType type;

        protected FunctionBlock(Block a, FunctionType type)
        {
            this.a = a;
            this.type = type;
        }

        internal static Block Create(Block a, FunctionType type)
        {
            return new FunctionBlock(a, type);
        }

        public override double Calculate(Dictionary<string, double> unknown)
        {
            switch (type)
            {
                case FunctionType.Sin:
                    return Math.Sin(a.Calculate(unknown));
                case FunctionType.Cos:
                    return Math.Cos(a.Calculate(unknown));
                case FunctionType.Tan:
                    return Math.Tan(a.Calculate(unknown));
                case FunctionType.Root:
                    return Math.Sqrt(a.Calculate(unknown));
                default:
                    throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return ToString(a.ToString(), type);
        }

        internal static string ToString(string str, FunctionType v)
        {
            switch (v)
            {
                case FunctionType.Sin:
                    return $"Sin({str})";
                case FunctionType.Cos:
                    return $"Cos({str})";
                case FunctionType.Tan:
                    return $"Tan({str})";
                case FunctionType.Root:
                    return $"Root({str})";
                default:
                    throw new NotImplementedException();
            }
        }

        public override Block Clone()
        {
            return Create(a.Clone(), type);
        }

        public override bool OperatorLowPriority(int p)
        {
            return false;
        }

        public override bool OperatorLowOrEqualPriority(int p)
        {
            return false;
        }

        public override Block Reduce(Dictionary<string, double> unknown)
        {
            var blockA = a.Reduce(unknown);
            return Create(blockA, type);
        }

        public override bool IsConst()
        {
            return false;
        }

        public override bool IsOperator()
        {
            return true;
        }

        public override bool IsBracket()
        {
            return false;
        }

        public override bool IsValid()
        {
            return a.IsValid();
        }
    }

    enum SignType
    {
        Plus,
        Minus,
    }

    class SignBlock : OperatorBlock
    {
        public readonly Block a;
        public readonly SignType type;

        protected SignBlock(Block a, SignType type)
        {
            this.a = a;
            this.type = type;
        }

        internal static Block Create(Block a, SignType type)
        {
            return new SignBlock(a, type);
        }

        public override double Calculate(Dictionary<string, double> unknown)
        {
            switch (type)
            {
                case SignType.Plus:
                    return +a.Calculate(unknown);
                case SignType.Minus:
                    return -a.Calculate(unknown);
                default:
                    throw new NotImplementedException();
            }
        }


        public override string ToString()
        {
            return ToString((IsSequence(type) ? a.OperatorLowOrEqualPriority(GetPriority(type)) : a.OperatorLowPriority(GetPriority(type)))
                ? $"({a.ToString()})" : a.ToString(), type);
        }
        internal static string ToString(string str, SignType v)
        {
            switch (v)
            {
                case SignType.Plus:
                case SignType.Minus:
                    return $"{ToChar(v)}{str}";
                default:
                    throw new NotImplementedException();
            }
        }

        public override Block Clone()
        {
            return Create(a.Clone(), type);
        }

        public override bool OperatorLowPriority(int p)
        {
            return GetPriority(type) < p;
        }

        public override bool OperatorLowOrEqualPriority(int p)
        {
            return GetPriority(type) <= p;
        }

        public override bool IsConst()
        {
            return false;
        }

        public override bool IsOperator()
        {
            return true;
        }

        public override bool IsBracket()
        {
            return false;
        }

        public override Block Reduce(Dictionary<string, double> unknown)
        {
            var blockA = a.Reduce(unknown);
            if (blockA.IsConst())
            {
                return ConstBlock.Create(Calculate(unknown));
            }
            return Create(blockA, type);
        }

        public override bool IsValid()
        {
            return a.IsValid();
        }
    }

    class UnknownBlock : Block
    {
        public readonly string name;

        private UnknownBlock(string name)
        {
            this.name = name;
        }
        internal static Block Create(string name)
        {
            return new UnknownBlock(name);
        }


        public override double Calculate(Dictionary<string, double> unknown)
        {
            return unknown[name];
        }

        public override Block Clone()
        {
            return Create(name);
        }

        public override bool IsBracket()
        {
            return false;
        }

        public override bool IsConst()
        {
            return false;
        }

        public override bool IsOperator()
        {
            return false;
        }

        public override bool IsValid()
        {
            return true;
        }

        public override bool OperatorLowOrEqualPriority(int p)
        {
            return false;
        }

        public override bool OperatorLowPriority(int p)
        {
            return false;
        }

        public override Block Reduce(Dictionary<string, double> unknown)
        {
            if (unknown != null && unknown.ContainsKey(name))
            {
                return ConstBlock.Create(unknown[name]);
            }
            return this;
        }
        public override string ToString()
        {
            return name;
        }
    }
}
