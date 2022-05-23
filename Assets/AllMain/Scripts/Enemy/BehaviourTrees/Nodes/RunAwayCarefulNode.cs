public class RunAwayCarefulNode : Node
{
    private EnemyAi _ai;
    private ActionPlayer _actionPlayer;
    private int _favoriteIdWeaponPlayer;

    public RunAwayCarefulNode(EnemyAi ai, ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Attack && _favoriteIdWeaponPlayer == 2)
        {
            _ai.SetWeapon(1);
            _ai.SetStrength(1);
            _ai.SetDexterity(3);
            _ai.EnemyTactic = EnemyAi.TacticEnemy.Passive;
            return NodeState.SOCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
