/*
Класс-скрипт для управления элементами интерфейса в основной игре

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerGame : MonoBehaviour
{
    public bool VisibleCursor = false; // Флаг отображения курсора

    [SerializeField] private TextMeshProUGUI _txtStats; // Текстовое поле с основной статистикой персонажа в окне статистики персонажа
    [SerializeField] private TextMeshProUGUI _txtCharacteristic; // Текстовое поле с характеристиками персонажа в окне статистики персонажа
    [SerializeField] private TextMeshProUGUI _txtMainDialog; // Текстовое поле для главнового текста в окне диалога
    [SerializeField] private Button _btnStartBattle; // Кнопка начала боя
    [SerializeField] private GameObject _dialogPanel; // Объект окна диалога
    [SerializeField] private GameObject _characteristicWindow; // Объект окна статистики
    [SerializeField] private GameObject _txtDialog; // Текст - информация о возможности начать диалог

    [SerializeField] private PlayerManager _playerManager; // Основной менеджер игрока
    [SerializeField] private GameObject _door; // Дверь входа на арену

    private GameManager _gameManager; // Основной менеджер игры
    private Player _player; // Основной персонаж игрока

    /// <summary>
    /// Start вызывается, когда скрипт включен, вызывается непосредственно перед первым вызовом любого из методов обновления.
    /// </summary>
    private void Start()
    {
        _txtMainDialog.text = "Добро пожаловать на арену, мой друг"; // Установить значение текста в главном тексте окна диалога
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); // Установить основного менеджра игры
    }

    /// <summary>
    /// Обновление вызывается каждый кадр, если MonoBehaviour включен.
    /// </summary>
    private void Update()
    {
        Cursor.visible = VisibleCursor;
    }

    /// <summary>
    /// Обновить информацию о персонаже для верного отображения в окне статистики
    /// </summary>
    public void UpdateInfo()
    {
        _player = _playerManager.Player; // Установить основного игрока из менеджера игрока
        _txtCharacteristic.text = string.Format("Сила: {0}\n\nЛовкость: {1}\n\nЗдоровье: {2}/{3}\n\nВыносливость: {4}\n\n", // Заполнить характеристики
        _player.Strength, _player.Dexterity, _player.Health, _player.MaxHealth, _player.Stamina                             //  в окне
    );                                                                                                                      //  статистики персонажа
        _txtStats.text = string.Format("Уровень: {0}\nОпыт: {1}\nСвободно очков прокачки: {3}\n",                       // Заполнить основную статистику
        _player.Level, _player.Experience, 500, _player.FreeXpPoints                                                        //  в окне
    );                                                                                                                      //  статистики персонажа
    }

    /// <summary>
    /// Начать бой, срабатывает по нажатию на кнопку "Начать бой" в окне диалога
    /// </summary>
    public void StartBattle()
    {
        _txtMainDialog.text = "Хорошо, следующий противник ждет тебя на арене"; // Установить значение текста в главном тексте окна диалога
        _gameManager.OpenDoor(); // Открыть дверь арены
        isActivatedButtonStart(false); // Диактивировать интерактивность кнопки "Начать бой" в окне диалога
        _gameManager.CreateEnemy();
    }
    /// <summary>
    /// Об игре, срабатывает по нажатию на кнопку "Об игре" в окне диалога
    /// </summary>
    public void AboutGame()
    {
        _txtMainDialog.text = "Об игре WarriorsOfArena"; // Установить значение текста в главном тексте окна диалога
    }
    /// <summary>
    /// Выйти с окна диалога
    /// </summary>
    public void ExitDialog()
    {
        _txtMainDialog.text = "Добро пожаловать на арену, мой друг";// Установить значение текста в главном тексте окна диалога
        _txtDialog.SetActive(false); //Диактивирвоать Текст
        _dialogPanel.SetActive(false); //Диактивирвоать Окно диалога
        VisibleCursor = false; // Скрыть курсор
        _playerManager.ActivatedInputs(true);
    }

    /// <summary>
    /// Начать диалог
    /// </summary>
    public void StartDialog()
    {
        _txtMainDialog.text = "Добро пожаловать на арену, мой друг";// Установить значение текста в главном тексте окна диалога
        _dialogPanel.SetActive(true); //Активирвоать Окно диалога
        VisibleCursor = true; // Показать курсор
        _playerManager.ActivatedInputs(false);
    }

    /// <summary>
    /// Открыть окно статистики
    /// </summary>
    public void OpenStatistic()
    {
        UpdateInfo();
        _characteristicWindow.SetActive(!_characteristicWindow.activeSelf);
        VisibleCursor = _characteristicWindow.activeSelf;
    }

    /// <summary>
    /// Установить интерактивна ли кнопка старта боя
    /// </summary>
    /// <param name="isActive">Флаг активности</param>
    public void isActivatedButtonStart(bool isActive)
    {
        _btnStartBattle.interactable = isActive;
    }

    /// <summary>
    /// Установить видимость текств информации о возможности начать диалог
    /// </summary>
    public void ActivateTxtDialog(bool active)
    {
        _txtDialog.SetActive(active);
    }

    /// <summary>
    /// Поднять ловкость на 1
    /// </summary>
    public void DextirityUp()
    {
        if (_player.FreeXpPoints > 0)
        {
            _player.Dexterity += 1;
            _player.FreeXpPoints -= 1;
            UpdateInfo();
        }
    }

    /// <summary>
    /// Поднять силу на 1
    /// </summary>
    public void StrengthUp()
    {
        if (_player.FreeXpPoints > 0)
        {
            _player.Strength += 1;
            _player.FreeXpPoints -= 1;
            UpdateInfo();
        }
    }
}
