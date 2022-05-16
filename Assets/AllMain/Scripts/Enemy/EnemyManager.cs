using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Image _enemyHealthImage;
    private Animator _animator;
    public GameManager GameManager;
    public Enemy Enemy;

    public int IdActiveWeapon = 0;
    public GameObject[] WeaponsVisual;
    public List<Weapon> Weapons = new List<Weapon>();

    // Start is called before the first frame update
    void Start()
    {
        Weapons = MyDataBase.GetWeapons();
        _animator = GetComponent<Animator>();
        ActivateWeapon(0);
        GameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        Enemy = new Enemy(0, 10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        float coef = (float)Enemy.MaxHealth / Enemy.Health;
        float precent = 100 / coef;
        _enemyHealthImage.fillAmount = precent / 100;
    }

    private void ActivateWeapon(int idWeapon)
    {
        _animator.SetBool("Equip", true);
        IdActiveWeapon = idWeapon;
    }

    public void ActivateWeaponWithAnimation()
    {
        foreach (GameObject weapon in WeaponsVisual)
        {
            weapon.SetActive(false);
        }
        WeaponsVisual[IdActiveWeapon].SetActive(true);
        float bonusAttackSpeed = (float)Enemy.Dexterity / 200;
        if (bonusAttackSpeed > 2.0f) bonusAttackSpeed = 2;
        _animator.SetFloat("AttackSpeed", Weapons[IdActiveWeapon].AttackSpeed + bonusAttackSpeed);
        _animator.SetBool("Equip", false);
    }
}
