using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Building : MonoBehaviour
{
    private Sprite normalSprite;
    public Sprite HappySprite;
    public bool LooseLifeIfHitByPresent = false;

    public delegate void LifeLostEventHandler();
    public event LifeLostEventHandler OnLifeLostEvent;

    private void OnCollisionEnter2D(Collision2D other)
    {
        GetComponent<BoxCollider2D>().enabled = false;

        var spriteRenderer = GetComponent<SpriteRenderer>();
        normalSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = HappySprite;

        var light = GetComponent<Light2D>();
        if (light != null)
            light.enabled = true;
        if (LooseLifeIfHitByPresent)
            OnLifeLostEvent?.Invoke();

        for (int i=0; i<transform.childCount; ++i)
            transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        for (int i=0; i<transform.childCount; ++i)
            transform.GetChild(i).gameObject.SetActive(true);

        GetComponent<BoxCollider2D>().enabled = true;
        if (normalSprite != null)
            GetComponent<SpriteRenderer>().sprite = normalSprite;
        var light = GetComponent<Light2D>();
        if (light != null)
            light.enabled = false;
    }
}
