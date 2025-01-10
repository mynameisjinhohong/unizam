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
    public Sprite sprite;
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
    public List<Behaviour> behaviours;
}


public class Behaviour : ScriptableObject
{
    public virtual void Do(Character[] target)
    {

    }
}



