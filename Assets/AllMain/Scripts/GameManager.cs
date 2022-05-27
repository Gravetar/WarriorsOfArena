/*
Основной менеджер игры, здесь производятся все основные действия с игрой

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isStartedBattle = false; // Флаг - Бой начался
    public UIManagerGame GameUI; // Основной UI интерфейс игровой сцены

    private float[] PlayerFavoriteWeapon = new float[3]; // Предпочитаемое игроком оружие (на основе времени использования)
    private float _battleTime = 0; // Время боя
    
    [SerializeField] private GameObject _door; // Объект дверь
    [SerializeField] private GameObject _enemyHealth; // Объект здоровье противника
    [SerializeField] private GameObject _startZone; // Зона начала боя
    [SerializeField] private GameObject _endZone; // Зона окнчания боя
    [SerializeField] private GameObject _player; // Объект персонажа игрока
    [SerializeField] private GameObject _enemyPrefab; // Префаб врага

    /// <summary>
    /// Start вызывается, когда скрипт включен, вызывается непосредственно перед первым вызовом любого из методов обновления.
    /// </summary>
    private void Start()
    {
        _player = GameObject.Find("Player"); // Найти объект игрока

        // Установить предпочитаемое игроком оружие начальные значения в 0
        PlayerFavoriteWeapon[0] = 0;
        PlayerFavoriteWeapon[1] = 0;
        PlayerFavoriteWeapon[2] = 0;

        Cursor.lockState = CursorLockMode.Confined; // Закрепить курсор в окне игры
    }

    /// <summary>
    /// Когда GameObject сталкивается с другим GameObject, Unity вызывает OnTriggerEnter.
    /// </summary>
    /// <param name="other">Объект, с которым взаимодействует текущий объект</param>
    private void FixedUpdate()
    {
        if (isStartedBattle && !_player.GetComponent<PlayerManager>().EnemyIsDead) // Если бой начат и враг живой
        {
            PlayerFavoriteWeapon[_player.GetComponent<PlayerManager>().IdActiveWeapon] += Time.deltaTime; // Прибавить время использования к текущему используемому игроком оружию
            _battleTime += Time.deltaTime; // Прибавить время к бою
        }
        
    }

    /// <summary>
    /// Завершения боя
    /// </summary>
    /// <param name="playerWinner">Флаг - Игрок победил</param>
    public void EndBattle(bool playerWinner)
    {
        RecordFight(playerWinner); // Записать бой
        isStartedBattle = false; // Бой не начат
        _startZone.SetActive(true); // Активировать стартовую зону боя
        _endZone.SetActive(false); // Диактивировать конечную зону боя

        GameUI.isActivatedButtonStart(true); // Активировать кнопку начала боя в окне диалога

        OpenDoor(); // Открыть дверь
        int fight_number; // Номер боя (Влияет на сложность противника)
        if (playerWinner) fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) + 1; // Если игрок победил то номер боя +
        else fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) - 1; // Иначе номер боя -

        if (playerWinner) // Если игрок победил
        {
            _player.GetComponent<PlayerManager>().Player.Experience += 100 * fight_number; // Прибавить игроку опыт
            _player.GetComponent<PlayerManager>().Player.CheckXp(); // Проверка опыта на повышение уровня
            _player.GetComponent<PlayerManager>().Player.Health = _player.GetComponent<PlayerManager>().Player.MaxHealth; // Восстановить здоровье персонажа
        }

        // Обнулить данные боя
        _battleTime = 0;
        PlayerFavoriteWeapon[0] = 0;
        PlayerFavoriteWeapon[1] = 0;
        PlayerFavoriteWeapon[2] = 0;
        if (_player.GetComponent<PlayerManager>().EnemyIsDead) // Если враг умер
        Destroy(GameObject.Find("Enemy").gameObject); // Удалить его объект
    }

    /// <summary>
    /// Начало боя
    /// </summary>
    public void StartBattle()
    {
        _startZone.SetActive(false); // Диактивировать стартовую зону боя
        _endZone.SetActive(true); // Активирвоать конечную зону боя
        HideHealthEnemy(true); // Показать здоровье врага
        isStartedBattle = true; // Бой начался
        CloseDoor(); // Закрыть дверь
    }

    /// <summary>
    /// Открыть дверь арены
    /// </summary>
    public void OpenDoor()
    {
        _door.GetComponent<Animator>().SetBool("Opened", true);
    }

    /// <summary>
    /// Закрыть дверь арены
    /// </summary>
    public void CloseDoor()
    {
        _door.GetComponent<Animator>().SetBool("Opened", false);
    }

    /// <summary>
    /// Показать/скрыть здоровье врага
    /// </summary>
    /// <param name="isHided">Флаг - Скрывать?</param>
    public void HideHealthEnemy(bool isHided)
    {
        _enemyHealth.SetActive(isHided);
    }

    /// <summary>
    /// Запись боя
    /// </summary>
    /// <param name="playerWinner">Флаг Игрок-победитель</param>
    private void RecordFight(bool playerWinner)
    {
        //Определить предпочитаемое оружие
        float maxValue = PlayerFavoriteWeapon.Max();
        int indexMaxFavoriteWeapon = Array.IndexOf(PlayerFavoriteWeapon, maxValue);

        ActionPlayer actionPlayer;//Предпочитаемое действие игрока
        if (_player.GetComponent<PlayerManager>().AttackCount >= _player.GetComponent<PlayerManager>().BlockTime) actionPlayer = ActionPlayer.Attack; // Если игрок больше атаковал, установить предпочтиаемое действие - атака
        else actionPlayer = ActionPlayer.Block; // Иначе установить пердпочитаемое действие игрока - блок
        int fight_number; // Номер боя (Влияет на сложность противника)
        if (playerWinner) fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) + 1; // Если игрок победил то номер боя +
        else fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) - 1; // Иначе номер боя -
        if (fight_number <= 1) fight_number = 1; // Номер боя не может быть меньше 1
        // Записать бой в базу данных
        MyDataBase.CreateFight(_player.GetComponent<PlayerManager>().Player.Id, playerWinner, indexMaxFavoriteWeapon, actionPlayer, fight_number, _battleTime);
    }

    /// <summary>
    /// Создать противника
    /// </summary>
    public void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(_enemyPrefab); // Создать объект враг
        newEnemy.name = "Enemy"; // Установить имя объекта
        _player.GetComponent<PlayerManager>().EnemyIsDead = false; // "Оживить" врага
    }
}

/// <summary>
/// Действия игрока
/// </summary>
public enum ActionPlayer
{
    Attack,
    Block
}
