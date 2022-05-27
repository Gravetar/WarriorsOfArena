/*
�������� ����� ������ ��������� - �������� ���������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

public class StateMachine
{
    /// <summary>
    /// ������� ���������
    /// </summary>
    public State CurrentState;

    /// <summary>
    /// ������������� �������� ���������
    /// </summary>
    /// <param name="startingState">��������� ���������</param>
    public void Initialize (State startingState)
    {
        CurrentState = startingState; // ���������� � �������� �������� ��������� - ��������� 
        startingState.Enter(); // ����� � ���������
    }

    /// <summary>
    /// ����������� ���������� - �������� � ���������
    /// </summary>
    public void LogicUpdate()
    {
        CurrentState?.LogicUpdate();
    }

    /// <summary>
    /// ������� ���������
    /// </summary>
    /// <param name="newState">����� ���������</param>
    public void ChangeState(State newState)
    {
        CurrentState.Exit(); // ����� �� �������� ���������

        CurrentState = newState; // ���������� � �������� �������� ��������� - �����
        newState.Enter(); // ����� � ����� ���������
    }
}
