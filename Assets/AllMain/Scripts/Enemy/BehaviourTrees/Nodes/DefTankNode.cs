public class DefTankNode : Node
{
    private EnemyAi _ai;
    private ActionPlayer _actionPlayer;
    private int _fightSeconds;
    private int _favoriteIdWeaponPlayer;

    public DefTankNode(EnemyAi ai, ActionPlayer actionPlayer, int fightSeconds, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _fightSeconds = fightSeconds;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Block && _fightSeconds < 60 && _favoriteIdWeaponPlayer == 1)
        {
            _ai.SetWeapon(3);
            _ai.SetStrength(3);
            _ai.SetDexterity(1);
            _ai.SetTactic(EnemyAi.TacticEnemy.Passive);
            return NodeState.SOCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
