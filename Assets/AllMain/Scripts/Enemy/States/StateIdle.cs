/*
Состояние ожидания противником

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/
public class StateIdle : State
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="enemy">Враг</param>
    /// <param name="machine">Машина состояний врага</param>
    public StateIdle(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// Вход в состояние
    /// </summary>
    public override void Enter()
    {
        Enemy.Animator.Idle(); // Установить в аниматоре Idle
    }

    /// <summary>
    /// Выполняемое действие в состоянии
    /// </summary>
    public override void LogicUpdate()
    {
        if (Enemy.GameManager.isStartedBattle) // Если бой начался
        {
            Enemy.StateMachine.ChangeState(Enemy.Movement); // Перейти в состояние движения
        }
    }
}
