/*
�����-������ ��� ���������� ���������� ���������� � �������� ����

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerGame : MonoBehaviour
{
    public bool VisibleCursor = false; // ���� ����������� �������

    [SerializeField] private TextMeshProUGUI _txtStats; // ��������� ���� � �������� ����������� ��������� � ���� ���������� ���������
    [SerializeField] private TextMeshProUGUI _txtCharacteristic; // ��������� ���� � ���������������� ��������� � ���� ���������� ���������
    [SerializeField] private TextMeshProUGUI _txtMainDialog; // ��������� ���� ��� ���������� ������ � ���� �������
    [SerializeField] private Button _btnStartBattle; // ������ ������ ���
    [SerializeField] private GameObject _dialogPanel; // ������ ���� �������
    [SerializeField] private GameObject _characteristicWindow; // ������ ���� ����������
    [SerializeField] private GameObject _txtDialog; // ����� - ���������� � ����������� ������ ������

    [SerializeField] private PlayerManager _playerManager; // �������� �������� ������
    [SerializeField] private GameObject _door; // ����� ����� �� �����

    private GameManager _gameManager; // �������� �������� ����
    private Player _player; // �������� �������� ������

    /// <summary>
    /// Start ����������, ����� ������ �������, ���������� ��������������� ����� ������ ������� ������ �� ������� ����������.
    /// </summary>
    private void Start()
    {
        _txtMainDialog.text = "����� ���������� �� �����, ��� ����"; // ���������� �������� ������ � ������� ������ ���� �������
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>(); // ���������� ��������� �������� ����
    }

    /// <summary>
    /// ���������� ���������� ������ ����, ���� MonoBehaviour �������.
    /// </summary>
    private void Update()
    {
        Cursor.visible = VisibleCursor;
    }

    /// <summary>
    /// �������� ���������� � ��������� ��� ������� ����������� � ���� ����������
    /// </summary>
    public void UpdateInfo()
    {
        _player = _playerManager.Player; // ���������� ��������� ������ �� ��������� ������
        _txtCharacteristic.text = string.Format("����: {0}\n\n��������: {1}\n\n��������: {2}/{3}\n\n������������: {4}\n\n", // ��������� ��������������
        _player.Strength, _player.Dexterity, _player.Health, _player.MaxHealth, _player.Stamina                             //  � ����
    );                                                                                                                      //  ���������� ���������
        _txtStats.text = string.Format("�������: {0}\n����: {1}\n�������� ����� ��������: {3}\n",                       // ��������� �������� ����������
        _player.Level, _player.Experience, 500, _player.FreeXpPoints                                                        //  � ����
    );                                                                                                                      //  ���������� ���������
    }

    /// <summary>
    /// ������ ���, ����������� �� ������� �� ������ "������ ���" � ���� �������
    /// </summary>
    public void StartBattle()
    {
        _txtMainDialog.text = "������, ��������� ��������� ���� ���� �� �����"; // ���������� �������� ������ � ������� ������ ���� �������
        _gameManager.OpenDoor(); // ������� ����� �����
        isActivatedButtonStart(false); // �������������� ��������������� ������ "������ ���" � ���� �������
        _gameManager.CreateEnemy();
    }
    /// <summary>
    /// �� ����, ����������� �� ������� �� ������ "�� ����" � ���� �������
    /// </summary>
    public void AboutGame()
    {
        _txtMainDialog.text = "�� ���� WarriorsOfArena"; // ���������� �������� ������ � ������� ������ ���� �������
    }
    /// <summary>
    /// ����� � ���� �������
    /// </summary>
    public void ExitDialog()
    {
        _txtMainDialog.text = "����� ���������� �� �����, ��� ����";// ���������� �������� ������ � ������� ������ ���� �������
        _txtDialog.SetActive(false); //�������������� �����
        _dialogPanel.SetActive(false); //�������������� ���� �������
        VisibleCursor = false; // ������ ������
        _playerManager.ActivatedInputs(true);
    }

    /// <summary>
    /// ������ ������
    /// </summary>
    public void StartDialog()
    {
        _txtMainDialog.text = "����� ���������� �� �����, ��� ����";// ���������� �������� ������ � ������� ������ ���� �������
        _dialogPanel.SetActive(true); //������������ ���� �������
        VisibleCursor = true; // �������� ������
        _playerManager.ActivatedInputs(false);
    }

    /// <summary>
    /// ������� ���� ����������
    /// </summary>
    public void OpenStatistic()
    {
        UpdateInfo();
        _characteristicWindow.SetActive(!_characteristicWindow.activeSelf);
        VisibleCursor = _characteristicWindow.activeSelf;
    }

    /// <summary>
    /// ���������� ������������ �� ������ ������ ���
    /// </summary>
    /// <param name="isActive">���� ����������</param>
    public void isActivatedButtonStart(bool isActive)
    {
        _btnStartBattle.interactable = isActive;
    }

    /// <summary>
    /// ���������� ��������� ������ ���������� � ����������� ������ ������
    /// </summary>
    public void ActivateTxtDialog(bool active)
    {
        _txtDialog.SetActive(active);
    }

    /// <summary>
    /// ������� �������� �� 1
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
    /// ������� ���� �� 1
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
