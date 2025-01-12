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
        if (Input.GetMouseButtonDown(0))
        {
            idx++;
            if(idx == sprites.Length)
            {
                SceneManager.LoadScene("MainScene");
            }
            sprite.sprite = sprites[idx];
        }
    }

    public void Skip()
    {
        SceneManager.LoadScene("MainScene");
    }
}
