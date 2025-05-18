using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InitialsInputHandler : MonoBehaviour
{
    private TMP_InputField inputField;

    void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        inputField.characterLimit = 3;
        inputField.characterValidation = TMP_InputField.CharacterValidation.Alphanumeric;
        inputField.onValueChanged.AddListener(forceUppercaseAndLimit);
    }
    void forceUppercaseAndLimit(string input)
    {
        string upper = input.ToUpper();
        if (upper != inputField.text)
        {
            inputField.text = upper;
        }
    }
}
