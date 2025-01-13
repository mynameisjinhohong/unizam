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
    public int turn;

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
                    BattelWin();
                    break;
                case BattleState.BattleLose:
                    gameOver.SetActive(true);
                    break;
            }

        }
    }

    //두억시니 스킬만을 위한 에드훅
    public int beforeDamage;

    public GameObject gameOver;

    public GameObject player;

    public SkillButton skillButton;

    public Transform[] spawnPos1;
    public Transform[] spawnPos2;
    public Transform[] spawnPos3;

    public Slider[] sliders;

    public DiceAni dice;

    public int diceSu;//주사위 몇나왔는가

    bool skill = false; //지금 쓰는게 스킬인지

    public bool choose; //선택 턴인지
    List<EnemyCharacter> characters;//전체 적
    List<GameObject> enemys; //전체 적 오브젝트... 바꾸기 귀찮아서 하나더 만들어버리기
    EnemyCharacter target;//플레이어가 선택한 적
    Behaviour behaviour;//플레이어의 이번턴 행동

    int enemyIdx = 0;

    public SpriteRenderer bg;

    public Transform mantae;

    public AudioClip[] audios;
    //0 - 망태 등장, 1 - 베기, 2- 선풍 스킬, 3- 망태스킬, 4- 두억스킬, 5- 구미호스킬, 6- 몬스터평타, 7 - 이무기오오라, 8 - 이무기 승천, 9 - 이무기공격, 10 - 몬스터 죽을때
    public AudioSource audioPlay;
    public GameObject newAudio;

    public Button restart;
    public Button quit;

    public GameObject clear;
    bool clearBool = false;

    public void PlaySound(int idx)
    {
        // 기본 AudioSource가 없거나 이미 재생 중인 경우
        if (audioPlay == null || audioPlay.isPlaying)
        {
            // 새 AudioSource를 동적으로 생성
            GameObject newAudioSourceObject = Instantiate(newAudio, transform);
            AudioSource newAudioSource = newAudioSourceObject.GetComponent<AudioSource>();

            if (newAudioSource == null)
            {
                Debug.LogError("audioSourcePrefab에 AudioSource 컴포넌트가 없습니다.");
                return;
            }

            // AudioClip과 볼륨 설정
            newAudioSource.clip = audios[idx];

            // 효과음 재생
            newAudioSource.Play();

            // AudioClip의 길이만큼 대기 후 오브젝트 제거
            Destroy(newAudioSourceObject, audios[idx].length);
        }
        else
        {
            // 기존 AudioSource에서 재생
            audioPlay.clip = audios[idx];
            audioPlay.Play();
        }
    }

        private void Start()
    {
        clearBool = false;
        clear.SetActive(false);
        for(int i =0; i < GameManager.Instance.player.behaviours.Count; i++)
        {
            GameManager.Instance.player.behaviours[i].unit = player;
        }
        //bg.sprite = GameManager.Instance.bg;
        switch (GameManager.Instance.monster)
        {
            case monster.man:
                StartCoroutine(manFade());
                break;
            case monster.snake:
                StartCoroutine(SnakeFade());
                break;
            default:
                AfterShow();
                break;
        }
        
    }
    public Slider snakeSlider;
    IEnumerator SnakeFade()
    {
        State = BattleState.StartDice;
        characters = new List<EnemyCharacter>();
        enemys = new List<GameObject>();
        player.GetComponent<BattlePlayer>().MakeBuff();
        GameObject enemy = Instantiate(GameManager.Instance.enemies[0].enemy.enemyPrefab);
        enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[0].enemy.hp;
        enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[0].enemy.behaviours;
        for (int j = 0; j < enemy.GetComponent<BattleEnemy>().enemy.behaviours.Count; j++)
        {
            enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].character = enemy.GetComponent<BattleEnemy>().enemy;
            enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].unit = enemy;
        }
        enemy.GetComponent<BattleEnemy>().hpBar = snakeSlider;
        enemy.GetComponent<BattleEnemy>().BuffParent = snakeSlider.transform.GetChild(2);
        enemy.GetComponent<BattleEnemy>().MakeBuff();
        characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
        enemys.Add(enemy);
        GameManager.Instance.enemies.Clear();
        yield return null;
    }

    IEnumerator manFade()
    {
        yield return new WaitForSeconds(1f);
        if (mantae != null)
        {
            mantae.gameObject.SetActive(true);
            audioPlay.clip = audios[0];
            audioPlay.Play();
            yield return new WaitForSeconds(1.0f);

            // Image 컴포넌트 가져오기
            Image monsterImage = mantae.GetComponent<Image>();
            if (monsterImage != null)
            {
                // 색상 변경 시작
                //yield return StartCoroutine(FadeToBlack(monsterImage));
            }
        }

        yield return new WaitForSeconds(0.5f);
        mantae.gameObject.SetActive(false);
        AfterShow();
    }

    public void AfterShow()
    {
        State = BattleState.StartDice;
        characters = new List<EnemyCharacter>();
        enemys = new List<GameObject>();
        player.GetComponent<BattlePlayer>().MakeBuff();
        switch (GameManager.Instance.enemies.Count)
        {
            case 1:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos1[i]);
                    enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[i].enemy.hp;
                    enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[i].enemy.behaviours;
                    for (int j = 0; j < enemy.GetComponent<BattleEnemy>().enemy.behaviours.Count; j++)
                    {
                        enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].character = enemy.GetComponent<BattleEnemy>().enemy;
                        enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].unit = enemy;
                    }
                    enemy.GetComponent<BattleEnemy>().hpBar = sliders[i];
                    enemy.GetComponent<BattleEnemy>().BuffParent = sliders[i].transform.GetChild(2);
                    enemy.GetComponent<BattleEnemy>().MakeBuff();
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                    enemys.Add(enemy);
                }
                break;
            case 2:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos2[i]);
                    enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[i].enemy.hp;
                    enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[i].enemy.behaviours;
                    for (int j = 0; j < enemy.GetComponent<BattleEnemy>().enemy.behaviours.Count; j++)
                    {
                        enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].character = enemy.GetComponent<BattleEnemy>().enemy;
                        enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].unit = enemy;
                    }
                    enemy.GetComponent<BattleEnemy>().hpBar = sliders[i];
                    enemy.GetComponent<BattleEnemy>().BuffParent = sliders[i].transform.GetChild(2);
                    enemy.GetComponent<BattleEnemy>().MakeBuff();
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                    enemys.Add(enemy);
                }
                break;
            case 3:
                for (int i = 0; i < GameManager.Instance.enemies.Count; i++)
                {
                    GameObject enemy = Instantiate(GameManager.Instance.enemies[i].enemy.enemyPrefab, spawnPos3[i]);
                    enemy.GetComponent<BattleEnemy>().enemy.hp = GameManager.Instance.enemies[i].enemy.hp;
                    enemy.GetComponent<BattleEnemy>().enemy.behaviours = GameManager.Instance.enemies[i].enemy.behaviours;
                    for (int j = 0; j < enemy.GetComponent<BattleEnemy>().enemy.behaviours.Count; j++)
                    {
                        enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].character = enemy.GetComponent<BattleEnemy>().enemy;
                        enemy.GetComponent<BattleEnemy>().enemy.behaviours[j].unit = enemy;
                    }
                    enemy.GetComponent<BattleEnemy>().hpBar = sliders[i];
                    enemy.GetComponent<BattleEnemy>().BuffParent = sliders[i].transform.GetChild(2);
                    enemy.GetComponent<BattleEnemy>().MakeBuff();
                    characters.Add(enemy.GetComponent<BattleEnemy>().enemy);
                    enemys.Add(enemy);
                }
                break;
        }
        GameManager.Instance.enemies.Clear();
    }

    private void Update()
    {
        if (clearBool)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextScene();
            }
        }
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
                        choose = false;
                    }
                }
            }
        }
    }

    public void StartDice()
    {
        //다이스 굴러가는 연출
        skillButton.gameObject.GetComponent<Button>().interactable = true;
        dice.gameObject.SetActive(false);
        dice.gameObject.SetActive(true);
    }

    public void StopDice()
    {
        int ran = Random.Range(0, GameManager.Instance.player.behaviours.Count);
        behaviour = GameManager.Instance.player.behaviours[ran];
        behaviour.character = GameManager.Instance.player;
        diceSu = behaviour.mpIdx;
        dice.Stop(diceSu);
        skillButton.gameObject.GetComponent<Button>().interactable = false;
        State = BattleState.ChooseEnemy;
    }


    public void ChooseSkill(int idx)
    {
        for(int i =0; i<GameManager.Instance.player.behaviours.Count; i++)
        {
            if (GameManager.Instance.player.behaviours[i].mpIdx == idx)
            {
                behaviour = GameManager.Instance.player.behaviours[i];
            }
        }
        behaviour.character = GameManager.Instance.player;
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
        StartCoroutine(Skill());
    }

    IEnumerator Skill()
    {
        audioPlay.clip = audios[diceSu + 1];
        audioPlay.Play();
        yield return null;
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
        GameManager.Instance.player.buffs.Clear();
        for (int i = 0; i < GameManager.Instance.player.nextBuffs.Count; i++)
        {
            GameManager.Instance.player.buffs.Add(GameManager.Instance.player.nextBuffs[i]);
        }
        GameManager.Instance.player.nextBuffs.Clear();
        player.GetComponent<BattlePlayer>().MakeBuff();
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].buffs.Clear();
            for (int j = 0; j < characters[i].nextBuffs.Count; j++)
            {
                characters[i].buffs.Add(characters[i].nextBuffs[j]);
            }
            characters[i].nextBuffs.Clear();
            if (enemys[i] != null)
            {
                enemys[i].GetComponent<BattleEnemy>().MakeBuff();
            }
        }
        beforeDamage = 0;
        //게임 클리어 확인.
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
            StartCoroutine(EnemyTurnGo());
        }
    }

    IEnumerator EnemyTurnGo()
    {
        yield return new WaitForSeconds(1f);
        State = BattleState.EnemyTurn;
    }


    public void EnemyTurn()
    {

        StartCoroutine(EnemyMove(enemyIdx));
    }

    IEnumerator EnemyMove(int idx)
    {
        int ran = Random.Range(0, characters[enemyIdx].behaviours.Count);
        Character[] ch = new Character[1];
        ch[0] = GameManager.Instance.player;
        int nowHp = GameManager.Instance.player.hp;
        if (characters[enemyIdx].hp> 0)
        {
            characters[enemyIdx].behaviours[ran].Do(ch);
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
        }
        //적 애니메이션 연출
        beforeDamage += nowHp - GameManager.Instance.player.hp;
        if(GameManager.Instance.player.hp < 0)
        {
            State = BattleState.BattleLose;
        }
        else
        {
            enemyIdx++;
            if (enemyIdx < characters.Count)
            {
                EnemyTurn();
            }
            else
            {
                //적 턴이 끝나고 나서 발동하는 것들
                GameManager.Instance.player.buffs.Clear();
                for (int i = 0; i < GameManager.Instance.player.nextBuffs.Count; i++)
                {
                    GameManager.Instance.player.buffs.Add(GameManager.Instance.player.nextBuffs[i]);
                }
                GameManager.Instance.player.nextBuffs.Clear();
                player.GetComponent<BattlePlayer>().MakeBuff();
                for (int i = 0; i<characters.Count; i++)
                {
                    characters[i].buffs.Clear();
                    for(int j =0; j < characters[i].nextBuffs.Count; j++)
                    {
                        characters[i].buffs.Add(characters[i].nextBuffs[j]);
                    }
                    characters[i].nextBuffs.Clear();
                    if (enemys[i] != null)
                    {
                        enemys[i].GetComponent<BattleEnemy>().MakeBuff();
                    }
                }
                turn += 1;
                State = BattleState.EnemyTurnEnd;
            }
        }
    }

    public void Retry()
    {
        //암전되다가 StartScene으로 이동
        SceneManager.LoadScene("StartScene");
    }

    public void Exit()
    {
        //암전 되다가 게임 종료.
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                        Application.Quit(); 
        #endif
    }


    public void BattelWin()
    {
        StartCoroutine(BattleWinCo());
    }

    IEnumerator BattleWinCo()
    {
        yield return new WaitForSeconds(1f);
        dice.gameObject.SetActive(true);
        for(int i = enemys.Count-1; i >= 0; i--)
        {
            if (enemys[i] == null)
            {
                enemys.RemoveAt(i);
            }
        }
        if (enemys[0].name.Contains("Mang"))
        {
            Debug.Log("망태 스킬 get");
            dice.Stop(5);
        }
        else if (enemys[0].name.Contains("Du"))
        {
            Debug.Log("두억시니 스킬 get");
            dice.Stop(6);
        }
        else if (enemys[0].name.Contains("Gu") || enemys[0].name.Contains("Fox"))
        {
            Debug.Log("구미호 스킬 get");
            dice.Stop(7);
        }
        else
        {
            SceneManager.LoadScene("EndingScene");
        }
        yield return new WaitForSeconds(1.5f);
        clear.SetActive(true);
        clearBool = true;
    }

    public void NextScene()
    {
        GameManager.Instance.player.behaviours.Add(GameManager.Instance.Reward);
        GameManager.Instance.player.buffs.Clear();
        SceneManager.LoadScene("MainScene");
    }


    public GameObject dragon;
    public GameObject dragon2;
    public Sprite[] dragonSprite;
    public void DragonPower()
    {
        dragon.SetActive(true);
        dragon2.SetActive(true);
        dragon2.GetComponent<Image>().sprite = dragonSprite[0];
        StartCoroutine(EndDragon());
    }

    IEnumerator EndDragon()
    {
        for(int i = 0; i< dragonSprite.Length; i++)
        {
            
            yield return new WaitForSeconds(0.1f);
            dragon2.GetComponent<Image>().sprite = dragonSprite[i];
        }
        yield return new WaitForSeconds(0.5f);
        gameOver.SetActive(true);
    }
}
