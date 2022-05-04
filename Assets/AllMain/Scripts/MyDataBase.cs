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

    public static List<Weapon> GetWeapons()
    {
        List<Weapon> TempListWeapons = new List<Weapon>();

        OpenConnection();

        DataTable TempTable = GetTable("SELECT * FROM Weapons");


        for (int i =0; i<TempTable.Rows.Count; i++)
        {
            TempListWeapons.Add(new Weapon(
                int.Parse(TempTable.Rows[i][2].ToString()),
                int.Parse(TempTable.Rows[i][3].ToString()),
                float.Parse(TempTable.Rows[i][4].ToString()),
                TempTable.Rows[i][1].ToString()
                ));
        }

        CloseConnection();

        return TempListWeapons;
    }
    public static void CreatePlayer()
    {
        OpenConnection();

        ExecuteQueryWithoutAnswer("INSERT INTO Player (experience, strength, dexterity) VALUES (0, 10, 10);");

        CloseConnection();
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
                    Answer = new Player(reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3));

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