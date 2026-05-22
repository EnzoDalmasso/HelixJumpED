# Helix Jump Clone

An optimized and faithful clone of the classic mobile arcade game **Helix Jump**, developed in **Unity** using **C#**. The goal is to guide a bouncing ball down a tower of rotating platforms while dodging dynamically generated obstacles and danger zones.

---

## Key Features

* **Procedural Level Generation:** Stages are not pre-built; they are generated in real-time when the level starts using configurations driven by `ScriptableObjects`.
* **Combo Mechanic (Super Speed):** If the ball drops cleanly through 3 or more gaps without touching any platform (`perfectPass`), it enters "fire mode" (super speed), destroying the first platform it impacts.
* **Fluid Control (New Input System):** Implemented using Unity's New Input System, ensuring highly responsive behavior for both mouse dragging on PC and touch gestures on mobile devices.
* **Data Persistence:** Local saving for high scores (`Best Score`) managed via `PlayerPrefs`.
* **Dynamic Visual Effects (VFX):** Paint splash effects instantiate upon collision and are dynamically parented to the platform to rotate alongside it realistically.
* **Randomized Death Zones:** Each level dynamically calculates which sectors become deadly obstacles (`DeathPart`) based on difficulty parameters.

---

## Technical Stack

* **Engine:** Unity (2022.3 LTS or higher).
* **Language:** C# (.NET).
* **Inputs:** Unity New Input System.
* **UI:** TextMeshPro for crisp text rendering.

---

## Architecture & Code Structure

The project features a modular structure with clearly divided responsibilities:

### Core & UI
* **`GameManager.cs`**: Central controller for the game loop (Singleton). It handles pause states, level restarts, scene transitions, and score tracking.
* **`UImanager.cs`**: Updates the user interface in real-time (current score, high score, and the progress bar calculating the remaining vertical distance to the finish line).

### Physics & Gameplay
* **`BallController.cs`**: Controls the physical behavior of the ball (`Rigidbody`), jump impulses, combo detection for super speed, and paint splash instantiation.
* **`CamController.cs`**: Smoothly tracks the ball exclusively on the Y-axis using a pre-calculated offset set in `Start`.

### Generation & Levels
* **`HelixController.cs`**: The engine of the level. It reads the user's drag input to rotate the central tower. It also handles spawning platform prefabs, deactivating random pieces to create gaps, and assigning death zones.
* **`Stage.cs`**: A data container (`ScriptableObject`) defining the level's color palettes and a list configuration for floor-by-floor difficulty (`partCount` and `deathPartCount`).
* **`DeathPart.cs`**: A dynamic script injected into hazardous pieces to alter their behavior and paint them red.
* **`PassScorePoint.cs`**: An invisible trigger placed inside gaps that detects when the player passes cleanly to award points and increment the combo.
* **`GoalController.cs`**: A trigger at the base of the tower that notifies the `GameManager` that the stage has been successfully cleared.

---

## Code Preview

### Super Speed Logic (BallController)
If the player chain-drops through enough gaps, the ball gains extra downward force and the ability to shatter the next platform it hits:
```csharp
if (isSuperSpeedActive && !collision.transform.GetComponent<GoalController>()) {
    Destroy(collision.transform.parent.gameObject, 0.2f);
}
