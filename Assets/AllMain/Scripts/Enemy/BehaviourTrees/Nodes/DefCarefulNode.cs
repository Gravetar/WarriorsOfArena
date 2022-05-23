public class DefCarefulNode : Node
{
    private EnemyAi _ai;
    private ActionPlayer _actionPlayer;
    private int _favoriteIdWeaponPlayer;

    public DefCarefulNode(EnemyAi ai, ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Attack && _favoriteIdWeaponPlayer == 3)
        {
            _ai.SetWeapon(2);
            _ai.SetStrength(2);
            _ai.SetDexterity(2);
            _ai.EnemyTactic = EnemyAi.TacticEnemy.Passive;
            return NodeState.SOCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }
    }
}
