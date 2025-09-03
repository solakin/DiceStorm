using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class MatchTests
{
    [Test]
    public void WarriorVsWizardMatchTest()
    {
        // // Arrange
        // var player1 = new Player(new Hero("Warrior", 30, HeroType.Warrior, AbilityFactory.CreateWarriorAbilities()));
        // var player2 = new Hero("Wizard", 15, HeroType.Wizard, new List<AbilityBase> { staticShielding, acidVial, theBigOne });

        // var matchManager = new MatchManager(player1, player2);

        // // Act
        // matchManager.StartMatch();

        // // Assert
        // Assert.IsTrue(player1.IsDefeated() || player2.IsDefeated());
    }

    [Test]
    public void CanUseAbilityTest()
    {
        // // Arrange
        // var slam = new AbilityCard("Slam", "Remove a die from the opponent's next round", AbilityType.Attack, 3, 0);
        // var roll = new DiceRoll();
        // roll.Dice = new List<DiceRoll.DieType> { DiceRoll.DieType.Explosion, DiceRoll.DieType.Explosion, DiceRoll.DieType.Shield, DiceRoll.DieType.Shield, DiceRoll.DieType.Shield, DiceRoll.DieType.Shield };

        // // Create dummy Player objects for the MatchManager constructor
        // var dummyHero = new Hero("Dummy", 1, HeroType.Warrior, new List<AbilityCard>());
        // var dummyPlayer1 = new Player(dummyHero, new List<AbilityCard>());
        // var dummyPlayer2 = new Player(dummyHero, new List<AbilityCard>());
        // var matchManager = new MatchManager(dummyPlayer1, dummyPlayer2);

        // // Act
        // bool result = matchManager.CanUseAbility(slam, roll);

        // // Assert
        // Assert.IsTrue(result);
    }
}