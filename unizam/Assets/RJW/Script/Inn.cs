using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inn : MonoBehaviour
{
    bool spendNight = false;
    bool enter = false;
    public int heal;

    public GameObject innUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (enter && Input.GetKeyDown(KeyCode.Space)) {
            innUI.SetActive(true);
        }
    }

    public void randomEvent() {
        if (spendNight == false)
        {
            int num = Random.Range(0, 3);

            if (GameManager.Instance.player.mp >= 1)
            {
                GameManager.Instance.player.mp -= 1;
                GameManager.Instance.player.hp += heal;

                Debug.Log("편히 쉴 수 있었다.");
                spendNight = true;
            }
            else
            {
                Debug.Log("마나가 부족합니다.");
            }
        
        }
        else {
            Debug.Log("더 쉴 수는 없다");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spendNight == false)
        {
            enter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        innUI.SetActive(false);
        enter = false;
    }
}
