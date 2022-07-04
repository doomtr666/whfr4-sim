namespace Whfr4
{
    public class Character
    {
        public string Name { get; set; } = "Monster";
        public uint WeaponSkill { get; set; } = 30;
        public uint BallisticSkill { get; set; } = 30;
        public uint Initiative { get; set; } = 30;
        public uint Strength { get; set; } = 30;
        public uint Toughness { get; set; } = 30;
        public uint WeaponDamage { get; set; } = 4;
        public uint Wounds { get; set; } = 12;
    
        public Character()
        {

        }

        public Character(Character other) : this(other.Name, other.WeaponSkill, other.BallisticSkill, other.Initiative, other.Strength, other.Toughness, other.Wounds, other.WeaponDamage) { }

        public Character(string name, uint weaponSkill, uint ballisticSkill, uint initiative, uint strength, uint toughness, uint wounds, uint weaponDamage)
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