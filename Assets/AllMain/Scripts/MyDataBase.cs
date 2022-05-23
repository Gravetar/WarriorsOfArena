using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

static class MyDataBase
{
    private const string fileName = "db.bytes";
    private static string DBPath;
    private static SqliteConnection connection;
    private static SqliteCommand command;

    static MyDataBase()
    {
        DBPath = Application.dataPath + "/StreamingAssets/" + fileName;
    }
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
    public static void CreatePlayer(string inputname)
    {
        ExecuteQueryWithoutAnswer(string.Format("INSERT INTO Players (name, experience, strength, dexterity) VALUES (\"{0}\", 0, 10, 10);", inputname));
    }

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

    /// <summary> Ётот метод возвращает таблицу, котора€ €вл€етс€ результатом выборки запроса query. </summary>
    /// <param name="query"> —обственно запрос. </param>
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

    /// <summary> Ётот метод открывает подключение к Ѕƒ. </summary>
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    /// <summary> Ётот метод закрывает подключение к Ѕƒ. </summary>
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    /// <summary> Ётот метод выполн€ет запрос query. </summary>
    /// <param name="query"> —обственно запрос. </param>
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }
}