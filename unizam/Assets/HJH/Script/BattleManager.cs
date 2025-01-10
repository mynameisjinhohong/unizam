using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    StartDice,
    EndDice,
    ChooseEnemy,
    UseSkill,
    EnemyTurn,
    EnemyTurnEnd,
}

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    BattleState state;

    public BattleState State
    {
        get { return state; }
        set
        {
            switch (value)
            {
                case BattleState.StartDice:
                    dice.gameObject.SetActive(true);
                    break;
                case BattleState.EndDice:
                    StopDice();
                    break;
                case BattleState.ChooseEnemy:
                    ChooseEnemy();
                    break;
                case BattleState.UseSkill:
                    UseSkill();
                    break;
                case BattleState.EnemyTurn:
                    break;
                case BattleState.EnemyTurnEnd:
                    break;

            }

        }
    }

    public BattlePlayer player;
    public Transform[] spawnPos1;
    public Transform[] spawnPos2;
    public Transform[] spawnPos3;

    public Image dice;
    public Sprite[] diceSprite;

    public int diceSu;

    bool choose;
    List<EnemyCharacter> characters;
    int count = 0;

    private void Start()
    {
        state = BattleState.StartDice;
        player.player = GameManager.Instance.player;
        characters = new List<EnemyCharacter>();
        switch (GameManager.Instance.enemies.Count)
        {
            case 1:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos1[i]);
                    enemy.GetComponent<BattleEnemy>().enemy = GameManager.Instance.enemies[i].enemy;
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                }
                break;
            case 2:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos2[i]);
                    enemy.GetComponent<BattleEnemy>().enemy = GameManager.Instance.enemies[i].enemy;
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                }
                break;
            case 3:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos3[i]);
                    enemy.GetComponent<BattleEnemy>().enemy = GameManager.Instance.enemies[i].enemy;
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                }
                break;
        }
    }

    private void Update()
    {
        if (choose)
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15f);
                Debug.DrawRay(mousePos,transform.forward * 10,Color.yellow);
                if(hit.transform.tag == "Enemy")
                {

                }
            }
        }
    }

    public void StartDice()
    {
        //다이스 굴러가는 연출
        state = BattleState.EndDice;
    }
    public void StopDice()
    {
        int ran = Random.Range(0, GameManager.Instance.player.behaviours.Count);
        diceSu = ran;
        dice.sprite = diceSprite[ran];
        state = BattleState.ChooseEnemy;
    }

    public void ChooseEnemy()
    {
        choose = true;
        count = 0;
    }

    public void UseSkill()
    {
        //스킬 사용 연출
        
    }
}
