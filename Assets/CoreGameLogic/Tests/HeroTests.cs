using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections.Generic;

[TestFixture]
public class HeroTests
{
    [Test]
    public void WarriorHeroInitializationTest()
    {
        // Arrange
        var warrior = Hero.CreateHero(HeroType.Warrior);
        // Act & Assert
        Assert.AreEqual("Warrior", warrior.Name);
        Assert.AreEqual(30, warrior.MaxHealth);
        Assert.AreEqual(HeroType.Warrior, warrior.HeroType);
    }

    [Test]
    public void WizardHeroInitializationTest()
    {
        // Arrange
        var wizard = Hero.CreateHero(HeroType.Wizard);
        // Act & Assert
        Assert.AreEqual("Wizard", wizard.Name);
        Assert.AreEqual(20, wizard.MaxHealth);
        Assert.AreEqual(HeroType.Wizard, wizard.HeroType);
    }

    [Test]
    public void GetAbilitiesByName()
    {
        var wizard = Hero.CreateHero(HeroType.Wizard);
        var abilityList = wizard.GetAbilities;

        var expectedAbilities = new string[] {
            nameof(AcidVial),
            nameof(TheBigOne),
            nameof(StaticShielding)
        };

        var actualAbilities = wizard.GetSelectedAbilities(expectedAbilities);
        Assert.AreEqual(3, actualAbilities.Count);

        foreach (var ability in expectedAbilities)
        {
            var foundAbility = actualAbilities.Find(a => a.GetType().FullName == ability);
            if (foundAbility == null)
                Assert.Fail($"Returned abilities did not contain: {ability}");
        }
    }

    [Test]
    public void GetAbilitiesRandomly()
    {
        var wizard = Hero.CreateHero(HeroType.Wizard);
        var abilityList = wizard.GetAbilities;

        var expectedAbilities = new string[] {
            nameof(AcidVial),
            nameof(TheBigOne),
            nameof(StaticShielding)
        };

        var actualAbilities = wizard.GetRandomAbilities(1);
        Assert.AreEqual(1, actualAbilities.Count);

        Assert.Contains(actualAbilities[0].GetType().FullName, expectedAbilities);
    }
}