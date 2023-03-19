using Microsoft.CodeAnalysis.Scripting;

namespace GeneticAlgorithm
{
    public class Individual
    {
        private Gene[] genes;

        private Script<double> fitness;

        public int Length { get; private set; }
        
        public int Count { get; private set; }

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

        public double Fitness
        {
            get
            {
                ScriptState<double>? result = this.fitness.RunAsync(new Globals(this.Numbers)).Result;
                return result.ReturnValue;
            }
        }

        public Individual(int length, int count, Script<double> fitness)
        {
            this.genes = new Gene[count];
            for (int i = 0; i < count; i++)
            {
                this.genes[i] = new Gene(length);
            }
            this.Length = length;
            this.Count = count;
            this.fitness = fitness;
        }

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

        public void Mutate()
        {
            foreach (Gene gene in this.genes)
            {
                gene.Mutate();
            }
        }

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