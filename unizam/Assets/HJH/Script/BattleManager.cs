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
                    //���߿� ���� ����� �����ϱ��
                    BattelWin();
                    break;
                case BattleState.BattleLose:
                    gameOver.SetActive(true);
                    break;
            }

        }
    }

    //�ξ�ô� ��ų���� ���� ������
    public int beforeDamage;

    public GameObject gameOver;

    public GameObject player;

    public SkillButton skillButton;

    public Transform[] spawnPos1;
    public Transform[] spawnPos2;
    public Transform[] spawnPos3;

    public Slider[] sliders;

    public DiceAni dice;

    public int diceSu;//�ֻ��� ��Դ°�

    bool skill = false; //���� ���°� ��ų����

    public bool choose; //���� ������
    List<EnemyCharacter> characters;//��ü ��
    List<GameObject> enemys; //��ü �� ������Ʈ... �ٲٱ� �����Ƽ� �ϳ��� ����������
    EnemyCharacter target;//�÷��̾ ������ ��
    Behaviour behaviour;//�÷��̾��� �̹��� �ൿ

    int enemyIdx = 0;

    public SpriteRenderer bg;

    public Transform mantae;

    public AudioClip[] audios;
    //0 - ���� ����, 1 - ����, 2- ��ǳ ��ų, 3- ���½�ų, 4- �ξｺų, 5- ����ȣ��ų, 6- ������Ÿ, 7 - �̹��������, 8 - �̹��� ��õ, 9 - �̹������, 10 - ���� ������
    public AudioSource audioPlay;
    public GameObject newAudio;

    public Button restart;
    public Button quit;

    public GameObject clear;
    bool clearBool = false;

    public void PlaySound(int idx)
    {
        // �⺻ AudioSource�� ���ų� �̹� ��� ���� ���
        if (audioPlay == null || audioPlay.isPlaying)
        {
            // �� AudioSource�� �������� ����
            GameObject newAudioSourceObject = Instantiate(newAudio, transform);
            AudioSource newAudioSource = newAudioSourceObject.GetComponent<AudioSource>();

            if (newAudioSource == null)
            {
                Debug.LogError("audioSourcePrefab�� AudioSource ������Ʈ�� �����ϴ�.");
                return;
            }

            // AudioClip�� ���� ����
            newAudioSource.clip = audios[idx];

            // ȿ���� ���
            newAudioSource.Play();

            // AudioClip�� ���̸�ŭ ��� �� ������Ʈ ����
            Destroy(newAudioSourceObject, audios[idx].length);
        }
        else
        {
            // ���� AudioSource���� ���
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

            // Image ������Ʈ ��������
            Image monsterImage = mantae.GetComponent<Image>();
            if (monsterImage != null)
            {
                // ���� ���� ����
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
            //�� ������ ���̿� ����� �� ���̶����� �ؾߵ�.
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
        //���̽� �������� ����
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
        //�ֻ��� �����̴� ���� �� ��.
        State = BattleState.ChooseEnemy;
    }

    public void ChooseEnemy()
    {
        choose = true;
    }

    public void UseSkill()
    {
        //��ų ��� ����
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
        if (skill)//��ų ����ϸ� �׸�ŭ ���� ���
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
        //���� Ŭ���� Ȯ��.
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
        //�� �ִϸ��̼� ����
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
                //�� ���� ������ ���� �ߵ��ϴ� �͵�
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
        //�����Ǵٰ� StartScene���� �̵�
        SceneManager.LoadScene("StartScene");
    }

    public void Exit()
    {
        //���� �Ǵٰ� ���� ����.
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
            Debug.Log("���� ��ų get");
            dice.Stop(5);
        }
        else if (enemys[0].name.Contains("Du"))
        {
            Debug.Log("�ξ�ô� ��ų get");
            dice.Stop(6);
        }
        else if (enemys[0].name.Contains("Gu") || enemys[0].name.Contains("Fox"))
        {
            Debug.Log("����ȣ ��ų get");
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
