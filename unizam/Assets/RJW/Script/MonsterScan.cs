using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScan : MonoBehaviour
{
    public GameObject hintUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.CompareTag("Enemy") == true && collision.GetComponent<Encounter>().clear == false)
        {
            //hintUI.SetActive(true);
            StartCoroutine(scan());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") == true)
        {
            hintUI.SetActive(false);
        }
    }

    IEnumerator scan() {
        hintUI.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        hintUI.SetActive(false);
    }
}
