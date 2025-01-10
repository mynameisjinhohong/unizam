using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<EnemeyData> enemies;
    public PlayerCharacter player;
    public int maxHp;
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
        player = new PlayerCharacter();
        player.hp = maxHp;
        player.mp = 0;
    }

}
