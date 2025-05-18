using UnityEngine;
using TMPro;

/// <summary>
/// Handles input validation for player initials input field,
/// enforcing uppercase and character limits.
/// </summary>
[RequireComponent(typeof(TMP_InputField))]
public class InitialsInputHandler : MonoBehaviour
{
    private TMP_InputField inputField;

    /// <summary>
    /// Initializes the input field by setting character limit, validation,
    /// and subscribing to input change events.
    /// </summary>
    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.characterLimit = 3;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
        inputField.onValueChanged.AddListener(ForceUppercaseAndLimit);
    }

    /// <summary>
    /// Forces the input text to uppercase and ensures it stays within limits.
    /// This method is called every time the input value changes.
    /// </summary>
    /// <param name="input">The current input string.</param>
    void ForceUppercaseAndLimit(string input)
    {
        string upper = input.ToUpper();
        if (upper != inputField.text)
        {
            inputField.text = upper;
        }
    }
}
