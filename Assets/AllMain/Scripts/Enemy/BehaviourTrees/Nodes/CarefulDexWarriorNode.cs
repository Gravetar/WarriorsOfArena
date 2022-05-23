public class CarefulDexWarriorNode : Node
{
    private EnemyAi _ai;
    private EnemyAi.ActionPlayer _actionPlayer;
    private int _favoriteIdWeaponPlayer;

    public CarefulDexWarriorNode(EnemyAi ai, EnemyAi.ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    public override NodeState Evaluate()
    {
        if (_actionPlayer == EnemyAi.ActionPlayer.Attack)
        {
            _ai.SetWeapon(1);
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
