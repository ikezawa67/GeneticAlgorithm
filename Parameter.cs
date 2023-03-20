namespace GeneticAlgorithm
{
    /// <summary>
    /// 遺伝的アルゴリズムパラメータークラス
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// 個体の総数
        /// </summary>
        public int numberOfIndividuals { get; set; } = 100;

        /// <summary>
        /// 遺伝子の長さ
        /// </summary>
        public int length { get; set; } = 10;

        /// <summary>
        /// 適応度関数で使用する引数配列の数
        /// </summary>
        public int count { get; set; } = 2;

        /// <summary>
        /// 適応度関数を記述した文字列
        /// </summary>
        public string fitness { get; set; } = "return args[0] * args[1];";

        /// <summary>
        /// 選択方法
        /// </summary>
        public SelectionMethod selectionMethod { get; set; } = SelectionMethod.Roulette;

        /// <summary>
        /// 交叉方法
        /// </summary>
        public CrossoverMethod crossoverMethod { get; set; } = CrossoverMethod.OnePoint;

        /// <summary>
        /// 交叉率
        /// </summary>
        public double crossoverProbability { get; set; } = 0.1;

        /// <summary>
        /// 突然変異率
        /// </summary>
        public double mutationProbability { get; set; } = 0.01;

        /// <summary>
        /// エリート選択数
        /// </summary>
        public int eliteNumber { get; set; } = 2;

        /// <summary>
        /// 実行世代数
        /// </summary>
        public int numberOfExecutionGenerations { get; set; } = 1000;
    }
}