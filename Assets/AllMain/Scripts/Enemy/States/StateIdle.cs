/*
��������� �������� �����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/
public class StateIdle : State
{
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="enemy">����</param>
    /// <param name="machine">������ ��������� �����</param>
    public StateIdle(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// ���� � ���������
    /// </summary>
    public override void Enter()
    {
        Enemy.Animator.Idle(); // ���������� � ��������� Idle
    }

    /// <summary>
    /// ����������� �������� � ���������
    /// </summary>
    public override void LogicUpdate()
    {
        if (Enemy.GameManager.isStartedBattle) // ���� ��� �������
        {
            Enemy.StateMachine.ChangeState(Enemy.Movement); // ������� � ��������� ��������
        }
    }
}
