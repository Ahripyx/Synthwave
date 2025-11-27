# Synthwave

**By Ahri**

---

## Table of Contents

- [Description](#description)
- [Purpose](#purpose)
- [Scope](#scope)
- [Class Description & Diagrams](#class-description--diagrams)
- [User Manual](#user-manual)
  - [Installation & Running](#installation--running)
  - [Controls](#controls)
  - [Gameplay Mechanics](#gameplay-mechanics)
  - [HUD & UI Elements](#hud--ui-elements)
  - [Scoring & Combos](#scoring--combos)
  - [Collectibles & Powerups](#collectibles--powerups)
  - [High Scores](#high-scores)
- [Credits](#credits)
- [Change Log](#change-log)
- [Bug Reports](#bug-reports)

---

## Description

Synthwave is a wave survival game where you must survive and increase your score as enemies spawn more frequently. The main goal is to survive as long as possible and try to get a new high score each time.

---

## Purpose

The goal of this project is to capture the repayable feel of arcade games with a primary focus on achieving the highest possible score. This project also demonstrates understanding of:

- Event handling
- Using dispatch timers
- Project organization, such as class separation
- Building XAML via C#
- Implementation of lists for managing game objects, sorting high scores, and filtering entries
- Data transfer objects such as: `HighScore.cs`, `Collectible.cs`, `Projectile.cs`, and `GamePiece.cs`
- Error handling
- Using File IO to store data locally

---

## Scope

This application includes the following:

- A start menu with “Play” and “Controls” options
- A player-controlled sprite with movement and projectile shooting
- Waves of enemies that increase over time as you survive longer
- Scoreboard tracking locally using FileIO for reading and writing
- Live scores and multiplier UI if the player doesn't get hit
- Controls screen to display game controls to the player
- Game over screen with restart and exit options
- Healing power-up and projectile speed boost power-up
- Random power-up item spawns during gameplay

---

## Class Description & Diagrams

### Class Descriptions

#### App.xaml.cs
- **Purpose:** Initializing UI and managing background music.
- **Responsibilities:**
  - Initialize the application and frames.
  - Start/stop looped background music.

#### MainMenu.xaml.cs
- **Purpose:** Shows the main menu UI and navigates to game or controls view.
- **Responsibilities:**
  - Build the menu UI via C# and set the preferred window size.
  - Handle play and controls button clicks for navigation.

#### Controls.xaml.cs
- **Purpose:** Display the controls and have back navigation to the menu.
- **Responsibilities:**
  - Display controls UI and key binds to the user.
  - Navigate back to the menu.

#### MainPage.xaml.cs
- **Purpose:** Run the main part of the game and display the game UI.
- **Responsibilities:**
  - Call the Game Manager and create HUD Text Blocks.
  - Forward keyboard and mouse position events to the Game Manager class.
  - Handle Game Over navigation when the player dies.

#### GameOver.xaml.cs
- **Purpose:** Show final scores and allow players to return to the menu, restart, or close the game.
- **Responsibilities:**
  - Display high scores, accept player names, and save the score.
  - Offer button options for retry, main menu, and quitting the game.

#### GameManager.cs
- **Purpose:** Running the main game systems and updates, such as time loops.
- **Responsibilities:**
  - Hold managers such as Player, Enemy, Projectile, Collectible, Health Bar, and Score.
  - Run the main game loop and timers (enemy spawn, projectile fire, collectibles, spawn acceleration for enemies over time).
  - Update managers every frame and trigger Game Over when health drops to 0.

#### PlayerManager.cs
- **Purpose:** Managing player sprite movement and health.
- **Responsibilities:**
  - Create player Game Piece and add to the grid UI.
  - Track pressed keys and move the player each frame.
  - Manage health (damage/healing events).

#### EnemyManager.cs
- **Purpose:** Spawn and update enemies; handle collisions and enemy removal.
- **Responsibilities:**
  - Spawn enemies at random positions (minimum distance from player).
  - Move enemies toward the player and check for collision.
  - Detect projectile hits and remove enemies.

#### ProjectileManager.cs
- **Purpose:** Create and update projectiles, play sound effects.
- **Responsibilities:**
  - Spawn projectiles based on mouse position and fire at specified velocity.
  - Remove projectiles when out of bounds or on enemy hit; manage projectile sounds.
  - Communicate any collisions to Enemy Manager.

#### CollectibleManager.cs
- **Purpose:** Spawn collectibles and apply effects (health and projectile powerup).
- **Responsibilities:**
  - Spawn collectible visuals and bind properties via `Collectible.cs`.
  - Detect player collision and apply effects.

#### HealthBarManager.cs
- **Purpose:** Update health bar HUD and percentage text.
- **Responsibilities:**
  - Add health bar UI to grid.
  - Resize fill and update percentage based on player health.

#### ScoreManager.cs
- **Purpose:** Track score and combo multiplier; update the HUD.
- **Responsibilities:**
  - Maintain score and combo state, display and update on enemy kill.
  - Reset multiplier if player gets hit.

#### HighScoreManager.cs
- **Purpose:** Keep track of top 5 high scores and display to user.
- **Responsibilities:**
  - Load/save scores from local storage (`highscores.txt`).
  - Keep scores sorted and display top 5.

#### GamePiece.cs
- **Purpose:** Create a UI image for movable entities.
- **Responsibilities:**
  - Manage position/size through `Image.Margin`.
  - Provide movement helpers.

#### Projectile.cs
- **Purpose:** Holds data for projectile state and image.
- **Responsibilities:**
  - Store position, velocity, and image for the projectile.
  - Provide width/height accessors.

#### HighScore.cs
- **Purpose:** DTO for high score records.
- **Responsibilities:**
  - Holds player name and score.

#### Collectible.cs
- **Purpose:** DTO for power-up collectibles.
- **Responsibilities:**
  - Store the Game Piece and type.

---

## User Manual

### Installation & Running

- Open the solution in Visual Studio 2022.
- Start the app with **Debug → Start Debugging** or press **F5**.
- The app automatically sets a preferred launch window size. The game is designed for this window size. Fullscreen will result in a black border.

### Controls

- **Move:** W / A / S / D or Arrow keys (Up, Left, Down, Right)
- **Aim:** Mouse Pointer (keep on screen to aim)
- **Firing:** Automatic
- **Menu navigation:** Click buttons on screen

### Gameplay Mechanics

- Enemies spawn at intervals, moving towards the player. Spawn rate increases over time.
- Projectiles are fired at a set interval in the direction of the mouse.
- **Collisions:**
  - Enemy ↔ Player: damages player, removes enemy.
  - Projectile ↔ Enemy: destroys enemy, increases score/multiplier.
  - Player ↔ Collectible: applies power-up effect and removes item.
- **Health:** Game ends and transitions to game over when health is zero.

### HUD & UI Elements

- **Score:** Top left
- **Combo:** Top right (shown when >1)
- **Health bar:** Top center (percentage and fill bar)
- **Game Over Screen:** Shows high scores, allows name entry and saving score, retry, main menu, quit.
- **Main Menu:** Control and Play options.

### Scoring & Combos

- Base points per enemy kill.
- Combo increases for consecutive kills without damage.
- Score scales faster with high combos.

### Collectibles & Powerups

- **Healthup:** Restores player to max health.
- **Powerup:** Temporarily increases fire rate (10s).
- Power-ups spawn periodically (blue = powerup, green = healthup).

### High Scores

- Enter your name and press "Save Score" on the Game Over screen.
- High scores saved locally in "Name | Score" format.
- Displays top 5 scores in sorted order.

---

## Credits

- Background music by ArnoCyreus
- Projectile Sound Effect by LilMati

---

## Change Log

**1.0 - October 6th, 2025**
- Initial version with basic game loop, player shooting, collectibles, enemy waves, health bar, and menu screens.

**1.1 - October 18th, 2025**
- Added numeric health status above health bar.

**1.2 - October 19th, 2025**
- Implemented on-screen scoring UI and high score list with saving on game over.

**1.3 - October 19th, 2025**
- Added combo multiplier feature; multipliers reset to 0 when player is hit.

**1.4 - October 19th, 2025**
- Displayed combo multiplier on HUD, fixed projectile sound bug on game over.

**1.5 - October 19th, 2025**
- Added looping background music
- Forced grid size UI so projectiles and controls remain consistent
- Vibrant UI overhaul and code cleanup

---

## Bug Reports

**Resolved:**
- Fixed projectile sound effects on menu by managing audio properties and cleanup.
- Resolved projectile boundary issue by enforcing consistent grid size across views.
- Adjusted UI launch size for correct screen alignment.

**Unresolved:**
- Enemies and collectibles may clip over HUD screens.

---
