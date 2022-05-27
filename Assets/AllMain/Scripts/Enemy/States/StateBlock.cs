/*
Состояние блока противника

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using UnityEngine;
public class StateBlock : State
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="enemy">Враг</param>
    /// <param name="machine">Машина состояний врага</param>
    public StateBlock(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// Вход в состояние
    /// </summary>
    public override void Enter()
    {
        Enemy.Animator.Block(); // Установить в аниматоре Block
        Enemy.Agent.isStopped = true; // Остановить движение агента
    }


    /// <summary>
    /// Выход из состояния
    /// </summary>
    public override void Exit()
    {
        Enemy.Animator.Reset(); // Сбросить аниматор
        Enemy.Agent.isStopped = false; // Продолжить движение агента
    }

    public override void LogicUpdate()
    {
        // Поворот к игроку
        var targetRotation = Quaternion.LookRotation(Enemy.Player.transform.position - Enemy.Self.transform.position);
        Enemy.Self.transform.rotation = Quaternion.Slerp(Enemy.Self.transform.rotation, targetRotation, 5 * Time.deltaTime);

        if (Enemy.Enemy.Stamina > Enemy.Enemy.MaxStamina / 2)// Если выносливости стало больше половины от максимальной
        {
            Enemy.StateMachine.ChangeState(Enemy.Movement); // Сменить состояние на движение
        }
    }
}
