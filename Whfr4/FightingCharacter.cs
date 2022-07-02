namespace Whfr4
{
    public class FightingCharacter
    {
        public Character Character { get; }
        public uint Group { get; }

        public int Wounds { get; set; }

        public uint Advantage { get; set; }

        public uint Order { get; set; }

        public FightingCharacter? Target { get; set; }

        public bool IsDown { get => Wounds <= 0; }

        public FightingCharacter(Character character, uint group)
        {
            Character = character;
            Wounds = (int)character.Wounds;
            Group = group;
            Advantage = 0;
            Order = 0;
            Target = null;
        }
    }
}
