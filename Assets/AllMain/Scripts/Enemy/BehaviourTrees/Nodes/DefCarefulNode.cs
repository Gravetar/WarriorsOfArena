/*
Узел оборонительного воина дерева поведения

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/
public class DefCarefulNode : Node
{
    private Ai _ai;// Система ИИ
    private ActionPlayer _actionPlayer;// Предпочитаемое действие игрока
    private int _favoriteIdWeaponPlayer;// Предпочитаемое оружие игрока

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="ai">Система ИИ</param>
    /// <param name="actionPlayer">Предпочитаемое действие игрока</param>
    /// <param name="favoriteIdWeaponPlayer">Предпочитаемое оружие игрока</param>
    public DefCarefulNode(Ai ai, ActionPlayer actionPlayer, int favoriteIdWeaponPlayer)
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
        if (_actionPlayer == ActionPlayer.Attack && _favoriteIdWeaponPlayer == 2) // Если игрок предпочитал атаковать и его предпочитаемое оружие - булава
        {
            _ai.SetWeapon(1);// Установить оружие врага - меч

            //Установить коэффициенты силы и ловкости
            _ai.SetStrength(2);
            _ai.SetDexterity(2);

            _ai.EnemyTactic = TacticEnemy.Passive;//Установить тактику пассивную
            return NodeState.SOCCESS;// Успешное выполнение узла
        }
        else
        {
            return NodeState.FAILURE;// Провал выполнения узла
        }
    }
}
