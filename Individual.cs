using Microsoft.CodeAnalysis.Scripting;

namespace GeneticAlgorithm
{
    /// <summary>
    /// 遺伝的アルゴリズムの個体クラス
    /// </summary>
    public class Individual
    {
        /// <summary>
        /// 遺伝子情報配列
        /// </summary>
        private Gene[] genes;

        /// <summary>
        /// 適応度を求めるスクリプト
        /// </summary>
        private Script<double> fitness;

        /// <summary>
        /// 遺伝子配列の長さ
        /// </summary>
        public int Length { get => this.genes[0].Length; }

        /// <summary>
        /// 遺伝子情報配列の数
        /// </summary>
        public int Count { get => this.genes.Length; }

        /// <summary>
        /// 遺伝子情報配列を十進数表記に変換した配列（0以上、1以下）
        /// </summary>
        public double[] Numbers
        {
            get
            {
                double[] numbers = new double[this.genes.Length];
                for (int i = 0; i < this.genes.Length; i++)
                {
                    numbers[i] = this.genes[i].Number;
                }
                return numbers;
            }
        }

        /// <summary>
        /// 個体の適応度
        /// </summary>
        public double Fitness { get => this.fitness.RunAsync(new Global(this.Numbers)).Result.ReturnValue; }

        /// <summary>
        /// 渡された遺伝子の長さのな遺伝子配列を遺伝子情報配列の数分生成する
        /// </summary>
        /// <param name="length">遺伝子配列の長さ</param>
        /// <param name="count">遺伝子情報配列の数</param>
        /// <param name="fitness">適応度を求める関数の文字列</param>
        public Individual(int length, int count, Script<double> fitness)
        {
            this.genes = new Gene[count];
            for (int i = 0; i < count; i++)
            {
                this.genes[i] = new Gene(length);
            }
            this.fitness = fitness;
        }

        /// <summary>
        /// 一点交叉メソッド
        /// </summary>
        /// <param name="child1">子の個体1</param>
        /// <param name="child2">子の個体2</param>
        private static void OnePointCrossover(Individual child1, Individual child2)
        {
            Random random = new Random(new Random().Next());
            for (int i = random.Next(child1.Length); i < child1.Length; i++)
            {
                for (int j = 0; j < child1.Count; j++)
                {
                    bool tmp = child1.genes[j][i];
                    child1.genes[j][i] = child2.genes[j][i];
                    child2.genes[j][i] = tmp;
                }
            }
        }

        /// <summary>
        /// 二点交叉メソッド
        /// </summary>
        /// <param name="child1">子の個体1</param>
        /// <param name="child2">子の個体2</param>
        private static void TwoPointCrossover(Individual child1, Individual child2)
        {
            Random random = new Random(new Random().Next());
            int min_index = random.Next(child1.Length);
            int max_index = random.Next(min_index, child1.Length);
            for (int i = min_index; i < max_index; i++)
            {
                for (int j = 0; j < child1.Count; j++)
                {
                    bool tmp = child1.genes[j][i];
                    child1.genes[j][i] = child2.genes[j][i];
                    child2.genes[j][i] = tmp;
                }
            }
        }

        /// <summary>
        /// 一様交差メソッド
        /// </summary>
        /// <param name="child1">子の個体1</param>
        /// <param name="child2">子の個体2</param>
        private static void UniformCrossover(Individual child1, Individual child2)
        {
            Random random = new Random(new Random().Next());
            bool[] mask = new bool[child1.Length];
            for (int i = 0; i < child1.Count; i++)
            {
                if (0.5 < random.NextDouble())
                {
                    for (int j = 0; j < child1.Count; j++)
                    {
                        bool tmp = child1.genes[j][i];
                        child1.genes[j][i] = child2.genes[j][i];
                        child2.genes[j][i] = tmp;
                    }
                }
            }
        }

        /// <summary>
        /// 交叉メソッド
        /// </summary>
        /// <param name="child1">子の個体1</param>
        /// <param name="child2">子の個体2</param>
        /// <param name="crossoverMethod">交叉方法の列挙型</param>
        public static void Crossover(Individual child1, Individual child2, CrossoverMethod crossoverMethod)
        {
            if (crossoverMethod == CrossoverMethod.OnePoint)
            {
                OnePointCrossover(child1, child2);
            }
            else if (crossoverMethod == CrossoverMethod.TwoPoint)
            {
                TwoPointCrossover(child1, child2);
            }
            else
            {
                UniformCrossover(child1, child2);
            }
        }

        /// <summary>
        /// 突然変異メソッド
        /// </summary>
        public void Mutate()
        {
            foreach (Gene gene in this.genes)
            {
                gene.Mutate();
            }
        }

        /// <summary>
        /// 深いコピーメソッド
        /// </summary>
        /// <returns>コピーした遺伝子情報オブジェクト</returns>
        public Individual DeepCopy()
        {
            Individual clone = (Individual)MemberwiseClone();
            Gene[] tmp = new Gene[this.genes.Length];
            for (int i = 0; i < this.genes.Length; i++)
            {
                tmp[i] = this.genes[i].DeepCopy();
            }
            clone.genes = tmp;
            return clone;
        }
    }
}