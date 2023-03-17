using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace GeneticAlgorithm
{
    public class Globals
    {
        public double[] args;

        public Globals(double[] args)
        {
            this.args = args;
        }
    }

    class Program
    {

        public static void Main()
        {
            Script<double> script = CSharpScript.Create<double>(@"
                return args[0] * args[1];
            ", globalsType: typeof(Globals));
            Globals globals = new Globals(new double[] { 2, 2 });
            var result = script.RunAsync(globals).Result;
            Console.Write(result.ReturnValue);
            // GeneticAlgorithm ga = new GeneticAlgorithm(100, 10, 2, fitness, SelectionMethod.Ranking, CrossoverMethod.Uniform, 0.1, 0.01, 2);
            // ga.Execution(1000);
        }
    }
}