/*
Inventor дерева поведения

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

public class Inventor : Node
{
    protected Node node;

    public Inventor (Node node)
    {
        this.node = node;
    }
    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                break;
            case NodeState.SOCCESS:
                _nodeState = NodeState.FAILURE;
                break;
            case NodeState.FAILURE:
                _nodeState = NodeState.SOCCESS;
                break;
            default:
                break;
        }
        return _nodeState;
    }
}
