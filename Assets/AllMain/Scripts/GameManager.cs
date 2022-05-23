using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isStartedBattle = false;
    
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _enemyHealth;
    [SerializeField] private GameObject _startZone;
    [SerializeField] private GameObject _endZone;
    public void EndBattle()
    {
        _startZone.SetActive(true);
        _endZone.SetActive(false);
        isStartedBattle = true;
        _door.GetComponent<Animator>().SetBool("Opened", true);
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
}
