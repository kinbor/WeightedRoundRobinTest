using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightedRoundRobin
{
    /// <summary>
    /// 此算法取自Nginx负载均衡实现。原理如下：
    /// 1.每个对象都有两个权重变量：
    ///     1.1.weight：配置文件中指定的对象权重
    ///     1.2.weight_current：对象目前的权重，默认为0，之后会动态调整
    /// 2.每当请求到来选取对象时，遍历对象集合中所有对象
    /// 3.针对每个对象weight_current先加上weight，然后取weight_current最大值的对象为结果
    /// 4.total值等于所有对象weight相加之和
    /// 5.重新计算选取的结果对象weight_current值，公式=total-weight_current
    /// 综合上述过程可知，此原理类似于割韭菜，谁长得快就割谁。
    /// </summary>
    class SWRCompute
    {
        public static void Test()
        {
            var wsList = new List<SWRModel>
            {
                new SWRModel { Name="a",Weight=1 },
                new SWRModel { Name = "b", Weight = 1 },
                new SWRModel { Name = "c", Weight = 5 }
            };
            Run(wsList);
        }
        private static void Run(List<SWRModel> ws)
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
        private static List<string> Execute(List<SWRModel> ws)
        {
            var result = new List<string>();

            int sumWeight = GetSumWeight(ws);
            for (int i = 0; i < sumWeight; i++)
            {
                int selectedIndex = -1;
                for (int j = 0; j < ws.Count; j++)
                {
                    ws[j].WeightCurrent += ws[j].Weight;

                    if (selectedIndex == -1 || ws[selectedIndex].WeightCurrent < ws[j].WeightCurrent)
                    {
                        selectedIndex = j;
                    }
                }
                ws[selectedIndex].WeightCurrent -= sumWeight;
                result.Add($"{ws[selectedIndex].Name}({ws[selectedIndex].Weight})");
            }

            return result;
        }
        private static int GetSumWeight(List<SWRModel> ws)
        {
            var result = 0;
            foreach (var item in ws)
            {
                result += item.Weight;
            }
            return result;
        }
    }
}
