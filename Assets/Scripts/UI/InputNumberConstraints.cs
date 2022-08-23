using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNumberConstraints : MonoBehaviour
{

    [SerializeField] private float maxNumber;
    [SerializeField] private float minNumber;
    [SerializeField] private TMP_InputField inputField;
    
    [Range(0, 6)]
    [SerializeField] private short allowedDecimals;
    public float CurrentNumber { get; private set; }
    
// Start is called before the first frame update
    void Start()
    {
        CurrentNumber = minNumber;
        
        if (inputField == null)
        {
            inputField = GetComponent<TMP_InputField>();
        }
        

        if (inputField == null)
        {
            Debug.LogError("Input field is not found/selected. Use this script on a component with a Input Field.");
        }
    }

    /**
     * TextMeshPro InputField event.
     * Argument is passed by the event.
     * Argument is based on the input value.
     * Event is called when user changes value of input field.
     * Note: Do not forget to assign this function to the "OnValueChanged" event in the inspector.
     */
    public void OnValueChanged(string value)
    {
        if (IsEditing(value)) return;

        float numberValue = GetCorrectValue(value);

        CurrentNumber = numberValue;

        SetInputFieldText(numberValue);
        
    }

    /**
     * TextMeshPro InputField event.
     * Argument is passed by the event.
     * Argument is based on the input value.
     * Event is called when user deselects value of input field.
     * Note: Do not forget to assign this function to the "OnDeselect" event in the inspector.
     */
    public void OnDeselect(string value)
    {
        float numberValue = GetCorrectValue(value);

        CurrentNumber = numberValue;

        SetInputFieldText(numberValue);
    }
    
    private float ParseString(string value)
    {
        float numberValue;

        float.TryParse(value, out numberValue);

        return numberValue;
    }

    private float GetCorrectValue(string value)
    {
        float numberValue = ParseString(value);

        return ValidateValue(numberValue);
    }

    /*
     * Checks how many characters the string has so we can determine if the user is editing the value because we dont want to interrupt the user in the process.
     * This is entirely done for user experience purposes. For example if the user wants to put "20" in the input field the user has to remove all characters
     * in the input field, but then our OnValueChanged event automatically puts it back to 10. This is extremely annoying to deal with as a user. 
     */
    private bool IsEditing(string value)
    {
        return value.Length <= 1 || HasDecimal(value) && value.Length <= allowedDecimals + 2;
    }

    /*
     * Checks if value is lower then the minimum number, if so it returns the minimum number.
     * Checks if value is higher then the maximum number, if so it returns the maximum number.
     */
    private float ValidateValue(float value)
    {
        float roundedValue = (float) Math.Round(value, allowedDecimals);
        
        return Mathf.Clamp(roundedValue, minNumber, maxNumber);
    }
    
    private void SetInputFieldText(float value)
    {
        inputField.text =  value.ToString();
    }

    private bool HasDecimal(string value)
    {
        return value.Contains(",");
    }
}
