/*
Данный класс- скрипт отвечает за работу оружия в игре

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using UnityEngine;

public class WeaponAction : MonoBehaviour
{
    private PlayerManager _player; // Менеджер персонажа игрока
    private Ai _enemy; // Противник
    [SerializeField] private bool isWeaponFromPlayer; // Пренадлежит ли оружие игроку

    /// <summary>
    /// Start вызывается, когда скрипт включен, вызывается непосредственно перед первым вызовом любого из методов обновления.
    /// </summary>
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerManager>(); // Найти и установить персонажа игрока
        if (!isWeaponFromPlayer) _enemy = GameObject.Find("Enemy").GetComponent<Ai>(); // Если оружие не принадлежит игроку найти и установить противника
    }

    /// <summary>
    /// Когда GameObject сталкивается с другим GameObject, Unity вызывает OnTriggerEnter.
    /// </summary>
    /// <param name="other">Объект, с которым взаимодействует текущий объект</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isWeaponFromPlayer) // Если задет враг и оружие принадлежит игроку
        {
            if (!(other.GetComponent<Ai>().StateMachine.CurrentState is StateBlock)) // Если враг не в блоку
                _player.NowEnemies.Add(other.gameObject); // Добавить врага в список задетых противников
        }
        else if (other.gameObject.tag == "Player" && !isWeaponFromPlayer && _player.BlockStatus == false) // Если задет игрок и оружие не принадлежит игроку и игроку не в блоке
        {
            _enemy.ShotPlayer = true; // Игрок задет
        }
    }
}
