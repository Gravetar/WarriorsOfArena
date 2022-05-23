using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isStartedBattle = false;

    private float[] PlayerFavoriteWeapon = new float[3];
    private float _battleTime = 0;
    
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _enemyHealth;
    [SerializeField] private GameObject _startZone;
    [SerializeField] private GameObject _endZone;
    [SerializeField] private GameObject _player;
    private void Start()
    {
        _player = GameObject.Find("Player");

        PlayerFavoriteWeapon[0] = 0;
        PlayerFavoriteWeapon[1] = 0;
        PlayerFavoriteWeapon[2] = 0;

        MyDataBase.CreateFight(1, true, 2, ActionPlayer.Attack, 1, 2.35f);
    }

    private void FixedUpdate()
    {
        if (isStartedBattle)
        {
            PlayerFavoriteWeapon[_player.GetComponent<PlayerManager>().IdActiveWeapon] += Time.deltaTime;
            _battleTime += Time.deltaTime;
            Debug.Log(_battleTime);
            //Debug.Log(string.Format("Sword-{0} Axe-{1} Mace-{2}", PlayerFavoriteWeapon[0], PlayerFavoriteWeapon[1], PlayerFavoriteWeapon[2]));
        }
        
    }
    public void EndBattle(bool playerWinner)
    {
        RecordFight(playerWinner);
        isStartedBattle = false;
        _startZone.SetActive(true);
        _endZone.SetActive(false);
        if (playerWinner)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
        _battleTime = 0;
        PlayerFavoriteWeapon[0] = 0;
        PlayerFavoriteWeapon[1] = 0;
        PlayerFavoriteWeapon[2] = 0;
    }

    public void StartBattle()
    {
        _startZone.SetActive(false);
        _endZone.SetActive(true);
        HideHealthEnemy(true);
        isStartedBattle = true;
        _door.GetComponent<Animator>().SetBool("Opened", false);
    }

    public void OpenDoor()
    {
        _door.GetComponent<Animator>().SetBool("Opened", true);
    }

    public void CloseDoor()
    {
        _door.GetComponent<Animator>().SetBool("Opened", false);
    }

    public void HideHealthEnemy(bool isHided)
    {
        _enemyHealth.SetActive(isHided);
    }

    private void RecordFight(bool playerWinner)
    {

    }
}

public enum ActionPlayer
{
    Attack,
    Block
}
