namespace GeneticAlgorithm
{
    class Program
    {
        static double fitness(double[] args)
        {
            double x = args[0];
            double y = args[1];
            return x * y;
        }

        public static void Main()
        {
            GeneticAlgorithm ga = new GeneticAlgorithm(50, 10, 2, fitness, SelectionMethod.Tournament, CrossoverMethod.OnePoint, 0.1, 0.01, 2);
            Console.WriteLine("Max: {0}, Min: {1}", ga.MaxIndividual, ga.MinIndividual);
            for (int i = 1; i <= 100; i++)
            {
                ga.Step();
            }
        }
    }
}