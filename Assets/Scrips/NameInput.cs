using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameInput : MonoBehaviour
{
    public GameController gameController;
    public TextMeshProUGUI nameText;
    InputField nameInputField;

    private void Awake() {
        nameInputField = nameText.GetComponent<InputField>();
        nameInputField.onValidateInput = (string text, int charIndex, char addedChar) => addedChar.ToString().ToUpper()[0];
        nameInputField.ActivateInputField();
        nameInputField.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (nameText.text.Length >= 3) {
            //gameController.SubmitScore(nameText.text);
        }
    }
}
