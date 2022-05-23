
using static EnemyAi;

public class Enemy
{
    private TacticEnemy _tactic;
    private int _weapon;
    private int _level;
    private int _strength;
    private int _dexterity;
    private int _health;
    private int _maxHealth;
    private int _stamina;
    private int _maxStamina;

    public Enemy(TacticEnemy tactic, int level, int strength, int dexterity, int weapon)
    {
        _level = level;

        _strength = strength;
        _dexterity = dexterity;

        _health = 50 + (10 * _level) + (5 * _strength);
        _maxHealth = _health;
        _stamina = 100 + (10 * _dexterity);
        _maxStamina = _stamina;

        _tactic = tactic;
        _weapon = weapon;
    }

    public int Level { get { return _level; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int Stamina { get { return _stamina; } set { _stamina = value; } }
    public int MaxStamina { get { return _maxStamina; } }
    public int Strength { get { return _strength; } set { _strength = value; } }
    public int Dexterity { get { return _dexterity; } set { _dexterity = value; } }
    public int Weapon { get { return _weapon; } set { _weapon = value; } }
}
