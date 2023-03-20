namespace GeneticAlgorithm
{
    /// <summary>
    /// 遺伝的アルゴリズムユーティリティクラス
    /// </summary>
    public class Utility
    {        
        /// <summary>
        /// クラス内で使用する疑似乱数ジェネレーター
        /// </summary>
        private static Random random = new Random(new Random().Next());

        /// <summary>
        /// ルーレット選択
        /// 個体の適応度に比例した割合でランダムに選択する
        /// </summary>
        /// <param name="population">個体のリスト</param>
        /// <returns>選択した個体</returns>
        public static Individual Roulette(List<Individual> population)
        {
            double randomValue = random.NextDouble();
            List<double> weights = population.Select(p => p.Fitness).ToList();
            double total = weights.Sum();
            for (int i = 0; i < population.Count; i++)
            {
                randomValue -= weights[i] / total;
                if (randomValue < 0)
                {
                    return population[i];
                }
            }
            return population.Last();
        }

        /// <summary>
        /// トーナメント選択
        /// 個体のリストの要素数の半数だけ集団からランダムに個体を取得し、最も適応度の高い個体を選択する
        /// </summary>
        /// <param name="population">個体のリスト</param>
        /// <returns>選択した個体</returns>
        public static Individual Tournamen(List<Individual> population)
        {
            int numberOfParticipants = (int)population.Count / 2;
            Individual result = population[random.Next(population.Count)];
            for (int i = 1; i < numberOfParticipants; i++)
            {
                Individual tmp = population[random.Next(population.Count)];
                if (result.Fitness < tmp.Fitness)
                {
                    result = tmp;
                }
            }
            return result!;
        }

        /// <summary>
        /// ランキング選択
        /// 個体のリストの適応度のランクに比例した割合でランダムに選択する
        /// </summary>
        /// <param name="population">個体のリスト</param>
        /// <returns>選択した個体</returns>
        public static Individual Ranking(List<Individual> population)
        {
            population = population.OrderBy(p => p.Fitness).ToList();
            double randomValue = random.NextDouble();
            List<double> weights = new List<double>();
            for (int i = 1; i <= population.Count; i++)
            {
                weights.Add(i);
            }
            double total = weights.Sum();
            for (int i = 0; i < population.Count; i++)
            {
                randomValue -= weights[i] / total;
                if (randomValue < 0)
                {
                    return population[i];
                }
            }
            return population.Last();
        }
    }
}