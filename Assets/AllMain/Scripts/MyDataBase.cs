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

    /// <summary> ���� ����� ��������� ������ query � ���������� ����� �������. </summary>
    /// <param name="query"> ���������� ������. </param>
    /// <returns> ���������� �������� 1 ������ 1 �������, ���� ��� �������. </returns>
    public static string ExecuteQueryWithAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        var answer = command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }

    /// <summary> ���� ����� ���������� �������, ������� �������� ����������� ������� ������� query. </summary>
    /// <param name="query"> ���������� ������. </param>
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

    /// <summary> ���� ����� ��������� ����������� � ��. </summary>
    private static void OpenConnection()
    {
        connection = new SqliteConnection("Data Source=" + DBPath);
        command = new SqliteCommand(connection);
        connection.Open();
    }

    /// <summary> ���� ����� ��������� ����������� � ��. </summary>
    public static void CloseConnection()
    {
        connection.Close();
        command.Dispose();
    }

    /// <summary> ���� ����� ��������� ������ query. </summary>
    /// <param name="query"> ���������� ������. </param>
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        command.CommandText = query;
        command.ExecuteNonQuery();
        CloseConnection();
    }
}