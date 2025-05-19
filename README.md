# Parcel Corps

> **Codename:** Parcel Corps  
> A Unity game project currently in development. This repository includes Unity assets, Spine animations, Krita/Inkscape artwork, and project code.

---

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/parcel-corps.git
cd parcel-corps
```

#### 1b. Install Git LFS via Homebrew if you never have
```bash
brew install git-lfs
git lfs install
```

### 2. Install Git LFS (once per machine)

```bash
git lfs install
```

### 3. Pull LFS-tracked files

```bash
git lfs pull
```

### 4. Open in Unity

Use the Unity Hub or open the `/ParcelCorps` folder directly.

---

## Git LFS Info

We use Git LFS to manage large binary files including:

- Unity scene/prefab assets
- Spine `.skel`, `.atlas`, `.json`
- Krita `.kra`
- Inkscape `.svg` (if large)

To add new file types to LFS:

```bash
git lfs track "*.EXT"
git add .gitattributes
git commit -m "Track new LFS extension"
```

---

## Contributing

- Make sure `git lfs install` has been run before working with this repo.
- Avoid merge conflicts by coordinating asset changes.
- Do not manually merge `.unity`, `.prefab`, or binary assets.

---

## Folder Structure
Assets/
│
├── Game/                  # Feature-oriented game code
│   ├── Core/              # GameState, Command System, PlayerLoop, Input Routing
│   ├── Player/            # PlayerController, PlayerState, Input Bindings
│   ├── Enemies/           # Enemy types, AI, spawners
│   ├── UI/                # HUD, menus, interaction elements
│   ├── Levels/            # Level-specific logic and scripts
│   ├── Items/             # Weapons, pickups, interactables
│   └── Multiplayer/       # Network abstractions, sync logic (even before netcode)
│
├── Library/               # Reusable systems not tied to game domain
│   ├── Utilities/         # Extensions, math helpers, etc.
│   ├── Messaging/         # IMessageBus, Event structs, Command queue
│   ├── Debugging/         # MonoBehaviourBase, logging utils, in-game console
│   ├── Input/             # Input abstraction (to support local vs remote input)
│   └── Data/              # Serialization, save/load, config parsing
│
├── ThirdParty/            # External assets or SDKs
│
├── Resources/             # Non-scene assets loaded at runtime
├── Scenes/                # Scene files organized by world or context
└── Tests/                 # Unity Test Framework compatible unit/integration tests

---

## License

This project is **not licensed for reuse**. Code, assets, and artwork are proprietary and may not be copied or reused.
