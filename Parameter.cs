namespace GeneticAlgorithm
{
    public class Parameter
    {
        /// <summary>
        /// 個体の総数
        /// </summary>
        /// <value>100</value>
        public int numberOfIndividuals { get; set; } = 100;
        /// <summary>
        /// 遺伝子の長さ
        /// </summary>
        /// <value>10</value>
        public int length { get; set; } = 10;
        /// <summary>
        /// 適応度関数で使用する値の数
        /// </summary>
        /// <value>2</value>
        public int count { get; set; } = 2;
        /// <summary>
        /// 適応度関数を記述した文字列
        /// </summary>
        /// <value>double x = args[0];\ndouble y = args[1];\nreturn x * y;</value>
        public string fitness { get; set; } = "double x = args[0];\ndouble y = args[1];\nreturn x * y;";
        /// <summary>
        /// 選択方法
        /// </summary>
        /// <value>SelectionMethod.Roulette</value>
        public SelectionMethod selectionMethod { get; set; } = SelectionMethod.Roulette;
        /// <summary>
        /// 交叉方法
        /// </summary>
        /// <value>CrossoverMethod.OnePoint</value>
        public CrossoverMethod crossoverMethod { get; set; } = CrossoverMethod.OnePoint;
        /// <summary>
        /// 交叉率
        /// </summary>
        /// <value>0.1</value>
        public double crossoverProbability { get; set; } = 0.1;
        /// <summary>
        /// 突然変異率
        /// </summary>
        /// <value>0.01</value>
        public double mutationProbability { get; set; } = 0.01;
        /// <summary>
        /// エリート選択数
        /// </summary>
        /// <value>2</value>
        public int eliteNumber { get; set; } = 2;
        /// <summary>
        /// 実行世代数
        /// </summary>
        /// <value>1000</value>
        public int numberOfExecutionGenerations { get; set; } = 1000;
    }
}