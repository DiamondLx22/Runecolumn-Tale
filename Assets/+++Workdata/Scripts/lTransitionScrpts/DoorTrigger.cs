using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public SpriteRenderer sr;

    /// <summary>
    /// 0 Door close
    /// 1 Door open
    /// </summary>
    public Sprite[] doorSprites;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.sprite = doorSprites[1];
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.sprite = doorSprites[0];
        }
    }
}
