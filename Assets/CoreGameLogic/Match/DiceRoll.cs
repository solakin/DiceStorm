using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceRoll
{
    public enum DieType { Shield, Explosion }

    public List<DieType> Dice { get; set; }
    public int Count(DieType type) => Dice.Count(d => d == type);




    public DiceRoll(int numberOfDice)
    {
        Dice = new List<DieType>();
        for (int i = 0; i < numberOfDice; i++)
        {
            Dice.Add(UnityEngine.Random.value > 0.5 ? DieType.Shield : DieType.Explosion);
        }
    }

    // Private constructor to allow creating a DiceRoll from a list of dice
    public DiceRoll(List<DieType> dice)
    {
        Dice = dice;
    }



    // Create a new DiceRoll with specified dice removed
    public DiceRoll ConsumeDice(int shieldCount, int explosionCount)
    {
        List<DieType> remainingDice = new List<DieType>(Dice);

        int shieldsToRemove = Mathf.Min(shieldCount, remainingDice.Count(d => d == DieType.Shield));
        int explosionsToRemove = Mathf.Min(explosionCount, remainingDice.Count(d => d == DieType.Explosion));

        remainingDice.RemoveAll(d => d == DieType.Shield && shieldsToRemove-- > 0);
        remainingDice.RemoveAll(d => d == DieType.Explosion && explosionsToRemove-- > 0);

        return new DiceRoll(remainingDice);
    }

}