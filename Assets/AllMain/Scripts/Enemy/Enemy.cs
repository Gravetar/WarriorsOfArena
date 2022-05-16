
public class Enemy
{
    private int _level;
    private int _experience;

    private int _strength;
    private int _dexterity;
    private int _health;
    private int _maxHealth;
    private int _stamina;
    private int _maxStamina;

    public Enemy(int experience, int strength, int dexterity)
    {
        _experience = experience;
        _level = experience / 500;

        _strength = strength;
        _dexterity = dexterity;

        _health = 50 + (10 * _level) + (5 * _strength);
        _maxHealth = _health;
        _stamina = 100 + (10 * _dexterity);
        _maxStamina = _stamina;
    }

    public int Level { get { return _level; } }
    public int Health { get { return _health; } set { _health = value; } }
    public int MaxHealth { get { return _maxHealth; } }
    public int Stamina { get { return _stamina; } set { _stamina = value; } }
    public int MaxStamina { get { return _maxStamina; } }
    public int Experience { get { return _experience; } set { _experience = value; } }
    public int Strength { get { return _strength; } set { _strength = value; } }
    public int Dexterity { get { return _dexterity; } set { _dexterity = value; } }
}
