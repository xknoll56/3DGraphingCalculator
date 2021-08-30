using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKCalculator
{
    class Interpreter
    {
        private delegate double Function1d(double x);
        private delegate double Function2d(double x, double y);


        private Tokenizer tokenizer;
        private readonly Dictionary<char, int> operatorPrecidence;
        private readonly Dictionary<string, int> functionArgs;
        private readonly Dictionary<string, Function1d> function1dDict;
        private readonly Dictionary<string, Function2d> function2dDict;
        Exception parseException = null;
        Token openBrack;
        Token closedBrack;



        public Interpreter()
        {
            tokenizer = new Tokenizer();
            operatorPrecidence = new Dictionary<char, int>();
            operatorPrecidence.Add('+', 0);
            operatorPrecidence.Add('-', 0);
            operatorPrecidence.Add('/', 1);
            operatorPrecidence.Add('*', 1);
            operatorPrecidence.Add('÷', 1);
            operatorPrecidence.Add('x', 1);
            operatorPrecidence.Add('^', 2);
            operatorPrecidence.Add('(', 3);
            operatorPrecidence.Add(')', 3);
            functionArgs = new Dictionary<string, int>();
            functionArgs.Add("Sin", 1);
            functionArgs.Add("Cos", 1);
            functionArgs.Add("Tan", 1);
            functionArgs.Add("Ln", 1);
            functionArgs.Add("Log2", 1);
            functionArgs.Add("Abs", 1);
            functionArgs.Add("Min", 2);
            functionArgs.Add("Max", 2);
            function1dDict = new Dictionary<string, Function1d>();
            Function1d sinDel = Math.Sin; 
            Function1d cosDel = Math.Cos; 
            Function1d tanDel = Math.Tan;
            function1dDict.Add("Sin", sinDel);
            function1dDict.Add("Cos", cosDel);
            function1dDict.Add("Tan", tanDel);
            //functionDict.Add("Ln", 1);
            //functionDict.Add("Log2", 1);
            //functionDict.Add("Abs", 1);
            //functionDict.Add("Min", 2);
            //functionDict.Add("Max", 2);
            openBrack = new Token('(');
            closedBrack = new Token(')');
        }

        private bool FindOpenBrackets(Token tok)
        {
            if (tok.type == TokenType.OPERATOR)
                return tok.GetData<char>() == '(';
            else
                return false;
        }

        private bool FindClosedBrackets(Token tok)
        {
            if (tok.type == TokenType.OPERATOR)
                return tok.GetData<char>() == ')';
            else
                return false;
        }

        private void ReplaceAllVariables(List<Token> tokens, double x, double y)
        {
            for(int i = 0; i<tokens.Count; i++)
            {
                if (tokens[i].type == TokenType.VARIABLE)
                {
                    if (Char.ToUpper(tokens[i].GetData<char>()) == 'X')
                        tokens[i] = new Token(x);
                    else if (Char.ToUpper(tokens[i].GetData<char>()) == 'Y')
                        tokens[i] = new Token(y);
                }
            }
        }

        public async Task<float> EvaluateExpression(string expression)
        {
            tokenizer.TokenizeExpression(expression);
            List<Token> tokens = tokenizer.tokens;
            parseException = null;


            if(tokens.FindAll(new Predicate<Token>(FindOpenBrackets)).Count != tokens.FindAll(new Predicate<Token>(FindClosedBrackets)).Count)
            {
                parseException = new Exception("Invalid bracketing");
                return float.NaN;
            }
            return (float)EvaluateExpressionRecursive(tokens.ToArray());

        }

        public async Task<float> EvaluateExpression(string expression, double x, double y)
        {
            tokenizer.TokenizeExpression(expression);
            List<Token> tokens = tokenizer.tokens;
            ReplaceAllVariables(tokens, x, y);
            parseException = null;

            if (tokens.FindAll(new Predicate<Token>(FindOpenBrackets)).Count != tokens.FindAll(new Predicate<Token>(FindClosedBrackets)).Count)
            {
                parseException = new Exception("Invalid bracketing");
                return float.NaN;
            }
            return (float)EvaluateExpressionRecursive(tokens.ToArray());

        }

        public double EvaluateExpressionRecursive(Token[] tokens)
        {
            Stack<Token> operatorStack = new Stack<Token>();
            Stack<Token> operandStack = new Stack<Token>();
            uint index = 0;
            Token tok;
            for (int i = 0; i < tokens.Length; i++)
            {
                tok = tokens[i];
                switch (tok.type)
                {
                    case TokenType.OPERATOR:
                        Token op;
                        if (operatorStack.TryPeek(out op))
                        {
                            char curOp = tok.GetData<char>();
                            if (curOp == '(')
                            {
                                tokens = EvaluateSubExpression(tokens, tok);
                                if (tokens.Length > 0)
                                {
                                    tok = tokens[i];
                                    operandStack.Push(tok);
                                }
                                break;
                            }
                            else
                            {
                                int curPrec = operatorPrecidence[curOp];
                                char prevOp = op.GetData<char>();
                                int prevPrec = operatorPrecidence[prevOp];
                                if (curPrec < prevPrec)
                                {
                                    //evaluate the operator on the top of the stack
                                    EvaluateTopOfStack(operatorStack, operandStack);
                                    i--;
                                    break;
                                }
                            }
                        }
                        operatorStack.Push(tok);
                        break;
                    case TokenType.NUMBER:
                        operandStack.Push(tok);
                        break;
                    case TokenType.FUNCTION:
                        tokens = EvaluateFunctionSubExpression(tokens, tok);
                        i--;
                        break;
                   
                }
            }

            if (parseException == null)
            {
                Token token;
                while (operatorStack.TryPeek(out token))
                {
                    EvaluateTopOfStack(operatorStack, operandStack);
                }

                return operandStack.Pop().GetData<double>();
            }
            else
                return double.NaN;
        }

        private Token[] EvaluateSubExpression(Token[] tokens, Token openingBracket)
        {
            List<Token> modifiedTokens = new List<Token>();
            List<Token> subExpression = new List<Token>();

            uint c = 0;
            bool s = false;
            bool evaluated = false;
            foreach(Token token in tokens)
            {
                if (!s)
                {
                    if (token.Equals(openingBracket))
                    {
                        s = true;
                        c++;
                    }
                    else
                        modifiedTokens.Add(token);
                }
                else if (!evaluated)
                {
                    if (token.type == TokenType.OPERATOR)
                    {
                        char v = token.GetData<char>();
                        if (v == '(')
                            c++;
                        else if (v == ')')
                            c--;
                    }
                    else if (token.type == TokenType.EOF)
                    {
                        parseException = new Exception("Invalid bracketing");
                        modifiedTokens = new List<Token>();
                        break;
                    }

                    //then the subexpression is found
                    if (c == 0)
                    {
                        modifiedTokens.Add(new Token(EvaluateExpressionRecursive(subExpression.ToArray())));
                        evaluated = true;
                    }
                    else
                        subExpression.Add(token);
                }
                else
                    modifiedTokens.Add(token);
            }
            return modifiedTokens.ToArray();
        }


        private Token[] EvaluateFunctionSubExpression(Token[] tokens, Token functionToken)
        {
            List<Token> modifiedTokens = new List<Token>();
            List<Token> subExpression = new List<Token>();
            string func = functionToken.GetData<string>();
            int numArguments = functionArgs[func];
            List<double> arguments = new List<double>();

            uint c = 0;
            bool s = false;
            bool evaluated = false;
            int argumentsEvaluated = 0;
            for(int i = 0; i<tokens.Length; i++)
            {
                Token token = tokens[i];
                if (!s)
                {
                    if (token.Equals(functionToken))
                    {
                        s = true;
                        c++;
                        i++;
                    }
                    else
                        modifiedTokens.Add(token);
                }
                else if (!evaluated)
                {
                    if (token.type == TokenType.OPERATOR)
                    {
                        char v = token.GetData<char>();
                        if (v == '(')
                            c++;
                        else if (v == ')')
                            c--;
                        else if(v == ',')
                        {
                            arguments.Add(EvaluateExpressionRecursive(subExpression.ToArray()));
                            subExpression.Clear();
                            argumentsEvaluated++;
                            continue;
                        }
                    }
                    else if (token.type == TokenType.EOF)
                    {
                        parseException = new Exception("Invalid bracketing");
                        modifiedTokens = new List<Token>();
                        break;
                    }

                    //then the subexpression is found
                    if (c == 0)
                    {
                        argumentsEvaluated++;
                        if (argumentsEvaluated == numArguments)
                        {
                            arguments.Add(EvaluateExpressionRecursive(subExpression.ToArray()));
                            evaluated = true;
                            modifiedTokens.Add(new Token(EvaluateFunction(arguments, func)));
                        } 
                    }
                    else
                        subExpression.Add(token);
                }
                else
                    modifiedTokens.Add(token);
            }
            return modifiedTokens.ToArray();
        }

        private double EvaluateFunction(List<double> arguments, string func)
        {
            switch(arguments.Count)
            {
                case 1:
                    return function1dDict[func](arguments[0]);
                case 2:
                    return function2dDict[func](arguments[0], arguments[1]);
            }

            parseException = new Exception("Function not found.");
            return 0;
        }

        private void EvaluateTopOfStack(Stack<Token> operatorStack, Stack<Token> operandStack)
        {
            char op = operatorStack.Pop().GetData<char>();
            double rhs = operandStack.Pop().GetData<double>();
            double lhs = operandStack.Pop().GetData<double>();
            double res = 0;
            switch (op)
            {
                case '+':
                    res = lhs + rhs;
                    break;
                case '-':
                    res = lhs - rhs;
                    break;
                case '/':
                    res = lhs / rhs;
                    break;
                case '÷':
                    res = lhs / rhs;
                    break;
                case '*':
                    res = lhs * rhs;
                    break;
                case 'x':
                    res = lhs * rhs;
                    break;
                case '^':
                    res = Math.Pow(lhs, rhs);
                    break;
            }
            operandStack.Push(new Token(res));

        }
    }
}
