// Assets/Abilities/Effects/StatusEffect.cs
public abstract class StatusEffect
{
    public int RemainingTurns { get; protected set; }
    
    protected StatusEffect(int duration)
    {
        RemainingTurns = duration;
    }

    public abstract void Apply(Player affected);
    public virtual void OnTurnEnd() => RemainingTurns--;
    public bool IsExpired => RemainingTurns <= 0;
}

// Assets/Abilities/Effects/HealOverTimeEffect.cs
public class HealOverTimeEffect : StatusEffect
{
    private readonly int healAmount;

    public HealOverTimeEffect(int healAmount, int duration) : base(duration)
    {
        this.healAmount = healAmount;
    }

    public override void Apply(Player affected)
    {
        affected.Heal(healAmount);
    }
}

// Assets/Abilities/Effects/DamageOverTimeEffect.cs
public class DamageOverTimeEffect : StatusEffect
{
    private readonly int damageAmount;

    public DamageOverTimeEffect(int damageAmount, int duration) : base(duration)
    {
        this.damageAmount = damageAmount;
    }

    public override void Apply(Player affected)
    {
        affected.TakeDamage(damageAmount);
    }
}

// Assets/Abilities/Effects/DamageReductionEffect.cs
public class DamageReductionEffect : StatusEffect
{
    private readonly float reductionPercent;

    public DamageReductionEffect(float reductionPercent, int duration) : base(duration)
    {
        this.reductionPercent = reductionPercent;
    }

    public override void Apply(Player affected)
    {
        // This will be handled when the player takes damage
        // The Player class should check for this effect
    }
}

// Assets/Abilities/Effects/ReduceDiceEffect.cs
public class ReduceDiceEffect : StatusEffect
{
    public readonly int diceReduction;

    public ReduceDiceEffect(int diceReduction, int duration) : base(duration)
    {
        this.diceReduction = diceReduction;
    }

    public override void Apply(Player affected)
    {
        // This is handled by the Player's DiceRoll usage
    }

}