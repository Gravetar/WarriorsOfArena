using UnityEngine;

public class StateMovement : State
{
    public StateMovement(Ai enemy, StateMachine machine) : base(enemy, machine)
    {

    }

    public override void Enter()
    {
        Enemy.Agent.isStopped = false; ;
        Enemy.Animator.Movement();
    }

    public override void LogicUpdate()
    {
        Enemy.Agent.SetDestination(Enemy.Player.position);
        if (Enemy.DistanceBetweenEnemyAndPlayer < 1f)
        {
            Enemy.StateMachine.ChangeState(Enemy.Attack);
        }
    }
}
