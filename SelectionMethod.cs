namespace GeneticAlgorithm
{
    /// <summary>
    /// 選択方法の列挙型
    /// </summary>
    public enum SelectionMethod
    {
        /// <summary>ルーレット選択</summary>
        Roulette,
        /// <summary>トーナメント選択</summary>
        Tournament,
        /// <summary>ランキング選択</summary>
        Ranking
    }
}