using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    private bool _isStartingBattle = false;
    [SerializeField] private Button btnStartBattle;
    [SerializeField] private TextMeshProUGUI txtMainDialog;

    [SerializeField] private GameObject _door;
    [SerializeField] private PlayerManager _player;

    private GameManager _gameManager;

    private void Update()
    {
        if (_isStartingBattle) btnStartBattle.interactable = false;
        else btnStartBattle.interactable = true;
    }
    private void Start()
    {
        txtMainDialog.text = "Добро пожаловать на арену, мой друг";
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void StartBattle()
    {
        _isStartingBattle = true;
        txtMainDialog.text = "Хорошо, следующий противник ждет тебя на арене";
        _gameManager.OpenDoor();
    }
    public void AboutGame()
    {
        txtMainDialog.text = "Об игре WarriorsOfArena";
    }
    public void ExitDialog()
    {
        txtMainDialog.text = "Добро пожаловать на арену, мой друг";
        _player.ExitFromDialog();
    }
}
