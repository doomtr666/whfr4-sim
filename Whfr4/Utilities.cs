namespace Whfr4
{
    public static class Utilities
    {
        public static int Bonus(this int value)
        {
            return value / 10;
        }

        public static bool IsDouble(this int value)
        {
            if(value < 11 || value > 99)
                return false;

            return value % 10 == value / 10;
        }

        public static int Invert(this int value)
        {
            if(value < 0 || value > 100)
                return value;

            return 10 * (value % 10) + value / 10;
        }

        public static Localization Localize(this int value)
        {
            if(value >= 90)
                return Localization.RightLeg;
            if (value >= 80)
                return Localization.LeftLeg;
            if (value >= 45)
                return Localization.Body;
            if (value >= 25)
                return Localization.RightArm;
            if (value >= 10)
                return Localization.LeftArm;
            return Localization.Head;
        }
    }
}
