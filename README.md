
# 🎨 PushToDraw

<p>
  <img alt="Unity" src="https://img.shields.io/badge/Unity-6000.0.58f2-black?logo=unity&logoColor=white">
  <img alt="Render Pipeline" src="https://img.shields.io/badge/Render%20Pipeline-URP%202D-blue">
  <img alt="Platform" src="https://img.shields.io/badge/Platform-PC-lightgrey">
  <img alt="Status" src="https://img.shields.io/badge/Status-Prototype-orange">
</p>

**PushToDraw** is a 2D top-down puzzle game where the player pushes colored boxes across a grid to "draw" a picture — a cactus, a flower, a fish, a butterfly — one push at a time. It combines classic Sokoban-style block-pushing with an art-reveal twist: every solved level completes a piece of pixel art, and the fewer moves you use, the more stars you earn.

This repository is a curated snapshot of a **team prototype built during a game development course**, kept here as part of my personal portfolio. It focuses on the systems I contributed to; see [My Contribution](#-my-contribution) and [Credits](#-credits--original-team) below for the full picture of authorship.

---

## 📋 Table of Contents

- [Gameplay](#-gameplay)
- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Getting Started](#-getting-started)
- [Levels](#-levels)
- [My Contribution](#-my-contribution)
- [Credits & Original Team](#-credits--original-team)
- [Status & License](#-status--license)

---

## 🎮 Gameplay

- **Grid-based movement** — the player character moves one tile at a time on a fixed grid, using a cooldown-driven step system (no diagonal movement).
- **Push, don't carry** — boxes can only be pushed, never pulled, forcing the player to plan their route before committing to a move.
- **Draw with color** — each box has a color (red, blue, green, yellow, pink, black, white, brown); pushing every box onto its matching target tile reveals the level's hidden picture.
- **Obstacles** — a hedgehog enemy roams parts of the grid and damages the player on contact, adding a light stealth/avoidance layer to some levels.
- **Star rating** — levels are scored (`perfect` / `nice` / `good`) based on move efficiency, encouraging replay and optimization.
- **Progression** — a level-select map tracks completion and best star rating per level; a short tutorial introduces the controls before the first puzzle.

## ✨ Features

- 11 hand-crafted puzzle levels on 8×8 and 12×12 grids, each drawing a different picture on completion
- Full menu flow: main menu → level map → in-level pause menu → win screen with stars
- Animated UI transitions and win-screen sequences (via DOTween)
- Interactive tutorial with a dialogue-driven UI
- Persistent level/star progress across scenes (`MapLevelsData`, `DontDestroyOnLoad`)
- Audio feedback for pushing boxes, UI hover/clicks, and level completion

## 🛠️ Tech Stack

| Category | Technology |
|---|---|
| Engine | Unity **6000.0.58f2** (Unity 6) |
| Rendering | Universal Render Pipeline (URP) — 2D |
| Input | Unity Input System |
| Tweening/Animation | DOTween (Demigiant) |
| UI Text | TextMesh Pro |
| Tilemaps | Unity 2D Tilemap + Tile Palettes |
| Language | C# |

## 📁 Project Structure

```
PushToDraw/
├── Assets/
│   ├── General/
│   │   ├── Scripts/
│   │   │   ├── Player/        # Movement, input, box-grabbing, visuals
│   │   │   ├── HeadgeHog/      # Enemy damage & visuals
│   │   │   ├── Levels/        # Level map data, level-select buttons
│   │   │   ├── UI/            # Menus, pause, win screen, transitions
│   │   │   ├── Sounds/        # SFX data and UI sound hooks
│   │   │   └── others/        # Shared/persistence utilities
│   │   ├── Levels/            # One scene per puzzle level + main/level menus
│   │   ├── PreFabs/            # Reusable prefabs (UI, gameplay objects)
│   │   ├── Assets/             # Sprites, tiles, UI art, drawing references
│   │   └── TutorialScripts/    # Guided-tutorial dialogue system
│   ├── Settings/               # URP render pipeline settings & scene template
│   ├── PLAYGROUND/             # Per-developer sandboxes (see Credits)
│   └── Plugins/                # Third-party packages (DOTween, TextMesh Pro)
├── Packages/                    # Unity Package Manager manifest
├── ProjectSettings/             # Unity project configuration
└── .gitignore                   # Unity-specific ignore rules
```

> `Assets/PLAYGROUND/` contains each team member's individual experiments and work-in-progress scripts, kept for context on how the project evolved during development.

## 🚀 Getting Started

### Prerequisites

- [Unity Hub](https://unity.com/download) with editor version **6000.0.58f2** (or a compatible Unity 6 LTS release) installed
- Git

### Setup

```bash
git clone https://github.com/ikeerfranzz/PushToDraw.git
```

1. Open **Unity Hub** → **Add** → select the cloned `PushToDraw` folder
2. Let the Unity Editor import all assets and resolve packages from `Packages/manifest.json`
3. Open `Assets/General/Levels/MainMenu.unity`
4. Press **Play** in the Editor

### Controls

| Input | Action |
|---|---|
| Arrow Keys / WASD | Move the character (grid-based, one tile per step) |
| Move into a box | Push the box in that direction |

## 🧩 Levels

| # | Theme | Grid Size |
|---|---|---|
| 1 | Cactus | 8×8 |
| 2 | Cloud | 8×8 |
| 3 | Fish | 8×8 |
| 4 | Flower | 8×8 |
| 5 | Sword | 8×8 |
| 6 | Watermelon | 8×8 |
| 7 | Frog | 12×12 |
| 8 | House | 12×12 |
| 9 | Butterfly | 12×12 |
| — | Palm Tree | — |
| 11 | Cat | 12×12 |

Each level is a self-contained Unity scene under `Assets/General/Levels/`; earlier iterations are preserved under `Assets/General/Levels/old/` for reference.

## 👨‍💻 My Contribution

As part of the team, I (**Iker Franzoni**) focused on:

- **Gameplay programming** — player movement/cooldown logic, and the per-color minimap "reveal" system (`BlackMinimap`, `RedMinimap`, `BlueMinimap`, etc.) that tracks when each colored box reaches its target tile
- **Enemy mechanics** — the hedgehog obstacle's patrol movement (`HedgeHogMovement`) and player damage handling (`HedgehogDamage`)
- **Level design** — building and balancing puzzle layouts, including the 8×8 scoring/completion logic (`ScoreController8x8`)
- **UI/UX** — supporting work on menus, transitions, and win-screen flow

My individual sandbox and work-in-progress scripts live under [`Assets/PLAYGROUND/Iker/`](./Assets/PLAYGROUND/Iker/).

## 🙌 Credits & Original Team

**PushToDraw** was built as a collaborative course prototype by:

- **Ferran Reyes** — original project author/repository ([Ferran-Reyes](https://github.com/Ferran-Reyes))
- **Andrea**
- **David**
- **Iker Franzoni** ([@ikeerfranzz](https://github.com/ikeerfranzz)) — see [My Contribution](#-my-contribution)

This repository is maintained by Iker Franzoni as a portfolio reference to the systems listed above. All original design and code credit for the full game is shared across the team listed here.

## 📄 Status & License

This is a **student/prototype project**, not a commercial release. It is shared privately as a portfolio piece; no open-source license is granted. Please reach out before reusing any part of this code or its assets.
