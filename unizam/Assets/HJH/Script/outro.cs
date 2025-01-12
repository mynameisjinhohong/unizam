using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class outro : MonoBehaviour
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
            if (idx == sprites.Length)
            {
                SceneManager.LoadScene("StartScene");
            }
            sprite.sprite = sprites[idx];
        }
    }
}
