/*
����� ���� ������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

[System.Serializable]
public abstract class Node
{
    protected NodeState _nodeState;
    public NodeState NodeState { get { return _nodeState; } }

    public abstract NodeState Evaluate();
}

/// <summary>
/// ��������� ����
/// </summary>
public enum NodeState
{
    RUNNING, SOCCESS, FAILURE
}
