using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private List<Player> _players = new List<Player>();
    [SerializeField] private GameObject _playerInfoPrefab;
    [SerializeField] private GameObject _playersPanel;
    [SerializeField] private GameObject _newGamePanel;
    [SerializeField] private GameObject _mainmenuPanel;
    [SerializeField] private Image _scrollImage;
    [SerializeField] private TextMeshProUGUI _NewGameNameChar;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    public void ReturnToMenu()
    {
        ActivateMenu(_mainmenuPanel);
    }

    public void StartNewGame()
    {
        MyDataBase.CreatePlayer(_NewGameNameChar.text);
        StartPlaying(MyDataBase.GetLastPlayerId());
    }

    public void OpenNewGame()
    {
        ActivateMenu(_newGamePanel);
    }

    public void OpenLoadGame()
    {
        ActivateMenu(_playersPanel);

        _players.Clear();
        foreach (Transform ch in _playersPanel.transform.Find("Content").Find("PlayersPanel"))
        {
            Destroy(ch.gameObject);
        }

        _players = MyDataBase.GetPlayers();

        foreach (Player pl in _players)
        {
            string description = string.Format("Уровень: {0}, Сила: {1}, Ловкость: {2}\nЗдоровье: {3}, Выносливость: {4}",
                pl.Level, pl.Strength, pl.Dexterity, pl.Health, pl.Stamina);
            GameObject Temp = Instantiate(_playerInfoPrefab, _playersPanel.transform.Find("Content").Find("PlayersPanel").transform);
            Temp.name = "Player " + pl.Id.ToString();
            Temp.transform.Find("TNamePlayer").GetComponent<TextMeshProUGUI>().text = pl.Name;
            Temp.transform.Find("TDescriptionPlayer").GetComponent<TextMeshProUGUI>().text = description;

            Button PlayButton = Temp.transform.GetComponent<Button>();
            PlayButton.onClick.AddListener(() => StartPlaying(pl.Id));
        }

        _scrollImage.SetNativeSize();
    }

    private void ActivateMenu(GameObject panel)
    {
        _playersPanel.SetActive(false);
        _mainmenuPanel.SetActive(false);
        _newGamePanel.SetActive(false);

        panel.SetActive(true);
    }

    private void StartPlaying(int Playerid)
    {
        DATAMovement.ActivePlayer = Playerid;
        SceneManager.LoadScene("Playground");
    }
}
