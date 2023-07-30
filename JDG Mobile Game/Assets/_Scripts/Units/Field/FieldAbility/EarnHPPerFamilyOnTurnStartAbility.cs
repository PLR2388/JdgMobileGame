using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using UnityEngine;

public class EarnHPPerFamilyOnTurnStartAbility : FieldAbility
{
    private float hp;
    private CardFamily family;
    
    public EarnHPPerFamilyOnTurnStartAbility(FieldAbilityName name, string description, float hpPerInvocation,
        CardFamily family)
    {
        Name = name;
        Description = description;
        hp = hpPerInvocation;
        this.family = family;
    }

    public override void OnTurnStart(PlayerCards playerCards, PlayerStatus playerStatus)
    {
        base.OnTurnStart(playerCards, playerStatus);
        var numberCard = playerCards.invocationCards.Count(card => card.Families.Contains(family));
        playerStatus.ChangePv(numberCard * hp);
    }
}
