/*
Абстрактный клас состояния

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/
public abstract class State
{
    protected Ai Enemy; // Противник
    protected StateMachine Machine; // Машина состояний противника

    // Конструктор
    protected State(Ai enemy, StateMachine machine)
    {
        Enemy = enemy;
        Machine = machine;
    }

    /// <summary>
    /// Метод, определяющий вход в состояние
    /// </summary>
    public virtual void Enter()
    {}

    /// <summary>
    /// Метод, определяющий выполняемые действия во время работы состояния
    /// </summary>
    public virtual void LogicUpdate()
    {}

    /// <summary>
    /// Метод, определяющий выход из состояния
    /// </summary>
    public virtual void Exit()
    {}
}
