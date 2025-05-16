using UnityEngine;

[CreateAssetMenu(fileName = "NewModifier", menuName = "Game/Modifier")]



public class Modifier : ScriptableObject
{
    public string modifierName;
    [TextArea]
    public string description;
    public float scoreMultiplier = 1f;

}
