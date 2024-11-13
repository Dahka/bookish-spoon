# Memory Game Example

This is an example memory game using several small improvements and features which include:

  <details>
  <summary>
    <strong>Smooth Gameplay</strong>
  </summary>
    
  - The player is able to flip multiple cards without having to wait for the comparison of two previous cards
    
  - There are animations for flipping the card and matching cards
    
  </details>
  
  <details>
    
  <summary>
    <strong>Customizable Settings </strong>
  </summary>
  
  - The number of rows and columns can be decided by the player, given some constraint parameter of between 1 and 20
    
  - The board game scales itself to fit without stretching to a target container object

  </details>

  <details>
  <summary>
    <strong>Saving and Loading</strong>
  </summary>
    
  - The player is able to save the game at any time when exiting to the title menu
    
  - The board game is restored in the same state it was when save, with the same cards, matched cards and score

  - The save is stored locally and can be found in your appdata folder

  - There are options for testing saving and loading inside the main game scene
    
  </details>

  <details>
  <summary>
    <strong>Score Mechanism</strong>
  </summary>
    
  - There is a score system where the player gets points each time he matches cards
    
  - There is also a combo multiplier system that multiplies the score the player gets when he matches many cards in a row

  - The multiplier returns to one when the player makes a mistake
    
  </details>

  <details>
  <summary>
    <strong>Sounds</strong>
  </summary>
    
  - Sound effects for when flipping a card, matching cards, making a mistake and finishing a game
    
  </details>


---
# Game Rules

The objective of the game is to clear the board of cards getting the highest possible score.

To do that you will have to balance a reasonable game board size with your memory ability.

You can achieve even higher scores by chaining matches together, increasing your score multiplier.

  
# Project Specifications

---

####  1 - The game uses Unity 2021.3.22f1

####  2 - Focus was given on the code rather than visuals

####  3 - The project uses free assets from the following places:

- [Card RPG) Items](https://sagak-art-pururu.itch.io/cardrpg-items)

- [Card RPG) Monsters](https://sagak-art-pururu.itch.io/cardrpg-monsters)

- [Pixel Fantasy Playing Cards](https://cazwolf.itch.io/pixel-fantasy-cards)

- [Mixkit Sound Effects](https://mixkit.co/free-sound-effects)
