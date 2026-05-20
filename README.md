# VRMeet

> A real-time, multi-user virtual reality meeting room built with Unity and Photon PUN 2. Remote participants share the same virtual space, move as avatars, present slides, and communicate through spatial voice audio — all over the internet.

![Unity](https://img.shields.io/badge/Unity-6000.4.0f1-black?logo=unity)
![Photon](https://img.shields.io/badge/Photon-PUN2-blue)
![Platform](https://img.shields.io/badge/Platform-Windows%20%7C%20Quest%202-lightgrey)
![Status](https://img.shields.io/badge/Status-In%20Development-orange)

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Setup & Installation](#setup--installation)
- [How to Use](#how-to-use)
- [Scripts Reference](#scripts-reference)
- [Development Log](#development-log)
- [Known Issues](#known-issues)
- [Roadmap](#roadmap)
- [Team](#team)

---

## Overview

VRMeet addresses a core problem with remote collaboration tools — they are fundamentally two-dimensional and fail to create a sense of shared presence. This project builds a fully functional multi-user VR meeting room from scratch where distributed participants can:

- Sit (and move) together in the same virtual room
- Present images and slides on a shared screen in real time
- Speak to each other with spatial voice audio
- Be represented by networked avatars with movement and hand animations

---

## Features

### Implemented
- **Multi-user networking** — up to 6 simultaneous participants via Photon cloud (works across different networks globally)
- **Room system** — create or join rooms with a room code and password
- **Avatar system** — each player spawns a unique avatar with synchronized position, rotation, and hand swing animations
- **Player name tags** — floating name above each avatar set at login
- **Shared presentation screen** — load local images (PNG, JPG), sync them to all participants via Photon RPC, navigate with prev/next controls
- **Spatial voice audio** — voices spatialized to each avatar's position using Photon Voice SDK
- **Scene-based flow** — clean Login scene → MeetingRoom scene transition
- **Cross-network play** — Photon handles all routing, no local network required

### In Progress
- Quest 2 build with head and hand tracking
- 3D meeting room environment (table, chairs, whiteboard, laptops)
- Wall collision detection
- Avatar visibility fixes for late joiners
- Object pickup (markers, laser pointer)

---

## Architecture

```
Login Scene
│
├── NetworkManager        → Connects to Photon, joins lobby, persists across scenes
├── RoomManager           → Handles room creation/joining, password check, scene load
└── JoinCanvas            → UI: player name, room code, password, create/join buttons

          │ PhotonNetwork.LoadLevel(1)
          ▼

MeetingRoom Scene
│
├── AvatarSpawner         → Spawns local avatar on join, attaches camera
├── Avatar Prefab
│   ├── PlayerController  → WASD movement, hand swing animation
│   ├── PhotonView        → Ownership and RPC routing
│   ├── PhotonTransformView → Position/rotation sync across clients
│   ├── PunVoiceClient    → Voice transmission
│   └── NameTag           → Floating player name, faces camera
│
├── SlideManager          → File browser, image loading, PNG sync via RPC
├── PresentationScreen    → Quad that receives synced textures
└── MeetingCanvas         → In-room UI: menu, slide controls
```

---

## Tech Stack

| Tool | Version | Purpose |
|------|---------|---------|
| Unity | 6000.4.0f1 | Game engine |
| Photon PUN 2 | Latest | Multi-user networking and state sync |
| Photon Voice SDK | Latest | Spatial voice communication |
| TextMeshPro | Built-in | UI text rendering |
| StandaloneFileBrowser | Latest | Local file picker for image upload |
| Meta XR SDK | Latest | Quest 2 VR support (in progress) |
| Blender | Latest | 3D environment modeling (teammate) |
| Git + GitHub | — | Version control |

---

## Project Structure

```
Assets/
├── Resources/
│   ├── Avatar.prefab         # Networked player prefab
│   └── Speaker.prefab        # Photon Voice speaker prefab
├── Scripts/
│   ├── NetworkManager.cs     # Photon connection, lobby, DontDestroyOnLoad
│   ├── RoomManager.cs        # Room create/join, password, scene transition
│   ├── AvatarSpawner.cs      # Spawn avatar on join, camera setup
│   ├── PlayerController.cs   # WASD movement, hand animation
│   ├── AvatarSync.cs         # (Disabled) Legacy camera sync
│   ├── CameraFollow.cs       # TPP camera following local avatar
│   ├── NameTag.cs            # Floating name tag above remote avatars
│   ├── SlideManager.cs       # Image loading, slide sync via RPC
│   └── PresentationScreen.cs # Screen texture receiver
├── Scenes/
│   ├── Login.unity           # Login/lobby scene (index 0)
│   └── MeetingRoom.unity     # Main VR meeting scene (index 1)
└── Photon/
    ├── PhotonUnityNetworking/
    └── PhotonVoice/
```

---

## Setup & Installation

### Prerequisites
- Unity 6000.4.0f1
- Photon account at [photonengine.com](https://www.photonengine.com)
- Two Photon apps created — one **Realtime**, one **Voice**

### Steps

1. **Clone the repo**
   ```bash
   git clone https://github.com/k-artik-k/meetingfullver.git
   cd meetingfullver
   ```

2. **Open in Unity**
   - Open Unity Hub → Add project from disk → select the cloned folder

3. **Configure Photon**
   - Window → Photon Unity Networking → PUN Wizard → Setup Project
   - Paste your **Realtime App ID**
   - Find `PhotonServerSettings` in Assets → paste your **Voice App ID**

4. **Build Settings**
   - File → Build Settings
   - Ensure scene order: `Login` (index 0), `MeetingRoom` (index 1)

5. **Run**
   - Hit Play in editor, or build to `.exe`
   - For multiplayer testing: run two instances simultaneously

---

## How to Use

### Host
1. Launch the app
2. Enter your name, a room code, and a password
3. Click **Create Room**
4. You will be loaded into the meeting room automatically

### Guest
1. Launch the app
2. Enter your name, the room code, and password shared by the host
3. Click **Join Room**
4. You will be placed in the same virtual room

### In Meeting
| Key | Action |
|-----|--------|
| W / A / S / D | Move avatar |
| Menu button | Toggle presentation panel |
| Open Images | Pick images from PC to present |
| < / > | Navigate slides |

---

## Scripts Reference

### `NetworkManager.cs`
Handles Photon connection lifecycle. Singleton with `DontDestroyOnLoad` — persists across scenes so connection is not reset on scene change.

### `RoomManager.cs`
Manages room creation and joining. Stores room code, password, and player name. On successful join, calls `PhotonNetwork.LoadLevel(1)` to load MeetingRoom.

### `AvatarSpawner.cs`
Spawns the local avatar prefab via `PhotonNetwork.Instantiate` when the player joins the room. Creates a personal camera for each player.

### `PlayerController.cs`
Controls local avatar movement with WASD. Uses `Raycast` to prevent moving through walls. Animates hand swing using `Mathf.Sin`.

### `CameraFollow.cs`
Third-person camera that follows the local avatar. Uses `TransformDirection` offset so camera rotates with the avatar.

### `NameTag.cs`
Displays the remote player's `NickName` above their avatar. Faces the local camera every frame so it is always readable.

### `SlideManager.cs`
Loads local images via StandaloneFileBrowser. Encodes selected image to PNG bytes and sends to all clients via `photonView.RPC`. Supports multi-image slide decks with index navigation.

---

## Development Log

| Date | Milestone |
|------|-----------|
| 11 May 2026 | Research — Photon PUN 2 and Unity XR fundamentals |
| 12 May 2026 | Version issues with Unity 6000.4.6f1, GITAM WiFi blocking Photon |
| 13 May 2026 | Succeeded on Unity 2022 LTS, then migrated to Unity 6000.4.0f1 |
| 14 May 2026 | Room system, avatar spawning, basic cube avatar, WASD movement, UI setup |
| 15 May 2026 | Photon connected, multi-user sync confirmed, cross-network testing passed |
| 16-17 May 2026 | Photon Voice SDK integrated, spatial audio configured, sampling rate fixed |
| 18 May 2026 | Presentation screen built, StandaloneFileBrowser, image sync via RPC |
| 19 May 2026 | Scene split into Login + MeetingRoom, DontDestroyOnLoad, scene transition fixed |
| 20 May 2026 | Login UI redesigned (dark theme), player name system, Voice AppID configured |

---

## Known Issues

- Avatar goes through walls — Raycast collision partially working, needs improvement
- Avatar disappears after opening file browser in editor (works fine in built exe)
- Late-joining players may not see existing avatars immediately
- Player name tag not always visible depending on scale configuration
- Movement is fixed-axis (not relative to camera direction)

---

## Roadmap

- [ ] Quest 2 APK build with head and hand tracking
- [ ] 3D meeting room environment from Blender (table, chairs, screen, whiteboard)
- [ ] Fix wall collision properly with Rigidbody-based movement
- [ ] Object pickup — markers, laser pointer
- [ ] Persistent whiteboard — drawings visible to late joiners
- [ ] PPT to image converter pipeline
- [ ] Participant list UI
- [ ] Mute/unmute button
- [ ] Leave room button

---

