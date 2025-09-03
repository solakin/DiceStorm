using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine.TestTools;

[TestFixture]
public class DiceRollTests
{
    [Test]
    public void TestDiceCreationByNumberOfDice()
    {
        DiceRoll diceRoll = new DiceRoll(5);
        Assert.AreEqual(5, diceRoll.Dice.Count);
    }

    [Test]
    public void TestConsumeDice()
    {
        var dice = new DiceRoll(new List<DiceRoll.DieType> { DiceRoll.DieType.Shield, DiceRoll.DieType.Explosion, DiceRoll.DieType.Shield });

        Assert.AreEqual(3, dice.Dice.Count);

        var remainingDice = dice.ConsumeDice(1, 1);

        Assert.AreEqual(1, remainingDice.Dice.Count);

        Assert.AreEqual(1, remainingDice.Count(DiceRoll.DieType.Shield));
        Assert.AreEqual(0, remainingDice.Count(DiceRoll.DieType.Explosion));
    }
}
