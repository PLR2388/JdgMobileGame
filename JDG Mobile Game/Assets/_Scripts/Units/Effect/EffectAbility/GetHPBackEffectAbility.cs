using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Units.Invocation;
using Cards;
using UnityEngine;

public class GetHPBackEffectAbility : EffectAbility
{
    private int numberInvocationToSacrifice;
    private float atkDefCondition;
    private float HPToRecover;

    public GetHPBackEffectAbility(EffectAbilityName name, string description, int numberInvocations,
        float atkDefCondition, float hpToRecover)
    {
        Name = name;
        Description = description;
        numberInvocationToSacrifice = numberInvocations;
        this.atkDefCondition = atkDefCondition;
        HPToRecover = hpToRecover;
    }

    private void DisplayOkMessage(Transform canvas)
    {
        MessageBox.CreateOkMessageBox(canvas, "Attention", "Il faut sélectionner un sacrifice");
    }

    public override bool CanUseEffect(PlayerCards playerCards)
    {
        return playerCards.invocationCards.Count(card =>
            card.Attack >= atkDefCondition || card.Defense >= atkDefCondition) >= numberInvocationToSacrifice;
    }

    public override void ApplyEffect(Transform canvas, PlayerCards playerCards, PlayerCards opponentPlayerCard,
        PlayerStatus playerStatus,
        PlayerStatus opponentStatus)
    {
        base.ApplyEffect(canvas, playerCards, opponentPlayerCard, playerStatus, opponentStatus);
        if (numberInvocationToSacrifice == 0)
        {
            playerStatus.ChangePv(HPToRecover);
        }
        else if (numberInvocationToSacrifice == 1)
        {
            var invocationCards = new List<InGameCard>(playerCards.invocationCards
                .Where(card => card.Attack >= atkDefCondition || card.Defense >= atkDefCondition).ToList());
            var messageBox = MessageBox.CreateMessageBoxWithCardSelector(canvas, "Carte à sacrifier", invocationCards);
            messageBox.GetComponent<MessageBox>().PositiveAction = () =>
            {
                var invocationCard = (InGameInvocationCard)messageBox.GetComponent<MessageBox>().GetSelectedCard();
                if (invocationCard == null)
                {
                    DisplayOkMessage(canvas);
                }
                else
                {
                    playerCards.yellowCards.Add(invocationCard);
                    playerCards.invocationCards.Remove(invocationCard);
                    playerStatus.ChangePv(HPToRecover);
                    Object.Destroy(messageBox);
                }
            };
            messageBox.GetComponent<MessageBox>().NegativeAction = () => { DisplayOkMessage(canvas); };
        }
    }
}