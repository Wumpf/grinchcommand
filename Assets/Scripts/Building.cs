using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Building : MonoBehaviour
{
    public Sprite HappySprite;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (HappySprite == null)
            GameObject.Destroy(this);

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = HappySprite;
        var light = GetComponent<Light2D>();
        if (light != null)
            light.enabled = true;
    }
}
