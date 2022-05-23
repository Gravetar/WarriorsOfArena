using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    private PlayerManager _player;
    private Ai _enemy;
    [SerializeField] private bool isWeaponFromPlayer;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerManager>();
        _enemy = GameObject.Find("Enemy").GetComponent<Ai>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isWeaponFromPlayer)
        {
            if (!(other.GetComponent<Ai>().StateMachine.CurrentState is StateBlock))
                _player.NowEnemies.Add(other.gameObject);
        }
        else if (other.gameObject.tag == "Player" && !isWeaponFromPlayer && _player.BlockStatus == false)
        {
            _enemy.ShotPlayer = true;
        }
    }
}
