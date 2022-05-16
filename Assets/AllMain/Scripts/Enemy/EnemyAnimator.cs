using UnityEngine;

public class EnemyAnimator
{
    private Animator _animator;

    public EnemyAnimator(Animator animator)
    {
        _animator = animator;
        _animator.SetBool("Grounded", true);
        _animator.SetFloat("MotionSpeed", 1);
    }

    public void Reset()
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 3.5f);
        _animator.SetBool(EnemyAnimationsKeystore.Block, false);
        _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        _animator.SetBool("Damaged", false);
        _animator.SetBool("Equip", false);
    }

    public void Idle()
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
    }

    public void Block()
    {
        _animator.SetBool(EnemyAnimationsKeystore.Block, true);
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
    }

    public void Attack(int status)
    {
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 0f);
        if (status == 0) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        if (status == 1) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 1);
        if (status == 2) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 2);
        if (status == 3) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 3);
        if (status == 4) _animator.SetInteger(EnemyAnimationsKeystore.Attack, 4);
    }

    public void Movement()
    {
        _animator.SetInteger(EnemyAnimationsKeystore.Attack, 0);
        _animator.SetFloat(EnemyAnimationsKeystore.Speed, 3.5f);
    }

    public void GetDamage()
    {
        _animator.SetBool("Damaged", true);
    }

    public void EndDamaged()
    {
        _animator.SetBool("Damaged", false);
    }

    public void EquipWeapon(bool equip)
    {
        _animator.SetBool("Equip", equip);
    }

    public void SetAttackSpeed(float speed)
    {
        _animator.SetFloat("AttackSpeed", speed);
    }

    public bool CheckEndAnimation()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Idle Walk Run Blend");
    }
}
