# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

PlanetaryDeception is a Unity (C#) interplanetary story-driven platformer set on Venus in 2203. The player explores levels, interacts with NPCs, manages an inventory and virtual currency (solarbits), and uses an in-game terminal OS called SolarOS to hack systems and uncover a conspiracy involving Venref, Voasis, and Lunar Defense (LD).

Story document: `docs/story.md`

## Build and Test

Game scripts live under `Assets/scripts/`. Unit tests use a separate MSBuild solution with Unity mocks.

**Build:**
```
dotnet build PlanetaryTest.sln
```

**Run all tests (MSTest):**
```
dotnet test PlanetaryTest.sln
```

**Run a single test:**
```
dotnet test PlanetaryTest.sln --filter "FullyQualifiedName~TestMethodName"
```

**Dialog/Console Runner (preview dialogs and SolarOS without Unity):**
```
dotnet run --project Tools/DialogRunner/DialogRunner.csproj -- flowershop
dotnet run --project Tools/DialogRunner/DialogRunner.csproj -- interrogation
dotnet run --project Tools/DialogRunner/DialogRunner.csproj -- console VenusHome
```

**StyleCop:** Configured via `Settings.StyleCop` — file headers and `this.` prefixing are disabled.

## Architecture

### Level System
- `LevelBase` (abstract, extends `MonoBehaviour`) — base for all levels; provides collision detection, UI alerts, and static instance tracking
- Level controllers (`LevelController_1`, `_2`, `_3`) inherit from `LevelBase` and handle scene-specific input and transitions
- Scene transitions use `CharacterSettings.TransitionToNewScene()` which saves player position and loads new scenes
- Chapter 1 (Levels 1-2) is fully implemented. Level 3 (Chapter 2) is an empty shell

### Dialog System
- `IDialogConsole` — interface with `NewQuestion()`, `AddPossibleAnswer()`, `Clear()` for dialog trees
- `TinyOS` implements `IDialogConsole` for in-game rendering
- `CliDialogConsole` implements `IDialogConsole` for CLI preview in the DialogRunner tool
- Dialog methods should be `public` and accept `IDialogConsole` + `Action onExit` so they work in both Unity and the CLI tool
- See `LevelController_2.FlowershopQA1()` and `Level_2_interrogation.InitQuestions()` for examples

### SolarOS (In-Game Terminal)
- `ISolarOS` — interface defining OS capabilities (menu, network, console text, breadcrumb navigation)
- `SolarOS` extends `LevelBase` and implements `ISolarOS` — provides menu system, breadcrumb nav, network management
- `TinyOS` extends `SolarOS` — specialized for Q&A dialog sequences
- `SolarOSApplication` (abstract) — base for OS apps (`SolarOSSocialMedia`, `SolarOSWeb`, `SolarOSGIT`)
- `SolarOSMenuItem` wraps description + callback delegates (`RunApplication`, `OnDisplay`)
- Web content loaded from text files in `Assets/resources/` via `Resources.Load()`

### Inventory System
- `ItemInventory` — base container with `Add()`, `Pull()`, `TransferItem()`, `ContainsItem()`, `GetItemIds()`, `Clear()`
- `PlayerInventory` — singleton for player's carried items
- `KnownItemsInventory` — singleton master catalog of all game items; its `TransferItem()` doesn't remove from source
- Items are `ItemTag` objects: `KnownItem` enum + `ItemClassType` (Trash, Identification, ProjectileWeapon, ParticleWeapon, Hidden, Ticket)
- Hidden items are used as game state flags (e.g. `VenrefInterrogated`, `CeresInvitation`, `FlowershopVisited`)
- When adding new items: add to `KnownItem` enum, add to `KnownItemsInventory` constructor

### Wallet System
- `SolarbitsWallet` — base currency wallet with `Transfer()`, `Add()`, `GetAmount()`, `SetAmount()`; throws on insufficient funds
- `PlayerWallet` — singleton, initialized to 500 solarbits (`PlayerWallet.StartingAmount`)

### Save/Load System
- `SaveManager.Save()` / `SaveManager.Load()` — serializes all game state to JSON at `Application.persistentDataPath/savegame.json`
- `SaveData` — serializable class capturing character settings, inventory, wallet, and scene positions
- `JsonHelper` — wrapper around `System.Text.Json`; swap implementation for Unity's `JsonUtility` if needed
- Not yet wired into game UI

### Singletons
`PlayerInventory`, `PlayerWallet`, `KnownItemsInventory`, `CharacterSettings` all use the singleton pattern.

## Testing Approach

- Framework: **MSTest** (`Microsoft.VisualStudio.TestTools.UnitTesting`)
- Tests are in `Unittesting/` (InventoryTests.cs, WalletTests.cs, SaveManagerTests.cs)
- Unity dependencies are mocked via stub classes in `Mocks/` (UnityEngine.cs, UnityEngineUI.cs, UnityEngineSceneManagement.cs)
- Mock `Resources.Load()` reads actual files from `Assets/resources/` on disk
- `PlanetaryMock.csproj` compiles game scripts + mocks into a library; `EnableDefaultCompileItems` is false
- When adding new game scripts to test, they must be added to `PlanetaryMock.csproj`'s `<Compile>` items
- `.gitignore` excludes `*.csproj` (Unity convention); tracked csproj files work fine but new ones need `git add -f`

## Chapter 1 Game Flow

PlayerSecurityAccessCard (closet) → Console access → IOT door unlock → GIT code review → VoasisCredentials → Voasis website login → VoasisWebsiteCredentialsUsage → Interrogation → VenrefInterrogated → Social media DMs → CeresInvitation → Buy launch ticket (100 solarbits) → Launch → Chapter2
