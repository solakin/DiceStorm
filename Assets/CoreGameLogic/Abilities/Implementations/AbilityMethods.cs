public class Strike : AbilityBase
{
    private const int DAMAGE = 8;

    public Strike() : base(
        "Strike",
        "Mid damage",
        AbilityType.Attack,
        2, 4)  // Pure explosion cost for basic attack
    { }

    public override void Execute(Player user, Player target)
    {
        target.TakeDamage(DAMAGE);
    }
}

public class Triage : AbilityBase
{
    private const int IMMEDIATE_HEAL = 5;
    private const int TURNS_DURATION = 1;
    private const int HOT_AMOUNT = 5;

    public Triage() : base(
        "Triage",
        "Heal self for 5hp for two turns",
        AbilityType.Defend,
        2, 3)  // High total cost split between shield and explosion
    { }

    public override void Execute(Player user, Player target)
    {
        user.Heal(IMMEDIATE_HEAL);
        user.AddStatusEffect(new HealOverTimeEffect(HOT_AMOUNT, TURNS_DURATION));
    }
}

public class Slam : AbilityBase
{
    private const int DAMAGE = 4;

    public Slam() : base(
        "Slam",
        "Remove a die from the opponent's next round",
        AbilityType.Attack,
        1, 2)  // Medium cost favoring explosion
    { }

    public override void Execute(Player user, Player target)
    {
        target.TakeDamage(DAMAGE);
        target.AddStatusEffect(new ReduceDiceEffect(1, 1)); // Removes 1 die for 1 turn
    }
}

public class StaticShielding : AbilityBase
{
    private const int COUNTER_DAMAGE = 3;
    private const int TURNS_DURATION = 2;
    private const float DAMAGE_REDUCTION = 0.5f;

    public StaticShielding() : base(
        "Static Shielding",
        "Halves damage for 2 turns, mild damage to opponent",
        AbilityType.Defend,
        4, 2)  // Shield-heavy cost for defensive ability
    { }

    public override void Execute(Player user, Player target)
    {
        target?.TakeDamage(COUNTER_DAMAGE);
        user.AddStatusEffect(new DamageReductionEffect(DAMAGE_REDUCTION, TURNS_DURATION));
    }
}

public class AcidVial : AbilityBase
{
    private const int INITIAL_DAMAGE = 5;
    private const int DOT_AMOUNT = 3;
    private const int TURNS_DURATION = 3;

    public AcidVial() : base(
        "Acid Vial",
        "Mid damage over 3 turns",
        AbilityType.Attack,
        1, 3)  // Explosion-focused for DOT attack
    { }

    public override void Execute(Player user, Player target)
    {
        target.TakeDamage(INITIAL_DAMAGE);
        target.AddStatusEffect(new DamageOverTimeEffect(DOT_AMOUNT, TURNS_DURATION));
    }
}

public class TheBigOne : AbilityBase
{
    private const int DAMAGE = 15;

    public TheBigOne() : base(
        "The Big One",
        "High damage",
        AbilityType.Attack,
        0, 4)  // Pure explosion cost but very high
    { }

    public override void Execute(Player user, Player target)
    {
        target.TakeDamage(DAMAGE);
    }
}