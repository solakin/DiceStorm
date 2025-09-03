using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

[TestFixture]
public class AbilityTests
{
    private Player attacker;
    private Player defender;

    [SetUp]
    public void Setup()
    {
        // Create test players
        var warriorHero = new Hero("Warrior", 30, HeroType.Warrior);
        var wizardHero = new Hero("Wizard", 15, HeroType.Wizard);

        attacker = new Player(warriorHero, warriorHero.GetAbilities);
        defender = new Player(wizardHero, wizardHero.GetAbilities);
    }

    [Test]
    public void Strike_DealsDamage()
    {
        // Arrange
        var strike = new Strike();
        int initialHealth = defender.CurrentHealth;

        // Act
        strike.Execute(attacker, defender);

        // Assert
        Assert.AreEqual(initialHealth - 8, defender.CurrentHealth);
    }

    [Test]
    public void Triage_HealsUser()
    {
        // Arrange
        var triage = new Triage();
        attacker.TakeDamage(10); // Take some damage first
        int healthBeforeHeal = attacker.CurrentHealth;

        // Act
        triage.Execute(attacker, defender);

        // Assert
        Assert.AreEqual(healthBeforeHeal + 5, attacker.CurrentHealth);
    }

    [Test]
    public void AcidVial_AppliesDamageOverTime()
    {
        // Arrange
        var acidVial = new AcidVial();
        var targetHero = new Hero("Test Hero", 30, HeroType.Warrior);
        var target = new Player(targetHero, targetHero.GetAbilities);

        // Act
        acidVial.Execute(null, target);
        int healthAfterInitial = target.CurrentHealth;
        
        target.ProcessStatusEffects(); // Turn 1
        int healthAfterTurn1 = target.CurrentHealth;
        
        // Assert
        Assert.AreEqual(25, healthAfterInitial); // 30 - 5 initial damage
        Assert.AreEqual(22, healthAfterTurn1);   // 25 - 3 DOT damage
    }

    [Test]
    public void StaticShielding_ReducesDamage()
    {
        // Arrange
        var staticShielding = new StaticShielding();
        var userHero = new Hero("Test Hero", 30, HeroType.Wizard);
        var userPlayer = new Player(userHero, userHero.GetAbilities);

        // Act
        staticShielding.Execute(userPlayer, null);
        userPlayer.TakeDamage(10);

        // Assert
        Assert.AreEqual(25, userPlayer.CurrentHealth); // 30 - (10/2)
    }

    // More tests for each ability...
}