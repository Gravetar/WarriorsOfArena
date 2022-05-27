/*
Узел атакующего танка дерева поведения

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/
public class DamageTankNode : Node
{
    private Ai _ai;// Система ИИ
    private ActionPlayer _actionPlayer;// Предпочитаемое действие игрока
    private float _fightTime; // Время боя
    private int _favoriteIdWeaponPlayer;// Предпочитаемое оружие игрока

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="ai">Система ИИ</param>
    /// <param name="actionPlayer">Предпочитаемое действие игрока</param>
    /// <param name="favoriteIdWeaponPlayer">Предпочитаемое оружие игрока</param>
    /// <param name="fightTime">Время боя</param>
    public DamageTankNode(Ai ai, ActionPlayer actionPlayer, float fightTime, int favoriteIdWeaponPlayer)
    {
        _ai = ai;
        _actionPlayer = actionPlayer;
        _fightTime = fightTime;
        _favoriteIdWeaponPlayer = favoriteIdWeaponPlayer;
    }

    /// <summary>
    /// Оценка узла
    /// </summary>
    /// <returns>Состояние узла</returns>
    public override NodeState Evaluate()
    {
        if (_actionPlayer == ActionPlayer.Block && _fightTime > 60 && _favoriteIdWeaponPlayer == 0) // Если игрок предпочитал блокировать и его предпочитаемое оружие - меч и время боя было больше минуты
        {
            _ai.SetWeapon(2);// Установить оружие врага - булава

            //Установить коэффициенты силы и ловкости
            _ai.SetStrength(3);
            _ai.SetDexterity(1);

            _ai.SetTactic(TacticEnemy.Aggresive);//Установить тактику агрессивная
            return NodeState.SOCCESS;// Успешное выполнение узла
        }
        else
        {
            return NodeState.FAILURE;// Провал выполнения узла
        }
    }
}
