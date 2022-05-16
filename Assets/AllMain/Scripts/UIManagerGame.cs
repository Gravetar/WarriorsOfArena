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
        _txtCharacteristic.text = string.Format("Сила: {0}\n\nЛовкость: {1}\n\nЗдоровье: {2}/{3}\n\nВыносливость: {4}\n\n",
        _player.Strength, _player.Dexterity, _player.Health, _player.MaxHealth, _player.Stamina
    );
        _txtStats.text = string.Format("Уровень: {0}\nОпыт: {1}/{2}\nСвободно очков прокачки: {3}\n",
        _player.Level, _player.Experience, 500, _player.FreeXpPoints
    );
    }
}
