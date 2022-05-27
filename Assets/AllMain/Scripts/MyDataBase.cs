/*
Класс для работы с базой данный проекта

Панасенко Сергей Сергеевич
(с) Панасенко Сергей
Создан: 2022 Изменен: 26.05.2022
Контактная информация: Kaylan00@mail.ru
*/

using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;

static class MyDataBase
{
    private const string fileName = "db.bytes"; // Имя файла базы данных
    private static string DBPath; // Путь до базы данных
    private static SqliteConnection connection; // Соеденение с базой данных
    private static SqliteCommand command; // Команда (текст) запроса к базе данных

    /// <summary>
    /// Конструктор
    /// </summary>
    static MyDataBase()
    {
        DBPath = Application.dataPath + "/StreamingAssets/" + fileName; // Установка полного пути до файла, включая сам файл базы данных
    }

    /// <summary>
    /// Получить всех персонажей пользователя
    /// </summary>
    /// <returns>Список персонажей</returns>
    public static List<Player> GetPlayers()
    {
        OpenConnection();

        List<Player> TempListWeapons = new List<Player>();
        command.CommandText = string.Format("SELECT * FROM Players");
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    TempListWeapons.Add(new Player(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5)));
                }
            }
        }

        CloseConnection();
        return TempListWeapons;
    }

    /// <summary>
    /// Получить все оружия
    /// </summary>
    /// <returns>Список оружий</returns>
    public static List<Weapon> GetWeapons()
    {
        List<Weapon> TempListWeapons = new List<Weapon>();

        DataTable TempTable = GetTable("SELECT * FROM Weapons");


        for (int i =0; i<TempTable.Rows.Count; i++)
        {
            TempListWeapons.Add(new Weapon(
                int.Parse(TempTable.Rows[i][2].ToString()),
                int.Parse(TempTable.Rows[i][3].ToString()),
                int.Parse(TempTable.Rows[i][5].ToString()),
                float.Parse(TempTable.Rows[i][4].ToString()),
                TempTable.Rows[i][1].ToString()
                ));
        }

        return TempListWeapons;
    }

    /// <summary>
    /// Создать персонажа
    /// </summary>
    /// <param name="inputname">Имя персонажа</param>
    public static void CreatePlayer(string inputname)
    {
        ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Players (name, experience, strength, dexterity) VALUES (\"{0}\", 0, 10, 10);", inputname));
    }

    /// <summary>
    /// Обновить данные персонажа
    /// </summary>
    /// <param name="player">Входные данные</param>
    public static void UpdatePlayer(Player player)
    {
        ExecuteQueryWithoutAnswer(string.Format("UPDATE Players SET experience = {0}, strength = {1}, dexterity = {2}, free_XP_Points = {3} WHERE id_player = {4};",
            player.Experience, player.Strength, player.Dexterity, player.FreeXpPoints, player.Id));
    }

    /// <summary>
    /// Создать бой
    /// </summary>
    /// <param name="id_player">Id персонажа</param>
    /// <param name="player_winner">Флаг: Игрок победил</param>
    /// <param name="player_favorite_weapon">Предпочитаемое оружие персонажа</param>
    /// <param name="player_favorite_action">Предпочитаемое действие персонажа</param>
    /// <param name="fight_number">Номер боя</param>
    /// <param name="fight_timer">Время боя</param>
    public static void CreateFight(int id_player, bool player_winner, int player_favorite_weapon, ActionPlayer player_favorite_action, int fight_number, float fight_timer)
    {
        string _fight_timer = fight_timer.ToString().Replace(',', '.');
        string _player_favorite_action = "\"" + player_favorite_action.ToString() + "\"";
        string query = string.Format("INSERT INTO Fights " +
            "(id_player, player_winner, player_favorite_weapon, player_favorite_action, fight_number, fight_timer) " +
            "VALUES ({0}, {1}, {2}, {3}, {4}, {5});",
            id_player.ToString(), player_winner.ToString(), player_favorite_weapon.ToString(), _player_favorite_action, fight_number, _fight_timer);
        ExecuteQueryWithoutAnswer(query);
    }
   
    /// <summary>
    /// Получить id последнего персонажа
    /// </summary>
    /// <returns></returns>
    public static int GetLastPlayerId()
    {
        OpenConnection();

        int Answer=0;
        command.CommandText = "SELECT * FROM Players ORDER BY id_player DESC LIMIT 1";
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    Answer = reader.GetInt32(0);
                    break;
                }
            }
        }

        CloseConnection();
        return Answer;
    }

    /// <summary>
    /// Получить последний номер боя персонажа
    /// </summary>
    /// <param name="player_id">Id персонажа</param>
    /// <returns></returns>
    public static int GetLastFightNumberByPlayerId(int player_id)
    {
        OpenConnection();

        int Answer = 0;
        command.CommandText = string.Format("SELECT * FROM Fights WHERE id_player = {0} ORDER BY id_fight DESC LIMIT 1", player_id);
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    Answer = reader.GetInt32(5);
                    break;
                }
            }
        }

        CloseConnection();
        return Answer;
    }

    /// <summary>
    /// Получить персонажа
    /// </summary>
    /// <param name="id">Id персонажа</param>
    /// <returns>Персонаж</returns>
    public static Player GetPlayerById(int id)
    {
        OpenConnection();

        Player Answer = null;
        command.CommandText = string.Format("SELECT * FROM Players WHERE id_player = {0};", id);
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    Answer = new Player(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));

                    break;
                }
            }
        }

        CloseConnection();
        return Answer;
    }

    /// <summary>
    /// Получить последний бой персонажа
    /// </summary>
    /// <param name="playerid">Id персонажа</param>
    /// <returns></returns>
    public static Fight GetLastFightByPlayerId(int playerid)
    {
        bool playerWinner;
        int favoriteWeaponPlayer;
        ActionPlayer favoriteActionPlayer;
        int fightNumber;
        float fightTime;

        OpenConnection();

        Fight Answer = null;
        command.CommandText = string.Format("SELECT * FROM Fights WHERE id_player = {0} ORDER BY id_fight DESC LIMIT 1;", playerid);
        using (SqliteDataReader reader = command.ExecuteReader())
        {
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read())   // построчно считываем данные
                {
                    playerWinner = reader.GetBoolean(2);
                    favoriteWeaponPlayer = reader.GetInt32(3);
                    if (reader.GetString(4) == "Attack") favoriteActionPlayer = ActionPlayer.Attack;
                    else favoriteActionPlayer = ActionPlayer.Block;
                    fightNumber = reader.GetInt32(5);
                    fightTime = reader.GetFloat(6);

                    Answer = new Fight(playerWinner, favoriteWeaponPlayer, favoriteActionPlayer, fightNumber, fightTime);

                    break;
                }
            }
        }

        CloseConnection();
        return Answer;
    }

    /// <summary> Этот метод возвращает таблицу, которая является результатом выборки запроса query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    public static DataTable GetTable(string query)
    {
        OpenConnection();

        SqliteDataAdapter adapter = new SqliteDataAdapter(query, connection);

        DataSet DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }

    /// <summary> Этот метод открывает подключение к БД. </summary>
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    /// <summary> Этот метод закрывает подключение к БД. </summary>
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    /// <summary> Этот метод выполняет запрос query. </summary>
    /// <param name="query"> Собственно запрос. </param>
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }
}