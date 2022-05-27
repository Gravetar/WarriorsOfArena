/*
�������� �������� ����, ����� ������������ ��� �������� �������� � �����

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using System;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isStartedBattle = false; // ���� - ��� �������
    public UIManagerGame GameUI; // �������� UI ��������� ������� �����

    private float[] PlayerFavoriteWeapon = new float[3]; // �������������� ������� ������ (�� ������ ������� �������������)
    private float _battleTime = 0; // ����� ���
    
    [SerializeField] private GameObject _door; // ������ �����
    [SerializeField] private GameObject _enemyHealth; // ������ �������� ����������
    [SerializeField] private GameObject _startZone; // ���� ������ ���
    [SerializeField] private GameObject _endZone; // ���� �������� ���
    [SerializeField] private GameObject _player; // ������ ��������� ������
    [SerializeField] private GameObject _enemyPrefab; // ������ �����

    /// <summary>
    /// Start ����������, ����� ������ �������, ���������� ��������������� ����� ������ ������� ������ �� ������� ����������.
    /// </summary>
    private void Start()
    {
        _player = GameObject.Find("Player"); // ����� ������ ������

        // ���������� �������������� ������� ������ ��������� �������� � 0
        PlayerFavoriteWeapon[0] = 0;
        PlayerFavoriteWeapon[1] = 0;
        PlayerFavoriteWeapon[2] = 0;

        Cursor.lockState = CursorLockMode.Confined; // ��������� ������ � ���� ����
    }

    /// <summary>
    /// ����� GameObject ������������ � ������ GameObject, Unity �������� OnTriggerEnter.
    /// </summary>
    /// <param name="other">������, � ������� ��������������� ������� ������</param>
    private void FixedUpdate()
    {
        if (isStartedBattle && !_player.GetComponent<PlayerManager>().EnemyIsDead) // ���� ��� ����� � ���� �����
        {
            PlayerFavoriteWeapon[_player.GetComponent<PlayerManager>().IdActiveWeapon] += Time.deltaTime; // ��������� ����� ������������� � �������� ������������� ������� ������
            _battleTime += Time.deltaTime; // ��������� ����� � ���
        }
        
    }

    /// <summary>
    /// ���������� ���
    /// </summary>
    /// <param name="playerWinner">���� - ����� �������</param>
    public void EndBattle(bool playerWinner)
    {
        RecordFight(playerWinner); // �������� ���
        isStartedBattle = false; // ��� �� �����
        _startZone.SetActive(true); // ������������ ��������� ���� ���
        _endZone.SetActive(false); // �������������� �������� ���� ���

        GameUI.isActivatedButtonStart(true); // ������������ ������ ������ ��� � ���� �������

        OpenDoor(); // ������� �����
        int fight_number; // ����� ��� (������ �� ��������� ����������)
        if (playerWinner) fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) + 1; // ���� ����� ������� �� ����� ��� +
        else fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) - 1; // ����� ����� ��� -

        if (playerWinner) // ���� ����� �������
        {
            _player.GetComponent<PlayerManager>().Player.Experience += 100 * fight_number; // ��������� ������ ����
            _player.GetComponent<PlayerManager>().Player.CheckXp(); // �������� ����� �� ��������� ������
            _player.GetComponent<PlayerManager>().Player.Health = _player.GetComponent<PlayerManager>().Player.MaxHealth; // ������������ �������� ���������
        }

        // �������� ������ ���
        _battleTime = 0;
        PlayerFavoriteWeapon[0] = 0;
        PlayerFavoriteWeapon[1] = 0;
        PlayerFavoriteWeapon[2] = 0;
        if (_player.GetComponent<PlayerManager>().EnemyIsDead) // ���� ���� ����
        Destroy(GameObject.Find("Enemy").gameObject); // ������� ��� ������
    }

    /// <summary>
    /// ������ ���
    /// </summary>
    public void StartBattle()
    {
        _startZone.SetActive(false); // �������������� ��������� ���� ���
        _endZone.SetActive(true); // ������������ �������� ���� ���
        HideHealthEnemy(true); // �������� �������� �����
        isStartedBattle = true; // ��� �������
        CloseDoor(); // ������� �����
    }

    /// <summary>
    /// ������� ����� �����
    /// </summary>
    public void OpenDoor()
    {
        _door.GetComponent<Animator>().SetBool("Opened", true);
    }

    /// <summary>
    /// ������� ����� �����
    /// </summary>
    public void CloseDoor()
    {
        _door.GetComponent<Animator>().SetBool("Opened", false);
    }

    /// <summary>
    /// ��������/������ �������� �����
    /// </summary>
    /// <param name="isHided">���� - ��������?</param>
    public void HideHealthEnemy(bool isHided)
    {
        _enemyHealth.SetActive(isHided);
    }

    /// <summary>
    /// ������ ���
    /// </summary>
    /// <param name="playerWinner">���� �����-����������</param>
    private void RecordFight(bool playerWinner)
    {
        //���������� �������������� ������
        float maxValue = PlayerFavoriteWeapon.Max();
        int indexMaxFavoriteWeapon = Array.IndexOf(PlayerFavoriteWeapon, maxValue);

        ActionPlayer actionPlayer;//�������������� �������� ������
        if (_player.GetComponent<PlayerManager>().AttackCount >= _player.GetComponent<PlayerManager>().BlockTime) actionPlayer = ActionPlayer.Attack; // ���� ����� ������ ��������, ���������� �������������� �������� - �����
        else actionPlayer = ActionPlayer.Block; // ����� ���������� �������������� �������� ������ - ����
        int fight_number; // ����� ��� (������ �� ��������� ����������)
        if (playerWinner) fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) + 1; // ���� ����� ������� �� ����� ��� +
        else fight_number = MyDataBase.GetLastFightNumberByPlayerId(_player.GetComponent<PlayerManager>().Player.Id) - 1; // ����� ����� ��� -
        if (fight_number <= 1) fight_number = 1; // ����� ��� �� ����� ���� ������ 1
        // �������� ��� � ���� ������
        MyDataBase.CreateFight(_player.GetComponent<PlayerManager>().Player.Id, playerWinner, indexMaxFavoriteWeapon, actionPlayer, fight_number, _battleTime);
    }

    /// <summary>
    /// ������� ����������
    /// </summary>
    public void CreateEnemy()
    {
        GameObject newEnemy = Instantiate(_enemyPrefab); // ������� ������ ����
        newEnemy.name = "Enemy"; // ���������� ��� �������
        _player.GetComponent<PlayerManager>().EnemyIsDead = false; // "�������" �����
    }
}

/// <summary>
/// �������� ������
/// </summary>
public enum ActionPlayer
{
    Attack,
    Block
}
