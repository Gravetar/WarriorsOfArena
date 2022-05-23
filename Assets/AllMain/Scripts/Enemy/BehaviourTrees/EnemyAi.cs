using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private Node _topNode;
    private float lowhp;

    private Enemy enemy;
    public int EnemyStrength;
    public int EnemyDexterity;
    public TacticEnemy EnemyTactic;

    public int FavoriteIdWeaponPlayer;
    public ActionPlayer FavoriteActionPlayer;
    public int NumberFight;
    public int FightSeconds;

    private int _activeWeaponId;
    private void Start()
    {
        FavoriteIdWeaponPlayer = 2;
        FavoriteActionPlayer = ActionPlayer.Block;
        NumberFight = 3;
        FightSeconds = 90;
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree()
    {
        WarriorNode warriorNode = new WarriorNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); // Attack 1
        DamageTankNode dmgTankNode = new DamageTankNode(this, FavoriteActionPlayer, FightSeconds, FavoriteIdWeaponPlayer); //Block >60 1
        DefTankNode defTankNode = new DefTankNode(this, FavoriteActionPlayer, FightSeconds, FavoriteIdWeaponPlayer); //Block <60 1
        RusherNode rusherNode = new RusherNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); //Block 3

        Selector tankSelector = new Selector(new List<Node> { dmgTankNode, defTankNode });
        Selector strengthSelector = new Selector(new List<Node> {warriorNode, tankSelector });

        _topNode = new Selector(new List<Node> { strengthSelector });
        _topNode.Evaluate();
    }

    private void Update()
    {
    }

    public void SetWeapon(int id)
    {
        _activeWeaponId = id;
    }

    public void SetStrength(int coef)
    {
        EnemyStrength = coef * NumberFight;
    }

    public void SetDexterity(int coef)
    {
        EnemyDexterity = coef * NumberFight;
    }

    public void SetTactic(TacticEnemy tactic)
    {
        EnemyTactic = tactic;
    }

    public void CreateEnemy()
    {
        enemy = new Enemy(NumberFight, EnemyStrength, EnemyDexterity);
    }

    public enum ActionPlayer
    {
        Attack,
        Block
    }

    public enum TacticEnemy
    {
        Aggresive,
        Passive
    }
}
