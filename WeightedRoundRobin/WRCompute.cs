using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedRoundRobin
{
    /// <summary>
    /// 此算法原理是利用对象权重的最大公约数，构建成等差数列循环
    /// </summary>
    class WRCompute
    {
        public static void Test()
        {
            var wsList = new List<WRModel>
            {
                new WRModel { Name="a",Weight=1 },
                new WRModel { Name = "b", Weight = 2 },
                new WRModel { Name = "c", Weight = 4 }
            };
            Run(wsList);
        }
        private static void Run(List<WRModel> ws)
        {
            Console.Write($"server is");
            foreach (var item in ws)
            {
                Console.Write($" {item.Name}({item.Weight})");
            }
            Console.WriteLine();

            Console.Write($"sequence is");
            var rList = Execute(ws);
            foreach (var item in rList)
            {
                Console.Write($" {item}");
            }
            Console.WriteLine();
        }
        private static List<string> Execute(List<WRModel> ws)
        {
            var result = new List<string>();
            int maxWeight = GetMaxWeight(ws);
            int sumWeight = GetSumWeight(ws);
            int gcd = GetGCD(ws);

            int server_Index = -1;
            int current_Weight = 0;
            for (int i = 0; i < sumWeight; i++)
            {
                if (server_Index < 0)
                {
                    //进入循环
                    server_Index = 0;
                    current_Weight = maxWeight;
                }
                else
                {
                    //继续循环
                    server_Index++;
                }

                //依据权重计算集合中的值
                while (true)
                {
                    if (server_Index >= ws.Count)
                    {
                        server_Index = 0;
                        current_Weight -= gcd;
                    }

                    if (ws[server_Index].Weight >= current_Weight)
                    {
                        //保存结果
                        result.Add($"{ws[server_Index].Name}({ws[server_Index].Weight})");
                        break;
                    }
                    else
                    {
                        if (current_Weight == 0)
                        {
                            //结束本轮
                            break;
                        }
                    }

                    server_Index++;
                }
            }
            return result;
        }
        private static int GetGCD(List<WRModel> ws)
        {
            /*
             * 辗转相除法（欧几里得算法）
             * 1.求两个自然数的最大公约数，以除数和余数反复做除法运算，当余数为 0 时，取当前算式除数为最大公约数。
             * 2.用辗转相除法求几个数的最大公约数，可以先求出其中任意两个数的最大公约数，再求这个最大公约数与第三个数的最大公约数，依次求下去，直到最后一个数为止。
             *   最后所得的那个最大公约数，就是所有这些数的最大公约数。
             */
            int dividendNum = ws[0].Weight;
            for (int i = 1; i < ws.Count; i++)
            {
                int divisorNum = ws[i].Weight;
                dividendNum = CalcGCD(dividendNum, divisorNum);
            }
            return dividendNum;
        }
        private static int CalcGCD(int dividendNum, int divisorNum)
        {
            int remainderNum = dividendNum % divisorNum;
            while (remainderNum != 0)
            {
                dividendNum = divisorNum;
                divisorNum = remainderNum;
                remainderNum = dividendNum % divisorNum;
            }
            return divisorNum;
        }
        private static int GetSumWeight(List<WRModel> ws)
        {
            var result = 0;
            foreach (var item in ws)
            {
                result += item.Weight;
            }
            return result;
        }
        private static int GetMaxWeight(List<WRModel> ws)
        {
            var result = 0;
            foreach (var item in ws)
            {
                if (item.Weight > result)
                {
                    result = item.Weight;
                }
            }
            return result;
        }
    }
}
