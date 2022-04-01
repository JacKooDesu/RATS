using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanTalkBehaviour : MonoBehaviour
{
    public Text targetText;
    public float showTime = 2;
    public float restTimeMin = 3, restTimeMax = 5;
    float randRestTime;
    bool isTalking = false;
    float timer;

    public string[] chats;

    private void Start()
    {
        randRestTime = Random.Range(restTimeMin, restTimeMax);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (isTalking)
        {
            if (timer >= showTime)
            {
                isTalking = false;
                ClearText();
                timer = 0;
                randRestTime = Random.Range(restTimeMin, restTimeMax);
            }
        }
        else
        {
            if (timer >= randRestTime)
            {
                isTalking = true;
                SetText(Random.Range(0, chats.Length));
                timer = 0;
            }
        }
    }

    void SetText(int index)
    {
        targetText.text = chats[index];
    }

    void ClearText()
    {
        targetText.text = "";
    }
}
