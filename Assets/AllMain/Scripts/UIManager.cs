/*
������ ���� ������������ ��� ����������
������� UI ��������� � ���� ���� ����������.

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerInfoPrefab; // ������ ��� ������ ���������� �� �������� ��������
    [SerializeField] private GameObject _playersPanel; // ������-������ ���� ��������� ����������
    [SerializeField] private GameObject _newGamePanel; // ������-������ ����� ����
    [SerializeField] private GameObject _mainmenuPanel; // ������ - ������ ��������� ���� ����
    [SerializeField] private Image _scrollImage; // ����������� - ������ ��� ������ ���������� � �������� ����
    [SerializeField] private TMP_InputField _newGameNameChar; // ���� ����� ����� ���������
    [SerializeField] private TextMeshProUGUI _notNameTextError; // ���� ��� �����������, ��� ������������ �� ���� ��� ���������

    private List<Player> _players = new List<Player>(); // ���� ���������� ��� ��������� �� �� �������� �������� ����

    /// <summary>
    /// Start ����������, ����� ������ �������, ���������� ��������������� ����� ������ ������� ������ �� ������� ����������
    /// </summary>
    void Start()
    {
        // ��������� �������
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// ����� ��� ����������� � ������� ���� � ����� ������ �������� ����.
    /// </summary>
    public void ReturnToMenu()
    {
        // �� ����������� ��������� � ����������� ����� ���������
        _notNameTextError.text = "";
        // ��������� � ����
        ActivateMenu(_mainmenuPanel);
    }

    /// <summary>
    /// ����� ��� ������ ����� ����.
    /// </summary>
    public void StartNewGame()
    {
        // ���� ���� ����� ������
        if (_newGameNameChar.text == "")
        {
            _notNameTextError.text = "������� ���������� ��� ���������"; // ���������� �����, ��� �� ������� ��� ���������
        }
        else // �����
        {
            MyDataBase.CreatePlayer(_newGameNameChar.text); // ������� ���������
            StartPlaying(MyDataBase.GetLastPlayerId()); // ����� ����
        }
    }

    /// <summary>
    /// ����� ��� �������� �������� ����� ����
    /// </summary>
    public void OpenNewGame()
    {
        ActivateMenu(_newGamePanel);
    }

    /// <summary>
    /// ����� ��� �������� �������� �������� ����
    /// </summary>
    public void OpenLoadGame()
    {
        ActivateMenu(_playersPanel); // ������������ ������ � �����������

        // �������� ������ ����������, ���� �� ���������� �����
        _players.Clear();
        foreach (Transform ch in _playersPanel.transform.Find("Content").Find("PlayersPanel"))
        {
            Destroy(ch.gameObject);
        }

        // �������� ���� ���������� �� ���� ������
        _players = MyDataBase.GetPlayers();

        // ��������� ������ ���������� �� �� ������
        foreach (Player pl in _players)
        {
            string description = string.Format("�������: {0}, ����: {1}, ��������: {2}\n��������: {3}, ������������: {4}",
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
    /// ����� ��� �������� ��������� ��������
    /// </summary>
    /// <param name="panel">������� ��������</param>
    private void ActivateMenu(GameObject panel)
    {
        //�������������� ��� ��������
        _playersPanel.SetActive(false);
        _mainmenuPanel.SetActive(false);
        _newGamePanel.SetActive(false);

        //������������ ������� ��������
        panel.SetActive(true);
    }

    /// <summary>
    /// ������ ���� �� ��������� ���������
    /// </summary>
    /// <param name="Playerid">Id ������������ ���������</param>
    private void StartPlaying(int Playerid)
    {
        // ���������� �������� ���������
        DATAMovement.ActivePlayer = Playerid;
        // ��������� �������� ����� ����
        SceneManager.LoadScene("Playground");
    }
}
