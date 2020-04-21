using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 解析表达式
{
    public class ExpressionDecoder
    {
        //1+2+3*5+7-6*7+5
        public double Decode(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                throw new ArgumentNullException("表达式不能为空");

            string expressionWithoutBlank = expression.Replace(" ", "");

            return DecodeWithoutBlank(expressionWithoutBlank);

        }
        private double DecodeWithoutBlank(string expression)
        {
            char operation = FindPriorityOperation(expression);
            string calculateUnitStr;
            string newExpression;
            double result;
            switch (operation)
            {
                case '+':
                    //string calculateUnitStr;
                    if (!FindCalculateUnit(expression, '+', out calculateUnitStr))
                    {
                        throw new Exception($"在表达式{expression}中没有找到包含+的计算单元");
                    }
                    if (!IsCalculateUnit(calculateUnitStr, '+', out result))
                    {
                        throw new Exception($"无法计算{calculateUnitStr}的值");
                    }
                    //计算完之后，把新的result替换进去
                    newExpression = ReplaceOne(expression, calculateUnitStr, result.ToString());
                    return DecodeWithoutBlank(newExpression);
                case '-':

                    if (!FindCalculateUnit(expression, '-', out calculateUnitStr))
                    {
                        throw new Exception($"在表达式{expression}中没有找到包含-的计算单元");
                    }

                    if (!IsCalculateUnit(calculateUnitStr, '-', out result))
                    {
                        throw new Exception($"无法计算{calculateUnitStr}的值");
                    }
                    //计算完之后，把新的result替换进去
                    newExpression = ReplaceOne(expression, calculateUnitStr, result.ToString());
                    return DecodeWithoutBlank(newExpression);
                case '*':
                    //string calculateUnitStr;
                    if (!FindCalculateUnit(expression, '*', out calculateUnitStr))
                    {
                        throw new Exception($"在表达式{expression}中没有找到包含*的计算单元");
                    }

                    if (!IsCalculateUnit(calculateUnitStr, '*', out result))
                    {
                        throw new Exception($"无法计算{calculateUnitStr}的值");
                    }
                    //计算完之后，把新的result替换进去
                    newExpression = ReplaceOne(expression, calculateUnitStr, result.ToString());
                    return DecodeWithoutBlank(newExpression);
                case '/':
                    //string calculateUnitStr;
                    if (!FindCalculateUnit(expression, '/', out calculateUnitStr))
                    {
                        throw new Exception($"在表达式{expression}中没有找到包含/的计算单元");
                    }

                    if (!IsCalculateUnit(calculateUnitStr, '/', out result))
                    {
                        throw new Exception($"无法计算{calculateUnitStr}的值");
                    }
                    //计算完之后，把新的result替换进去
                    newExpression = ReplaceOne(expression, calculateUnitStr, result.ToString());
                    return DecodeWithoutBlank(newExpression);
                default:
                    if (double.TryParse(expression, out result))
                    {
                        return result;
                    }
                    else
                    {
                        throw new Exception($"表达式{expression}不包含任何运算符");
                    }
                    break;
            }

        }

        /// <summary>
        /// 判断一个表达式是否是有效的计算单元，例如"5*2"    "5+4"是有效的计算单元 并计算其值
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool IsCalculateUnit(string expression, char op, out double result)
        {
            result = 0;
            if (string.IsNullOrEmpty(expression))
                return false;
            var strs = expression.Split(new char[] { op });
            if (strs.Length != 2)
            {
                return false;
            }
            var a = strs[0];

            var b = strs[1];
            double num1, num2;
            //a 或者 b不是数字时
            if (!double.TryParse(a.ToString(), out num1) || !double.TryParse(b.ToString(), out num2))
            {
                return false;
            }
            if (op == '+')
            {
                result = num1 + num2;
                return true;
            }
            else if (op == '-')
            {
                result = num1 - num2;
                return true;
            }
            else if (op == '*')
            {
                result = num1 * num2;
                return true;
            }
            else if (op == '/')
            {
                result = num1 / num2;
                return true;
            }
            return false;
        }
        private bool FindCalculateUnit(string expression, char operation, out string calcUnitString)
        {
            calcUnitString = "";
            int index_left = -1;
            int index_right = -1;
            if (!GetLeftNumber(expression, operation, out index_left))
            {
                return false;
            }
            if (!GetRightNumber(expression, operation, out index_right))
            {
                return false;
            }
            calcUnitString = expression.Substring(index_left, index_right - index_left + 1);
            return true;
        }
        /// <summary>
        /// 获取运算符左边的数字，可能是1位可能是n位，返回左边数字的最开始索引,注意最左边可能是-号
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operation"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetLeftNumber(string expression, char operation, out int index)
        {
            index = -1;
            if (!expression.Contains(operation))
            {
                return false;
            }
            int i = expression.IndexOf(operation,1);//获取运算符的位置
            
            if (i == 0 || i == expression.Length - 1)
            {
                return false;
            }
            int count = 1;
            int num;
            while (i - count > 0)
            {
                if (int.TryParse(expression[i - count].ToString(), out num))
                {
                    if (!int.TryParse(expression[i - count - 1].ToString(), out num) &&
                        expression[i - count - 1] != '.')//下一位不是数字且不是小数点
                    {
                        break;//当当前索引为数字，当前索引的前一个索引不为数字时，结束
                    }
                }
                count++;
            }

            index = i - count;
            if (index == 1 && expression[index - 1] == '-')
            {
                index = index - 1;//注意第0个数可能为的'-'号
            }
            return true;
        }
        /// <summary>
        /// 获取运算符右边的数字，可能是1位可能是n位，返回右边数字的最后的索引
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="operation"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private bool GetRightNumber(string expression, char operation, out int index)
        {
            index = -1;
            if (!expression.Contains(operation))
            {
                return false;
            }
            int i = expression.IndexOf(operation,1);//获取运算符的位置
            if (i == 0 || i == expression.Length - 1)
            {
                return false;
            }
            int count = 1;
            int num;
            while (i + count < expression.Length - 1)
            {
                if (int.TryParse(expression[i + count].ToString(), out num))
                {
                    if (!int.TryParse(expression[i + count + 1].ToString(), out num)
                        && expression[i + count + 1] != '.') //下一位不是数字且不是小数点
                    {
                        break;//当当前索引为数字，当前索引的前一个索引不为数字时，结束
                    }
                }
                count++;
            }
            index = i + count;
            return true;
        }
        /// <summary>
        /// 用newStr替换目标中出现的oldstr一次
        /// </summary>
        /// <param name="target"></param>
        /// <param name="oldStr"></param>
        /// <param name="newStr"></param>
        /// <returns></returns>
        private string ReplaceOne(string target, string oldStr, string newStr)
        {
            if (!target.Contains(oldStr))
            {
                return target;
            }
            int index = target.IndexOf(oldStr);
            string firstStr = target.Substring(0, index);
            string lastStr = target.Substring(index + oldStr.Length);

            return firstStr + newStr + lastStr;
        }
        /// <summary>
        /// 寻找优先级最高的运算符，没找到返回o
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private char FindPriorityOperation(string target)
        {
            if (string.IsNullOrEmpty(target))
                throw new ArgumentNullException("参数不能为空");

            if (target.Contains('*') | target.Contains('/'))
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (i != 0 && (target[i] == '*' || target[i] == '/'))
                    {
                        return target[i];
                    }
                }
                return 'o';//other
            }
            else
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (i != 0 && (target[i] == '+' || target[i] == '-'))
                    {
                        return target[i];
                    }
                }
                return 'o';//other
            }
        }
    }
}
