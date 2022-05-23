using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    private Node _topNode;

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
        WarriorNode warriorNode = new WarriorNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); // Attack 1 - 3 2 2 P
        DamageTankNode dmgTankNode = new DamageTankNode(this, FavoriteActionPlayer, FightSeconds, FavoriteIdWeaponPlayer); //Block >60 1 - 3 3 1 A
        DefTankNode defTankNode = new DefTankNode(this, FavoriteActionPlayer, FightSeconds, FavoriteIdWeaponPlayer); //Block <60 1 - 3 3 1 P
        RusherNode rusherNode = new RusherNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); //Block 3 - 1 1 3 A
        CarefulDexAssasinNode carefulDexAssasinNode = new CarefulDexAssasinNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); //Block 2 - 1 1 3 P
        DefCarefulNode defCarefulNode = new DefCarefulNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); //Attack 3 - 2 2 2 P
        RunAwayCarefulNode runAwayCarefulNode = new RunAwayCarefulNode(this, FavoriteActionPlayer, FavoriteIdWeaponPlayer); //Attack 2 - 1 1 3 P

        Selector tankSelector = new Selector(new List<Node> { dmgTankNode, defTankNode });
        Selector strengthSelector = new Selector(new List<Node> {warriorNode, tankSelector });

        Selector carefulSelector = new Selector(new List<Node> { defCarefulNode, runAwayCarefulNode });
        Selector assasinSelector = new Selector(new List<Node> { rusherNode, carefulDexAssasinNode });
        Selector dextiritySelector = new Selector(new List<Node> { assasinSelector, carefulSelector });

        _topNode = new Selector(new List<Node> { strengthSelector, dextiritySelector });
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

    public Enemy CreateEnemy()
    {
        return new Enemy(EnemyTactic, NumberFight, EnemyStrength, EnemyDexterity, _activeWeaponId);
    }

    public enum TacticEnemy
    {
        Aggresive,
        Passive
    }
}
