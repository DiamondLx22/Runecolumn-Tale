using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimelinDetector : MonoBehaviour
{
    public List<string> tagTargets = new List<string> { "Player", "AlphaSlimelin" };

    public List<Collider2D> detectObjects = new List<Collider2D>();
    public Collider2D col;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Add(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (tagTargets.Contains(collider.gameObject.tag))
        {
            detectObjects.Remove(collider);
        }
    }
}
