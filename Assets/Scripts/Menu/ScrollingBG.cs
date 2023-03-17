using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float backgroundLength;
    
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 scrollingDir;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, backgroundLength);
        transform.position = startPos + scrollingDir * newPos;
    }
}
