public class DamageTankNode : Node
{
    private EnemyAi _ai;
    private EnemyAi.ActionPlayer _actionPlayer;
    private int _fightSeconds;
    private int _favoriteIdWeaponPlayer;

    public DamageTankNode(EnemyAi ai, EnemyAi.ActionPlayer actionPlayer, int fightSeconds, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _fightSeconds = fightSeconds;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    public override NodeState Evaluate()
    {
        if (_actionPlayer == EnemyAi.ActionPlayer.Block && _fightSeconds > 60 && _favoriteIdWeaponPlayer == 1)
        {
            _ai.SetWeapon(3);
            _ai.SetStrength(3);
            _ai.SetDexterity(1);
            _ai.SetTactic(EnemyAi.TacticEnemy.Aggresive);
            return NodeState.SOCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
