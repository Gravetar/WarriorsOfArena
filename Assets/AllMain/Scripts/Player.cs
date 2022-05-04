using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int _level;
    private int _experience;

    private int _strength;
    private int _dexterity;
    private int _health;
    private int _stamina;

    public Player (int experience, int strength, int dexterity)
    {
        _experience = experience;
        _level = experience / 500;

        _strength = strength;
        _dexterity = dexterity;

        _health = 50 + (10 * _level) + (5 * _strength);
        _stamina = 100 + (10 * _dexterity);
    }

    public int Level { get { return _level; } }
    public int Health { get { return _health; } }
    public int Stamina { get { return _stamina; } }
    public int Experience { get { return _experience; } set { _experience = value; } }
    public int Strength { get { return _strength; } set { _strength = value; } }
    public int Dexterity { get { return _dexterity; } set { _dexterity = value; } }
}
