using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AntiSpamClick : MonoBehaviour
{
    private Button button;
    private float timer = 0;
    private bool isClick = false;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            if(gameObject.activeInHierarchy)
            {
                button.interactable = false;
                isClick = true;
            }
        });
    }

    private void Update()
    {
        if(isClick)
        {
            timer += Time.deltaTime;
            if(timer >= 1.0f)
            {
                timer = 0;
                isClick = false;
                button.interactable = true;
            }
        }
    }
}
