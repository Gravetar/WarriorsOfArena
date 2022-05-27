/*
Управление анимацие и аниматором противника

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using UnityEngine;

public class EnemyAnimator
{
    private Animator _animator; // Аниматор противника
    private bool isDead = false; // Флаг - Противник умер

    /// <summary>
    /// Конструктор EnemyAnimator
    /// </summary>
    /// <param name="animator"></param>
    public EnemyAnimator(Animator animator)
    {
        // Инициализировать аниматор и установить значения нужных данных
        _animator = animator;
        _animator.SetBool("Grounded", true);
        _animator.SetFloat("MotionSpeed", 1);
    }

    /// <summary>
    /// Метод сброса анимаций противника
    /// </summary>
    public void Reset()
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 3.5f);
        _animator.SetBool(EnemyAnimationsKeystore.Block, false);
        _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        _animator.SetBool("Damaged", false);
        _animator.SetBool("Equip", false);
    }

    /// <summary>
    /// Установить в аниматоре ожидание
    /// </summary>
    public void Idle()
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
    }

    /// <summary>
    /// Установить в аниматоре блокирование
    /// </summary>
    public void Block()
    {
        _animator.SetBool(EnemyAnimationsKeystore.Block, true);
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
    }

    /// <summary>
    /// Установить в аниматоре атаку
    /// </summary>
    /// <param name="status">Номер в серии атак противника</param>
    public void Attack(int status)
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
        if (status == 0) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        if (status == 1) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 1);
        if (status == 2) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 2);
        if (status == 3) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 3);
        if (status == 4) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 4);
    }

    /// <summary>
    /// Установить в аниматоре движение
    /// </summary>
    public void Movement()
    {
        _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 3.5f);
    }

    /// <summary>
    /// Установить в аниматоре смерть
    /// </summary>
    public void Death()
    {
        if (!isDead)
        {
            _animator.SetTrigger("Death");
            isDead = true;
        }
    }

    /// <summary>
    /// Установить в аниматоре получения повреждения
    /// </summary>
    public void GetDamage()
    {
        _animator.SetBool("Damaged", true);
    }

    /// <summary>
    /// Установить ваниматоре, окончание получения повреждения
    /// </summary>
    public void EndDamaged()
    {
        _animator.SetBool("Damaged", false);
    }

    /// <summary>
    /// Установить в аниматоре одевание оружия
    /// </summary>
    /// <param name="equip">Флаг одевания оружия</param>
    public void EquipWeapon(bool equip)
    {
        _animator.SetBool("Equip", equip);
    }

    /// <summary>
    /// Установить скорость атаки противника
    /// </summary>
    /// <param name="speed">Скорость атаки</param>
    public void SetAttackSpeed(float speed)
    {
        _animator.SetFloat("AttackSpeed", speed);
    }

    /// <summary>
    /// Проверка, что противник находится в стандартном состоянии
    /// </summary>
    /// <returns></returns>
    public bool CheckEndAnimation()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle Walk Run Blend");
    }
}
