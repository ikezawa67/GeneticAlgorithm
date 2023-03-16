namespace GeneticAlgorithm
{
    class Program
    {
        static double fitness(double[] args)
        {
            double x = args[0];
            return x;
        }

        public static void Main()
        {
            GeneticAlgorithm ga = new GeneticAlgorithm(100, 8, 1, fitness);
            Console.WriteLine("Max: {0}, Min: {1}", ga.MaxIndividual, ga.MinIndividual);
            for (int i = 1; i <= 50; i++)
            {
                ga.Step(SelectionMethod.Tournament, CrossoverMethod.OnePoint, 0.1, 0.01, 2);
            }
        }
    }
}