using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Player Player => _player;
    private Player _player;
    public GameManager GameManager;
    private bool _pinaltyDamage = false;
    [SerializeField] private Image _playerHealthImage;
    [SerializeField] private Image _playerStaminaImage;
    [SerializeField] private GameObject _characteristicWindow;
    private float _playerMaxSpeed = 6;

    private MyPlayerInput _input;
    private Animator _animator;
    public int AttackStatus=0;
    private bool _canAttack = true;

    public List<GameObject> NowEnemies = new List<GameObject>();

    
    public List<Weapon> Weapons = new List<Weapon>();

    public int IdActiveWeapon=0;

    public GameObject[] WeaponsVisual;

    private float _secondFinished = 0;

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
        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        ActivateWeapon(0);

        Debug.Log(_player.FreeXpPoints);
    }

    private void FixedUpdate()
    {
        _secondFinished += Time.deltaTime;

        float coef = (float)_player.MaxHealth / _player.Health;
        float precent = 100 / coef;
        _playerHealthImage.fillAmount = precent / 100;

        if (_secondFinished >= 0.1f)
        {

            if (_player.Stamina < _player.MaxStamina)
            {
                _player.Stamina += 1;
                _secondFinished = 0;
            }
            if (_player.Stamina < 0)
            {
                _player.Stamina = 0;
            }
        }
        if (_player.Stamina < Weapons[IdActiveWeapon].NeedStamina)
        {
            _pinaltyDamage = true;
        }
        else
        {
            _pinaltyDamage = false;
        }
    }

    private void Update()
    {
        if (Mouse.current.rightButton.isPressed) Block("Block");
        else Block("UnBlock");

        if (_playerMaxSpeed == 0) GetComponent<StarterAssetsInputs>().move = new Vector2(0, 0);

        float coef = (float)_player.MaxStamina / _player.Stamina;
        float precent = 100 / coef;
        _playerStaminaImage.fillAmount = precent / 100;
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
        _input.Player.OpenStats.performed += context => OpenCharacteristic();
    }

    private void OpenCharacteristic()
    {
        FindObjectOfType<UIManagerGame>().GetComponent<UIManagerGame>().UpdateInfo();
        _characteristicWindow.SetActive(!_characteristicWindow.activeSelf);
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
        if (_canAttack) NowEnemies.Clear();
        if (AttackStatus == 0 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 1;
            _player.Stamina -= Weapons[IdActiveWeapon].NeedStamina;
            Debug.Log(_player.Stamina);

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 1 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 2;
            _player.Stamina -= Weapons[IdActiveWeapon].NeedStamina;

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 2 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 3;
            _player.Stamina -= Weapons[IdActiveWeapon].NeedStamina;

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 3 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 4;
            _player.Stamina -= Weapons[IdActiveWeapon].NeedStamina;

            _animator.SetInteger("Attack", AttackStatus);
        }
        else if (AttackStatus == 4 && _canAttack)
        {
            _canAttack = false;
            AttackStatus = 0;
            _player.Stamina -= Weapons[IdActiveWeapon].NeedStamina;

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

    public void GetDamage(int damage)
    {
        _player.Health -= damage;
        _animator.SetBool("Damaged", true);
    }

    private void EndDamaged()
    {
        _animator.SetBool("Damaged", false);
        _canAttack = true;
        AttackStatus = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StartZone")
        {
            GameManager.isStartedBattle = true;
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
            if (!_pinaltyDamage) enemy.GetComponent<Ai>().Enemy.Health -= Weapons[IdActiveWeapon].Damage + _player.Strength;
            else enemy.GetComponent<Ai>().Enemy.Health -= 5;
            enemy.GetComponent<Ai>().Animator.GetDamage();
            Debug.Log(string.Format("{0} + {1}", Weapons[IdActiveWeapon].Damage, _player.Strength));
            if (enemy.GetComponent<Ai>().Enemy.Health <= 0) Destroy(enemy);
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
