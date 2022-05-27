/*
Узел Убегающего дерева поведения

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

public class RunAwayCarefulNode : Node
{
    private Ai _ai;// Система ИИ
    private ActionPlayer _actionPlayer;// Система ИИ
    private int _favoriteIdWeaponPlayer;// Предпочитаемое оружие игрока

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="ai">Система ИИ</param>
    /// <param name="actionPlayer">// Предпочитаемое действие игрока</param>
    /// <param name="favoriteIdWeaponPlayer">// Предпочитаемое оружие игрока</param>
    public RunAwayCarefulNode(Ai ai, ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    /// <summary>
    /// Оценка узла
    /// </summary>
    /// <returns>Состояние узла</returns>
    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Attack && _favoriteIdWeaponPlayer == 1) // Если игрок предпочитал атаковать и его предпочитаемое оружие - топор
        {
            _ai.SetWeapon(0);// Установить оружие врага - меч

            //Установить коэффициенты силы и ловкости
            _ai.SetStrength(1);
            _ai.SetDexterity(3);

            _ai.EnemyTactic = TacticEnemy.Passive; //Установить тактику пассивную
            return NodeState.SOCCESS;// Успешное выполнение узла
        }
        else
        {
            return NodeState.FAILURE;// Провал выполнения узла
        }
    }
}
