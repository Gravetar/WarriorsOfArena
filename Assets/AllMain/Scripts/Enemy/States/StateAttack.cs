/*
��������� ����� ����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using UnityEngine;

public class StateAttack : State
{
    private bool _canAttack = true; // ����������� ���������
    private int _stateAttack = 0; // ����� � ����� ����
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="enemy">����</param>
    /// <param name="machine">������ ��������� �����</param>
    public StateAttack(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// ����������� �������� � ���������
    /// </summary>
    public override void LogicUpdate()
    {
        Enemy.Agent.isStopped = true; // ���������� ������
        Enemy.Agent.velocity = Vector3.zero;
        // ����������� � ������
        var targetRotation = Quaternion.LookRotation(Enemy.Player.transform.position - Enemy.Self.transform.position);
        Enemy.Self.transform.rotation = Quaternion.Slerp(Enemy.Self.transform.rotation, targetRotation, 5 * Time.deltaTime);

        if (Enemy.Enemy.Stamina < Enemy.Weapons[Enemy.ActiveWeaponId].NeedStamina) // ���� ������������ ���������� �� ������ �� ��������� ����.
        {
            Enemy.StateMachine.ChangeState(Enemy.Block); // ������� ��������� �� ����
        }
        else if (Enemy.DistanceBetweenEnemyAndPlayer > 1f) // ���� ���������� �� ��������� ������ ������ 1
        {
            if (Enemy.Animator.CheckEndAnimation()) Enemy.StateMachine.ChangeState(Enemy.Movement); // ��������� ����������� �� ���������� ��������, ������� ��������� �� ��������
        }
        else if (_canAttack) // ���� ����� ���������
        {
            ChangeAttack(); // ���������
            Enemy.Enemy.Stamina -= Enemy.Weapons[Enemy.ActiveWeaponId].NeedStamina; // ������ ������������
            _canAttack = false; // ��������� ���������
        }
    }

    /// <summary>
    /// ����� �� ���������
    /// </summary>
    public override void Exit()
    {
        ResetAttack(); // �������� �����
        Enemy.Animator.Movement(); // ���������� � ��������� ��������
    }

    /// <summary>
    /// ���������
    /// </summary>
    private void ChangeAttack()
    {
        if (_stateAttack == 0) // ���� ����� ���� �� 0
        {
            _stateAttack = 1; // ������� � ��������� ����� ����� ����
            Enemy.Animator.Attack(1); // ���������� � ��������� ����� ������� ����
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

    /// <summary>
    /// �������� �����
    /// </summary>
    public void ResetAttack()
    {
        _stateAttack = 0; // �������� ����� �����
        _canAttack = true; // ��������� ���������
        Enemy.Animator.Attack(0); // ���������� � ��������� ����� ����� 0
    }

    /// <summary>
    /// ��������� �����
    /// </summary>
    public void EndAttack()
    {
        _canAttack = true; // ��������� ���������
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Hit()
    {
        if (Enemy.ShotPlayer) // ���� ����� ��������� ������
        {
            if (Enemy.PinaltyDamage) Enemy.Player.GetComponent<PlayerManager>().GetDamage(5); // ���� ���� ����� � �����-������� 5 ����� ��������� ������
            else Enemy.Player.GetComponent<PlayerManager>().GetDamage(Enemy.Weapons[Enemy.ActiveWeaponId].Damage + Enemy.Enemy.Strength); // ����� - ������� ���� ������ ��������� ������
            if (Enemy.Player.GetComponent<PlayerManager>().Player.Health <= 0) // ���� �������� ������ �������� 0
            {
                Enemy.StateMachine.ChangeState(Enemy.Idle); // ������� ��������� �� ��������
            }
        }
        _canAttack = true; // ��������� ���������
        Enemy.ShotPlayer = false; // ���� �� ����� ��������� ������
    }
}
