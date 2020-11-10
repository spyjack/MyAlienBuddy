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
using System.IO;

public class PetData : MonoBehaviour
{
    string readPath;
    public List<string> _petSpecies = new List<string>();

    [Header("Pet Bodyparts")]
    public List<Sprite> _heads = new List<Sprite>();
    public List<Sprite> _eyes = new List<Sprite>();
    public List<MouthStates> _mouths = new List<MouthStates>();
    public List<Sprite> _torsos = new List<Sprite>();
    public List<Sprite> _arms = new List<Sprite>();
    public List<Sprite> _hands = new List<Sprite>();
    public List<Sprite> _legs = new List<Sprite>();
    public List<Sprite> _feet = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        readPath = Application.dataPath + "/Resources/species.txt";
        GetWordsFromFile(readPath);
    }

    //Reads the file from the file path
    public void GetWordsFromFile(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        string txt = "";
        while((txt = reader.ReadLine()) != null)
        {
            _petSpecies.Add(txt);
        }
        reader.Close();
    }

    [System.Serializable]
    public struct MouthStates
    {
        [SerializeField]
        public Sprite mouthHappy;
        [SerializeField]
        public Sprite mouthUnhappy;
        [SerializeField]
        public Sprite mouthSad;
    }
}
