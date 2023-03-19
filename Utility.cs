namespace GeneticAlgorithm
{
    public class Utility
    {
        /// <summary>
        /// ルーレット選択
        /// 個体の適応度に比例した割合でランダムに選択する
        /// </summary>
        /// <param name="population">個体のリスト</param>
        /// <returns>選択した個体</returns>
        public static Individual Roulette(List<Individual> population)
        {
            Random random = new Random(new Random().Next());
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
            Random random = new Random(new Random().Next());
            int numberOfParticipants = (int)population.Count / 2;
            Individual[] participants = new Individual[numberOfParticipants];
            for (int i = 0; i < numberOfParticipants; i++)
            {
                participants[i] = population[random.Next(population.Count)];
            }
            return participants.OrderBy(p => -p.Fitness).First();
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
            Random random = new Random(new Random().Next());
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