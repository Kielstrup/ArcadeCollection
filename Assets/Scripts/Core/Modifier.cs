using UnityEngine;

/// <summary>
/// Represents a gameplay modifier that can affect scoring or other mechanics.
/// This is a ScriptableObject to allow easy creation and configuration in the Unity Editor.
/// 
/// NOTE: This class and all modifier-related code go unused in the final project.
/// They were implemented for potential features and debugging but were not integrated.
/// </summary>
[CreateAssetMenu(fileName = "NewModifier", menuName = "Game/Modifier")]
public class Modifier : ScriptableObject
{
    /// <summary>
    /// The name of the modifier, e.g., "Double Score" or "No Gravity".
    /// </summary>
    public string modifierName;

    /// <summary>
    /// A detailed description of what this modifier does.
    /// </summary>
    [TextArea]
    public string description;

    /// <summary>
    /// Multiplier applied to the player's score when this modifier is active.
    /// Default is 1 (no change).
    /// </summary>
    public float scoreMultiplier = 1f;
}
