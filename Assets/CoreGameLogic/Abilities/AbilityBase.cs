using UnityEngine;

public abstract class AbilityBase
{
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public AbilityType Type { get; protected set; }
    public int ShieldCost { get; protected set; }
    public int ExplosionCost { get; protected set; }

    protected AbilityBase(string name, string description, AbilityType type, int shieldCost, int explosionCost)
    {
        Name = name;
        Description = description;
        Type = type;
        ShieldCost = shieldCost;
        ExplosionCost = explosionCost;
    }

    public abstract void Execute(Player user, Player target);
}

public enum AbilityType
{
    Attack,
    Defend
}