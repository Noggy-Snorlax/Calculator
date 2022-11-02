using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {

        bool isOperator(string c)
        {
            if (c == "+" || c == "-" || c == "*" || c == "/" || c == "^")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int precedence(string c)
        {
            if (c == "^")
                return 3;
            else if (c == "*" || c == "/")
                return 2;
            else if (c == "+" || c == "-")
                return 1;
            else
                return -1;
        }

        public double simpleCalculator(double number1, double number2, string operatorSymbol)
        {
            switch (operatorSymbol)
            {

                case "+":
                    return number1 + number2;

                case "-":
                    return number1 - number2;
                case "*":
                    return number1 * number2;
                case "/":
                    return number1 / number2;
                default:
                    Console.WriteLine("Nothing");
                    break;
            }
            return 0;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.answer.Text = "" + Calculate(textBoxInput.Text);
            }catch
            {
                this.answer.Text = "Invalid Input or Have some bug. Sorry";
                this.answer.ForeColor = System.Drawing.Color.Red;
            }

        }

        private ArrayList PrefixList(string prefixText)
        {
            ArrayList ArrayListPrefix = new ArrayList();
            string prefixTrim = prefixText.Replace(" ", "");
            while (!prefixTrim.Equals(""))
            {
                int operatorIndex = prefixTrim.IndexOfAny(new char[] { '+', '-', '/', '*', '(', ')' });
                if (operatorIndex == 0)
                {
                    operatorIndex = operatorIndex + 1;
                }
                if (operatorIndex != -1)
                {
                    ArrayListPrefix.Add(prefixTrim.Substring(0, operatorIndex));
                    prefixTrim = prefixTrim.Remove(0, operatorIndex);
                    if (!prefixTrim.Equals(""))
                    {
                        int operatorIndex2 = prefixTrim.IndexOfAny(new char[] { '+', '-', '/', '*', '(', ')' });
                        if (operatorIndex2 == 0)
                        {
                            operatorIndex2++;
                        }
                        if (operatorIndex2 != -1)
                        {
                            ArrayListPrefix.Add(prefixTrim.Substring(0, operatorIndex2));
                            prefixTrim = prefixTrim.Remove(0, operatorIndex2);
                        }
                    }
                }
                else
                {
                    ArrayListPrefix.Add(prefixTrim.Substring(0, prefixTrim.Length));
                    prefixTrim = prefixTrim.Remove(0, prefixTrim.Length);
                }
            }

            return ArrayListPrefix;
        }

        public string getPostFixFromPostFixArraylist(ArrayList postFixArrayList)
        {
            string PostFixAnswer = "";
            foreach (Object X in postFixArrayList)
            {
                PostFixAnswer += X.ToString() + " ";
            }


            return PostFixAnswer;
        }

        public ArrayList convertPreFixIntoPostFix(ArrayList preFix)
        {
            ArrayList postFixList = new ArrayList();
            Stack<string> stackOperator = new Stack<string>();
            foreach (string item in preFix)
            {
                if (item.Equals("("))
                {
                    stackOperator.Push(item);
                }
                else if (item.Equals(")"))
                {
                    while (!stackOperator.Peek().Equals("("))
                    {
                        postFixList.Add(stackOperator.Pop());
                    }
                    stackOperator.Pop();
                }
                else
                {
                    if (isOperator(item) && stackOperator.Count == 0)
                    {
                        stackOperator.Push(item);
                    }
                    else if (isOperator(item) && stackOperator.Count != 0)
                    {
                        if (stackOperator.Peek().Equals("("))
                        {
                            stackOperator.Push(item);
                        }
                        else
                        {
                            if (precedence(item) < precedence(stackOperator.Peek()))
                            {
                                postFixList.Add(stackOperator.Pop());

                            }
                            if (stackOperator.Count != 0)
                            {
                                if (precedence(item) > precedence(stackOperator.Peek()))
                                {
                                    stackOperator.Push(item);
                                }
                                else if (precedence(item) == precedence(stackOperator.Peek()))
                                {
                                    postFixList.Add(stackOperator.Pop());
                                    stackOperator.Push(item);
                                }
                            }
                            else
                            {
                                stackOperator.Push(item);
                            }
                        }
                    }
                    else
                    {
                        postFixList.Add(item);
                    }
                }
            }
            foreach (string operatorItem in stackOperator)
            {
                postFixList.Add(operatorItem);
            }
            return postFixList;
        }

        public double postFixToAnswer(ArrayList Postfix)
        {
            ArrayList postFixToAnswerTemp = Postfix;

            while (postFixToAnswerTemp.Count > 1)
            {
                for (int count = 0; count < postFixToAnswerTemp.Count; count++)
                {
                    if (isOperator("" + postFixToAnswerTemp[count]))
                    {
                        double number1 = Convert.ToDouble(postFixToAnswerTemp[count - 2]);
                        double number2 = Convert.ToDouble(postFixToAnswerTemp[count - 1]);
                        string operatorSymbol = postFixToAnswerTemp[count].ToString();

                        double tempcal = simpleCalculator(number1, number2, operatorSymbol);
                        postFixToAnswerTemp.RemoveAt(count - 2);
                        postFixToAnswerTemp.RemoveAt(count - 2);
                        postFixToAnswerTemp.RemoveAt(count - 2);
                        postFixToAnswerTemp.Insert(count - 2, tempcal);
                        break;
                    }
                }
            }

            return Convert.ToDouble(postFixToAnswerTemp[0]);
        }

        public double Calculate(string sum)
        {
            this.PostFixValue.Text = "";
            string prefix = sum;
            //string prefix = ("( 11.5 + 15.4 ) + 10.1");
             
            ArrayList arrayListPrefix = PrefixList(prefix);
            ArrayList postFixList = convertPreFixIntoPostFix(arrayListPrefix);
            string postFixAnswer = getPostFixFromPostFixArraylist(postFixList);
            double postFixToAnswerDisplay = postFixToAnswer(postFixList);
            this.PostFixValue.Text = ""+ postFixAnswer;

            return Convert.ToDouble(postFixToAnswerDisplay);
        }

    }
}
