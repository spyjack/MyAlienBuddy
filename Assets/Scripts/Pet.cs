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

public class Pet
{
    private string _name;
    private string _species;

    private Color _color;

    private float _healthMax;
    private float _fullnessMax;
    private float _happinessMax;
    private float _energyMax;

    private float _health;
    private float _fullness;
    private float _happiness;
    private float _energy;

    private bool _isSaturated = false;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    public string Species
    {
        get { return _species; }
        set { _species = value; }
    }

    public Color Color
    {
        get { return _color; }
        set { _color = value; }
    }

    public float Health
    {
        get { return _health; }
    }

    public float GetHealth
    {
        get { return _health / _healthMax; }
    }

    public float Fullness
    {
        get { return _fullness; }
    }

    public float GetFullness
    {
        get { return _fullness / _fullnessMax; }
    }

    public float Happiness
    {
        get { return _happiness; }
    }

    public float GetHappiness
    {
        get { return _happiness / _happinessMax; }
    }

    public float Energy
    {
        get { return _energy; }
    }

    public float GetEnergy
    {
        get { return _energy / _energyMax; }
    }
    public bool IsSaturated
    {
        get { return _isSaturated; }
        set { _isSaturated = value; }
    }
    public bool IsDying
    {
        get
        {
            bool isDying = false;
            if ((_fullness <= 0 && _energy <= 0))
            {
                isDying = true;
            }
            return isDying;
        }
    }
    public bool IsStarving
    {
        get
        {
            if (_fullness <= 0)
            {
                return true;
            }
            return false;
        }
    }
    public bool IsDead
    {
        get
        {
            if (_health <= 0)
            {
                return true;
            }
            return false;
        }
    }

    //Construct default pet
    public Pet(string name, float healthMax)
    {
        _name = name;
        _healthMax = healthMax;

        _fullnessMax = 100;
        _happinessMax = 100;
        _energyMax = 100;

        SetStatsToMax();
    }

    //Construct Complex Pet
    public Pet(string name, string species, float healthMax, float fullnessMax, float happinessMax, float energyMax, Color color)
    {
        _name = name;
        _species = species;

        _healthMax = healthMax;

        _fullnessMax = fullnessMax;
        _happinessMax = happinessMax;
        _energyMax = energyMax;

        _color = color;

        SetStatsToMax();
    }

    public float Heal(float healthAmount)
    {
        return DoStatIncrease(ref _health, healthAmount, _healthMax);
    }

    public float Hurt(float damage)
    {
        return DoStatDecrease(ref _health, damage, _healthMax, 0);
    }

    //Add the food amount to their fullness if they are hungry, returns 0-1 of amount of fullness, makes the pet saturated
    public float Eat(float foodAmount)
    {
        if (_fullness + foodAmount >= _fullnessMax)
        {
            IsSaturated = true;
        }
        return DoStatIncrease(ref _fullness, foodAmount, _fullnessMax);
    }

    public float MakeHungry(float hungerAmount)
    {
        IsSaturated = false;
        return DoStatDecrease(ref _fullness, hungerAmount, _fullnessMax, 0);
    }

    public float Play(float happinessAmount)
    {
        return DoStatIncrease(ref _happiness, happinessAmount, _happinessMax);
    }

    public float MakeSad(float saddnessAmount)
    {
        return DoStatDecrease(ref _happiness, saddnessAmount, _happinessMax, 0);
    }

    public float Rest(float restAmount)
    {
        return DoStatIncrease(ref _energy, restAmount, _energyMax);
    }

    public float MakeTired(float tiredAmount)
    {
        return DoStatDecrease(ref _energy, tiredAmount, _energyMax, 0);
    }

    //Increase the statToChange by the statChangeAmount
    private float DoStatIncrease(ref float statToChange, float statChangeAmount, float statMax)
    {
        if (statToChange + statChangeAmount <= statMax)
        {
            statToChange += statChangeAmount;
        }
        else if (statToChange + statChangeAmount > statMax && statToChange < statMax)
        {
            statToChange = statMax;
        }

        return statToChange / statMax;
    }

    //Lowers the statToChange by the statChangeAmount
    private float DoStatDecrease(ref float statToChange, float statChangeAmount, float statMax, float statMin)
    {
        if (statToChange - statChangeAmount >= statMin)
        {
            statToChange -= statChangeAmount;
        }
        else if (statToChange - statChangeAmount < statMin)
        {
            statToChange = statMin;
        }

        return statToChange / statMax;
    }

    private void SetStatsToMax()
    {
        _health = _healthMax;
        _fullness = _fullnessMax;
        _happiness = _happinessMax;
        _energy = _energyMax;
    }
}
