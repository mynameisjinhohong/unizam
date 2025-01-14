using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public Sprite[] sprites;
    public Image sprite;
    int idx = 0;
    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Skip(1);
        }
        */
    }

    public void Skip(int su)
    {
        if (su == 1)
        {
            idx++;
            if (idx == sprites.Length)
            {
                SceneManager.LoadScene("MainScene");
                return;
            }

            sprite.sprite = sprites[idx];

        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }

    }
}
