/*
Класс узла дерева

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

[System.Serializable]
public abstract class Node
{
    protected NodeState _nodeState;
    public NodeState NodeState { get { return _nodeState; } }

    public abstract NodeState Evaluate();
}

/// <summary>
/// Состояние узла
/// </summary>
public enum NodeState
{
    RUNNING, SOCCESS, FAILURE
}
