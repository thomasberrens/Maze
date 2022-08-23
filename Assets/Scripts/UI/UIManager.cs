using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private MazeManager mazeManager;
    
    [SerializeField] private InputNumberConstraints widthInputField;
    [SerializeField] private InputNumberConstraints heightInputField;

    [SerializeField] private TMP_Dropdown dropdown;

    private bool delay;

    private string algorithm;
 
    // Start is called before the first frame update
    void Start()
    {

        if (widthInputField == null || heightInputField == null)
        {
            Debug.LogError("Width Input Field or Height Input Field is not selected. Can't generate a maze without a valid width and height.");
            return;
        }
        
        if (mazeManager == null)
        {
            mazeManager = FindObjectOfType<MazeManager>();
            
            if (mazeManager == null)
            {
                Debug.LogError("MazeManager object is not found/selected. Create a game object with a Maze Manger script attached to it.");
                return;
            }
            
        }

        if (dropdown == null)
        {
            Debug.LogError("Dropdown is not configured, can't choose an algorithm now.");

            algorithm = AlgorithmTypes.HuntAndKill.ToString();
        }
        else
        {
            InitializeDropDown();
        }
    }

    public void SetAlgorithm(int dropDownIndex)
    {
        algorithm = dropdown.options[dropDownIndex].text;
    }

    public void SetDelay(bool value)
    {
        delay = value;
    }

    public void GenerateMaze()
    {
        mazeManager.DestroyMaze();
        
        mazeManager.SetWidth((int) widthInputField.CurrentNumber);
        mazeManager.SetHeight((int) heightInputField.CurrentNumber);
        mazeManager.SetDelay(delay);
        
        mazeManager.SetAlgorithm(algorithm);
        
        mazeManager.Generate();
    }

    private void InitializeDropDown()
    {
        dropdown.ClearOptions();


        Array algorithmTypes = Enum.GetValues(typeof(AlgorithmTypes));
        
        foreach (AlgorithmTypes algorithmType in algorithmTypes)
        {
            if (algorithmType == AlgorithmTypes.NULL) continue;

            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData();
            optionData.text = algorithmType.ToString();
            
            dropdown.options.Add(optionData);
        }

        
        string firstOption = dropdown.options[0].text;
        
        // Sets place holder text to the first option name
        dropdown.captionText.text = firstOption;

        algorithm = firstOption;


    }
    
}
