/*
����������� ���� ���������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/
public abstract class State
{
    protected Ai Enemy; // ���������
    protected StateMachine Machine; // ������ ��������� ����������

    // �����������
    protected State(Ai enemy, StateMachine machine)
    {
        Enemy = enemy;
        Machine = machine;
    }

    /// <summary>
    /// �����, ������������ ���� � ���������
    /// </summary>
    public virtual void Enter()
    {}

    /// <summary>
    /// �����, ������������ ����������� �������� �� ����� ������ ���������
    /// </summary>
    public virtual void LogicUpdate()
    {}

    /// <summary>
    /// �����, ������������ ����� �� ���������
    /// </summary>
    public virtual void Exit()
    {}
}
