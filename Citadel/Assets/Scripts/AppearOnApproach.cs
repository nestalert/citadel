using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearOnApproach : MonoBehaviour
{
    public SpriteRenderer itemRenderer;

    void Start()
    {
        if (itemRenderer == null)
            itemRenderer = GetComponent<SpriteRenderer>();

        itemRenderer.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemRenderer.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemRenderer.enabled = false; // αν θες να ξανακρύβεται
        }
    }
}