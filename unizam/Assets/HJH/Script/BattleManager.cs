using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState
{
    StartDice,
    ChooseEnemy,
    UseSkill,
    EnemyTurn,
    EnemyTurnEnd,
    BattleWin,
    BattleLose,
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
            state = value;
            Debug.Log(state.ToString());
            switch (state)
            {
                case BattleState.StartDice:
                    StartDice();
                    break;
                case BattleState.ChooseEnemy:
                    ChooseEnemy();
                    break;
                case BattleState.UseSkill:
                    UseSkill();
                    break;
                case BattleState.EnemyTurn:
                    enemyIdx= 0;
                    EnemyTurn();
                    break;
                case BattleState.EnemyTurnEnd:
                    State = BattleState.StartDice;
                    break;
                case BattleState.BattleWin:
                    //나중에 연출 생기면 변경하기로
                    SceneManager.LoadScene("MainScene");
                    break;
                case BattleState.BattleLose:
                    break;
            }

        }
    }
    public SkillButton skillButton;

    public Transform[] spawnPos1;
    public Transform[] spawnPos2;
    public Transform[] spawnPos3;

    public Slider[] sliders;

    public DiceAni dice;
    public Sprite[] diceSprite;

    public int diceSu;//주사위 몇나왔는가

    bool skill = false; //지금 쓰는게 스킬인지

    public bool choose; //선택 턴인지
    List<EnemyCharacter> characters;//전체 적
    EnemyCharacter target;//플레이어가 선택한 적
    Behaviour behaviour;//플레이어의 이번턴 행동

    int enemyIdx = 0;


    private void Start()
    {
        State = BattleState.StartDice;
        characters = new List<EnemyCharacter>();
        switch (GameManager.Instance.enemies.Count)
        {
            case 1:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos1[i]);
                    enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[i].enemy.hp;
                    enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[i].enemy.behaviours;
                    enemy.GetComponent<BattleEnemy>().hpBar = sliders[i];
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                }
                break;
            case 2:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos2[i]);
                    enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[i].enemy.hp;
                    enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[i].enemy.behaviours;
                    enemy.GetComponent<BattleEnemy>().hpBar = sliders[i];
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                }
                break;
            case 3:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos3[i]);
                    enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[i].enemy.hp;
                    enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[i].enemy.behaviours;
                    enemy.GetComponent<BattleEnemy>().hpBar = sliders[i];
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                }
                break;
        }
    }

    private void Update()
    {
        if (choose)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, transform.forward, 15f);
            Debug.DrawRay(mousePos, transform.forward * 10, Color.yellow);
            //각 적들이 레이에 닿았을 때 하이라이팅 해야됨.
            if(hit.transform != null)
            {
                if (hit.transform.tag == "Enemy")
                {
                    if (behaviour.all)
                    {

                    }
                    else
                    {

                    }
                }
                if (Input.GetMouseButtonDown(0))
                {
                    if (hit.transform.tag == "Enemy")
                    {
                        target = hit.transform.GetComponent<BattleEnemy>().enemy;
                        State = BattleState.UseSkill;
                    }
                }
            }

        }
    }

    public void StartDice()
    {
        //다이스 굴러가는 연출
        dice.gameObject.SetActive(false);
        dice.gameObject.SetActive(true);
    }

    public void StopDice()
    {
        int ran = Random.Range(0, GameManager.Instance.player.behaviours.Count);
        behaviour = GameManager.Instance.player.behaviours[ran];
        diceSu = ran;
        dice.Stop(diceSu);
        State = BattleState.ChooseEnemy;
    }

    public void ChooseSkill(int idx)
    {
        behaviour = GameManager.Instance.player.behaviours[idx];
        diceSu = idx;
        dice.Stop(diceSu);
        skill = true;
        //주사위 깜박이는 연출 할 것.
        State = BattleState.ChooseEnemy;
    }

    public void ChooseEnemy()
    {
        choose = true;
    }

    public void UseSkill()
    {
        //스킬 사용 연출

        EndSkillEffect();
    }

    public void EndSkillEffect()
    {
        skillButton.OffSkillButton();
        if (skill)//스킬 사용하면 그만큼 마나 까기
        {
            skill = false;
            GameManager.Instance.player.mp -= diceSu + 1;
        }
        else
        {
            GameManager.Instance.player.mp += 1;
        }
        dice.gameObject.SetActive(false);
        if (behaviour.all)
        {
            behaviour.Do(characters.ToArray());
        }
        else
        {
            Character[] ch = new Character[1];
            ch[0] = target;
            behaviour.Do(ch);
        }
        bool end = false;

        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].hp > 0)
            {
                break;
            }
            if (i == characters.Count - 1)
            {
                end = true;
            }
        }

        if (end)
        {
            State = BattleState.BattleWin;
        }
        else
        {
            State = BattleState.EnemyTurn;
        }
    }

    public void EnemyTurn()
    {
        StartCoroutine(EnemyMove(enemyIdx));
    }

    IEnumerator EnemyMove(int idx)
    {
        //적 애니메이션 연출
        yield return null;
        int ran = Random.Range(0, characters[enemyIdx].behaviours.Count);
        Character[] ch = new Character[1];
        ch[0] = GameManager.Instance.player;
        characters[enemyIdx].behaviours[ran].Do(ch);
        if(GameManager.Instance.player.hp < 0)
        {
            State = BattleState.BattleLose;
        }
        else
        {
            enemyIdx++;
            if (enemyIdx < characters.Count - 1)
            {
                EnemyTurn();
            }
            else
            {
                State = BattleState.EnemyTurnEnd;
            }
        }
    }
}
