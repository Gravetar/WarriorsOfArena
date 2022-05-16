using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int _id;
    private string _name;

    private int _level;
    private int _experience;
    private int _freeXpPoints;

    private int _strength;
    private int _dexterity;
    private int _health;
    private int _maxHealth;
    private int _stamina;
    private int _maxStamina;

    public Player (int id, string name, int experience, int strength, int dexterity, int freeXpPoints)
    {
        _id = id;
        _name = name;
        _experience = experience;
        _level = experience / 500;
        _freeXpPoints = freeXpPoints;

        _strength = strength;
        _dexterity = dexterity;

        _health = 50 + (10 * _level) + (5 * _strength);
        _maxHealth = _health;
        _stamina = 100 + (10 * _dexterity);
        _maxStamina = _stamina;
    }

    public int Id { get { return _id; } }
    public string Name { get { return _name; } }
    public int Level { get { return _level; } }
    public int FreeXpPoints { get { return _freeXpPoints; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int Stamina { get { return _stamina; } set { _stamina = value; } }
    public int MaxStamina { get { return _maxStamina; } }
    public int Experience { get { return _experience; } set { _experience = value; } }
    public int Strength { get { return _strength; } set { _strength = value; } }
    public int Dexterity { get { return _dexterity; } set { _dexterity = value; } }
}
