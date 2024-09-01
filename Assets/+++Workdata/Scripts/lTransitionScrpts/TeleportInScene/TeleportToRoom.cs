using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToRoom : MonoBehaviour
{
    public Transform spawnPos;
    public Animator anim;

    private void Start()
    {
        anim = GameObject.Find("Blackscreen").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(InitiateTeleport(other));
        }
    }

    IEnumerator InitiateTeleport(Collider2D other)
    {
        anim.Play("BlackscreenFadeIn");
        yield return new WaitForSeconds(1);
        other.transform.position = spawnPos.position;
        yield return new WaitForSeconds(.5f);
        anim.Play("BlackscreenFadeOut");
    }
}
