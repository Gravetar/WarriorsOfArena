/*
Менеджер игрока, данный класс-скрипт отвечает за все, что связано с персонажем игрока

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Player Player => _player; // Установка только для чтения(геттер)

    public float BlockTime; // Время блока игрока в бою
    public int AttackCount; // Количество атак, проведенных игроком
    public bool BlockStatus = false; // Флаг, описывающий в блоке ли сейчас игрок
    public bool EnemyIsDead = false; // Флаг, указывающий, что противник побежден.
    public List<GameObject> NowEnemies = new List<GameObject>(); // Враги, задетые атакой
    public int IdActiveWeapon = 0; // Id текущего оружия игрока
    
    [SerializeField] private GameObject[] WeaponsVisual; // Объекты оружий
    [SerializeField] private Transform _instructor; // Инструктор
    [SerializeField] private Image _playerHealthImage; // Картинка здоровья игрока
    [SerializeField] private Image _playerStaminaImage; // Картинка выносливости игрока
    [SerializeField] private GameObject _characteristicWindow; // Панель - статистика игрока
    [SerializeField] private GameObject _door; // Объект - дверь

    private List<Weapon> _weapons = new List<Weapon>(); // Список оружий
    private GameManager _gameManager; // Менеджер игры
    private int _attackStatus = 0; // Порядковый номер атаки
    private Player _player; // Персонаж игрока
    private bool _pinaltyDamage = false; // Штраф к урону
    private float _playerMaxSpeed = 6; // Максимальная доступная скорость персонажа

    private MyPlayerInput _input; // Система ввода
    private StarterAssetsInputs _inputMovement; // Система ввода (шаблонная), для ограничения движения персонажа

    private Animator _animator; // Аниматор персонажа
    private bool _canAttack = true; // Флаг, может ли персонаж провести следующую атаку

    private float _secondFinished = 0; // Счетчик времени для восстановления выносливости персонажа

    /// <summary>
    /// Функция Awake вызывается когда экземпляр скрипта будет загружен
    /// </summary>
    private void Awake()
    {
        PerformedInputs(); // Настройка системы управления
    }

    /// <summary>
    /// Функция OnEnable вызывается, когда объект становится включенным и активным.
    /// </summary>
    private void OnEnable()
    {
        _input.Enable();
    }

    /// <summary>
    /// Эта функция вызывается, когда поведение становится отключенным.
    /// Она также вызывается при уничтожении объекта
    /// </summary>
    private void OnDisable()
    {
        _input.Disable(); // Отключить систему управления
    }

    /// <summary>
    /// Start вызывается, когда скрипт включен, вызывается непосредственно перед первым вызовом любого из методов обновления.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // Закрепить курсор "в окне игры"

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _player = MyDataBase.GetPlayerById(DATAMovement.ActivePlayer); // Загрузить персонажа игрока
        _inputMovement = GetComponent<StarterAssetsInputs>(); // Установить систему ввода (Шаблонную)

        _animator = GetComponent<Animator>(); // Установка аниматора персонажа
        _weapons = MyDataBase.GetWeapons(); // Загрузить данные оружия из базы данных
        
        ActivateWeapon(0); // Активировать меч, в качестве оружия персонажа
    }

    /// <summary>
    /// Обновление, не зависящее от частоты кадров
    /// </summary>
    private void FixedUpdate()
    {
        // Заполнить шкалу здоровья игрока
        float coef = (float)_player.MaxHealth / _player.Health;
        float precent = 100 / coef;
        _playerHealthImage.fillAmount = precent / 100;
        // Заполнить шкалу выносливости игрока
        coef = (float)_player.MaxStamina / _player.Stamina;
        precent = 100 / coef;
        _playerStaminaImage.fillAmount = precent / 100;

        _secondFinished += Time.deltaTime; // Подсчет времени

        if (_secondFinished >= 0.1f) // Если прошла 0.1 секунда от начала подсчета времени
        {
            if (_player.Stamina < _player.MaxStamina) // Если выносливость персонажа меньше его макимальной выносливости
            {
                _player.Stamina += 1; // Восстановить еденицу выносливости
                _secondFinished = 0; // Обнулить счетчик времени
            }
            if (_player.Stamina < 0) // Если выносливость персонажа меньше 0
            {
                _player.Stamina = 0; // Обнулить выносливость
            }
        }
        if (_player.Stamina < _weapons[IdActiveWeapon].NeedStamina) // Если выносливость персонажа меньше выносливости, которой требуется на удар
        {
            _pinaltyDamage = true; // Установить флаг штрафа к урону в true
        }
        else // иначе
        {
            _pinaltyDamage = false; // Установить флаг штрафа к урону в false
        }
        if (BlockStatus) BlockTime += Time.deltaTime; // если игрок в блоку, прибавлять время к блоку персонажа
    }

    /// <summary>
    /// Обновление вызывается каждый кадр, если MonoBehaviour включен.
    /// </summary>
    private void Update()
    {
        // Держит ли игрок блок
        if (Mouse.current.rightButton.isPressed) Block("Block");
        else Block("UnBlock");

        // Обнулить движение персонажа
        if (_playerMaxSpeed == 0) GetComponent<StarterAssetsInputs>().move = new Vector2(0, 0);

        // Если игрок достаточно близко к инструктору
        if (Vector3.Distance(transform.position, _instructor.position) <= 3) _gameManager.GameUI.ActivateTxtDialog(true); // Активировать текст, который говорит, что можно начать диалог
        else _gameManager.GameUI.ActivateTxtDialog(false); // иначе диактивировать его
    }

    /// <summary>
    /// Настройка системы вводв
    /// </summary>
    private void PerformedInputs()
    {
        _input = new MyPlayerInput(); // Создать новую систему ввода

        // Назначить функции к действиям в системе ввода
        _input.Player.Attack.performed += context => Attack();
        _input.Player.GetSword.performed += context => ActivateWeapon(0);
        _input.Player.GetAxe.performed += context => ActivateWeapon(1);
        _input.Player.GetMace.performed += context => ActivateWeapon(2);
        _input.Player.ExitToMenu.performed += context => ExitToMenu();
        _input.Player.Kick.performed += context => StartKick();
        _input.Player.OpenStats.performed += context => OpenCharacteristic();
        _input.Player.StartDialog.performed += context => StartDialog();
    }

    /// <summary>
    /// Начать диалог с интруктором
    /// </summary>
    private void StartDialog()
    {
        // Если игрок достаточно близко к инструктору
        if (Vector3.Distance(transform.position, _instructor.position) <= 3)
        {
            _gameManager.GameUI.StartDialog(); // Открыть диалог
            ActivatedInputs(false); // Диактивировать ввод игрока
        }
    }

    /// <summary>
    /// Метод установки активности системы ввода игрока
    /// </summary>
    /// <param name="active">Флаг активности</param>
    public void ActivatedInputs(bool active)
    {
        if (!active) // Если система ввода не активна
        {
            _input.Player.Disable(); // Отключить систему ввода
            GetComponent<StarterAssetsInputs>().move = new Vector2(0, 0); // Обнулить движение персонажа
        }
        else
        {
            _input.Player.Enable(); // Включить систему ввода
        }
        _inputMovement.IsCanMovement = active; // Установить флаг может ли игрок перемещать персонажа
    }

    /// <summary>
    /// Открыть статистику персонажа
    /// </summary>
    private void OpenCharacteristic()
    {
        _gameManager.GameUI.OpenStatistic(); // Открыть статистику персонажа
    }

    /// <summary>
    /// Проезвести пинок
    /// </summary>
    private void StartKick()
    {
        _animator.SetBool("Kick", true); // Запустить анимацию пинка
    }

    /// <summary>
    /// Пинок, вызывается с события анимации
    /// </summary>
    private void Kick()
    {
        _animator.SetBool("Kick", false); // Остановить анимацию пинка
    }

    /// <summary>
    /// Метод, который срабатывает при выходу в меню
    /// </summary>
    private void ExitToMenu()
    {
        MyDataBase.UpdatePlayer(Player); // Сохранить данные персонажа
        SceneManager.LoadScene("Menu"); // Загрузить сцену меню
    }

    /// <summary>
    /// Атака персонажа
    /// </summary>
    private void Attack()
    {
        if (_canAttack) // Если персонаж может нанести удар
        {
            AttackCount += 1; // Прибавить в счетчик атак игрока
            NowEnemies.Clear(); // Очистить список задетых враго
            _canAttack = false; // Персонаж не может атаковать
            _player.Stamina -= _weapons[IdActiveWeapon].NeedStamina; // Отнять выносливость персонажа, согласно требованиям оружия
            if (_attackStatus == 0) // Если серия атак = 0
            {
                _attackStatus = 1; // Установить серию атак на 1...
            }
            else if (_attackStatus == 1)
            {
                _attackStatus = 2;
            }
            else if (_attackStatus == 2)
            {
                _attackStatus = 3;
            }
            else if (_attackStatus == 3)
            {
                _attackStatus = 4;
            }
            else if (_attackStatus == 4)
            {
                _attackStatus = 0;
            }
            _animator.SetInteger("Attack", _attackStatus); // Установить анимацию атаки
        }
    }

    /// <summary>
    /// Достать оружие
    /// </summary>
    /// <param name="idWeapon">ID оружия</param>
    private void ActivateWeapon(int idWeapon)
    {
        _animator.SetBool("Equip", true); // Установить анимацию достования оружия
        IdActiveWeapon = idWeapon; // Установить айди текущего оружия на сменное
        _playerMaxSpeed = 0; // Запретить двигаться персонажу
    }

    /// <summary>
    /// Активировать оружие, вызывается из анимации
    /// </summary>
    public void ActivateWeaponWithAnimation()
    {
        foreach (GameObject weapon in WeaponsVisual) // Пройти по всем оружиям
        {
            weapon.SetActive(false); // Диактивировать оружия
        }
        WeaponsVisual[IdActiveWeapon].SetActive(true); // Активировать нужное оружие
        float bonusAttackSpeed = (float)_player.Dexterity / 200; // Расчитать бонус к скорости атаки оружия
        if (bonusAttackSpeed > 2.0f) bonusAttackSpeed = 2; // Ораничить скорость на максимум 2
        _animator.SetFloat("AttackSpeed", _weapons[IdActiveWeapon].AttackSpeed + bonusAttackSpeed); // Установить скорость атаки
        _animator.SetBool("Equip", false); // Остановить анимацию достования оружия
        _playerMaxSpeed = 1; // Вернуть возможность двигаться
    }

    /// <summary>
    /// Блок игрока
    /// </summary>
    /// <param name="type">Block/UnBlock</param>
    private void Block(string type)
    {
        if (type == "Block") // Если блокирует
        {
            _animator.SetBool("Block", true); // Установить анимацию блоки
            BlockStatus = true; // Статус блокирования = true
            if (_animator.GetFloat("Speed") > 2) // Ограничить скорость движения игрока максимум 2
            {
                _animator.SetFloat("Speed", 2);
                GetComponent<StarterAssetsInputs>().sprint = false;
            }
        }
        else
        {
            //Debug.Log("UnBlock");
            BlockStatus = false;
            _animator.SetBool("Block", false);
        }
    }

    /// <summary>
    /// Получение повреждения персонажем
    /// </summary>
    /// <param name="damage">Урон по персонажу</param>
    public void GetDamage(int damage)
    {
        _player.Health -= damage; // Отнять здоровье у персонажа игрока
        _animator.SetBool("Damaged", true); // Установить анимацию получения урона персонажем
        if (Player.Health <= 0) // Если здоровье персонажа достигло нуля
        {
            _gameManager.EndBattle(false); // Завершить бой
            _animator.SetTrigger("Death"); // Установить анимацию смерти персонажа
            ActivatedInputs(false); // Диактивировать систему ввода
        }
    }

    /// <summary>
    /// Конец получения урона, вызывается с события анимации
    /// </summary>
    private void EndDamaged()
    {
        _animator.SetBool("Damaged", false); // Отключить анимацию получения урона
        _canAttack = true; // Персонажа может атаковать
        _attackStatus = 0; // Обнулить серию атак персонажа
    }

    /// <summary>
    /// Когда GameObject сталкивается с другим GameObject, Unity вызывает OnTriggerEnter.
    /// </summary>
    /// <param name="other">Объект, с которым взаимодействует текущий объект</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StartZone") // Если персонаж вошел в старт зону боя
        {
            _gameManager.StartBattle(); // Начать бой
        }
        else if (other.tag == "EndZone" && _gameManager.isStartedBattle) // Если персонаж вошел в конечную зону  боя и бой был начат
        {
            _gameManager.EndBattle(true); // Закончить бой
            _gameManager.CloseDoor(); // Закрыть двер арены
        }
    }

    /// <summary>
    /// Удар персонажа, вызывается событием анимации
    /// </summary>
    private void Hit()
    {
        _canAttack = true; // Персонаж может атаковать

        _animator.SetInteger("Attack", _attackStatus); // Установить статус серии атак

        NowEnemies = NowEnemies.Distinct().ToList(); // Очистить лист задетых врагов от дубликатов, чтобы урона за один удар не защитался несколько раз
        foreach (GameObject enemy in NowEnemies) // Пройтись по всем врагам в списке задетых врагов
        {
            if (!_pinaltyDamage) enemy.GetComponent<Ai>().Enemy.Health -= _weapons[IdActiveWeapon].Damage + _player.Strength; // Если нет штрафа к урону, то нанести противнику полный урон оружия + сила персонажа
            else enemy.GetComponent<Ai>().Enemy.Health -= 5; // Иначе нанести штрафной урон в максимум 5 едениц
            enemy.GetComponent<Ai>().Animator.GetDamage(); // Противник получил урон
            if (enemy.GetComponent<Ai>().Enemy.Health <= 0) // Если здоровье врага достигло нуля
            {
                _gameManager.OpenDoor(); // Открыть дверь арены
                _gameManager.HideHealthEnemy(false); // Скрыть здоровье врага
                EnemyIsDead = true; // Установить флаг, что враг побежден
            }
        }
        NowEnemies.Clear(); // Очистить список задетых врагов
    }

    /// <summary>
    /// Окончание атаки, вызывается из анимации
    /// </summary>
    private void EndAttack()
    {
        _canAttack = true; // Персонаж может атаковать
        _attackStatus = 0; // Обнулить серию атак персонажа

        _animator.SetInteger("Attack", _attackStatus); // Установить серию атак персонажа
    }

    /// <summary>
    /// Этот метод срабатывает после смерти персонажа, вызывается из события анимации
    /// </summary>
    private void AfterDeath()
    {
        MyDataBase.UpdatePlayer(Player); // Обновить данные персонажа(Сохранить их в базу данных)
        SceneManager.LoadScene("Playground"); // Перезагрузить сцену
    }
}
