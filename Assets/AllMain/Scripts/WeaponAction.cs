/*
������ �����- ������ �������� �� ������ ������ � ����

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    private PlayerManager _player; // �������� ��������� ������
    private Ai _enemy; // ���������
    [SerializeField] private bool isWeaponFromPlayer; // ����������� �� ������ ������

    /// <summary>
    /// Start ����������, ����� ������ �������, ���������� ��������������� ����� ������ ������� ������ �� ������� ����������.
    /// </summary>
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerManager>(); // ����� � ���������� ��������� ������
        if (!isWeaponFromPlayer) _enemy = GameObject.Find("Enemy").GetComponent<Ai>(); // ���� ������ �� ����������� ������ ����� � ���������� ����������
    }

    /// <summary>
    /// ����� GameObject ������������ � ������ GameObject, Unity �������� OnTriggerEnter.
    /// </summary>
    /// <param name="other">������, � ������� ��������������� ������� ������</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isWeaponFromPlayer) // ���� ����� ���� � ������ ����������� ������
        {
            if (!(other.GetComponent<Ai>().StateMachine.CurrentState is StateBlock)) // ���� ���� �� � �����
                _player.NowEnemies.Add(other.gameObject); // �������� ����� � ������ ������� �����������
        }
        else if (other.gameObject.tag == "Player" && !isWeaponFromPlayer && _player.BlockStatus == false) // ���� ����� ����� � ������ �� ����������� ������ � ������ �� � �����
        {
            _enemy.ShotPlayer = true; // ����� �����
        }
    }
}
