using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTKCalculator
{
    class Interpreter
    {
        private Tokenizer tokenizer;
        private readonly Dictionary<char, int> operatorPrecidence;

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

        public double EvaluateSimpleExpression(string expression)
        {
            tokenizer.TokenizeExpression(expression);
            List<Token> tokens = tokenizer.tokens;
            Stack<Token> operatorStack = new Stack<Token>();
            Stack<Token> operandStack = new Stack<Token>();
            
            foreach(Token tok in tokens)
            {
                switch (tok.type)
                {
                    case TokenType.OPERATOR:
                        Token op;
                        if (operatorStack.TryPeek(out op))
                        {
                            char prevOp = op.GetData<char>();
                            int prevPrec = operatorPrecidence[prevOp];
                            char curOp = tok.GetData<char>();
                            int curPrec = operatorPrecidence[curOp];
                            if(curPrec < prevPrec)
                            {
                                //evaluate the operator on the top of the stack
                                EvaluateTopOfStack(operatorStack, operandStack);
                            }
                        }
                         operatorStack.Push(tok);
                        break;
                    case TokenType.NUMBER:
                        operandStack.Push(tok);
                        break;
                }
            }

            Token token;
            while (operatorStack.TryPeek(out token))
            {
                EvaluateTopOfStack(operatorStack, operandStack);
            }

            return operandStack.Pop().GetData<double>();
        }


        public double EvaluateExpression(String expression)
        {
            //tokenizer.TokenizeExpression(expression);
            //List<Token> tokens = tokenizer.tokens;
            //int tokInd = 0;
            //Token curTok = tokens[tokInd];
            //Stack<Token> operatorStack = new Stack<Token>();
            //Stack<Token> operandStack = new Stack<Token>();

            //while(curTok.type != TokenType.EOF)
            //{
            //    switch(curTok.type)
            //    {
            //        case TokenType.OPERATOR:
            //            char op = curTok.GetData<char>();
            //            if (operatorStack.Count > 0)
            //            {
            //                char top = operatorStack.Peek().GetData<char>();
            //            }

            //        break;
            //        case TokenType.NUMBER:

            //        break;

            //    }
            //}
            return 0;
        }

        private Token peek(int curInd, List<Token> tokens)
        {
            return tokens[curInd + 1];
        }

        public double EvaluateExpression2Vars(String expression, double x, double y)
        {



            return 0;
        }

        public double EvaluateExpression1Var(String expression, double x)
        {
            return 0;
        }

        public double EvaluateExpressionNoVar(String expression)
        {
            return 0;
        }
    }
}
