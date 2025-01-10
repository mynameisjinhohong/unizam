using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : MonoBehaviour
{
    bool spendNight = false;

    public GameObject innUI;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void randomEvent() {
        if (spendNight == false)
        {
            int num = Random.Range(0, 3);

            if (GameManager.Instance.player.mp >= 1)
            {
                GameManager.Instance.player.mp -= 1;
                GameManager.Instance.player.hp += 20;
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
            innUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        innUI.SetActive(false);
    }
}
