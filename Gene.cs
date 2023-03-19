namespace GeneticAlgorithm
{
    public class Gene
    {
        public bool[] Value { get; internal set; }
        
        public int Length { get => this.Value.Length; }        

        public bool this[int i]
        {
            get { return this.Value[i]; }
            internal set { this.Value[i] = value; }
        }

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

        private Random random;

        public Gene(int length)
        {
            this.random = new Random(new Random().Next());
            this.Value = new bool[length];
            for (int i = 0; i < length; i++)
            {
                this.Value[i] = 0.5 < this.random.NextDouble();
            }
        }

        public Gene(bool[] value)
        {
            this.Value = value;
            this.random = new Random(new Random().Next());
        }

        public void Mutate()
        {
            int randomIndex = this.random.Next(this.Length);
            this[randomIndex] = !this[randomIndex];
        }

        public Gene DeepCopy()
        {
            Gene clone = (Gene)MemberwiseClone();
            clone.Value = (bool[])this.Value.Clone();
            return clone;
        }
    }
}