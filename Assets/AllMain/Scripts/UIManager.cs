/*
Данный файл предназначен для управления
логикой UI элементов в меню игры приложения.

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerInfoPrefab; // Объект для вывода информации на странице загрузки
    [SerializeField] private GameObject _playersPanel; // Объект-панель всех доступных персонажей
    [SerializeField] private GameObject _newGamePanel; // Объект-панель новой игры
    [SerializeField] private GameObject _mainmenuPanel; // Объект - панель основного меню игры
    [SerializeField] private Image _scrollImage; // Изображение - Скролл для панели персонажей в загрезке игры
    [SerializeField] private TMP_InputField _newGameNameChar; // Поле ввода имени персонажа
    [SerializeField] private TextMeshProUGUI _notNameTextError; // Поле для отображения, что пользователь не ввел имя персонажа

    private List<Player> _players = new List<Player>(); // Лист персонажей для подгрузки их на страницу загрузки игры

    /// <summary>
    /// Start вызывается, когда скрипт включен, вызывается непосредственно перед первым вызовом любого из методов обновления
    /// </summary>
    void Start()
    {
        // Обработки курсора
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// Метод для возвращения в главное меню с любой другой страницы меню.
    /// </summary>
    public void ReturnToMenu()
    {
        // Не отбоброжать сообщение о невведенном имени персонажа
        _notNameTextError.text = "";
        // Вернуться в меню
        ActivateMenu(_mainmenuPanel);
    }

    /// <summary>
    /// Метод для старта новой игры.
    /// </summary>
    public void StartNewGame()
    {
        // Если поле ввода пустое
        if (_newGameNameChar.text == "")
        {
            _notNameTextError.text = "Введите пожалуйста имя персонажа"; // Установить текст, что не введено имя персонажа
        }
        else // Иначе
        {
            MyDataBase.CreatePlayer(_newGameNameChar.text); // Создать персонажа
            StartPlaying(MyDataBase.GetLastPlayerId()); // Старт игры
        }
    }

    /// <summary>
    /// Метод для открытия страницы новой игры
    /// </summary>
    public void OpenNewGame()
    {
        ActivateMenu(_newGamePanel);
    }

    /// <summary>
    /// Метод для открытия страницы загрузки игры
    /// </summary>
    public void OpenLoadGame()
    {
        ActivateMenu(_playersPanel); // Активировать панель с персонажами

        // Очистить список персонажей, если он загружался ранее
        _players.Clear();
        foreach (Transform ch in _playersPanel.transform.Find("Content").Find("PlayersPanel"))
        {
            Destroy(ch.gameObject);
        }

        // Получить всех персонажей из базы данных
        _players = MyDataBase.GetPlayers();

        // Загрузить данные персонажей на их панели
        foreach (Player pl in _players)
        {
            string description = string.Format("Уровень: {0}, Сила: {1}, Ловкость: {2}\nЗдоровье: {3}, Выносливость: {4}",
                pl.Level, pl.Strength, pl.Dexterity, pl.Health, pl.Stamina);
            GameObject Temp = Instantiate(_playerInfoPrefab, _playersPanel.transform.Find("Content").Find("PlayersPanel").transform);
            Temp.name = "Player " + pl.Id.ToString();
            Temp.transform.Find("TNamePlayer").GetComponent<TextMeshProUGUI>().text = pl.Name;
            Temp.transform.Find("TDescriptionPlayer").GetComponent<TextMeshProUGUI>().text = description;

            Button PlayButton = Temp.transform.GetComponent<Button>();
            PlayButton.onClick.AddListener(() => StartPlaying(pl.Id));
        }

        _scrollImage.SetNativeSize();
    }

    /// <summary>
    /// Метод для открытия указанной страницы
    /// </summary>
    /// <param name="panel">Целевая страница</param>
    private void ActivateMenu(GameObject panel)
    {
        //Диактивировать все страницы
        _playersPanel.SetActive(false);
        _mainmenuPanel.SetActive(false);
        _newGamePanel.SetActive(false);

        //Активировать целевую страницу
        panel.SetActive(true);
    }

    /// <summary>
    /// Запуск игры на указанном персонажа
    /// </summary>
    /// <param name="Playerid">Id запускаемого персонажа</param>
    private void StartPlaying(int Playerid)
    {
        // Установить текущего персонажа
        DATAMovement.ActivePlayer = Playerid;
        // Загрузить основную сцену игры
        SceneManager.LoadScene("Playground");
    }
}
