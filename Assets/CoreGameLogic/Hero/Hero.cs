using System.Collections.Generic;
using UnityEngine;

public class Hero
{
    public string Name { get; private set; }
    public int MaxHealth { get; private set; }
    public HeroType HeroType { get; private set; }
    public List<AbilityBase> GetAbilities => (this.HeroType == HeroType.Warrior) ? AbilityFactory.CreateWarriorAbilities() : AbilityFactory.CreateWizardAbilities();

    public Hero(string name, int maxHealth, HeroType heroType)
    {
        Name = name;
        MaxHealth = maxHealth;
        HeroType = heroType;
    }

    public static Hero CreateHero(HeroType heroType)
    {
        if (heroType == HeroType.Warrior)
        {
            return new Hero("Warrior", 30, HeroType.Warrior);
        }
        else
        {
            return new Hero("Wizard", 20, HeroType.Wizard);
        }
    }

    public List<AbilityBase> GetSelectedAbilities(string[] selectedAbilityTypeNames)
    {
        var selectedAbilities = new List<AbilityBase>();
        foreach (var typeFullName in selectedAbilityTypeNames)
        {
            var ability = GetAbilities.Find(a => a.GetType().FullName == typeFullName);
            if (ability != null)
            {
                selectedAbilities.Add(ability);
            }
            else
            {
                Debug.LogWarning($"Could not find ability of type {typeFullName} for hero {Name}");
            }
        }
        return selectedAbilities;
    }

    public List<AbilityBase> GetRandomAbilities(int count)
    {
        var abilities = new List<AbilityBase>(GetAbilities);
        var selectedAbilities = new List<AbilityBase>();
        
        for (int i = 0; i < count; i++)
        {
            if (abilities.Count > 0)
            {
                int randomIndex = Random.Range(0, abilities.Count);
                selectedAbilities.Add(abilities[randomIndex]);
                abilities.RemoveAt(randomIndex);
            }
        }
        
        return selectedAbilities;
    }
}

public enum HeroType
{
    Wizard,
    Warrior
}