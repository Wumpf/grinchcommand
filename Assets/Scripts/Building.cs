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

    private void Start()
    {
        normalSprite = GetComponent<SpriteRenderer>().sprite;
        Restart();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (HappySprite == null)
            gameObject.SetActive(false);

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = HappySprite;
        var light = GetComponent<Light2D>();
        if (light != null)
            light.enabled = true;
        if (LooseLifeIfHitByPresent)
            OnLifeLostEvent?.Invoke();
    }

    public void Restart()
    {
        gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SpriteRenderer>().sprite = normalSprite;
        var light = GetComponent<Light2D>();
        if (light != null)
            light.enabled = false;
    }
}
