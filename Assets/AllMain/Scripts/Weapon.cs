using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    int _minDamage;
    int _maxDamage;
    int _needStamina;

    string _name;

    float _attackSpeed;

    public Weapon(int minDamage, int maxDamage, int needStamina, float attackSpeed, string name)
    {
        _minDamage = minDamage;
        _maxDamage = maxDamage;
        _needStamina = needStamina;
        _attackSpeed = attackSpeed;

        _name = name;
    }

    public int Damage
    {
        get 
        {
            return Random.Range(_minDamage, _maxDamage);
        }
    }

    public float AttackSpeed
    {
        get
        {
            return _attackSpeed;
        }
    }

    public int NeedStamina
    {
        get
        {
            return _needStamina;
        }
    }
}
