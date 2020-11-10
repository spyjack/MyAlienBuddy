//////////////////////////////////////////////
//Assignment/Lab/Project: Virtual Pet Game
//Name: Terran orion
//Section: 2019SP.SGD.213.2172
//Instructor: Brian Sowers
//Date: 2/11/2020
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject _menuPanel;

    [SerializeField]
    GameObject _instructionsPanel;

    [SerializeField]
    GameObject _adoptPanel;

    [SerializeField]
    InputField _nameInput;

    [SerializeField]
    InputField _nameRestartInput;

    [SerializeField]
    Text _deathScreenText;

    [SerializeField]
    Button _adoptButton;

    [SerializeField]
    Button _adoptRestartButton;

    [SerializeField]
    PetController _petController;

    [SerializeField]
    private PetData _petData;
    // Start is called before the first frame update
    void Start()
    {
        _menuPanel.SetActive(true);
        _instructionsPanel.SetActive(false);
        _adoptPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Disables buttons if there is no input
        if (_menuPanel.activeSelf)
        {
            if (_nameInput.text == "")
            {
                _adoptButton.interactable = false;
            }
            else
            {
                _adoptButton.interactable = true;
            }
        }

        if (_adoptPanel.activeSelf)
        {
            if (_nameRestartInput.text == "")
            {
                _adoptRestartButton.interactable = false;
            }
            else
            {
                _adoptRestartButton.interactable = true;
            }
        }
    }

    //Enable the instructions for the first adoption
    public void DoFirstAdopt()
    {
        _instructionsPanel.SetActive(true);
    }

    //Adopt a new pet
    public void DoAdopt(InputField nameInput)
    {
        _petController.SpawnPet(new Pet(nameInput.text, _petData._petSpecies[Random.Range(0,_petData._petSpecies.Count)], Random.Range(100,501), Random.Range(100, 501), Random.Range(100, 501), Random.Range(100, 501), RandomColor()));
        _menuPanel.SetActive(false);
        _instructionsPanel.SetActive(false);
        _adoptPanel.SetActive(false);
    }

    //Close the game
    public void DoQuit()
    {
        Application.Quit();
    }

    //Display the death screen
    public void DoDeathScreen(string message)
    {
        _adoptPanel.SetActive(true);
        _deathScreenText.text = message;
    }

    //Returns a random color with definable minimums
    public Color RandomColor(float minR = 0, float minG = 0, float minB = 0)
    {
        Color color = new Color();

        color.r = Random.Range(minR, 1f);
        color.g = Random.Range(minG, 1f);
        color.b = Random.Range(minB, 1f);
        color.a = 1;

        return color;
    }
}
