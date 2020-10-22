﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="New Card",menuName="ContreCard")]
public class ContreCard : Card
{
    private void Awake()
    {
        this.type = "contre";
    }
}
