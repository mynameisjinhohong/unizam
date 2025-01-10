using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inn : MonoBehaviour
{
    bool isVisit = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 클릭이 처음 발생한 순간에만 실행
        if (Input.GetKeyDown(KeyCode.Space) && isVisit == true)
        {
            int num = Random.Range(0, 3);

            if (num == 0)
            {
                Debug.Log("체력 회복");
            }
            else if (num == 1)
            {
                Debug.Log("체력 감소");
            }
            else if (num == 2)
            {
                Debug.Log("마나 회복");
            }
            this.GetComponent<Inn>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("방문");
            isVisit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("나감");
        isVisit = false;
    }
}
