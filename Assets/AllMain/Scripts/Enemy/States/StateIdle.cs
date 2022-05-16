public class StateIdle : State
{
    public StateIdle(Ai enemy, StateMachine machine) : base(enemy, machine)
    {

    }

    public override void Enter()
    {
        Enemy.Animator.Idle();
    }

    public override void LogicUpdate()
    {
        if (Enemy.GameManager.isStartedBattle)
        {
            Enemy.StateMachine.ChangeState(Enemy.Movement);
        }
    }
}
