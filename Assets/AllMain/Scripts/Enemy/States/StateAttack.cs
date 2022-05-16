using UnityEngine;

public class StateAttack : State
{
    private bool _canAttack = true;
    private int _stateAttack = 0;
    public StateAttack(Ai enemy, StateMachine machine) : base(enemy, machine)
    {

    }
    public override void Enter()
    {

    }

    public override void LogicUpdate()
    {
        Enemy.Agent.isStopped = true;
        Enemy.Agent.velocity = Vector3.zero;
        var targetRotation = Quaternion.LookRotation(Enemy.Player.transform.position - Enemy.Self.transform.position);
        Enemy.Self.transform.rotation = Quaternion.Slerp(Enemy.Self.transform.rotation, targetRotation, 5 * Time.deltaTime);
        if (Enemy.Enemy.Stamina < Enemy.Weapons[Enemy.IdActiveWeapon].NeedStamina)
        {
            Enemy.StateMachine.ChangeState(Enemy.Block);
        }
        else if (Enemy.DistanceBetweenEnemyAndPlayer > 1f)
        {
            if (Enemy.Animator.CheckEndAnimation()) Enemy.StateMachine.ChangeState(Enemy.Movement);
        }
        else if (_canAttack)
        {
            ChangeAttack();
            Enemy.Enemy.Stamina -= Enemy.Weapons[Enemy.IdActiveWeapon].NeedStamina;
            _canAttack = false;
        }
    }

    public override void Exit()
    {
        ResetAttack();
        Enemy.Animator.Movement();
    }

    private void ChangeAttack()
    {
        if (_stateAttack == 0)
        {
            _stateAttack = 1;
            Enemy.Animator.Attack(1);
        }
        else if (_stateAttack == 1)
        {
            _stateAttack = 2;
            Enemy.Animator.Attack(2);
        }
        else if (_stateAttack == 2)
        {
            _stateAttack = 3;
            Enemy.Animator.Attack(3);
        }
        else if (_stateAttack == 3)
        {
            _stateAttack = 4;
            Enemy.Animator.Attack(4);
        }
        else if (_stateAttack == 4)
        {
            _stateAttack = 0;
            Enemy.Animator.Attack(0);
        }
    }

    public void ResetAttack()
    {
        _stateAttack = 0;
        _canAttack = true;
        Enemy.Animator.Attack(0);
    }
    public void EndAttack()
    {
        _canAttack = true;
    }

    public void Hit()
    {
        if (Enemy.ShotPlayer)
        {
            if (Enemy.PinaltyDamage) Enemy.Player.GetComponent<PlayerManager>().GetDamage(5);
            else Enemy.Player.GetComponent<PlayerManager>().GetDamage(Enemy.Weapons[Enemy.IdActiveWeapon].Damage);
            if (Enemy.Player.GetComponent<PlayerManager>().Player.Health <= 0)
            {
                Debug.Log("PLAYER DEAD");
            }
        }
        _canAttack = true;
        Enemy.ShotPlayer = false;
    }
}
