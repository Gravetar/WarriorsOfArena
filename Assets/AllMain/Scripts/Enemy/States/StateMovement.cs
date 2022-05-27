/*
Состояние приследования игрока (движения) противником

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

public class StateMovement : State
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="enemy">Враг</param>
    /// <param name="machine">Машина состояний врага</param>
    public StateMovement(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// Вход в состояние
    /// </summary>
    public override void Enter()
    {
        Enemy.Agent.isStopped = false; // Продолжить движения агента
        Enemy.Animator.Movement(); // Установить в аниматор Movement
    }

    /// <summary>
    /// Выполняемое действие в состоянии
    /// </summary>
    public override void LogicUpdate()
    {
        Enemy.Agent.SetDestination(Enemy.Player.position); // Установить в качестве цели агента - игрока
        if (Enemy.DistanceBetweenEnemyAndPlayer < 1f) // Если дистанция до игрока меньше 1
        {
            Enemy.StateMachine.ChangeState(Enemy.Attack); // Сменить состояние на "Атака"
        }
    }
}
