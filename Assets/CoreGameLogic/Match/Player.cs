using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum PlayerType
{
    Human,
    AI
}

public class Player
{
    public Hero Hero { get; private set; }
    public int CurrentHealth { get; private set; }
    public List<AbilityBase> SelectedAbilities { get; private set; }
    public DiceRoll CurrentRoll { get; set; }

    public PlayerType Type { get; private set; } = PlayerType.Human;

    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private int DicePool;

    public Player(Hero hero, List<AbilityBase> selectedAbilities)
    {
        Hero = hero;
        CurrentHealth = hero.MaxHealth;
        SelectedAbilities = selectedAbilities;
        CurrentRoll = null;
        DicePool = 6; // Initialize dice pool
    }

    public static Player CreatePlayer(HeroType heroType, string[] selectedAbilityTypeNames)
    {
        var hero = Hero.CreateHero(heroType);
        var selectedAbilities = hero.GetSelectedAbilities(selectedAbilityTypeNames);
        return new Player(hero, selectedAbilities) { Type = PlayerType.Human};
    }

    public static Player CreateAIPlayer(HeroType heroType)
    {
        var hero = Hero.CreateHero(heroType);
        var selectedAbilities = hero.GetRandomAbilities(2);
        return new Player(hero, selectedAbilities) { Type = PlayerType.AI };
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        statusEffects.Add(effect);
    }

    public void TakeDamage(int damage)
    {
        // Apply damage reduction effects
        var damageReduction = statusEffects
            .OfType<DamageReductionEffect>()
            .FirstOrDefault();

        if (damageReduction != null)
        {
            damage = Mathf.RoundToInt(damage * 0.5f); // Half damage
        }

        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(Hero.MaxHealth, CurrentHealth + amount);
    }

    public bool IsDefeated() => CurrentHealth <= 0;

    public void StartTurn()
    {
        RollNewDice();
    }

    public void RollNewDice()
    {
        var dicePoolReduction = statusEffects
            .OfType<ReduceDiceEffect>()
            .FirstOrDefault();

        var modifiedDicePool = DicePool;

        if (dicePoolReduction != null)
        {
            modifiedDicePool = Mathf.Max(DicePool - dicePoolReduction.diceReduction, 0);
        }

        CurrentRoll = new DiceRoll(modifiedDicePool);
    }

    public void EndTurn()
    {
        CurrentRoll = null;
    }

    public void ProcessStatusEffects()
    {
        // Apply all active effects
        foreach (var effect in statusEffects.ToList())
        {
            effect.Apply(this);
            effect.OnTurnEnd();

            if (effect.IsExpired)
            {
                statusEffects.Remove(effect);
            }
        }
    }

    public IReadOnlyList<StatusEffect> GetStatusEffects()
    {
        return statusEffects.AsReadOnly();
    }
}
