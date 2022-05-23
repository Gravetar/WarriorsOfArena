
public class StateDeath : State
{
    public StateDeath(Ai enemy, StateMachine machine) : base(enemy, machine)
    {

    }

    public override void Enter()
    {
        Enemy.Animator.Death();
    }
}
