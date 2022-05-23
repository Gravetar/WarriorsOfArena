using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Ai : MonoBehaviour
{
    public StateMachine StateMachine { get; private set; }
    public StateIdle Idle { get; private set; }
    public StateMovement Movement { get; private set; }
    public StateAttack Attack { get; private set; }
    public StateBlock Block { get; private set; }
    public StateDeath Death { get; private set; }

    public Transform Player => _playerTransform;
    public Transform Self => _selfTransform;
    public float DistanceBetweenEnemyAndPlayer => Vector3.Distance(_selfTransform.position, _playerTransform.position);
    public GameManager GameManager;
    public Enemy Enemy;
    public int IdActiveWeapon;
    public GameObject[] WeaponsVisual;
    public List<Weapon> Weapons = new List<Weapon>();

    public Transform _target; // ”казываем переменную, к которой будет двигатьс€ наш агент  
    public EnemyAnimator Animator => _enemyAnimator;

    public NavMeshAgent Agent => _agent;
    private Transform _selfTransform;
    private Transform _playerTransform;
    private NavMeshAgent _agent; // ”казываем переменную агента
    private EnemyAnimator _enemyAnimator;
    private float _secondFinished = 0;
    public bool PinaltyDamage = false;
    [SerializeField] private Image _enemyHealthImage;
    public bool ShotPlayer = false;


    void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // ”казываем, что переменна€ _agent - это наш агент. 
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>());
        _selfTransform = GetComponent<Transform>();
        _playerTransform = FindObjectOfType<PlayerManager>().transform;
        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        _target = transform;

        Weapons = MyDataBase.GetWeapons();
        ActivateWeapon(2);

        Enemy = new Enemy(0, 10, 20);

        InitStates();
    }

    void Update()
    {
        StateMachine.LogicUpdate();
        if (Enemy.Health <= 0)
        {
            StateMachine.ChangeState(Death);
        }
    }

    private void FixedUpdate()
    {
        _secondFinished += Time.deltaTime;

        float coef = (float)Enemy.MaxHealth / Enemy.Health;
        float precent = 100 / coef;
        _enemyHealthImage.fillAmount = precent / 100;

        if (_secondFinished >= 0.1f)
        {

            if (Enemy.Stamina < Enemy.MaxStamina)
            {
                Enemy.Stamina += 1;
                _secondFinished = 0;
            }
            if (Enemy.Stamina < 0)
            {
                Enemy.Stamina = 0;
            }
        }
        if (Enemy.Stamina < Weapons[IdActiveWeapon].NeedStamina)
        {
            PinaltyDamage = true;
        }
        else
        {
            PinaltyDamage = false;
        }
    }

    private void InitStates()
    {
        StateMachine = new StateMachine();
        Idle = new StateIdle(this, StateMachine);
        Movement = new StateMovement(this, StateMachine);
        Attack = new StateAttack(this, StateMachine);
        Block = new StateBlock(this, StateMachine);
        Death = new StateDeath(this, StateMachine);

        StateMachine.Initialize(Idle);
    }

    private void ActivateWeapon(int idWeapon)
    {
        Animator.EquipWeapon(true);
        IdActiveWeapon = idWeapon;
    }

    public void ActivateWeaponWithAnimation()
    {
        foreach (GameObject weapon in WeaponsVisual)
        {
            weapon.SetActive(false);
        }
        WeaponsVisual[IdActiveWeapon].SetActive(true);
        float bonusAttackSpeed = (float)Enemy.Dexterity / 200;
        if (bonusAttackSpeed > 2.0f) bonusAttackSpeed = 2;
        Animator.SetAttackSpeed(Weapons[IdActiveWeapon].AttackSpeed + bonusAttackSpeed);
        Animator.EquipWeapon(false);
    }

    private void EndAttack() => Attack.EndAttack();
    private void Hit() => Attack.Hit();
    private void EndDamaged()
    {
        Animator.EndDamaged();
        Attack.ResetAttack();
    }
}
