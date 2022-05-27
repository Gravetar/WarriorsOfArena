/*
�������� ������, ������ �����-������ �������� �� ���, ��� ������� � ���������� ������

��������� ������ ���������
(�) ��������� ������
������: 2022 �������: 26.05.2022
���������� ����������: Kaylan00@mail.ru
*/

using StarterAssets;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Player Player => _player; // ��������� ������ ��� ������(������)

    public float BlockTime; // ����� ����� ������ � ���
    public int AttackCount; // ���������� ����, ����������� �������
    public bool BlockStatus = false; // ����, ����������� � ����� �� ������ �����
    public bool EnemyIsDead = false; // ����, �����������, ��� ��������� ��������.
    public List<GameObject> NowEnemies = new List<GameObject>(); // �����, ������� ������
    public int IdActiveWeapon = 0; // Id �������� ������ ������
    
    [SerializeField] private GameObject[] WeaponsVisual; // ������� ������
    [SerializeField] private Transform _instructor; // ����������
    [SerializeField] private Image _playerHealthImage; // �������� �������� ������
    [SerializeField] private Image _playerStaminaImage; // �������� ������������ ������
    [SerializeField] private GameObject _characteristicWindow; // ������ - ���������� ������
    [SerializeField] private GameObject _door; // ������ - �����

    private List<Weapon> _weapons = new List<Weapon>(); // ������ ������
    private GameManager _gameManager; // �������� ����
    private int _attackStatus = 0; // ���������� ����� �����
    private Player _player; // �������� ������
    private bool _pinaltyDamage = false; // ����� � �����
    private float _playerMaxSpeed = 6; // ������������ ��������� �������� ���������

    private MyPlayerInput _input; // ������� �����
    private StarterAssetsInputs _inputMovement; // ������� ����� (���������), ��� ����������� �������� ���������

    private Animator _animator; // �������� ���������
    private bool _canAttack = true; // ����, ����� �� �������� �������� ��������� �����

    private float _secondFinished = 0; // ������� ������� ��� �������������� ������������ ���������

    /// <summary>
    /// ������� Awake ���������� ����� ��������� ������� ����� ��������
    /// </summary>
    private void Awake()
    {
        PerformedInputs(); // ��������� ������� ����������
    }

    /// <summary>
    /// ������� OnEnable ����������, ����� ������ ���������� ���������� � ��������.
    /// </summary>
    private void OnEnable()
    {
        _input.Enable();
    }

    /// <summary>
    /// ��� ������� ����������, ����� ��������� ���������� �����������.
    /// ��� ����� ���������� ��� ����������� �������
    /// </summary>
    private void OnDisable()
    {
        _input.Disable(); // ��������� ������� ����������
    }

    /// <summary>
    /// Start ����������, ����� ������ �������, ���������� ��������������� ����� ������ ������� ������ �� ������� ����������.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // ��������� ������ "� ���� ����"

        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _player = MyDataBase.GetPlayerById(DATAMovement.ActivePlayer); // ��������� ��������� ������
        _inputMovement = GetComponent<StarterAssetsInputs>(); // ���������� ������� ����� (���������)

        _animator = GetComponent<Animator>(); // ��������� ��������� ���������
        _weapons = MyDataBase.GetWeapons(); // ��������� ������ ������ �� ���� ������
        
        ActivateWeapon(0); // ������������ ���, � �������� ������ ���������
    }

    /// <summary>
    /// ����������, �� ��������� �� ������� ������
    /// </summary>
    private void FixedUpdate()
    {
        // ��������� ����� �������� ������
        float coef = (float)_player.MaxHealth / _player.Health;
        float precent = 100 / coef;
        _playerHealthImage.fillAmount = precent / 100;
        // ��������� ����� ������������ ������
        coef = (float)_player.MaxStamina / _player.Stamina;
        precent = 100 / coef;
        _playerStaminaImage.fillAmount = precent / 100;

        _secondFinished += Time.deltaTime; // ������� �������

        if (_secondFinished >= 0.1f) // ���� ������ 0.1 ������� �� ������ �������� �������
        {
            if (_player.Stamina < _player.MaxStamina) // ���� ������������ ��������� ������ ��� ����������� ������������
            {
                _player.Stamina += 1; // ������������ ������� ������������
                _secondFinished = 0; // �������� ������� �������
            }
            if (_player.Stamina < 0) // ���� ������������ ��������� ������ 0
            {
                _player.Stamina = 0; // �������� ������������
            }
        }
        if (_player.Stamina < _weapons[IdActiveWeapon].NeedStamina) // ���� ������������ ��������� ������ ������������, ������� ��������� �� ����
        {
            _pinaltyDamage = true; // ���������� ���� ������ � ����� � true
        }
        else // �����
        {
            _pinaltyDamage = false; // ���������� ���� ������ � ����� � false
        }
        if (BlockStatus) BlockTime += Time.deltaTime; // ���� ����� � �����, ���������� ����� � ����� ���������
    }

    /// <summary>
    /// ���������� ���������� ������ ����, ���� MonoBehaviour �������.
    /// </summary>
    private void Update()
    {
        // ������ �� ����� ����
        if (Mouse.current.rightButton.isPressed) Block("Block");
        else Block("UnBlock");

        // �������� �������� ���������
        if (_playerMaxSpeed == 0) GetComponent<StarterAssetsInputs>().move = new Vector2(0, 0);

        // ���� ����� ���������� ������ � �����������
        if (Vector3.Distance(transform.position, _instructor.position) <= 3) _gameManager.GameUI.ActivateTxtDialog(true); // ������������ �����, ������� �������, ��� ����� ������ ������
        else _gameManager.GameUI.ActivateTxtDialog(false); // ����� �������������� ���
    }

    /// <summary>
    /// ��������� ������� �����
    /// </summary>
    private void PerformedInputs()
    {
        _input = new MyPlayerInput(); // ������� ����� ������� �����

        // ��������� ������� � ��������� � ������� �����
        _input.Player.Attack.performed += context => Attack();
        _input.Player.GetSword.performed += context => ActivateWeapon(0);
        _input.Player.GetAxe.performed += context => ActivateWeapon(1);
        _input.Player.GetMace.performed += context => ActivateWeapon(2);
        _input.Player.ExitToMenu.performed += context => ExitToMenu();
        _input.Player.Kick.performed += context => StartKick();
        _input.Player.OpenStats.performed += context => OpenCharacteristic();
        _input.Player.StartDialog.performed += context => StartDialog();
    }

    /// <summary>
    /// ������ ������ � �����������
    /// </summary>
    private void StartDialog()
    {
        // ���� ����� ���������� ������ � �����������
        if (Vector3.Distance(transform.position, _instructor.position) <= 3)
        {
            _gameManager.GameUI.StartDialog(); // ������� ������
            ActivatedInputs(false); // �������������� ���� ������
        }
    }

    /// <summary>
    /// ����� ��������� ���������� ������� ����� ������
    /// </summary>
    /// <param name="active">���� ����������</param>
    public void ActivatedInputs(bool active)
    {
        if (!active) // ���� ������� ����� �� �������
        {
            _input.Player.Disable(); // ��������� ������� �����
            GetComponent<StarterAssetsInputs>().move = new Vector2(0, 0); // �������� �������� ���������
        }
        else
        {
            _input.Player.Enable(); // �������� ������� �����
        }
        _inputMovement.IsCanMovement = active; // ���������� ���� ����� �� ����� ���������� ���������
    }

    /// <summary>
    /// ������� ���������� ���������
    /// </summary>
    private void OpenCharacteristic()
    {
        _gameManager.GameUI.OpenStatistic(); // ������� ���������� ���������
    }

    /// <summary>
    /// ���������� �����
    /// </summary>
    private void StartKick()
    {
        _animator.SetBool("Kick", true); // ��������� �������� �����
    }

    /// <summary>
    /// �����, ���������� � ������� ��������
    /// </summary>
    private void Kick()
    {
        _animator.SetBool("Kick", false); // ���������� �������� �����
    }

    /// <summary>
    /// �����, ������� ����������� ��� ������ � ����
    /// </summary>
    private void ExitToMenu()
    {
        MyDataBase.UpdatePlayer(Player); // ��������� ������ ���������
        SceneManager.LoadScene("Menu"); // ��������� ����� ����
    }

    /// <summary>
    /// ����� ���������
    /// </summary>
    private void Attack()
    {
        if (_canAttack) // ���� �������� ����� ������� ����
        {
            AttackCount += 1; // ��������� � ������� ���� ������
            NowEnemies.Clear(); // �������� ������ ������� �����
            _canAttack = false; // �������� �� ����� ���������
            _player.Stamina -= _weapons[IdActiveWeapon].NeedStamina; // ������ ������������ ���������, �������� ����������� ������
            if (_attackStatus == 0) // ���� ����� ���� = 0
            {
                _attackStatus = 1; // ���������� ����� ���� �� 1...
            }
            else if (_attackStatus == 1)
            {
                _attackStatus = 2;
            }
            else if (_attackStatus == 2)
            {
                _attackStatus = 3;
            }
            else if (_attackStatus == 3)
            {
                _attackStatus = 4;
            }
            else if (_attackStatus == 4)
            {
                _attackStatus = 0;
            }
            _animator.SetInteger("Attack", _attackStatus); // ���������� �������� �����
        }
    }

    /// <summary>
    /// ������� ������
    /// </summary>
    /// <param name="idWeapon">ID ������</param>
    private void ActivateWeapon(int idWeapon)
    {
        _animator.SetBool("Equip", true); // ���������� �������� ���������� ������
        IdActiveWeapon = idWeapon; // ���������� ���� �������� ������ �� �������
        _playerMaxSpeed = 0; // ��������� ��������� ���������
    }

    /// <summary>
    /// ������������ ������, ���������� �� ��������
    /// </summary>
    public void ActivateWeaponWithAnimation()
    {
        foreach (GameObject weapon in WeaponsVisual) // ������ �� ���� �������
        {
            weapon.SetActive(false); // �������������� ������
        }
        WeaponsVisual[IdActiveWeapon].SetActive(true); // ������������ ������ ������
        float bonusAttackSpeed = (float)_player.Dexterity / 200; // ��������� ����� � �������� ����� ������
        if (bonusAttackSpeed > 2.0f) bonusAttackSpeed = 2; // ��������� �������� �� �������� 2
        _animator.SetFloat("AttackSpeed", _weapons[IdActiveWeapon].AttackSpeed + bonusAttackSpeed); // ���������� �������� �����
        _animator.SetBool("Equip", false); // ���������� �������� ���������� ������
        _playerMaxSpeed = 1; // ������� ����������� ���������
    }

    /// <summary>
    /// ���� ������
    /// </summary>
    /// <param name="type">Block/UnBlock</param>
    private void Block(string type)
    {
        if (type == "Block") // ���� ���������
        {
            _animator.SetBool("Block", true); // ���������� �������� �����
            BlockStatus = true; // ������ ������������ = true
            if (_animator.GetFloat("Speed") > 2) // ���������� �������� �������� ������ �������� 2
            {
                _animator.SetFloat("Speed", 2);
                GetComponent<StarterAssetsInputs>().sprint = false;
            }
        }
        else
        {
            //Debug.Log("UnBlock");
            BlockStatus = false;
            _animator.SetBool("Block", false);
        }
    }

    /// <summary>
    /// ��������� ����������� ����������
    /// </summary>
    /// <param name="damage">���� �� ���������</param>
    public void GetDamage(int damage)
    {
        _player.Health -= damage; // ������ �������� � ��������� ������
        _animator.SetBool("Damaged", true); // ���������� �������� ��������� ����� ����������
        if (Player.Health <= 0) // ���� �������� ��������� �������� ����
        {
            _gameManager.EndBattle(false); // ��������� ���
            _animator.SetTrigger("Death"); // ���������� �������� ������ ���������
            ActivatedInputs(false); // �������������� ������� �����
        }
    }

    /// <summary>
    /// ����� ��������� �����, ���������� � ������� ��������
    /// </summary>
    private void EndDamaged()
    {
        _animator.SetBool("Damaged", false); // ��������� �������� ��������� �����
        _canAttack = true; // ��������� ����� ���������
        _attackStatus = 0; // �������� ����� ���� ���������
    }

    /// <summary>
    /// ����� GameObject ������������ � ������ GameObject, Unity �������� OnTriggerEnter.
    /// </summary>
    /// <param name="other">������, � ������� ��������������� ������� ������</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "StartZone") // ���� �������� ����� � ����� ���� ���
        {
            _gameManager.StartBattle(); // ������ ���
        }
        else if (other.tag == "EndZone" && _gameManager.isStartedBattle) // ���� �������� ����� � �������� ����  ��� � ��� ��� �����
        {
            _gameManager.EndBattle(true); // ��������� ���
            _gameManager.CloseDoor(); // ������� ���� �����
        }
    }

    /// <summary>
    /// ���� ���������, ���������� �������� ��������
    /// </summary>
    private void Hit()
    {
        _canAttack = true; // �������� ����� ���������

        _animator.SetInteger("Attack", _attackStatus); // ���������� ������ ����� ����

        NowEnemies = NowEnemies.Distinct().ToList(); // �������� ���� ������� ������ �� ����������, ����� ����� �� ���� ���� �� ��������� ��������� ���
        foreach (GameObject enemy in NowEnemies) // �������� �� ���� ������ � ������ ������� ������
        {
            if (!_pinaltyDamage) enemy.GetComponent<Ai>().Enemy.Health -= _weapons[IdActiveWeapon].Damage + _player.Strength; // ���� ��� ������ � �����, �� ������� ���������� ������ ���� ������ + ���� ���������
            else enemy.GetComponent<Ai>().Enemy.Health -= 5; // ����� ������� �������� ���� � �������� 5 ������
            enemy.GetComponent<Ai>().Animator.GetDamage(); // ��������� ������� ����
            if (enemy.GetComponent<Ai>().Enemy.Health <= 0) // ���� �������� ����� �������� ����
            {
                _gameManager.OpenDoor(); // ������� ����� �����
                _gameManager.HideHealthEnemy(false); // ������ �������� �����
                EnemyIsDead = true; // ���������� ����, ��� ���� ��������
            }
        }
        NowEnemies.Clear(); // �������� ������ ������� ������
    }

    /// <summary>
    /// ��������� �����, ���������� �� ��������
    /// </summary>
    private void EndAttack()
    {
        _canAttack = true; // �������� ����� ���������
        _attackStatus = 0; // �������� ����� ���� ���������

        _animator.SetInteger("Attack", _attackStatus); // ���������� ����� ���� ���������
    }

    /// <summary>
    /// ���� ����� ����������� ����� ������ ���������, ���������� �� ������� ��������
    /// </summary>
    private void AfterDeath()
    {
        MyDataBase.UpdatePlayer(Player); // �������� ������ ���������(��������� �� � ���� ������)
        SceneManager.LoadScene("Playground"); // ������������� �����
    }
}
