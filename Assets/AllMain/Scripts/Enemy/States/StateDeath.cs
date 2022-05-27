/*
Состояние смерти противника

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

public class StateDeath : State
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="enemy">Враг</param>
    /// <param name="machine">Машина состояний врага</param>
    public StateDeath(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// Вход в состояние
    /// </summary>
    public override void Enter()
    {
        Enemy.Animator.Death(); // Установить в аниматоре Death
    }
}
