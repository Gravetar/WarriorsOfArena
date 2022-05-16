using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManagerGame : MonoBehaviour
{
    private Player _player;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private TextMeshProUGUI _txtStats;
    [SerializeField] private TextMeshProUGUI _txtCharacteristic;

    public void UpdateInfo()
    {
        _player = _playerManager.Player;
        _txtCharacteristic.text = string.Format("����: {0}\n\n��������: {1}\n\n��������: {2}/{3}\n\n������������: {4}\n\n",
        _player.Strength, _player.Dexterity, _player.Health, _player.MaxHealth, _player.Stamina
    );
        _txtStats.text = string.Format("�������: {0}\n����: {1}/{2}\n�������� ����� ��������: {3}\n",
        _player.Level, _player.Experience, 500, _player.FreeXpPoints
    );
    }
}
