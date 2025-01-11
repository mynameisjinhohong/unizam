using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DiceAni : MonoBehaviour
{
    Image image;
    Button button;
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
        button.interactable = false;
        image.sprite = diceSprite[idx];
    }
}
