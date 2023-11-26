
public class Stats
{
    public float CurrentHealth { get; set; }
    public float CurrentMana { get; set; }
    public float CurrentStamina { get; set; }
    public float MaxHealth { get; set; }
    public float MaxMana { get; set; }
    public float MaxStamina { get; set; }
    public float Agility { get; set; }
    public float Speed { get; set; }
    public float Luck { get; set; }
    public float PStrength { get; set; }
    public float MStrength { get; set; }
    public float PDefense { get; set; }
    public float MDefense { get; set; }

    public Stats (float health, float mana, float stamina, float agility, float speed, float luck, float pStrength, float mStrength, float pDefense, float mDefense)
    {
        MaxHealth = health;
        MaxMana = mana;
        MaxStamina = stamina;
        Agility = agility;
        Speed = speed;
        Luck = luck;
        PStrength = pStrength;
        MStrength = mStrength;
        PDefense = pDefense;
        MDefense = mDefense;

        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
        CurrentStamina = MaxStamina;
    }

    public override string ToString()
    {
        return "Health: " + CurrentHealth + " out of " + MaxHealth + "\nMana: " + CurrentMana + " out of " + MaxMana + "\nStamina: " + CurrentStamina + " out of " + MaxStamina + "\nAgility: " + Agility + "\nSpeed: " + Speed + "\nLuck: " + Luck +
               "\nPhysical Strength: " + PStrength + "\nMagical Strength: " + MStrength + "\nPhysical Defense: " + PDefense + "\nMagical Defense: " + MDefense;
    }
}
