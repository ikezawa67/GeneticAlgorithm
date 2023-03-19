namespace GeneticAlgorithm
{
    public class Parameter
    {
        public int numberOfIndividuals { get; set; } = 100;
        public int length { get; set; } = 10;
        public int count { get; set; } = 2;
        public string fitness { get; set; } = "double x = args[0];\ndouble y = args[1];\nreturn x * y;";
        public SelectionMethod selectionMethod { get; set; } = SelectionMethod.Roulette;
        public CrossoverMethod crossoverMethod { get; set; } = CrossoverMethod.OnePoint;
        public double crossoverProbability { get; set; } = 0.1;
        public double mutationProbability { get; set; } = 0.01;
        public int eliteNumber { get; set; } = 2;
        public int numberOfExecutionGenerations { get; set; } = 1000;
    }
}