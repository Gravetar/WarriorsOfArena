/*
Класс-скрипт, отвечающий за полное поведение искусственного интеллекта

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class Ai : MonoBehaviour
{
    // СОСТОЯНИЯ КОНЕЧНЫХ АВТОМАТОВ
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
    /// Start вызывается, когда скрипт включен, вызывается непосредственно перед первым вызовом любого из методов обновления.
    /// </summary>
    void Start()
    {
        _playerManager = GameObject.Find("Player").GetComponent<PlayerManager>(); // Установить менеджер игрока
        _lastFightPlayer = MyDataBase.GetLastFightByPlayerId(_playerManager.Player.Id); // Получить последний бой игрока
        _agent = GetComponent<NavMeshAgent>(); // Установить агента
        _enemyAnimator = new EnemyAnimator(GetComponent<Animator>()); // Установить аниматор врага
        _selfTransform = GetComponent<Transform>(); // Получить Transform самого себя 
        _playerTransform = FindObjectOfType<PlayerManager>().transform; // Получить Transform персонажа игрока
        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); // Получить менеджера игры
        Weapons = MyDataBase.GetWeapons(); // Получить оружия из базы данных

        if (_lastFightPlayer == null) _lastFightPlayer = new Fight(); // Если у игрока боев нет, то создается стандартный боя
        _enemyHealthImage = GameObject.Find("Canvas").transform.Find("BackgroundHealthEnemy").transform.Find("HealthEnemy").GetComponent<Image>(); // Установить изображение здоровья игрока

        ConstructBehaviourTree(); // Сконструрировать дерево поведения

        Enemy = CreateEnemy(); // Создать противника на основе установленных праметров в дереве
        ActivateWeapon(Enemy.Weapon); // Обнажить оружие

        InitStates(); // ИНциализировать машину состояний
    }

    /// <summary>
    /// Обновление вызывается каждый кадр, если MonoBehaviour включен.
    /// </summary>
    void Update()
    {
        StateMachine.LogicUpdate(); // Постояное обновление машины состояний
        if (Enemy.Health <= 0) // Если здоровье врага достигло 0
        {
            StateMachine.ChangeState(Death); // Сменить его состояние на "смерть"
        }
    }

    /// <summary>
    /// Фиксированные обновления, не зависящее от частоты кадров
    /// </summary>
    private void FixedUpdate()
    {
        // Заполнить шкалу здоровья противника
        float coef = (float)Enemy.MaxHealth / Enemy.Health;
        float precent = 100 / coef;
        _enemyHealthImage.fillAmount = precent / 100;

        _secondFinished += Time.deltaTime; // Подсчет времени

        if (_secondFinished >= 0.1f) // Если подсчет времени достиг 0.1 секунды
        {
            if (Enemy.Stamina < Enemy.MaxStamina) // Если у врага выносливости меньше максимальной
            {
                Enemy.Stamina += 1; // Восстановить выносливость на 1 еденицу
                _secondFinished = 0; // Обнулить подсчет времени
            }
            if (Enemy.Stamina < 0) // Если выносливость противника меньше нуля
            {
                Enemy.Stamina = 0; // Обнулить выносливость противника
            }
        }
        if (Enemy.Stamina < Weapons[ActiveWeaponId].NeedStamina) // Если у врага не хватаем выносливости на удар
        {
            PinaltyDamage = true; // Установить штраф к урону
        }
        else
        {
            PinaltyDamage = false; // Снять штраф к урону
        }
    }

    ////////////////// BehaviourTrees //////////////////
    /// <summary>
    /// Конструрирование дерева поведения
    /// </summary>
    private void ConstructBehaviourTree()
    {
        // Установить Узлы деревьев
        WarriorNode warriorNode = new WarriorNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); // Attack 1 - 2 2 2 P
        DamageTankNode dmgTankNode = new DamageTankNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FightTime, _lastFightPlayer.FavoriteWeaponPlayer); //Block >60 1 - 2 3 1 A
        DefTankNode defTankNode = new DefTankNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FightTime, _lastFightPlayer.FavoriteWeaponPlayer); //Block <60 1 - 2 3 1 P
        RusherNode rusherNode = new RusherNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Block 3 - 0 1 3 A
        CarefulDexAssasinNode carefulDexAssasinNode = new CarefulDexAssasinNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Block 2 - 0 1 3 P
        DefCarefulNode defCarefulNode = new DefCarefulNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Attack 3 - 1 2 2 P
        RunAwayCarefulNode runAwayCarefulNode = new RunAwayCarefulNode(this, _lastFightPlayer.FavoriteActionPlayer, _lastFightPlayer.FavoriteWeaponPlayer); //Attack 2 - 0 1 3 P

        // Установить СЕЛЕКТОР ветки танка
        Selector tankSelector = new Selector(new List<Node> { dmgTankNode, defTankNode });
        // Установить СЕЛЕКТОР ветки силы
        Selector strengthSelector = new Selector(new List<Node> { warriorNode, tankSelector });

        // Установить СЕЛЕКТОР ветки осторожного бойца
        Selector carefulSelector = new Selector(new List<Node> { defCarefulNode, runAwayCarefulNode });
        // Установить СЕЛЕКТОР ветки ассасина
        Selector assasinSelector = new Selector(new List<Node> { rusherNode, carefulDexAssasinNode });
        // Установить СЕЛЕКТОР ветки ловкости
        Selector dextiritySelector = new Selector(new List<Node> { assasinSelector, carefulSelector });

        // Установить СЕЛЕКТОР между ловкостью и силы
        _topNode = new Selector(new List<Node> { strengthSelector, dextiritySelector });
        _topNode.Evaluate(); // Проверка узлов
    }

    /// <summary>
    /// Установить оружие противника
    /// </summary>
    /// <param name="id">Id оружия</param>
    public void SetWeapon(int id)
    {
        ActiveWeaponId = id;
    }

    /// <summary>
    /// Установить силу противника(Согласно количеству боев игрока)
    /// </summary>
    /// <param name="coef">Коэфициент</param>
    public void SetStrength(int coef)
    {
        _enemyStrength = coef * _lastFightPlayer.FightNumber;
    }

    /// <summary>
    /// Установить ловкость противника(Согласно количеству боев игрока)
    /// </summary>
    /// <param name="coef">Коэфициент</param>
    public void SetDexterity(int coef)
    {
        _enemyDexterity = coef * _lastFightPlayer.FightNumber;
    }

    /// <summary>
    /// Установить тактику противника
    /// </summary>
    /// <param name="tactic">Тип тактики</param>
    public void SetTactic(TacticEnemy tactic)
    {
        EnemyTactic = tactic;
    }

    /// <summary>
    /// Создать врага
    /// </summary>
    /// <returns>Объект врага</returns>
    public Enemy CreateEnemy()
    {
        return new Enemy(EnemyTactic, _lastFightPlayer.FightNumber, _enemyStrength, _enemyDexterity, ActiveWeaponId);
    }
    //////////////////////////////////////////////////////////////////////////////

    /////////////////STATE MACHINES//////////////////////////////
    /// <summary>
    /// Инициализация машины состояний(конечных автоматов)
    /// </summary>
    private void InitStates()
    {
        StateMachine = new StateMachine();
        Idle = new StateIdle(this, StateMachine);
        Movement = new StateMovement(this, StateMachine);
        Attack = new StateAttack(this, StateMachine);
        Block = new StateBlock(this, StateMachine);
        Death = new StateDeath(this, StateMachine);

        StateMachine.Initialize(Idle); // Установить стартовое состояние
    }

    /// <summary>
    /// Обнажить оружие
    /// </summary>
    /// <param name="idWeapon">Id Оружия</param>
    private void ActivateWeapon(int idWeapon)
    {
        Animator.EquipWeapon(true);
        ActiveWeaponId = idWeapon;
    }

    /// <summary>
    /// Активировать оружия, вызывается событием анимации
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

    // Инциализация событий анимации у противника //
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
/// Тактика врага
/// </summary>
public enum TacticEnemy
{
    Aggresive,
    Passive
}
