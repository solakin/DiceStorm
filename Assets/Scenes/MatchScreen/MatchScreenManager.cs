using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MatchScreenManager : MonoBehaviour
{
    private UIDocument document;
    private VisualElement root;
    private bool isPlayer1Turn = true;
    private VisualElement actionEffectLabel;

    public Match currentMatch;


    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;

        InitializePlayersAndMatch();
        SetupUI();
        StartMatch();

        actionEffectLabel = root.Q<VisualElement>("action-effect");
    }

    private void InitializePlayersAndMatch()
    {
        // Get selected hero and abilities from PlayerPrefs
        HeroType playerChosenHeroType = PlayerPrefs.GetString("SelectedHeroType") == HeroType.Warrior.ToString() ? HeroType.Warrior : HeroType.Wizard;
        string[] playerSelectedAbilities = PlayerPrefs.GetString("SelectedAbilities").Split(',');

        currentMatch = new Match(
            Player.CreatePlayer(playerChosenHeroType, playerSelectedAbilities),
            Player.CreateAIPlayer(playerChosenHeroType == HeroType.Warrior ? HeroType.Wizard : HeroType.Warrior)
        );
    }

    private void SetupUI()
    {
        // Setup player boards
        SetupPlayerBoard(currentMatch.Player1, "player1");
        SetupPlayerBoard(currentMatch.Player2, "player2");

        // Setup turn button
        var endTurnButton = root.Q<Button>("end-turn-button");
        endTurnButton.clicked += OnEndTurnClicked;

        UpdateTurnIndicator();
    }

    private void SetupPlayerBoard(Player player, string boardPrefix)
    {
        var nameLabel = root.Q<Label>($"{boardPrefix}-name");
        nameLabel.text = player.Hero.Name;

        var healthBar = root.Q<ProgressBar>($"{boardPrefix}-health");
        healthBar.value = player.Hero.MaxHealth;
        healthBar.highValue = player.Hero.MaxHealth;

        var heroFrame = root.Q<VisualElement>($"{boardPrefix}-hero-frame");
        heroFrame.AddToClassList(player.Hero.HeroType.ToString().ToLower());

        SetupAbilityCards(player, boardPrefix);
    }

    private void StartMatch()
    {
        // Initial dice roll for first player
        RollDice(currentMatch.Player1);
        UpdateUI();
    }

    private void OnEndTurnClicked()
    {
        EndTurn();
    }

    private void EndTurn()
    {
        // Process end-of-turn effects for the current player
        if (isPlayer1Turn)
            currentMatch.Player1.ProcessStatusEffects();
        else
            currentMatch.Player2.ProcessStatusEffects();

        // Check for game end after processing effects
        CheckGameEnd();

        isPlayer1Turn = !isPlayer1Turn;

        // Roll dice for the new player
        if (isPlayer1Turn)
            RollDice(currentMatch.Player1);
        else
            RollDice(currentMatch.Player2);

        UpdateTurnIndicator();
        UpdateUI();

        // If it's AI's turn, make them play
        if (!isPlayer1Turn)
            StartCoroutine(StartAITurn());
    }

    // Simple AI turn logic: use the first available ability, then end turn
    private void PlayAITurn()
    {
        var availableAbility = currentMatch.Player2.SelectedAbilities.FirstOrDefault(a => CanUseAbility(a, currentMatch.Player2.CurrentRoll));
        if (availableAbility != null)
        {
            UseAbility(currentMatch.Player2, currentMatch.Player1, availableAbility);
            UpdateUI();
        }

        EndTurn();
    }


    private WaitForSeconds aiTurnDelay = new WaitForSeconds(1.5f);

    private IEnumerator StartAITurn()
    {
        yield return aiTurnDelay;
        PlayAITurn();
    }

    private void RollDice(Player player)
    {
        player.RollNewDice();
        UpdateDiceUI(player);
    }

    private void UpdateUI()
    {
        // Update health bars with animation
        UpdateHealthBar(currentMatch.Player1, "player1-health");
        UpdateHealthBar(currentMatch.Player2, "player2-health");

        // Update dice displays
        UpdateDiceUI(currentMatch.Player1);
        UpdateDiceUI(currentMatch.Player2);

        // Enable/disable ability cards based on available dice
        UpdateAbilityCardStates();

        // Update status effect indicators
        UpdateStatusEffects(currentMatch.Player1, "player1-board");
        UpdateStatusEffects(currentMatch.Player2, "player2-board");
    }

    private void UpdateHealthBar(Player player, string healthBarName)
    {
        var healthBar = root.Q<ProgressBar>(healthBarName);
        healthBar.value = player.CurrentHealth;

        // Add visual feedback for health changes
        if (healthBar.value < healthBar.value)
        {
            healthBar.AddToClassList("damage-flash");
            healthBar.schedule.Execute(() => healthBar.RemoveFromClassList("damage-flash")).StartingIn(500);
        }
    }

    private void UpdateStatusEffects(Player player, string boardName)
    {
        var statusContainer = root.Q(boardName).Q("status-effects");
        if (statusContainer == null)
        {
            // Create status effect container if it doesn't exist
            statusContainer = new VisualElement();
            statusContainer.name = "status-effects";
            statusContainer.AddToClassList("status-effects");
            root.Q(boardName).Add(statusContainer);
        }

        // Clear and rebuild status indicators
        statusContainer.Clear();

        foreach (var effect in player.GetStatusEffects())
        {
            var effectIcon = new Label();
            effectIcon.AddToClassList("status-icon");

            // Set icon based on effect type
            if (effect is HealOverTimeEffect)
                effectIcon.text = "💚";
            else if (effect is DamageOverTimeEffect)
                effectIcon.text = "🔥";
            else if (effect is DamageReductionEffect)
                effectIcon.text = "🛡️";
            else if (effect is ReduceDiceEffect)
                effectIcon.text = "❌";

            effectIcon.tooltip = $"{effect.GetType().Name} ({effect.RemainingTurns} turns)";
            statusContainer.Add(effectIcon);
        }
    }

    private void UpdateTurnIndicator()
    {
        var turnIndicator = root.Q<Label>("turn-indicator");
        turnIndicator.text = isPlayer1Turn ? "Your Turn" : "Opponent's Turn";

        var endTurnButton = root.Q<Button>("end-turn-button");
        endTurnButton.SetEnabled(isPlayer1Turn);
    }

    private void SetupAbilityCards(Player player, string boardPrefix)
    {
        var container = root.Q<VisualElement>($"{boardPrefix}-ability-cards");
        container.Clear(); // Clear any existing cards

        foreach (var ability in player.SelectedAbilities)
        {
            // Create the main card container
            var card = new VisualElement();
            card.AddToClassList("card"); // Add base card class
            card.AddToClassList("ability-cards"); // Add ability specific class

            // Create name element
            var nameLabel = new Label(ability.Name);
            nameLabel.AddToClassList("card-name"); // Use card-name class from CSS
            card.Add(nameLabel);

            // Create description element
            var descriptionLabel = new Label(ability.Description);
            descriptionLabel.AddToClassList("card-description"); // Use card-description class from CSS
            card.Add(descriptionLabel);

            // Create cost element
            var costLabel = new Label($"Cost: {ability.ShieldCost}🛡️ {ability.ExplosionCost}💥");
            costLabel.AddToClassList("card-cost"); // Use card-cost class from CSS
            card.Add(costLabel);

            // Add click handler for player 1's cards only
            if (boardPrefix == "player1")
            {
                card.RegisterCallback<ClickEvent>(evt => OnAbilityCardClicked(ability, card));

                // Add initial state class (will be updated in UpdateAbilityCardStates)
                card.AddToClassList("disabled");
            }
            else
            {
                // For opponent cards, add a specific class for styling
                card.AddToClassList("opponent-card");
            }

            container.Add(card);
        }
    }


    private void UpdateDiceUI(Player player)
    {
        string boardPrefix = player == currentMatch.Player1 ? "player1" : "player2";
        var dicePool = root.Q<VisualElement>($"{boardPrefix}-dice-pool");

        // Clear existing dice
        dicePool.Clear();

        // Create dice based on current roll
        if (player.CurrentRoll != null)
        {
            foreach (var dieType in player.CurrentRoll?.Dice)
            {
                var dieElement = CreateDieElement(dieType);
                dicePool.Add(dieElement);
            }
        }
    }

    private VisualElement CreateDieElement(DiceRoll.DieType dieType)
    {
        // Create main die container
        var die = new VisualElement();
        die.AddToClassList("die");
        die.AddToClassList(dieType == DiceRoll.DieType.Shield ? "shield-die" : "explosion-die");

        // Create face container
        var dieFace = new VisualElement();
        dieFace.AddToClassList("die-face");

        // Create icon
        var icon = new Label();
        icon.AddToClassList("die-icon");
        icon.text = dieType == DiceRoll.DieType.Shield ? "🛡️" : "💥";

        // Assemble the die
        dieFace.Add(icon);
        die.Add(dieFace);

        return die;
    }

    private void UpdateAbilityCardStates()
    {
        var abilityCards = root.Q("player1-ability-cards").Children();
        foreach (var card in abilityCards)
        {
            var ability = currentMatch.Player1.SelectedAbilities[abilityCards.ToList().IndexOf(card)];
            bool canUse = isPlayer1Turn && CanUseAbility(ability, currentMatch.Player1.CurrentRoll);

            if (canUse)
            {
                card.RemoveFromClassList("disabled");
                card.AddToClassList("usable");
            }
            else
            {
                card.RemoveFromClassList("usable");
                card.AddToClassList("disabled");
            }
        }
    }

    private bool CanUseAbility(AbilityBase ability, DiceRoll roll)
    {
        int shieldCount = roll.Dice.Count(d => d == DiceRoll.DieType.Shield);
        int explosionCount = roll.Dice.Count(d => d == DiceRoll.DieType.Explosion);

        return shieldCount >= ability.ShieldCost && explosionCount >= ability.ExplosionCost;
    }

    private void OnAbilityCardClicked(AbilityBase ability, VisualElement card)
    {
        if (!isPlayer1Turn) return;
        if (!CanUseAbility(ability, currentMatch.Player1.CurrentRoll)) return;

        // Use the ability
        UseAbility(currentMatch.Player1, currentMatch.Player2, ability);

        // Update the UI
        UpdateUI();

        // End the turn
        EndTurn();
    }

    private void UseAbility(Player attacker, Player defender, AbilityBase ability)
    {
        // Consume the dice used for the ability
        attacker.CurrentRoll = attacker.CurrentRoll.ConsumeDice(ability.ShieldCost, ability.ExplosionCost);

        // Execute the ability
        ability.Execute(attacker, defender);

        ApplyAbilityEffect(ability.Name); // Show ability effect

        // Check for game end condition after ability execution
        CheckGameEnd();
    }

    private void ApplyAbilityEffect(string effectText)
    {
        // Validate that the action effect label exists before using it
        if (actionEffectLabel == null)
        {
            Debug.LogWarning("Action effect label not found in the UI. Make sure 'action-effect' exists in the UXML file.");
            return;
        }

        var effectLabel = new Label();
        actionEffectLabel.Add(effectLabel);
        effectLabel.text = effectText;
        actionEffectLabel.AddToClassList("flash");
        Invoke("RemoveFlashEffect", 0.5f);
    }

    private void RemoveFlashEffect()
    {
        actionEffectLabel.RemoveFromClassList("flash");
    }

    private void CheckGameEnd()
    {
        if (currentMatch.CheckGameEnd)
        {
            ShowGameEndScreen();
        }
    }

    private void ShowGameEndScreen()
    {
        // Disable normal game UI elements
        root.Q("player1-board").SetEnabled(false);
        root.Q("player2-board").SetEnabled(false);
        root.Q("end-turn-button").SetEnabled(false);

        // Show victory/defeat screen
        var gameEndScreen = new VisualElement();
        gameEndScreen.name = "game-end-screen";
        gameEndScreen.AddToClassList("game-end-screen");

        var resultLabel = new Label();
        resultLabel.AddToClassList("result-text");
        resultLabel.text = currentMatch.Player1.CurrentHealth <= 0 ? "Defeat!" : "Victory!";
        gameEndScreen.Add(resultLabel);

        var playAgainButton = new Button();
        playAgainButton.text = "Play Again";
        playAgainButton.AddToClassList("play-again-button");
        playAgainButton.clicked += () => UnityEngine.SceneManagement.SceneManager.LoadScene("screen_heroselect");
        gameEndScreen.Add(playAgainButton);

        root.Add(gameEndScreen);
    }
}