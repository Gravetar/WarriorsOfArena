using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    private Player _player;
    private float _playerMaxSpeed = 6;

    private MyPlayerInput _input;
    private Animator _animator;
    public int AttackStatus=0;
    private bool _canAttack = true;

    public List<GameObject> NowEnemies = new List<GameObject>();

    
    public List<Weapon> Weapons = new List<Weapon>();

    public int IdActiveWeapon=0;

    public GameObject[] WeaponsVisual;

    private void Awake()
    {
        PerformedInputs();
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        _animator = GetComponent<Animator>();

        Weapons = MyDataBase.GetWeapons();
       _player = MyDataBase.GetPlayerById(DATAMovement.ActivePlayer);

        ActivateWeapon(0);
    }

    private void Update()
    {
        if (Mouse.current.rightButton.isPressed) Block("Block");
        else Block("UnBlock");

        if (_playerMaxSpeed == 0) GetComponent<StarterAssetsInputs>().move = new Vector2(0, 0);
    }

    private void PerformedInputs()
    {
        _input = new MyPlayerInput();

        _input.Player.Attack.performed += context => Attack();
        _input.Player.GetSword.performed += context => ActivateWeapon(0);
        _input.Player.GetAxe.performed += context => ActivateWeapon(1);
        _input.Player.GetMace.performed += context => ActivateWeapon(2);
        _input.Player.ExitToMenu.performed += context => ExitToMenu();
        _input.Player.Kick.performed += context => StartKick();
    }

    private void StartKick()
    {
        _animator.SetBool("Kick", true);
    }

    private void Kick()
    {
        _animator.SetBool("Kick", false);
        Debug.Log("Kick");
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    private void Attack()
    {
        if (AttackStatus == 0 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 1;
            Debug.Log("Attack" + _canAttack);

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 1 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 2;
            Debug.Log("Attack" + _canAttack);

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 2 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 3;
            Debug.Log("Attack" + _canAttack);

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 3 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 4;
            Debug.Log("Attack" + _canAttack);

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 4 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 0;
            Debug.Log("Attack" + _canAttack);

            _animator.SetInteger("Attack", AttackStatus);
        }
    }
    private void ActivateWeapon(int idWeapon)
    {
        _animator.SetBool("Equip", true);
        IdActiveWeapon = idWeapon;
        _playerMaxSpeed = 0;
    }
    public void ActivateWeaponWithAnimation()
    {
        foreach (GameObject weapon in WeaponsVisual)
        {
            weapon.SetActive(false);
        }
        WeaponsVisual[IdActiveWeapon].SetActive(true);
        float bonusAttackSpeed = (float)_player.Dexterity / 200;
        if (bonusAttackSpeed > 2.0f) bonusAttackSpeed = 2;
        _animator.SetFloat("AttackSpeed", Weapons[IdActiveWeapon].AttackSpeed + bonusAttackSpeed);
        _animator.SetBool("Equip", false);
        _playerMaxSpeed = 1;
    }

    private void Block(string type)
    {
        if (type == "Block")
        {
            //Debug.Log("Block");

            _animator.SetBool("Block", true);
            if (_animator.GetFloat("Speed") > 2)
            {
                _animator.SetFloat("Speed", 2);
                GetComponent<StarterAssetsInputs>().sprint = false;
            }
        }
        else
        {
            //Debug.Log("UnBlock");

            _animator.SetBool("Block", false);
        }
    }

    private void Hit()
    {
        _canAttack = true;
        Debug.Log("HIT");

        _animator.SetInteger("Attack", AttackStatus);

        NowEnemies = NowEnemies.Distinct().ToList();
        foreach (GameObject enemy in NowEnemies)
        {
            enemy.GetComponent<EnemyManager>().Health -= Weapons[IdActiveWeapon].Damage + _player.Strength;
            Debug.Log(string.Format("{0} + {1}", Weapons[IdActiveWeapon].Damage, _player.Strength));
            if (enemy.GetComponent<EnemyManager>().Health <= 0) Destroy(enemy);
        }

        NowEnemies.Clear();
    }
    private void EndAttack()
    {

        _canAttack = true;
        AttackStatus = 0;
        Debug.Log("END");

        _animator.SetInteger("Attack", AttackStatus);
    }
}
