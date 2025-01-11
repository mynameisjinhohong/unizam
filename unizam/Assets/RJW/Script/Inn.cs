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

                Debug.Log("���� �� �� �־���.");
                spendNight = true;
            }
            else
            {
                Debug.Log("������ �����մϴ�.");
            }
        
        }
        else {
            Debug.Log("�� �� ���� ����");
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
        if (innUI != null) {
            innUI.SetActive(false);
            enter = false;
        }
    }
}
