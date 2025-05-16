using UnityEngine;

public class ModifierDebugger : MonoBehaviour
{
    void Start()
    {
        Modifier[] mods = Resources.LoadAll<Modifier>("Modifiers");
        foreach (Modifier mod in mods)
        {
            Debug.Log($"{mod.modifierName} | x{mod.scoreMultiplier} | {mod.description}");
        }
    }

}
