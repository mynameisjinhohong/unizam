using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data")]
public class EnemeyData : ScriptableObject
{
    [SerializeField]
    public EnemyCharacter enemy;
}

[Serializable]
public class Character
{
    public int hp;
    public List<Buff> buffs;
    public List<Buff> nextBuffs;
    public List<Behaviour> behaviours;
}

[System.Serializable]
public class PlayerCharacter : Character
{
    public int mp;

}

[System.Serializable]
public class EnemyCharacter : Character
{
    public GameObject enemyPrefab;

}

public class Buff : ScriptableObject
{
    public GameObject buffIcon;
    public virtual int BuffEffect(bool myEffect,int su)
    {
        return su;
    }
}


public class Behaviour : ScriptableObject
{
    public BehaviourState state;
    public Character character;
    public bool all;
    public string behaviourName;
    public virtual void Do(Character[] target)
    {

    }
}

[System.Serializable]
public enum BehaviourState
{
    special,
    nomal
}


