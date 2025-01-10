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
}

[System.Serializable]
public class PlayerCharacter : Character
{
    public int mp;
    public List<Behaviour> behaviours;
}

[System.Serializable]
public class EnemyCharacter : Character
{
    public GameObject enemyPrefab;
    public List<Behaviour> behaviours;
}


public class Behaviour : ScriptableObject
{
    public BehaviourState state;
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


