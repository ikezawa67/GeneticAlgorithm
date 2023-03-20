using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace GeneticAlgorithm
{
    /// <summary>
    /// 
    /// </summary>
    public class GeneticAlgorithm
    {
        /// <summary>
        /// 遺伝的アルゴリズムパラメーター
        /// </summary>
        private Parameter parameter;

        /// <summary>
        /// 現在の世代数
        /// </summary>
        public int CurrentGenerationNumber { get; private set; } = 0;

        /// <summary>
        /// 現在の世代の個体リスト
        /// </summary>
        private List<Individual> population = new List<Individual>();

        /// <summary>
        /// クラス内で使用する疑似乱数ジェネレーター
        /// </summary>
        private Random random = new Random(new Random().Next());

        /// <summary>
        /// 適応度が最も高い個体
        /// </summary>
        public Individual MaxIndividual { get => this.population.OrderBy(p => -p.Fitness).First(); }

        /// <summary>
        /// 適応度が最も低い個体
        /// </summary>
        public Individual MinIndividual { get => this.population.OrderBy(p => p.Fitness).First(); }

        /// <summary>
        /// 現在の世代の適応度の平均値
        /// </summary>
        public double AverageFitness { get => this.population.Select(p => p.Fitness).Average(); }

        public GeneticAlgorithm(Parameter parameter)
        {
            this.parameter = parameter;
            for (int i = 0; i < this.parameter.numberOfIndividuals; i++)
            {
                Script<double> script = CSharpScript.Create<double>(this.parameter.fitness, null, typeof(Global));
                this.population.Add(new Individual(this.parameter.length, this.parameter.count, script));
            }
            try{                
                double tmp = this.population[0].Fitness;
            }  
            catch (Exception ex) when (ex is CompilationErrorException || ex is ArgumentException)
            {
                throw new ("演算不可能な適応度関数が渡されました。");
            }
        }

        /// <summary>
        /// ルーレット選択メソッド
        /// </summary>
        /// <returns>選択した二個の個体の深いコピー</returns>
        private (Individual, Individual) RouletteSelection()
        {
            Task<Individual> task1 = Task.Run(() =>
            {
                Individual tmp = Utility.Roulette(this.population);
                return tmp.DeepCopy();
            });
            Task<Individual> task2 = Task.Run(() =>
            {
                Individual tmp = Utility.Roulette(this.population);
                return tmp.DeepCopy();
            });
            Task.WaitAll(new Task<Individual>[] { task1, task2 });
            return (task1.Result, task2.Result);
        }

        /// <summary>
        /// トーナメント選択メソッド
        /// </summary>
        /// <returns>選択した二個の個体の深いコピー</returns>
        private (Individual, Individual) TournamentSelection()
        {
            Task<Individual> task1 = Task.Run(() =>
            {
                Individual tmp = Utility.Tournamen(this.population);
                return tmp.DeepCopy();
            });
            Task<Individual> task2 = Task.Run(() =>
            {
                Individual tmp = Utility.Tournamen(this.population);
                return tmp.DeepCopy();
            });
            Task.WaitAll(new Task<Individual>[] { task1, task2 });
            return (task1.Result, task2.Result);
        }

        /// <summary>
        /// ランキング選択メソッド
        /// </summary>
        /// <returns>選択した二個の個体の深いコピー</returns>
        private (Individual, Individual) RankingSelection()
        {
            Task<Individual> task1 = Task.Run(() =>
            {
                Individual tmp = Utility.Ranking(this.population);
                return tmp.DeepCopy();
            });
            Task<Individual> task2 = Task.Run(() =>
            {
                Individual tmp = Utility.Ranking(this.population);
                return tmp.DeepCopy();
            });
            Task.WaitAll(new Task<Individual>[] { task1, task2 });
            return (task1.Result, task2.Result);
        }

        /// <summary>
        /// 選択メソッド
        /// </summary>
        /// <param name="selectionMethod">選択方法の列挙型</param>
        /// <returns>選択した二個の個体の深いコピー</returns>
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

        /// <summary>
        /// 次世代を生成するメソッド
        /// </summary>
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
                Task task1 = Task.Run(() =>
                {
                    if (this.random.NextDouble() < this.parameter.mutationProbability)
                    {
                        child1.Mutate();
                    }
                });
                Task task2 = Task.Run(() =>
                {
                    if (this.random.NextDouble() < this.parameter.mutationProbability)
                    {
                        child2.Mutate();
                    }
                });
                Task.WaitAll(new Task[] { task1, task2 });
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

        /// <summary>
        /// 渡された実行する世代数分計算をするメソッド
        /// </summary>
        /// <param name="numberOfExecutionGenerations">実行する世代数</param>
        public void Execution(int numberOfExecutionGenerations)
        {
            for (int i = 0; i < numberOfExecutionGenerations; i++)
            {
                Step();
            }
        }
    }
}