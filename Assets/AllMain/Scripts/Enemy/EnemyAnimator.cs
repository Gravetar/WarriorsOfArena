/*
���������� �������� � ���������� ����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using UnityEngine;

public class EnemyAnimator
{
    private Animator _animator; // �������� ����������
    private bool isDead = false; // ���� - ��������� ����

    /// <summary>
    /// ����������� EnemyAnimator
    /// </summary>
    /// <param name="animator"></param>
    public EnemyAnimator(Animator animator)
    {
        // ���������������� �������� � ���������� �������� ������ ������
        _animator = animator;
        _animator.SetBool("Grounded", true);
        _animator.SetFloat("MotionSpeed", 1);
    }

    /// <summary>
    /// ����� ������ �������� ����������
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
    /// ���������� � ��������� ��������
    /// </summary>
    public void Idle()
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
    }

    /// <summary>
    /// ���������� � ��������� ������������
    /// </summary>
    public void Block()
    {
        _animator.SetBool(EnemyAnimationsKeystore.Block, true);
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
    }

    /// <summary>
    /// ���������� � ��������� �����
    /// </summary>
    /// <param name="status">����� � ����� ���� ����������</param>
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
    /// ���������� � ��������� ��������
    /// </summary>
    public void Movement()
    {
        _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 3.5f);
    }

    /// <summary>
    /// ���������� � ��������� ������
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
    /// ���������� � ��������� ��������� �����������
    /// </summary>
    public void GetDamage()
    {
        _animator.SetBool("Damaged", true);
    }

    /// <summary>
    /// ���������� ����������, ��������� ��������� �����������
    /// </summary>
    public void EndDamaged()
    {
        _animator.SetBool("Damaged", false);
    }

    /// <summary>
    /// ���������� � ��������� �������� ������
    /// </summary>
    /// <param name="equip">���� �������� ������</param>
    public void EquipWeapon(bool equip)
    {
        _animator.SetBool("Equip", equip);
    }

    /// <summary>
    /// ���������� �������� ����� ����������
    /// </summary>
    /// <param name="speed">�������� �����</param>
    public void SetAttackSpeed(float speed)
    {
        _animator.SetFloat("AttackSpeed", speed);
    }

    /// <summary>
    /// ��������, ��� ��������� ��������� � ����������� ���������
    /// </summary>
    /// <returns></returns>
    public bool CheckEndAnimation()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle Walk Run Blend");
    }
}
