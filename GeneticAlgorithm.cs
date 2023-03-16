namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private int NumberOfIndividuals;
        public int CurrentGenerationNumber { get; private set; } = 0;
        private List<Individual> population = new List<Individual>();
        private Random random;

        public Individual MaxIndividual { get => this.population.OrderBy(p => -p.Fitness).First(); }
        public Individual MinIndividual { get => this.population.OrderBy(p => p.Fitness).First(); }
        public double AverageFitness { get => this.population.Select(p => p.Fitness).Average(); }


        public GeneticAlgorithm(int numberOfIndividuals, int length, int count, Func<double[], double> fitness)
        {
            this.NumberOfIndividuals = numberOfIndividuals;
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                this.population.Add(new Individual(length, count, fitness));
            }
            this.random = new Random(new Random().Next());
        }

        private (Individual, Individual) RouletteSelection()
        {
            Individual parent1 = Utility.Roulette(this.population);
            Individual parent2 = Utility.Roulette(this.population);
            return (parent1.DeepCopy(), parent2.DeepCopy());
        }

        private (Individual, Individual) TournamentSelection()
        {
            Individual parent1 = Utility.Tournamen(this.population);
            Individual parent2 = Utility.Tournamen(this.population);
            return (parent1.DeepCopy(), parent2.DeepCopy());
        }

        private (Individual, Individual) RankingSelection()
        {
            Individual parent1 = Utility.Ranking(this.population);
            Individual parent2 = Utility.Ranking(this.population);
            return (parent1.DeepCopy(), parent2.DeepCopy());
        }

        public (Individual, Individual) Selection(SelectionMethod selectionMethod)
        {
            if (selectionMethod == SelectionMethod.Roulette)
            {
                return RouletteSelection();
            }
            else if (selectionMethod == SelectionMethod.Tournament)
            {
                return TournamentSelection();
            }
            else
            {
                return RankingSelection();
            }
        }

        public void Step(SelectionMethod selectionMethod, CrossoverMethod crossoverMethod, double crossoverProbability, double mutationProbability, int eliteNumber)
        {
            this.CurrentGenerationNumber += 1;
            List<Individual> nextPopulation = new List<Individual>();
            nextPopulation.AddRange(this.population.OrderBy(p => -p.Fitness).Take(eliteNumber).ToList());
            while (nextPopulation.Count() <= this.NumberOfIndividuals)
            {
                (Individual child1, Individual child2) = Selection(selectionMethod);
                if (this.random.NextDouble() < crossoverProbability)
                {
                    Individual.Crossover(child1, child2, crossoverMethod);
                }
                if (this.random.NextDouble() < mutationProbability)
                {
                    child1.Mutate();
                }
                if (this.random.NextDouble() < mutationProbability)
                {
                    child2.Mutate();
                }
                nextPopulation.Add(child1);
                nextPopulation.Add(child2);
            }
            while (this.NumberOfIndividuals < nextPopulation.Count)
            {
                nextPopulation.RemoveAt(nextPopulation.Count - 1);
            }
            this.population = nextPopulation;
            Console.WriteLine("Current Generation Number: {0}", this.CurrentGenerationNumber);
            Console.WriteLine("\tMax Numbers: {0}, Max Fitness: {1}", String.Join(", ", this.MaxIndividual.Numbers), this.MaxIndividual.Fitness);
            Console.WriteLine("\tMin Numbers: {0}, Min Fitness: {1}", String.Join(", ", this.MinIndividual.Numbers), this.MinIndividual.Fitness);
            Console.WriteLine("\tAverage: {0}", this.AverageFitness);
            Console.WriteLine();
        }
    }
}