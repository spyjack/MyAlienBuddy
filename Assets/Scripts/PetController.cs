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

public class PetController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private Image _healthBar;

    [Header("Hunger Variables")]
    [SerializeField]
    private Image _hungerBar;
    [SerializeField]
    private float _metabolismTickTimer = 1;
    private float _metabolismTick = 0;
    [SerializeField]
    private float _saturationTickTimer = 1;
    private float _saturationTick = 0;

    [Header("Happiness")]
    [SerializeField]
    private Image _happinessBar;
    [SerializeField]
    private float _happinessTickTimer = 1;
    private float _happinessTick = 0;

    [Header("Energy")]
    [SerializeField]
    private Image _energyBar;
    [SerializeField]
    private float _energyTickTimer = 1;
    private float _energyTick = 0;

    [SerializeField]
    private float _gameTickInSeconds = 0;
    private float _gameTick = 0;

    [SerializeField]
    private float _buttonTickTimer = 1;
    private float _buttonTick = 0;

    [Header("Texts")]
    [SerializeField]
    private Text _petNameText;

    [SerializeField]
    private PetData _petData;

    private Pet _myPet;

    [Header("Audio")]
    [SerializeField]
    private List<AudioClip> _soundList;

    [SerializeField]
    private AudioSource _soundEmitter;

    private Sprite _mouthHappy;
    private Sprite _mouthFrown;
    private Sprite _mouthSad;

    [SerializeField]
    private PetBody _myPetBody;
    // Start is called before the first frame update
    void Start()
    {
        //_myPet = new Pet("test", 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (_myPet != null)
        {
            DoGameTick();
        }
    }

    //Every x seconds do a game tick which controls health, hunger, happiness, and energy levels
    void DoGameTick()
    {
        if (_gameTick >= _gameTickInSeconds)
        {
            _gameTick = 0;
            UpdateHealth();
            UpdateHunger();
            UpdateHappiness();
            UpdateEnergy();
            if (Timer(ref _buttonTick, _buttonTickTimer))
            {
                _buttonTick = -1;
            }
        }
        else
        {
            _gameTick += Time.deltaTime;
        }
    }

    //Updates all the pets health related information
    void UpdateHealth()
    {
        if (_myPet.IsDying && !_myPet.IsDead)
        {
            _myPet.Hurt(5);
            _myPet.MakeSad(10);
            
            if (Random.Range(0,11) < 2)
            {
                _soundEmitter.PlayOneShot(_soundList[Random.Range(3, 6)]);
                _soundEmitter.pitch = Random.Range(0.5f, 1.75f);
            }
            
        }
        _healthBar.fillAmount = _myPet.GetHealth;
        if (_myPet.IsDead)
        {
            string message = "Your alien buddy " + _myPet.Name + " of the species " + _myPet.Species + " has died... \n Don't worry, you can adopt a new one!";
            FindObjectOfType<MenuController>().DoDeathScreen(message);
        }
    }

    //Updates all the pets hunger related information
    void UpdateHunger()
    {
        if (DoMetabolism(_metabolismTickTimer) && !_myPet.IsSaturated)
        {
            if (_myPet.Fullness > 0)
            {
                _myPet.MakeHungry(5);
            }
        }
        if (_myPet.IsSaturated)
        {
            DoSaturation(_saturationTickTimer);
        }
        DoStarvation();
        _hungerBar.fillAmount = _myPet.GetFullness;
    }

    //Returns true if the metabolism tick is greater than or equal to the desired tick timer
    bool DoMetabolism(float tickTimer)
    {
        bool isHungery = false;
        if (Timer(ref _metabolismTick, _metabolismTickTimer))
        {
            isHungery = true;
        }
        return isHungery;
    }

    //Disables pet saturation if the saturation tick is greater than or equal to the desired tick timer
    void DoSaturation(float tickTimer)
    {
        if (Timer(ref _saturationTick, _saturationTickTimer))
        {
            _myPet.IsSaturated = false;
        }
    }

    bool DoStarvation()
    {
        bool isStarving = false;
        if (_myPet.IsStarving)
        {
            isStarving = true;
            _myPet.MakeTired(2);
        }
        return isStarving;
    }

    //Every tick update happiness
    void UpdateHappiness()
    {
        DoMoodSwings();
        ChangeExpression();
        if (_myPet.Happiness <= 0)
        {
            _myPet.MakeTired(Random.Range(1,11));
        }
        _happinessBar.fillAmount = _myPet.GetHappiness;
    }

    //Make the pet sad
    void DoMoodSwings()
    {
        if (Timer(ref _happinessTick, _happinessTickTimer))
        {
            _myPet.MakeSad(5);
        }
    }

    //Based on happiness change the facial expression
    void ChangeExpression()
    {
        SpriteRenderer spriteRenderer = _myPetBody.mouth.GetComponent<SpriteRenderer>();
        if (_myPet.GetHappiness > 0.5f)
        {
            spriteRenderer.sprite = _mouthHappy;
        }
        else if (_myPet.GetHappiness > 0.3f)
        {
            spriteRenderer.sprite = _mouthFrown;
        }
        else
        {
            spriteRenderer.sprite = _mouthSad;
        }
    }

    void UpdateEnergy()
    {
        DoTire();
        _energyBar.fillAmount = _myPet.GetEnergy;
    }

    //Make the pet tired
    void DoTire()
    {
        if (Timer(ref _energyTick, _energyTickTimer))
        {
            _myPet.MakeTired(5);
        }
    }

    //Do a timer, return bool if it has compeleted or not
    bool Timer(ref float time, float tickTimer)
    {
        if (time >= tickTimer)
        {
            time = 0;
            return true;
        }
        else if (time > -1)
        {
            time += 1;
            return false;
        }
        return false;
    }

    //Return if the timer has activated or not without modifying it.
    bool GetTimer(float time, float tickTimer)
    {
        if (time >= tickTimer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Feed the pet. Increases food, slightly heals, increases happiness, increases energy
    public void FeedPet()
    {
        if (_buttonTick == -1 && !_myPet.IsDead)
        {
            if (!_myPet.IsSaturated)
            {
                int amount = Random.Range(10, 26);
                _myPet.Eat(amount);
                _myPet.Heal(2);
                _myPet.Play(Mathf.Round(amount / 3));
                _myPet.Rest(Mathf.Round(amount / 5));
                _soundEmitter.PlayOneShot(_soundList[0]);
                _soundEmitter.pitch = Random.Range(0.75f, 1.5f);
                _buttonTick = 0;
                FindObjectOfType<ParticleSystem>().Play();
            }
            else { print("Pet is Full"); }
        }
    }

    //Make the pet play. Increases happiness, lowers energy, and lowers fullness
    public void PlayWithPet()
    {
        if (_buttonTick == -1 && !_myPet.IsDead)
        {
            int amount = Random.Range(10, 26);
            _myPet.Play(amount);
            _myPet.MakeTired(Mathf.Round(amount / 2));
            _myPet.MakeHungry(Mathf.Round(amount / 3));

            _soundEmitter.PlayOneShot(_soundList[1]);
            _soundEmitter.pitch = Random.Range(0.5f, 1.75f);

            _buttonTick = 0;
        }
    }

    //Make the pet sleep. Increases energy, lowers fullness, and increases health
    public void RestPet()
    {
        if (_buttonTick == -1 && !_myPet.IsDead)
        {
            int amount = Random.Range(10, 26);
            _myPet.Rest(amount);
            _myPet.MakeHungry(Mathf.Round(amount / 4));
            _myPet.Heal(Mathf.Round(amount / 2));
            _soundEmitter.PlayOneShot(_soundList[2]);
            _soundEmitter.pitch = Random.Range(0.5f, 2f);
            _buttonTick = 0;
        }
        
    }

    //Spawns a pet. Sets all the body parts up.
    public void SpawnPet(Pet pet)
    {
        if (_myPet == null || _myPet.IsDead)
        {
            _myPet = pet;

            SetPetColor(_myPet.Color);

            _myPetBody.head.GetComponent<SpriteRenderer>().sprite = _petData._heads[Random.Range(1, _petData._heads.Count)];
            _myPetBody.eyeL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.eyeR.GetComponent<SpriteRenderer>().sprite = _petData._eyes[Random.Range(1, _petData._eyes.Count)]);

            //Set mouths
            int mouthIndex = Random.Range(0, _petData._mouths.Count);
            print(mouthIndex);
            _mouthHappy = _petData._mouths[mouthIndex].mouthHappy;
            _mouthFrown = _petData._mouths[mouthIndex].mouthUnhappy;
            _mouthSad = _petData._mouths[mouthIndex].mouthSad;
            _myPetBody.mouth.GetComponent<SpriteRenderer>().sprite = _mouthHappy;

            _myPetBody.torso.GetComponent<SpriteRenderer>().sprite = _petData._torsos[Random.Range(1, _petData._torsos.Count)];

            _myPetBody.armL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.armR.GetComponent<SpriteRenderer>().sprite = _petData._arms[Random.Range(1, _petData._arms.Count)]);
            _myPetBody.handL.GetComponent<SpriteRenderer>().sprite = _petData._hands[Random.Range(1, _petData._hands.Count)];
            _myPetBody.handR.GetComponent<SpriteRenderer>().sprite = _petData._hands[Random.Range(1, _petData._hands.Count)];
            _myPetBody.legL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.legR.GetComponent<SpriteRenderer>().sprite = _petData._legs[Random.Range(1, _petData._legs.Count)]);
            _myPetBody.footL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.footR.GetComponent<SpriteRenderer>().sprite = _petData._feet[Random.Range(1, _petData._feet.Count)]);

            if (_myPet.Name.ToLower() == "debugger")
            {
                SpawnDebuggerDummer();
            }
            _petNameText.text = _myPet.Name + " the " + _myPet.Species;
        }
    }

    //Spawns a special debugger dummy model instead of a random one.
    private void SpawnDebuggerDummer()
    {
        _myPet.Species = "Dummy";

        _myPet.Color = Color.white;

        _myPetBody.head.GetComponent<SpriteRenderer>().sprite = _petData._heads[0];
        _myPetBody.eyeL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.eyeR.GetComponent<SpriteRenderer>().sprite = _petData._eyes[0]);

        //Set mouths
        _mouthHappy = _petData._mouths[0].mouthHappy;
        _mouthFrown = _petData._mouths[0].mouthUnhappy;
        _mouthSad = _petData._mouths[0].mouthSad;
        _myPetBody.mouth.GetComponent<SpriteRenderer>().sprite = _mouthHappy;

        _myPetBody.torso.GetComponent<SpriteRenderer>().sprite = _petData._torsos[0];

        _myPetBody.armL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.armR.GetComponent<SpriteRenderer>().sprite = _petData._arms[0]);
        _myPetBody.handL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.handR.GetComponent<SpriteRenderer>().sprite = _petData._hands[0]);
        _myPetBody.legL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.legR.GetComponent<SpriteRenderer>().sprite = _petData._legs[0]);
        _myPetBody.footL.GetComponent<SpriteRenderer>().sprite = (_myPetBody.footR.GetComponent<SpriteRenderer>().sprite = _petData._feet[0]);

        SetPetColor(Color.white);
    }

    private void SetPetColor(Color color)
    {
        //Inefficently set the color
        _myPetBody.head.GetComponent<SpriteRenderer>().color = color;

        _myPetBody.torso.GetComponent<SpriteRenderer>().color = color;

        _myPetBody.armL.GetComponent<SpriteRenderer>().color = (_myPetBody.armR.GetComponent<SpriteRenderer>().color = color);
        _myPetBody.handL.GetComponent<SpriteRenderer>().color = (_myPetBody.handR.GetComponent<SpriteRenderer>().color = color);
        _myPetBody.legL.GetComponent<SpriteRenderer>().color = (_myPetBody.legR.GetComponent<SpriteRenderer>().color = color);
        _myPetBody.footL.GetComponent<SpriteRenderer>().color = (_myPetBody.footR.GetComponent<SpriteRenderer>().color = color);
    }
}

[System.Serializable]
public struct PetBody
{
    public Transform head;
    public Transform eyeL;
    public Transform eyeR;
    public Transform mouth;

    public Transform torso;

    public Transform armL;
    public Transform handL;

    public Transform armR;
    public Transform handR;

    public Transform legL;
    public Transform footL;

    public Transform legR;
    public Transform footR;
}
