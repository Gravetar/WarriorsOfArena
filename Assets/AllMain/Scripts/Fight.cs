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
}
