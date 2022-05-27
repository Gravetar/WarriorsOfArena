/*
��������� ������ ����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

public class StateDeath : State
{
    /// <summary>
    /// �����������
    /// </summary>
    /// <param name="enemy">����</param>
    /// <param name="machine">������ ��������� �����</param>
    public StateDeath(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// ���� � ���������
    /// </summary>
    public override void Enter()
    {
        Enemy.Animator.Death(); // ���������� � ��������� Death
    }
}
