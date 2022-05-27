/*
Состояние атаки противника

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using UnityEngine;

public class StateAttack : State
{
    private bool _canAttack = true; // Возможность атаковать
    private int _stateAttack = 0; // Номер в серии атак
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="enemy">Враг</param>
    /// <param name="machine">Машина состояний врага</param>
    public StateAttack(Ai enemy, StateMachine machine) : base(enemy, machine)
    {}

    /// <summary>
    /// Выполняемое действие в состоянии
    /// </summary>
    public override void LogicUpdate()
    {
        Enemy.Agent.isStopped = true; // Остановить агента
        Enemy.Agent.velocity = Vector3.zero;
        // Повернуться к игроку
        var targetRotation = Quaternion.LookRotation(Enemy.Player.transform.position - Enemy.Self.transform.position);
        Enemy.Self.transform.rotation = Quaternion.Slerp(Enemy.Self.transform.rotation, targetRotation, 5 * Time.deltaTime);

        if (Enemy.Enemy.Stamina < Enemy.Weapons[Enemy.ActiveWeaponId].NeedStamina) // Если выносливости противника не хватит на следующий удар.
        {
            Enemy.StateMachine.ChangeState(Enemy.Block); // Сменить состояние на Блок
        }
        else if (Enemy.DistanceBetweenEnemyAndPlayer > 1f) // Если расстояние до персонажа игрока больше 1
        {
            if (Enemy.Animator.CheckEndAnimation()) Enemy.StateMachine.ChangeState(Enemy.Movement); // Проверить закончилось ли выполнение анимаций, сменить состояние на движение
        }
        else if (_canAttack) // Если может атаковать
        {
            ChangeAttack(); // Атаковать
            Enemy.Enemy.Stamina -= Enemy.Weapons[Enemy.ActiveWeaponId].NeedStamina; // Отнять выносливость
            _canAttack = false; // Запретить атаковать
        }
    }

    /// <summary>
    /// Выход из состояния
    /// </summary>
    public override void Exit()
    {
        ResetAttack(); // Сбросить атаку
        Enemy.Animator.Movement(); // Установить в аниматоре движение
    }

    /// <summary>
    /// Атаковать
    /// </summary>
    private void ChangeAttack()
    {
        if (_stateAttack == 0) // Если серия атак на 0
        {
            _stateAttack = 1; // Перейти к следующей части серии атак
            Enemy.Animator.Attack(1); // Установить в аниматоре атаку первого типа
        }
        else if (_stateAttack == 1)
        {
            _stateAttack = 2;
            Enemy.Animator.Attack(2);
        }
        else if (_stateAttack == 2)
        {
            _stateAttack = 3;
            Enemy.Animator.Attack(3);
        }
        else if (_stateAttack == 3)
        {
            _stateAttack = 4;
            Enemy.Animator.Attack(4);
        }
        else if (_stateAttack == 4)
        {
            _stateAttack = 0;
            Enemy.Animator.Attack(0);
        }
    }

    /// <summary>
    /// Сбросить атаку
    /// </summary>
    public void ResetAttack()
    {
        _stateAttack = 0; // Обнулить серию атаки
        _canAttack = true; // Разрешить атаковать
        Enemy.Animator.Attack(0); // Установить в аниматоре атаку серии 0
    }

    /// <summary>
    /// Завершить атаку
    /// </summary>
    public void EndAttack()
    {
        _canAttack = true; // Разрешить атаковать
    }

    /// <summary>
    /// Удар
    /// </summary>
    public void Hit()
    {
        if (Enemy.ShotPlayer) // Если задел персонажа игрока
        {
            if (Enemy.PinaltyDamage) Enemy.Player.GetComponent<PlayerManager>().GetDamage(5); // Если есть штраф к урону-нанести 5 урона персонажу игрока
            else Enemy.Player.GetComponent<PlayerManager>().GetDamage(Enemy.Weapons[Enemy.ActiveWeaponId].Damage + Enemy.Enemy.Strength); // Иначе - нанести урон оружия персонажу игрока
            if (Enemy.Player.GetComponent<PlayerManager>().Player.Health <= 0) // Если здоровье игрока достигло 0
            {
                Enemy.StateMachine.ChangeState(Enemy.Idle); // Сменить состояние на ожидание
            }
        }
        _canAttack = true; // Разрешить атаковать
        Enemy.ShotPlayer = false; // Враг не задел персонажа игрока
    }
}
