using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Player> Players = new List<Player>();
    public GameObject PlayerPrefab;
    public GameObject PlayersPanel;

    // Start is called before the first frame update
    void Start()
    {
        Players = MyDataBase.GetPlayers();
        Cursor.visible = true;

        foreach (Player pl in Players)
        {
            string description = string.Format("Уровень: {0}, Сила: {1}, Ловкость: {2}, Здоровье: {3}, Выносливость: {4}", 
                pl.Id, pl.Strength, pl.Dexterity, pl.Health, pl.Stamina);
            GameObject Temp = Instantiate(PlayerPrefab, PlayersPanel.transform);
            Temp.name = "Player " + pl.Id.ToString();
            Temp.transform.Find("TNamePlayer").GetComponent<TextMeshProUGUI>().text = pl.Id.ToString();
            Temp.transform.Find("TDescriptionPlayer").GetComponent<TextMeshProUGUI>().text = description;

            Button PlayButton = Temp.transform.Find("PlayButton").GetComponent<Button>();
            PlayButton.onClick.AddListener(() => StartPlaying(pl.Id));
        }
    }

    private void StartPlaying(int Playerid)
    {
        DATAMovement.ActivePlayer = Playerid;
        SceneManager.LoadScene("Playground");
    }
}
