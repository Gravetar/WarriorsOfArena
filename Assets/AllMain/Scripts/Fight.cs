/*
Класс боя

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/
public class Fight
{
    private bool _playerWinner;
    private int _favoriteWeaponPlayer;
    private ActionPlayer _favoriteActionPlayer;
    private int _fightNumber;
    private float _fightTime;

    public Fight(bool playerWinner, int favoriteWeaponPlayer, ActionPlayer favoriteActionPlayer, int fightNumbe, float fightTime)
    {
        _playerWinner = playerWinner;
        _favoriteWeaponPlayer = favoriteWeaponPlayer;
        _favoriteActionPlayer = favoriteActionPlayer;
        _fightNumber = fightNumbe;
        _fightTime = fightTime;
    }

    /// <summary>
    /// Стандартный бой
    /// </summary>
    public Fight()
    {
        _playerWinner = true;
        _favoriteWeaponPlayer = 0;
        _favoriteActionPlayer = ActionPlayer.Attack;
        _fightNumber = 1;
        _fightTime = 0;
    }

    public bool PlayerWinner { get { return _playerWinner; } set { _playerWinner = value; } }
    public int FavoriteWeaponPlayer { get { return _favoriteWeaponPlayer; } set { _favoriteWeaponPlayer = value; } }
    public ActionPlayer FavoriteActionPlayer { get { return _favoriteActionPlayer; } set { _favoriteActionPlayer = value; } }
    public int FightNumber { get { return _fightNumber; } set { _fightNumber = value; } }
    public float FightTime { get { return _fightTime; } set { _fightTime = value; } }
}
