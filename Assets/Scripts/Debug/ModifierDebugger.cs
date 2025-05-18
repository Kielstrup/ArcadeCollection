using UnityEngine;

/// <summary>
/// A simple debug utility that loads all Modifier ScriptableObjects from the Resources/Modifiers folder
/// and logs their details (name, score multiplier, and description) to the console on start.
/// 
/// NOTE: This script is functionally unused in the final project. It was implemented
/// to help during development to verify Modifier assets but has no gameplay role now.
/// </summary>
public class ModifierDebugger : MonoBehaviour
{
    void Start()
    {
        // Load all Modifier assets from the Resources/Modifiers folder
        Modifier[] mods = Resources.LoadAll<Modifier>("Modifiers");

        // Log each modifier's details to the console
        foreach (Modifier mod in mods)
        {
            Debug.Log($"{mod.modifierName} | x{mod.scoreMultiplier} | {mod.description}");
        }
    }
}
