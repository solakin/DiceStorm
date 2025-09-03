using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class HeroSelectManager : MonoBehaviour
{
    private UIDocument document;
    private VisualElement root;
    private Button readyButton;
    private Hero selectedHero;
    private List<AbilityBase> selectedAbilities = new List<AbilityBase>(); // Changed from AbilityCard to AbilityBase

    void Start()
    {
        document = GetComponent<UIDocument>();
        root = document.rootVisualElement;
        
        // Remove placeholder ability cards that are used for UIBuilder preview
        var abilityCards = root.Q<VisualElement>("ability-cards");
        abilityCards.Clear();
        
        SetupHeroCards();
        SetupReadyButton();
    }

    private void SetupHeroCards()
    {
        var warriorCard = root.Q<VisualElement>("warrior-card");
        var wizardCard = root.Q<VisualElement>("wizard-card");

        warriorCard.RegisterCallback<ClickEvent>(evt => SelectHero(HeroType.Warrior));
        wizardCard.RegisterCallback<ClickEvent>(evt => SelectHero(HeroType.Wizard));
    }

    private void SelectHero(HeroType heroType)
    {
        // Reset previous selection
        root.Q("warrior-card").RemoveFromClassList("selected");
        root.Q("wizard-card").RemoveFromClassList("selected");
        
        // Select new hero
        string cardName = heroType == HeroType.Warrior ? "warrior-card" : "wizard-card";
        root.Q(cardName).AddToClassList("selected");

        // Create hero instance
        selectedHero = CreateHero(heroType);
        
        // Update ability cards display
        UpdateAbilityCards();
        
        // Update ready button state
        UpdateReadyButtonState();
    }

    private Hero CreateHero(HeroType heroType)
    {
        if (heroType == HeroType.Warrior)
        {
            return new Hero("Warrior", 30, HeroType.Warrior);
        }
        else
        {
            return new Hero("Wizard", 15, HeroType.Wizard);
        }
    }

    private void UpdateAbilityCards()
    {
        var container = root.Q<VisualElement>("ability-cards");
        container.Clear();

        if (selectedHero == null) return;

        foreach (var ability in selectedHero.GetAbilities)
        {
            var card = new VisualElement();
            card.AddToClassList("ability-card");
            
            var name = new Label(ability.Name);
            name.AddToClassList("ability-name");
            
            var description = new Label(ability.Description);
            description.AddToClassList("ability-description");
            
            var cost = new Label($"Cost: {ability.ShieldCost}🛡️ {ability.ExplosionCost}💥");
            cost.AddToClassList("ability-cost");
            
            card.Add(name);
            card.Add(description);
            card.Add(cost);
            
            card.RegisterCallback<ClickEvent>(evt => ToggleAbilitySelection(ability, card));
            
            container.Add(card);
        }
    }

    private void ToggleAbilitySelection(AbilityBase ability, VisualElement card)
    {
        if (selectedAbilities.Contains(ability))
        {
            selectedAbilities.Remove(ability);
            card.RemoveFromClassList("selected");
        }
        else if (selectedAbilities.Count < 2)
        {
            selectedAbilities.Add(ability);
            card.AddToClassList("selected");
        }
        
        UpdateReadyButtonState();
    }

    private void UpdateReadyButtonState()
    {
        readyButton.SetEnabled(selectedHero != null && selectedAbilities.Count == 2);
    }

    private void SetupReadyButton()
    {
        readyButton = root.Q<Button>("ready-button");
        readyButton.clicked += OnReadyClicked;
        readyButton.SetEnabled(false);
    }

    private void OnReadyClicked()
    {
        if (selectedHero == null || selectedAbilities.Count != 2)
        {
            Debug.LogError("Cannot proceed: Hero or abilities not properly selected");
            return;
        }

        // Store selected hero and abilities for the match
        PlayerPrefs.SetString("SelectedHeroType", selectedHero.HeroType.ToString());
        
        // Store the fully qualified type names of the abilities to recreate them in the match scene
        var abilityTypes = selectedAbilities.Select(a => a.GetType().FullName).ToArray();
        if (abilityTypes.Any(t => string.IsNullOrEmpty(t)))
        {
            Debug.LogError("One or more ability types could not be resolved");
            return;
        }
        
        PlayerPrefs.SetString("SelectedAbilities", string.Join(",", abilityTypes));
        Debug.Log($"Stored hero type: {selectedHero.HeroType}, abilities: {string.Join(", ", abilityTypes)}");
        
        // Load match scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("screen_match");
    }
}