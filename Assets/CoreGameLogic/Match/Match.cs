using System.Linq;

public class Match
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public int CurrentTurn { get; private set; }

    public Match(Player player1, Player player2)
    {
        Player1 = player1;
        Player2 = player2;
        CurrentTurn = 1;
    }

    public void StartMatch()
    {
        while (!Player1.IsDefeated() && !Player2.IsDefeated())
        {
            PlayTurn();
            CurrentTurn++;
        }
    }

    private void PlayTurn()
    {
        Player currentPlayer = CurrentTurn % 2 == 1 ? Player1 : Player2;
        Player opponent = CurrentTurn % 2 == 1 ? Player2 : Player1;

        // Step 1: Roll dice
        currentPlayer.RollNewDice();

        // Step 2: Choose an ability (mocked for now)
        AbilityBase chosenAbility = currentPlayer.SelectedAbilities[0];

        // Step 3: Apply ability
        ApplyAbility(currentPlayer, opponent, chosenAbility);
        // matchScreenManager.ApplyAbilityEffect($"{currentPlayer.Hero.Name} used {chosenAbility.Name}!");
        // matchScreenManager.EndTurn();

        // Step 4: Check for win
        if (opponent.IsDefeated())
        {
            // Debug.Log($"Player {currentPlayer.Hero.Name} wins!");
        }
    }

    private void ApplyAbility(Player attacker, Player defender, AbilityBase ability)
    {
        if (CanUseAbility(ability, attacker.CurrentRoll))
        {
            // Consume the required dice
            attacker.CurrentRoll = attacker.CurrentRoll.ConsumeDice(ability.ShieldCost, ability.ExplosionCost);

            // Apply the ability's effect
            defender.TakeDamage(10); // Placeholder for actual effect
        }
        else
        {
            // Debug.Log($"{attacker.Hero.Name} cannot use {ability.Name}!");
        }
    }

    public bool CanUseAbility(AbilityBase ability, DiceRoll roll)
    {
        int shieldCount = roll.Dice.Count(d => d == DiceRoll.DieType.Shield);
        int explosionCount = roll.Dice.Count(d => d == DiceRoll.DieType.Explosion);

        return shieldCount >= ability.ShieldCost && explosionCount >= ability.ExplosionCost;
    }

    public bool CheckGameEnd => (Player1.IsDefeated() || Player2.IsDefeated());

    public void SetPlayer1(Player player)
    {
        Player1 = player;
    }

    public void SetPlayer2(Player player)
    {
        Player2 = player;
    }
}