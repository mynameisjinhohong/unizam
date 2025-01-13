using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class outro : MonoBehaviour
{
    public Sprite[] sprites;
    public Image sprite;
    public int idx = 0;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Skip(1);
        }
    }
    public void Skip(int su)
    {
        if (su == 1)
        {
            idx++;
            if (idx == sprites.Length)
            {
                SceneManager.LoadScene("StartScene");
                return;
            }
            sprite.sprite = sprites[idx];
        }
        else
        {
            SceneManager.LoadScene("StartScene");
        }

    }
}
