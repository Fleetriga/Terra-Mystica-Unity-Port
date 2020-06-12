Conversion of Terra Mystica.

Any questions on implementation can be answered if contacted.

Rules found here - http://www.feuerland-spiele.de/dateien/Terra_Mystica_EN_1.0_.pdf

Recent Unity Build - https://mega.nz/folder/sjJT2IpK#WZp3EV_cvenn-T_kDwDhKg
(Start a game through the multiplayer option only)
(Game is kind of broken in singleplayer at the moment, try with 2 players)


Scenes:
Game Scene
Title Screen (On game load)

Features Breakdown:
Gameplay:
  - Resources
  - Building
      + Built upon a tile. Restricted to terrain types consistent with the player factions habitat.
      + Build in exchange for resources
      + Buildings give play resource income, awarded once per round.
      + Temple buildings allow the picking of a favour tile
      + Worth a certain amount of Building Points (Used to built a town)
  - Cult Favour
      + Awarded when the player attains a favour tile
      + Awarded if the player chooses to sacrifice a Priest (Priest resource)
      + Victory points awarded to each player based on their place on each cult favour track. (End game)
  - Terraforming
      + Player can spend Workers (resource) to permanently change the terrain of a tile.
      + Player's faction's habitat terrain will be shown at the top of the Terraforming UI
  - Favour Tiles:
      + Awarded upon building a temple or sanctuary building
      + Awards the player with a cult favour bonus (Between 1 and 3 favour)
      + Awards the player a unique permanent bonus
          + Resource income
          + Point income for certain action type
  - Victory Points
      + Determine which player wins the game
      + Awarded for certain actions, if the player has a point bonus for this action
      + Point Bonuses stack infinitely
      + Point bonuses can be permanent or temporary
          + Temporary bonuses will be lost upon the player retiring for the round
      + Point Bonuses can be reactive or cumulative
          + Reactive bonuses are awarded as soon as the player completes the corresponding action
          + Cumulative bonusese are awarded at the end of each round based on the players game state
  - Round Bonuses
      + Six chosen randomly before game start 
      + Give temporary point bonuses to all players every round
      + Give resource bonuses for players with corresponding cult favour
  - Round Start Bonuses
      + (Number of players + 3) chosen at the start of the game
      + Give players temporary Point Bonus
      + Give players immediate resource bonus
      + Chosen first in counter player order, then chosen when a player retires for the round
      + A Round Start Bonus cannot be chosen by two players
  - Town Building
      + Buildings built adjacent pool their building points
      + When a cluster of buildings reaches 7 points and contains at least 4 buildings a town is formed
      + Player awarded a Town Tile (Immidiate resource and points bonus) upon a town being built
          + Each town tile can be taken 2 times max
          + 5 unique town tiles (Means 10 max towns can be formed in one game)

UI Features:
  - Character Sheets
      + Shows resources available to all players in the game
  - Player action recap
      + Shows what a player has done during their turn to other players
  - Tool tips
      + Shows information of object when object is hovered over
          + e.g. hovering over a tile will tell you it's terrain, owner, progress towards a town, building type
  
