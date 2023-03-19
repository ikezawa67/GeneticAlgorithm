using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private Parameter parameter;
        public int CurrentGenerationNumber { get; private set; } = 0;
        private List<Individual> population = new List<Individual>();
        private Random random;

        public Individual MaxIndividual { get => this.population.OrderBy(p => -p.Fitness).First(); }
        public Individual MinIndividual { get => this.population.OrderBy(p => p.Fitness).First(); }
        public double AverageFitness { get => this.population.Select(p => p.Fitness).Average(); }

        public GeneticAlgorithm(Parameter parameter)
        {
            this.parameter = parameter;
            for (int i = 0; i < this.parameter.numberOfIndividuals; i++)
            {                
                Script<double> script = CSharpScript.Create<double>(this.parameter.fitness, globalsType: typeof(Globals));
                this.population.Add(new Individual(this.parameter.length, this.parameter.count, script));
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

        public void Step()
        {
            this.CurrentGenerationNumber += 1;
            List<Individual> nextPopulation = new List<Individual>();
            nextPopulation.AddRange(this.population.OrderBy(p => -p.Fitness).Take(this.parameter.eliteNumber).ToList());
            while (nextPopulation.Count() <= this.parameter.numberOfIndividuals)
            {
                (Individual child1, Individual child2) = Selection(this.parameter.selectionMethod);
                if (this.random.NextDouble() < this.parameter.crossoverProbability)
                {
                    Individual.Crossover(child1, child2, this.parameter.crossoverMethod);
                }
                if (this.random.NextDouble() < this.parameter.mutationProbability)
                {
                    child1.Mutate();
                }
                if (this.random.NextDouble() < this.parameter.mutationProbability)
                {
                    child2.Mutate();
                }
                nextPopulation.Add(child1);
                nextPopulation.Add(child2);
            }
            while (this.parameter.numberOfIndividuals < nextPopulation.Count)
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

        public void Execution(int numberOfExecutionGenerations)
        {
            for (int i = 0; i< numberOfExecutionGenerations;i++)
            {
                Step();
            }
        }
    }
}