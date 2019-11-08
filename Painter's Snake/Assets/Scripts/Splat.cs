using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{

    public float MinSizeMod;
    public float MaxSizeMod;

    public Sprite[] Sprites;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize()
    {
        SetSprite();
        SetSize();
        SetRotation();
    }

    private void SetSprite()
    {
        int randomIndex = Random.Range(0, Sprites.Length);
        _spriteRenderer.sprite = Sprites[randomIndex];
    }

    private void SetSize()
    {
        float sizeMod = Random.Range(MinSizeMod, MaxSizeMod);
        transform.localScale *= sizeMod;
    }

    private void SetRotation()
    {
        float randomRotation = Random.Range(-360f, 360f);
        transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
    }
}
