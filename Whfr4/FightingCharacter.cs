namespace Whfr4
{
    public class FightingCharacter
    {
        private int advantage = 0;

        public Character Character { get; }

        public int Group { get; }

        public int Wounds { get; set; }

        public int Advantage 
        { 
            get => advantage; 
            set {
                advantage = value;
                if(advantage < 0) advantage = 0;
                //if(advantage > Character.Initiative.Bonus()) advantage = Character.Initiative.Bonus();
            }
        }

        public FightingCharacter? Target { get; set; }

        public bool IsDown { get => Wounds <= 0; }

        public Conditions Conditions { get; } = new Conditions();

        public List<Injury> Injuries { get; } = new List<Injury>();

        public FightingCharacter(Character character, int group)
        {
            Character = character;
            Wounds = character.Wounds;
            Group = group;
            Advantage = 0;
            Target = null;
        }
    }
}
