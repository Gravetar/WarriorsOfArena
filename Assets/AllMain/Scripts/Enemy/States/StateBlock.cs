using UnityEngine;

public class StateBlock : State
{
    public StateBlock(Ai enemy, StateMachine machine) : base(enemy, machine)
    {

    }

    public override void Enter()
    {
        Enemy.Animator.Block();
        Enemy.Agent.isStopped = true;
    }

    public override void Exit()
    {
        Enemy.Animator.Reset();
        Enemy.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        var targetRotation = Quaternion.LookRotation(Enemy.Player.transform.position - Enemy.Self.transform.position);
        Enemy.Self.transform.rotation = Quaternion.Slerp(Enemy.Self.transform.rotation, targetRotation, 5 * Time.deltaTime);

        if (Enemy.Enemy.Stamina > Enemy.Enemy.MaxStamina / 2)
        {
            Enemy.StateMachine.ChangeState(Enemy.Movement);
        }
    }
}
