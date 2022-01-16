﻿using Cards;
using UnityEngine;

public class PhysicalCardDisplay : MonoBehaviour
{
    public Card card;
    public Material defaultMaterial;
    public bool bIsFaceHidden;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (bIsFaceHidden)
        {
            meshRenderer.material = defaultMaterial;
        }
        else
        {
            meshRenderer.material = card ? card.MaterialCard : defaultMaterial;
        }
    }
}