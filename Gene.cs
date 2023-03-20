namespace GeneticAlgorithm
{
    /// <summary>
    /// 遺伝子情報クラス
    /// </summary>
    public class Gene
    {
        /// <summary>
        /// 遺伝子配列
        /// </summary>
        public bool[] Value { get; private set; }

        /// <summary>
        /// 遺伝子配列の長さ
        /// </summary>
        public int Length { get => this.Value.Length; }

        /// <summary>
        /// 遺伝子情報を十進数表記に変換した値（0以上、1以下）
        /// </summary>
        public double Number
        {
            get
            {
                int[] table = new int[this.Length];
                for (int i = this.Length - 1; 0 <= i; i--)
                {
                    table[this.Length - 1 - i] = (int)Math.Pow(2, i);
                }
                double number = 0;
                for (int i = 0; i < this.Length; i++)
                {
                    number += Convert.ToDouble(this.Value[i]) * table[i];
                }
                return number / table.Sum();
            }
        }

        /// <summary>
        /// クラス内で使用する疑似乱数ジェネレーター
        /// </summary>
        private Random random = new Random(new Random().Next());

        /// <summary>
        /// 渡された遺伝子の長さの遺伝子配列をランダムに生成する
        /// </summary>
        /// <param name="length">遺伝子の長さ</param>
        public Gene(int length)
        {
            this.Value = new bool[length];
            for (int i = 0; i < length; i++)
            {
                this.Value[i] = 0.5 < this.random.NextDouble();
            }
        }

        /// <summary>
        /// 渡された遺伝子配列でクラスを初期化する
        /// </summary>
        /// <param name="value">遺伝子配列</param>
        public Gene(bool[] value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 突然変異メソッド
        /// </summary>
        public void Mutate()
        {
            int randomIndex = this.random.Next(this.Length);
            this[randomIndex] = !this[randomIndex];
        }

        /// <summary>
        /// 深いコピーメソッド
        /// </summary>
        /// <returns>コピーした遺伝子情報オブジェクト</returns>
        public Gene DeepCopy()
        {
            Gene clone = (Gene)MemberwiseClone();
            clone.Value = (bool[])this.Value.Clone();
            return clone;
        }

        /// <summary>
        /// インデクサー
        /// </summary>
        public bool this[int i] { get => this.Value[i]; internal set => this.Value[i] = value; }
    }
}