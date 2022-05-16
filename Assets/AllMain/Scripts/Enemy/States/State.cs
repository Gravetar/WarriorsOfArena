public abstract class State
{
    protected Ai Enemy;
    protected StateMachine Machine;
    protected State(Ai enemy, StateMachine machine)
    {
        Enemy = enemy;
        Machine = machine;
    }
    public virtual void Enter()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void Exit()
    {

    }
}
