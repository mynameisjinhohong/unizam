using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<EnemeyData> enemies;
    public int maxHp;
    public int maxMp;
    public PlayerCharacter player;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        player.hp = maxHp;
    }

    private void Update()
    {
        if(player.hp > maxHp)
        {
            player.hp = maxHp;
        }
        if(player.mp > maxMp)
        {
            player.mp = maxMp;
        }
    }

}
