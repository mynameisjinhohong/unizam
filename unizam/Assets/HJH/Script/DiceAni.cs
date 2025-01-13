using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceAni : MonoBehaviour
{
    Image image;
    Button button;
    public GameObject skillDescribe;
    bool stop = false;
    public TMP_Text skillTexts;
    public TMP_Text skillDescribeText;

    public Sprite[] aniSprites;
    public Sprite[] diceSprite;
    int idx = 0;
    public float time = 1;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.interactable = true;
        skillDescribe.SetActive(false);
        stop = false;
        skillTexts.gameObject.SetActive(false);
        StartCoroutine(ChangeImage());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator ChangeImage()
    {
        while (true)
        {
            image.sprite = aniSprites[idx];
            idx++;
            if (idx == aniSprites.Length)
            {
                idx = 0;
            }
            yield return new WaitForSeconds(time);

        }
    }

    public void Stop(int idx)
    {
        StopAllCoroutines();
        stop = true;
        skillTexts.gameObject.SetActive(true);
        for(int i =0; i< GameManager.Instance.player.behaviours.Count; i++)
        {
            if(idx == GameManager.Instance.player.behaviours[i].mpIdx)
            {
                skillTexts.text = GameManager.Instance.player.behaviours[i].behaviourName;
                break;
            }
        }
        button.interactable = false;
        image.sprite = diceSprite[idx];
    }
     
    public void MoveSkillText()
    {
        if (stop)
        {
            skillDescribe.SetActive(true);
            StartCoroutine(MoveSkill());
        }
    }

    public void StopSkillTest()
    {
        StopAllCoroutines();
        skillDescribe.SetActive(false);
    }

    IEnumerator MoveSkill()
    {
        while (true)
        {
            yield return null;
            skillDescribe.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(5, -5, 0);
        }
    }
}
