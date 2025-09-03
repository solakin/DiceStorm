# README
This is a portfolio project in Unity started after being inspired by a tabletop-turned-virtual game concept that sounded fun. The aim is to implement a very small slice of this game but (hopefully) architected and designed well enough to convey how one might progress this to a proper product. And whatever else I feel like throwing in.

## Description of Game Concept
NOTE: I am changing this somewhat to respect the original source, but will try to maintain its spirit.

Dice Storm is a multiplatform tabletop-esque battling card-and-dice game. You choose from distinct heroes with unique stats and powers, then battle AI or human opponents. 

At the beginning of the match, players may choose a limited amount of ability cards to have in play for the battle. Those cards have abilities that cost a certain combination of dice to activate. Players can activate Attack abilities on their turn.

The match is over when someone reaches 0 health. GG!

## Heroes
### Hero A
Warrior: 30hp
Ability Cards:
- Triage: High cost, heal self for 5hp for two turns
- Slam: Mid cost, lowish damage, removes a die from the opponent's next round
- Strike: Mid cost, mid damage

### Hero B
Wizard: 15hp
Ability Cards:
- Static Shielding: High cost, halves damage for 2 turns, does mild damage to opponent when attacked
- Acid Vial: Mid cost, mid damage over 3 turns
- The Big One: High cost, high damage

## Application Flow
- Welcome Screen shows AI or Online Battle choices
- Pick AI or Online Battle
  - AI Battle, goes to Hero Select
  - Online Battle shows 3 emoji or icons to act as a lobby selection
    - Online Battle joins lobby identified by emoji. Waits for opponent to join.
    - Online Battle lobby lets you select Hero. When chosen, status changes to "Ready". On both players being ready, the match begins
- Hero Select
  - Choose between Hero cards. Choose limited amount of Ability cards.
- Match Screen
  - Shows both player boards. Actions are somewhat synced

## Match Flow
- Start of Turn: Roll Dice
  - Roll 6 dice each with 6 sides. Instead of numbers, the dice have shield icons or explosion icons
- Current player may choose to spend the dice on an attack they can afford based on the dice they rolled, from the abilities they chose before the start of the match
- The player ends their turn and play proceeds to the opponent.

## Networking
Uses Photon free tier to handle networking. Lobby segmented and joined by emoji selections.

## Game AI
Just random selection, as basic as possible at first.

## Multiplatform
WebGL and 1 other platform. Maybe Android or Quest.

## Player Board Layout
Left Side: 2 slots for Attack Cards
Center: Hero Card, stats, current HP

## Implementation Checklist (someday)
### Core Game Systems
- [X] Hero class implementation
- [X] Ability Cards implementation
- [X] Dice system
  - [X] Basic dice roll mechanics
  - [X] Visual dice representation
- [X] Turn system
  - [X] Basic turn management
  - [X] End turn conditions

### Match System
- [X] Player class
- [X] Match initialization
- [X] Combat resolution
  - [X] Attack calculation
  - [X] Health modification
- [X] Status effects
  - [X] Multi-turn effects (healing, damage over time)
  - [X] Temporary effects (halved damage)
  - [X] Dice removal effects

### UI Implementation
- [X] Welcome Screen
  - [X] AI Battle button
  - [X] Online Battle button
  - [X] Basic layout
- [X] Hero Select Screen
  - [X] Hero card display
  - [X] Ability card selection (2 out of 3)
  - [X] Ready button
- [X] Match Screen
  - [~] Player boards
  - [X] Dice display
  - [X] Card slots
  - [X] Health display
  - [X] Turn indicators

### Networking (Photon)
- [ ] Basic connection setup
- [ ] Lobby System
  - [ ] Emoji-based lobby selection
  - [ ] Player ready status
  - [ ] Match start synchronization
- [ ] In-Game Networking
  - [ ] Turn synchronization
  - [ ] Action broadcasting
  - [ ] State synchronization

### AI Implementation
- [~] Basic AI
  - [X] Random card selection
  - [ ] Basic decision making
  - [X] Turn execution
- [ ] Advanced AI (Optional)
  - [ ] Strategic dice usage
  - [ ] Card combination awareness

### Testing
- [X] Hero unit tests
- [X] Ability card tests
- [X] Match system tests
- [X] Dice system tests
- [ ] Combat resolution tests
- [ ] Network synchronization tests
- [ ] AI behavior tests

### Platform Support
- [X] WebGL Build
  - [ ] Performance optimization
  - [ ] Input handling
  - [ ] Browser compatibility
- [ ] Secondary Platform (Android/Quest)
  - [ ] Platform-specific UI
  - [ ] Touch/Controller input
  - [ ] Performance optimization

### Polish & Quality of Life
- [ ] Visual Effects
  - [ ] Card animations
  - [ ] Dice rolling animations
  - [ ] Combat effects
- [ ] Sound Effects
  - [ ] UI sounds
  - [ ] Dice rolling
  - [ ] Combat sounds
- [ ] Tutorial
  - [ ] Basic game mechanics
  - [ ] Card explanation
  - [ ] Dice usage guide