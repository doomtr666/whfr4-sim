namespace Whfr4
{
    public enum Condition
    {
        Ablaze,
        Bleeding,
        Blinded,
        Broken,
        Deafened,
        Entangled,
        Fatigued,
        Poisoned,
        Prone,
        Stunned,
        Surprised,
        Unconscious,
    }

    public class Conditions
    {
        private int[] counters = new int[Enum.GetNames(typeof(Condition)).Length];

        public void Add(Condition condition, int count = 1)
        {
            counters[(int)condition]+=count;
        }

        public void Remove(Condition condition, int count = 1)
        {
            counters[(int)condition]-=count;
            if (counters[(int)condition] < 0)
                counters[(int)condition] = 0;
        }

        public int Get(Condition condition)
        {
            var val = counters[(int)condition];
            return val > 0 ? val : 0;
        }

        public bool Has(Condition condition)
        {
            return counters[(int)condition] > 0;
        }
    }
}
