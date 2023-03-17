using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer cursorSprite;

    // Start is called before the first frame update
    private void Awake()
    {
        cam = Camera.main;
        AudioController.instance.PlayDrawingSound();
        AudioController.instance.MuteDrawingSound();
    }

    private void Start()
    {
        StartCoroutine(PlayDrawAudio());
    }

    IEnumerator PlayDrawAudio()
    {
        while (true)
        {
            if (!GameManager.instance.isPause && Input.GetMouseButton(0) && GameManager.instance.drawPointLeft > 0)
            {
                Vector2 currentPos = transform.position;
                yield return new WaitForSeconds(0.05f);
                Vector2 newPos = transform.position;

                if (Vector2.Distance(currentPos, newPos) > 0.01f)
                {
                    AudioController.instance.UnmuteDrawingSound();
                }
                else
                {
                    AudioController.instance.MuteDrawingSound();
                }
            }
            else
            {
                AudioController.instance.MuteDrawingSound();
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    void Update()
    {
        if (IsMouseOverGUI())
        {
            cursorSprite.enabled = false;
            Cursor.visible = true;
        }
        else
        {
            cursorSprite.enabled = true;
            Cursor.visible = false;
        }

        Vector2 cursorPos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;
    }

    private bool IsMouseOverGUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}