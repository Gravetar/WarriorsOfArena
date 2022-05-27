/*
��������� ����� ����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using UnityEngine;
public class StateBlock : State
{
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="enemy">����</param>
    /// <param name="machine">������ ��������� �����</param>
    public StateBlock(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// ���� � ���������
    /// </summary>
    public override void Enter()
    {
        Enemy.Animator.Block(); // ���������� � ��������� Block
        Enemy.Agent.isStopped = true; // ���������� �������� ������
    }


    /// <summary>
    /// ����� �� ���������
    /// </summary>
    public override void Exit()
    {
        Enemy.Animator.Reset(); // �������� ��������
        Enemy.Agent.isStopped = false; // ���������� �������� ������
    }

    public override void LogicUpdate()
    {
        // ������� � ������
        var targetRotation = Quaternion.LookRotation(Enemy.Player.transform.position - Enemy.Self.transform.position);
        Enemy.Self.transform.rotation = Quaternion.Slerp(Enemy.Self.transform.rotation, targetRotation, 5 * Time.deltaTime);

        if (Enemy.Enemy.Stamina > Enemy.Enemy.MaxStamina / 2)// ���� ������������ ����� ������ �������� �� ������������
        {
            Enemy.StateMachine.ChangeState(Enemy.Movement); // ������� ��������� �� ��������
        }
    }
}
