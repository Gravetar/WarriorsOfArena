public class RusherNode : Node
{
    private EnemyAi _ai;
    private ActionPlayer _actionPlayer;
    private int _favoriteIdWeaponPlayer;

    public RusherNode(EnemyAi ai, ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Block && _favoriteIdWeaponPlayer == 3)
        {
            _ai.SetWeapon(1);
            _ai.SetStrength(1);
            _ai.SetDexterity(3);
            _ai.EnemyTactic = EnemyAi.TacticEnemy.Aggresive;
            return NodeState.SOCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
