using System.Collections;
using System.Collections.Generic;
using _Scripts.Units.Invocation;
using UnityEngine;

public class KillBothCardsIfAttackAbility : Ability
{

    private string cardName;

    public KillBothCardsIfAttackAbility(AbilityName name, string description, string cardName)
    {
        Name = name;
        Description = description;
        this.cardName = cardName;
    }
    
    public override void ApplyEffect(Transform canvas, PlayerCards playerCards, PlayerCards opponentPlayerCards)
    {
        
    }

    public override void OnTurnStart(Transform canvas, PlayerCards playerCards, PlayerCards opponentPlayerCards)
    {
     
    }

    public override void OnCardAdded(Transform canvas, InGameInvocationCard newCard, PlayerCards playerCards,
        PlayerCards opponentPlayerCards)
    {
       
    }

    public override void OnCardRemove(Transform canvas, InGameInvocationCard removeCard, PlayerCards playerCards,
        PlayerCards opponentPlayerCards)
    {
        
    }
    
    protected override void OnCardAttacked(Transform canvas, InGameInvocationCard attackedCard,
        InGameInvocationCard attacker, PlayerCards playerCards, PlayerCards opponentPlayerCards, PlayerStatus currentPlayerStatus, PlayerStatus opponentPlayerStatus)
    {
        if (attackedCard.Title == cardName)
        {
            base.OnCardAttacked(canvas, attackedCard, attacker, playerCards, opponentPlayerCards, currentPlayerStatus, opponentPlayerStatus);
            if (opponentPlayerCards.yellowTrash.Contains(attackedCard) && !playerCards.yellowTrash.Contains(attacker))
            {
                playerCards.invocationCards.Remove(attacker);
                playerCards.yellowTrash.Add(attacker);
            }
        }
        else
        {
            base.OnCardAttacked(canvas, attackedCard, attacker, playerCards, opponentPlayerCards, currentPlayerStatus, opponentPlayerStatus);
        }
    }
}
