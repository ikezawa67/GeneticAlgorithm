namespace GeneticAlgorithm
{
    public class Utility
    {
        public static Individual Roulette(List<Individual> population)
        {
            return Roulette(population, population.Select(p => p.Fitness).ToList());
        }

        public static Individual Roulette(List<Individual> population, List<double> weights)
        {
            Random random = new Random(new Random().Next());
            double randomValue = random.NextDouble();
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

        public static Individual Tournamen(List<Individual> population)
        {
            int numberOfParticipants = (int)population.Count / 2;
            Individual[] participants = new Individual[numberOfParticipants];
            for (int i = 0; i < numberOfParticipants; i++)
            {
                participants[i] = Utility.Roulette(population);
            }
            return participants.OrderBy(p => -p.Fitness).First();
        }

        public static Individual Ranking(List<Individual> population)
        {
            population = population.OrderBy(p => p.Fitness).ToList();
            List<double> weights = new List<double>();
            for (int i = 1; i <= population.Count; i++)
            {
                weights.Add(i);
            }
            return Roulette(population, weights);
        }
    }
}