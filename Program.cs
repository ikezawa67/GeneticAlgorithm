using System.Text.Json;
using System.CommandLine;

namespace GeneticAlgorithm
{
    class Program
    {
        public static int Main(params string[] args)
        {
            Option<string> fileOption = new Option<string>(aliases: new string[] { "--file", "-f" }, description: "遺伝的アルゴリズムのパラメーターを記述したjsonファイル。");
            RootCommand rootCommand = new RootCommand("遺伝的アルゴリズム");
            rootCommand.AddOption(fileOption);
            rootCommand.SetHandler((filePath) =>
            {
                Parameter parameter = new Parameter();
                if (filePath is not null)
                {
                    if (!File.Exists(filePath))
                    {
                        filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath!);
                    }
                    if (File.Exists(filePath))
                    {
                        string json = File.ReadAllText(filePath);
                        Parameter? tmp = JsonSerializer.Deserialize<Parameter>(json);
                        if (tmp is not null)
                        {
                            parameter = tmp;
                        }
                        else
                        {
                            json = JsonSerializer.Serialize(new Parameter(), new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText("parameter.json", json);
                        }
                    }
                    else
                    {
                        string json = JsonSerializer.Serialize(new Parameter(), new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText("parameter.json", json);
                    }
                }
                try
                {
                    GeneticAlgorithm ga = new GeneticAlgorithm(parameter);
                    ga.Execution(parameter.numberOfExecutionGenerations);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            },
            fileOption);
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}