/*
�����-������, ���������� �� ������ ��������� �������������� ����������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Ai : MonoBehaviour
{
    // ��������� �������� ���������
    public StateMachine StateMachine { get; private set; }
    public StateIdle Idle { get; private set; }
    public StateMovement Movement { get; private set; }
    public StateAttack Attack { get; private set; }
    public StateBlock Block { get; private set; }
    public StateDeath Death { get; private set; }

    // ____________________________________________________
    public Transform Player => _playerTransform;
    public Transform Self => _selfTransform;
    public float DistanceBetweenEnemyAndPlayer => Vector3.Distance(_selfTransform.position, _playerTransform.position);
    public EnemyAnimator Animator => _enemyAnimator;
    public NavMeshAgent Agent => _agent;

    public TacticEnemy EnemyTactic;
    public int ActiveWeaponId;
    public GameManager GameManager;
    public Enemy Enemy;
    public List<Weapon> Weapons = new List<Weapon>();
    public GameObject[] WeaponsVisual;
    public bool PinaltyDamage = false;
    public bool ShotPlayer = false;

    [SerializeField] private Image _enemyHealthImage;
    private PlayerManager _playerManager;
    private Node _topNode;
    private int _enemyStrength;
    private int _enemyDexterity;
    private Fight _lastFightPlayer;

    private Transform _selfTransform;
    private Transform _playerTransform;
    private NavMeshAgent _agent;
    private EnemyAnimator _enemyAnimator;
    private float _secondFinished = 0;

    /// <summary>
    /// Start ����������, ����� ������ �������, ���������� ��������������� ����� ������ ������� ������ �� ������� ����������.
    /// </summary>
    void Start()
    {
        _playerManager = GameObject.Find("Player").GetComponent<PlayerManager>(); // ���������� �������� ������
        _lastFightPlayer = MyDataBase.GetLastFightByPlayerId(_playerManager.Player.Id); // �������� ��������� ��� ������
        _agent = GetComponent<NavMeshAgent>(); // ���������� ������
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>()); // ���������� �������� �����
        _selfTransform = GetComponent<Transform>(); // �������� Transform ������ ���� 
        _playerTransform = FindObjectOfType<PlayerManager>().transform; // �������� Transform ��������� ������
        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); // �������� ��������� ����
        Weapons = MyDataBase.GetWeapons(); // �������� ������ �� ���� ������

        if (_lastFightPlayer == null) _lastFightPlayer = new Fight(); // ���� � ������ ���� ���, �� ��������� ����������� ���
        _enemyHealthImage = GameObject.Find("Canvas").transform.Find("BackgroundHealthEnemy").transform.Find("HealthEnemy").GetComponent<Image>(); // ���������� ����������� �������� ������

        ConstructBehaviourTree(); // ���������������� ������ ���������

        Enemy = CreateEnemy(); // ������� ���������� �� ������ ������������� ��������� � ������
        ActivateWeapon(Enemy.Weapon); // �������� ������

        InitStates(); // ��������������� ������ ���������
    }

    /// <summary>
    /// ���������� ���������� ������ ����, ���� MonoBehaviour �������.
    /// </summary>
    void Update()
    {
        StateMachine.LogicUpdate(); // ��������� ���������� ������ ���������
        if (Enemy.Health <= 0) // ���� �������� ����� �������� 0
        {
            StateMachine.ChangeState(Death); // ������� ��� ��������� �� "������"
        }
    }

    /// <summary>
    /// ������������� ����������, �� ��������� �� ������� ������
    /// </summary>
    private void FixedUpdate()
    {
        // ��������� ����� �������� ����������
        float coef = (float)Enemy.MaxHealth / Enemy.Health;
        float precent = 100 / coef;
        _enemyHealthImage.fillAmount = precent / 100;

        _secondFinished += Time.deltaTime; // ������� �������

        if (_secondFinished >= 0.1f) // ���� ������� ������� ������ 0.1 �������
        {
            if (Enemy.Stamina < Enemy.MaxStamina) // ���� � ����� ������������ ������ ������������
            {
                Enemy.Stamina += 1; // ������������ ������������ �� 1 �������
                _secondFinished = 0; // �������� ������� �������
            }
            if (Enemy.Stamina < 0) // ���� ������������ ���������� ������ ����
            {
                Enemy.Stamina = 0; // �������� ������������ ����������
            }
        }
        if (Enemy.Stamina < Weapons[ActiveWeaponId].NeedStamina) // ���� � ����� �� ������� ������������ �� ����
        {
            PinaltyDamage = true; // ���������� ����� � �����
        }
        else
        {
            PinaltyDamage = false; // ����� ����� � �����
        }
    }

    ////////////////// BehaviourTrees //////////////////
    /// <summary>
    /// ���������������� ������ ���������
    /// </summary>
    private void ConstructBehaviourTree()
    {
        // ���������� ���� ��������
        WarriorNode warriorNode = new WarriorNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); // Attack 1 - 2 2 2 P
        DamageTankNode dmgTankNode = new DamageTankNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FightTime, _lastFightPlayer.FavoriteWeaponPlayer); //Block >60 1 - 2 3 1 A
        DefTankNode defTankNode = new DefTankNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FightTime, _lastFightPlayer.FavoriteWeaponPlayer); //Block <60 1 - 2 3 1 P
        RusherNode rusherNode = new RusherNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Block 3 - 0 1 3 A
        CarefulDexAssasinNode carefulDexAssasinNode = new CarefulDexAssasinNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Block 2 - 0 1 3 P
        DefCarefulNode defCarefulNode = new DefCarefulNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Attack 3 - 1 2 2 P
        RunAwayCarefulNode runAwayCarefulNode = new RunAwayCarefulNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Attack 2 - 0 1 3 P

        // ���������� �������� ����� �����
        Selector tankSelector = new Selector(new List<Node> { dmgTankNode, defTankNode });
        // ���������� �������� ����� ����
        Selector strengthSelector = new Selector(new List<Node> { warriorNode, tankSelector });

        // ���������� �������� ����� ����������� �����
        Selector carefulSelector = new Selector(new List<Node> { defCarefulNode, runAwayCarefulNode });
        // ���������� �������� ����� ��������
        Selector assasinSelector = new Selector(new List<Node> { rusherNode, carefulDexAssasinNode });
        // ���������� �������� ����� ��������
        Selector dextiritySelector = new Selector(new List<Node> { assasinSelector, carefulSelector });

        // ���������� �������� ����� ��������� � ����
        _topNode = new Selector(new List<Node> { strengthSelector, dextiritySelector });
        _topNode.Evaluate(); // �������� �����
    }

    /// <summary>
    /// ���������� ������ ����������
    /// </summary>
    /// <param name="id">Id ������</param>
    public void SetWeapon(int id)
    {
        ActiveWeaponId = id;
    }

    /// <summary>
    /// ���������� ���� ����������(�������� ���������� ���� ������)
    /// </summary>
    /// <param name="coef">����������</param>
    public void SetStrength(int coef)
    {
        _enemyStrength = coef * _lastFightPlayer.FightNumber;
    }

    /// <summary>
    /// ���������� �������� ����������(�������� ���������� ���� ������)
    /// </summary>
    /// <param name="coef">����������</param>
    public void SetDexterity(int coef)
    {
        _enemyDexterity = coef * _lastFightPlayer.FightNumber;
    }

    /// <summary>
    /// ���������� ������� ����������
    /// </summary>
    /// <param name="tactic">��� �������</param>
    public void SetTactic(TacticEnemy tactic)
    {
        EnemyTactic = tactic;
    }

    /// <summary>
    /// ������� �����
    /// </summary>
    /// <returns>������ �����</returns>
    public Enemy CreateEnemy()
    {
        return new Enemy(EnemyTactic, _lastFightPlayer.FightNumber, _enemyStrength, _enemyDexterity, ActiveWeaponId);
    }
    //////////////////////////////////////////////////////////////////////////////

    /////////////////STATE MACHINES//////////////////////////////
    /// <summary>
    /// ������������� ������ ���������(�������� ���������)
    /// </summary>
    private void InitStates()
    {
        StateMachine = new StateMachine();
        Idle = new StateIdle(this, StateMachine);
        Movement = new StateMovement(this, StateMachine);
        Attack = new StateAttack(this, StateMachine);
        Block = new StateBlock(this, StateMachine);
        Death = new StateDeath(this, StateMachine);

        StateMachine.Initialize(Idle); // ���������� ��������� ���������
    }

    /// <summary>
    /// �������� ������
    /// </summary>
    /// <param name="idWeapon">Id ������</param>
    private void ActivateWeapon(int idWeapon)
    {
        Animator.EquipWeapon(true);
        ActiveWeaponId = idWeapon;
    }

    /// <summary>
    /// ������������ ������, ���������� �������� ��������
    /// </summary>
    public void ActivateWeaponWithAnimation()
    {
        foreach (GameObject weapon in WeaponsVisual)
        {
            weapon.SetActive(false);
        }
        WeaponsVisual[ActiveWeaponId].SetActive(true);
        float bonusAttackSpeed = (float)Enemy.Dexterity / 200;
        if (bonusAttackSpeed > 2.0f) bonusAttackSpeed = 2;
        Animator.SetAttackSpeed(Weapons[ActiveWeaponId].AttackSpeed + bonusAttackSpeed);
        Animator.EquipWeapon(false);
    }

    // ������������ ������� �������� � ���������� //
    private void EndAttack() => Attack.EndAttack();
    private void Hit() => Attack.Hit();
    private void EndDamaged()
    {
        Animator.EndDamaged();
        Attack.ResetAttack();
    }
    private void AfterDeath()
    {}
}

/// <summary>
/// ������� �����
/// </summary>
public enum TacticEnemy
{
    Aggresive,
    Passive
}
