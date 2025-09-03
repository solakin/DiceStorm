using System.Collections.Generic;

public static class AbilityFactory
{
    public static AbilityBase CreateAbility(string abilityName)
    {
        return abilityName switch
        {
            "Strike" => new Strike(),
            "Triage" => new Triage(),
            "Slam" => new Slam(),
            "Acid Vial" => new AcidVial(),
            "The Big One" => new TheBigOne(),
            "Static Shielding" => new StaticShielding(),
            _ => throw new System.ArgumentException($"Unknown ability: {abilityName}")
        };
    }

    public static List<AbilityBase> CreateWarriorAbilities()
    {
        return new List<AbilityBase>
        {
            new Triage(),
            new Slam(),
            new Strike()
        };
    }

    public static List<AbilityBase> CreateWizardAbilities()
    {
        return new List<AbilityBase>
        {
            new StaticShielding(),
            new AcidVial(),
            new TheBigOne()
        };
    }
}