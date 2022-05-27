/*
��������� ������������� ������ (��������) �����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

public class StateMovement : State
{
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="enemy">����</param>
    /// <param name="machine">������ ��������� �����</param>
    public StateMovement(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// ���� � ���������
    /// </summary>
    public override void Enter()
    {
        Enemy.Agent.isStopped = false; // ���������� �������� ������
        Enemy.Animator.Movement(); // ���������� � �������� Movement
    }

    /// <summary>
    /// ����������� �������� � ���������
    /// </summary>
    public override void LogicUpdate()
    {
        Enemy.Agent.SetDestination(Enemy.Player.position); // ���������� � �������� ���� ������ - ������
        if (Enemy.DistanceBetweenEnemyAndPlayer < 1f) // ���� ��������� �� ������ ������ 1
        {
            Enemy.StateMachine.ChangeState(Enemy.Attack); // ������� ��������� �� "�����"
        }
    }
}
