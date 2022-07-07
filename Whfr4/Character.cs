namespace Whfr4
{
    public class Character
    {
        public string Name { get; set; } = "Monster";
        public int WeaponSkill { get; set; } = 30;
        public int BallisticSkill { get; set; } = 30;
        public int Initiative { get; set; } = 30;
        public int Strength { get; set; } = 30;
        public int Toughness { get; set; } = 30;
        public int WeaponDamage { get; set; } = 4;
        public int Wounds { get; set; } = 12;
    
        public Character()
        {
         
        }

        public Character(Character other) : this(other.Name, other.WeaponSkill, other.BallisticSkill, other.Initiative, other.Strength, other.Toughness, other.Wounds, other.WeaponDamage) { }

        public Character(string name, int weaponSkill, int ballisticSkill, int initiative, int strength, int toughness, int wounds, int weaponDamage)
        {
            Name = name;
            WeaponSkill = weaponSkill;
            BallisticSkill = ballisticSkill;
            Initiative = initiative;
            Strength = strength;
            Toughness = toughness;
            Wounds = wounds;
            WeaponDamage = weaponDamage;
        }
    }
}