﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using _Scripts.Units.Invocation;
using Cards;
using Cards.EffectCards;
using Cards.EquipmentCards;
using Cards.FieldCards;
using Cards.InvocationCards;
using UnityEngine;

public class PlayerCards : MonoBehaviour
{
    public List<InGameCard> deck = new List<InGameCard>();
    public List<InGameCard> handCards = new List<InGameCard>();

    //public List<InGameInvocationCard> invocationCards = new List<InGameInvocationCard>();
    public List<InGameEffectCard> effectCards = new List<InGameEffectCard>();

    [SerializeField] public bool isPlayerOne;

    [SerializeField] private GameObject nextPhaseButton;
    [SerializeField] private GameObject inHandButton;
    [SerializeField] private Transform canvas;
    [SerializeField] private CardLocation cardLocation;

    private UnitManager unitManager;


    public List<InGameCard> secretCards = new List<InGameCard>(); // Where combine card go


    public string Tag => isPlayerOne ? "card1" : "card2";
    
    private List<InGameCard> oldHandCards = new List<InGameCard>();
    private List<InGameCard> oldYellowTrash = new List<InGameCard>();

    public bool IsFieldDesactivate { get; private set; }

    private InGameFieldCard _fieldCard;

    public InGameFieldCard FieldCard
    {
        get { return _fieldCard; }
        set
        {
            if (_fieldCard != value && _fieldCard != null)
            {
                foreach (var fieldCardFieldAbility in _fieldCard.FieldAbilities)
                {
                    fieldCardFieldAbility.OnFieldCardRemoved(this);
                }
            }
            _fieldCard = value;
            CardLocation.UpdateLocation.Invoke();
         
        }
    }

    public ObservableCollection<InGameInvocationCard> invocationCards =
        new ObservableCollection<InGameInvocationCard>();

    public ObservableCollection<InGameCard> yellowCards = new ObservableCollection<InGameCard>();

    private List<InGameInvocationCard> oldInvocations = new List<InGameInvocationCard>();

    // Start is called before the first frame update
    private void Start()
    {
        invocationCards = new ObservableCollection<InGameInvocationCard>();
        effectCards = new List<InGameEffectCard>(4);
        var gameStateGameObject = GameObject.Find("GameState");
        var gameState = gameStateGameObject.GetComponent<GameState>();
        deck = isPlayerOne ? gameState.deckP1 : gameState.deckP2;
        UnitManager.Instance.InitPhysicalCards(deck, isPlayerOne);

        for (var i = deck.Count - 5; i < deck.Count; i++)
        {
            handCards.Add(deck[i]);
        }

        deck.RemoveRange(deck.Count - 5, 5);
        cardLocation.HideCards(handCards);

        invocationCards.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnInvocationCardAdded(invocationCards.Last());
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    var removedInvocationCard = oldInvocations.Except(invocationCards).First();
                    OnInvocationCardsRemoved(removedInvocationCard);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            OnInvocationCardsChanged();
            oldInvocations = invocationCards.ToList();
        };
        
        yellowCards.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnYellowTrashAdded();
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            CardLocation.UpdateLocation.Invoke();
            oldYellowTrash = yellowCards.ToList();
        };
    }

    /*public void DesactivateFieldCardEffect()
    {
        if (FieldCard != null)
        {
            OnFieldCardDesactivate(FieldCard);
        }

        IsFieldDesactivate = true;
    }

    public void ActivateFieldCardEffect()
    {
        IsFieldDesactivate = false;
        if (FieldCard != null)
        {
            FieldFunctions.ApplyFieldCardEffect(field, this);
        }
    }*/


    public void ResetInvocationCardNewTurn()
    {
        foreach (var invocationCard in invocationCards.Where(invocationCard =>
                     invocationCard != null && invocationCard.Title != null))
        {
            invocationCard.ResetNewTurn();
        }
    }

    public bool ContainsCardInInvocation(InGameInvocationCard invocationCard)
    {
        return invocationCards.Any(invocation => invocation != null && invocation.Title == invocationCard.Title);
    }

    // Update is called once per frame
    private void Update()
    {
        if (oldHandCards.Count != handCards.Count)
        {
            foreach (var invocationCard in invocationCards)
            {
                if (invocationCard.EquipmentCard == null ||
                    invocationCard.EquipmentCard.EquipmentPermEffect == null) continue;
                var permEffect = invocationCard.EquipmentCard.EquipmentPermEffect;
                if (permEffect.Keys.Contains(PermanentEffect.AddAtkBaseOnHandCards))
                {
                    var value = float.Parse(permEffect.Values[
                        permEffect.Keys.FindIndex(key => key == PermanentEffect.AddAtkBaseOnHandCards)]);
                    invocationCard.Attack += (handCards.Count - oldHandCards.Count) * value;
                }
                else if (permEffect.Keys.Contains(PermanentEffect.AddDefBaseOnHandCards))
                {
                    var value = float.Parse(permEffect.Values[
                        permEffect.Keys.FindIndex(key => key == PermanentEffect.AddDefBaseOnHandCards)]);
                    invocationCard.Defense += (handCards.Count - oldHandCards.Count) * value;
                }
            }

            oldHandCards = new List<InGameCard>(handCards);
            CardLocation.UpdateLocation.Invoke();
        }
    }

    public void SendToSecretHide(InGameCard card)
    {
        secretCards.Add(card);
    }

    public void RemoveFromSecretHide(InGameCard card)
    {
        secretCards.Remove(card);
    }

    public void SendCardToYellowTrash(InGameCard card)
    {
        for (var i = 0; i < invocationCards.Count; i++)
        {
            if (invocationCards[i].Title != card.Title) continue;
            var invocationCard = card as InGameInvocationCard;
            if (invocationCard == null) continue;
            if (invocationCard is InGameSuperInvocationCard superInvocationCard)
            {
                var invocationListCards = superInvocationCard.invocationCards;
                foreach (var cardFromList in invocationListCards)
                {
                    secretCards.Remove(cardFromList);
                    yellowCards.Add(cardFromList);
                }

                invocationCards.Remove(superInvocationCard);
            }
            else
            {
                if (invocationCard.EquipmentCard != null)
                {
                    var equipmentCard = invocationCard.EquipmentCard;
                    yellowCards.Add(equipmentCard);
                    invocationCard.SetEquipmentCard(null);
                }

                invocationCards.Remove((InGameInvocationCard)card);
                yellowCards.Add(card);
            }
        }

        for (var i = 0; i < effectCards.Count; i++)
        {
            if (effectCards[i].Title == card.Title)
            {
                effectCards.Remove(card as InGameEffectCard);
                yellowCards.Add(card);
            }
        }

        if (FieldCard != null && FieldCard.Title == card.Title)
        {
            FieldCard = null;
            yellowCards.Add(card);
        }
    }

    public void SendInvocationCardToYellowTrash(InGameInvocationCard specificCardFound)
    {
        var equipmentCard = specificCardFound.EquipmentCard;
        specificCardFound.IncrementNumberDeaths();
        if (specificCardFound.IsControlled)
        {
            var opponentPlayerCards = isPlayerOne
                ? GameObject.Find("Player2").GetComponent<PlayerCards>()
                : GameObject.Find("Player1").GetComponent<PlayerCards>();
            opponentPlayerCards.secretCards.Remove(specificCardFound);
            opponentPlayerCards.yellowCards.Add(specificCardFound);
            if (equipmentCard != null)
            {
                opponentPlayerCards.yellowCards.Add(equipmentCard);
            }

            invocationCards.Remove(specificCardFound);

            cardLocation.RemovePhysicalCard(specificCardFound);

            var abilities = specificCardFound.Abilities;
            foreach (var ability in abilities)
            {
                ability.OnCardDeath(canvas, specificCardFound, opponentPlayerCards);
            }


            /*var invocationDeathEffect = specificCardFound.InvocationDeathEffect;
            var keys = invocationDeathEffect.Keys;
            var values = invocationDeathEffect.Values;

            var cardName = "";
            for (var i = 0; i < keys.Count; i++)
            {
                var value = values[i];
                switch (keys[i])
                {
                    case DeathEffect.GetSpecificCard:
                        cardName = values[i];
                        break;
                    case DeathEffect.GetCardSource:
                        opponentPlayerCards.GetCardSourceDeathEffect(specificCardFound, value, cardName);
                        break;
                    case DeathEffect.ComeBackToHand:
                        //opponentPlayerCards.ComeBackToHandDeathEffect(specificCardFound, value);
                        break;
                    case DeathEffect.KillAlsoOtherCard:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }*/
        }
        else
        {
            invocationCards.Remove(specificCardFound);
            yellowCards.Add(specificCardFound);
            if (equipmentCard != null)
            {
                yellowCards.Add(equipmentCard);
            }
            
            var abilities = specificCardFound.Abilities;
            foreach (var ability in abilities)
            {
                ability.OnCardDeath(canvas, specificCardFound, this);
            }

            /*if (specificCardFound.InvocationDeathEffect == null) return;
            var invocationDeathEffect = specificCardFound.InvocationDeathEffect;
            var keys = invocationDeathEffect.Keys;
            var values = invocationDeathEffect.Values;

            var cardName = "";
            for (var i = 0; i < keys.Count; i++)
            {
                var value = values[i];
                switch (keys[i])
                {
                    case DeathEffect.GetSpecificCard:
                        cardName = values[i];
                        break;
                    case DeathEffect.GetCardSource:
                        GetCardSourceDeathEffect(specificCardFound, value, cardName);
                        break;
                    case DeathEffect.ComeBackToHand:
                        //ComeBackToHandDeathEffect(specificCardFound, value);
                        break;
                    case DeathEffect.KillAlsoOtherCard:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }*/
        }

        specificCardFound.FreeCard();
        // specificCardFound.Reset();
    }

    public void RemoveSuperInvocation(InGameCard superInvocationCard)
    {
        invocationCards.Remove(superInvocationCard as InGameInvocationCard);
        cardLocation.RemovePhysicalCard(superInvocationCard);
    }

    private void ComeBackToHandDeathEffect(InGameInvocationCard invocationCard, string value)
    {
        var isParsed = int.TryParse(value, out var number);
        if (isParsed)
        {
            if (invocationCard.NumberOfDeaths > number)
            {
                SendInvocationCardToYellowTrash(invocationCard);
            }
            else
            {
                SendCardToHand(invocationCard);
            }
        }
        else
        {
            SendCardToHand(invocationCard);
        }
    }

    private void AskUserToAddCardInHand(string cardName, InGameCard cardFound, bool isFound)
    {
        if (!isFound) return;

        nextPhaseButton.SetActive(false);
        inHandButton.SetActive(false);

        void PositiveAction()
        {
            handCards.Add(cardFound);
            deck.Remove(cardFound);
            nextPhaseButton.SetActive(true);
            inHandButton.SetActive(true);
        }

        void NegativeAction()
        {
            nextPhaseButton.SetActive(true);
            inHandButton.SetActive(true);
        }

        MessageBox.CreateSimpleMessageBox(canvas, "Carte en main",
            "Veux-tu aussi ajouter " + cardName + " à ta main ?", PositiveAction, NegativeAction);
    }

    private void GetCardSourceDeathEffect(InGameCard invocationCard, string source,
        string cardName)
    {
        InGameCard cardFound = null;

        SendCardToYellowTrash(invocationCard);
        switch (source)
        {
            case "deck":
            {
                if (cardName != "")
                {
                    var isFound = false;
                    var j = 0;
                    while (j < deck.Count && !isFound)
                    {
                        if (deck[j].Title == cardName)
                        {
                            isFound = true;
                            cardFound = deck[j];
                        }

                        j++;
                    }

                    AskUserToAddCardInHand(cardName, cardFound, isFound);
                }

                break;
            }
            case "trash":
            {
                var trash = yellowCards;
                if (cardName != "")
                {
                    var isFound = false;
                    var j = 0;
                    while (j < trash.Count && !isFound)
                    {
                        if (trash[j].Title == cardName)
                        {
                            isFound = true;
                            cardFound = trash[j];
                        }

                        j++;
                    }

                    AskUserToAddCardInHand(cardName, cardFound, isFound);
                }

                break;
            }
        }
    }

    public void SendCardToHand(InGameCard card)
    {
        for (var i = 0; i < invocationCards.Count; i++)
        {
            if (invocationCards[i].Title == card.Title)
            {
                invocationCards.Remove(card as InGameInvocationCard);
            }
        }

        handCards.Add(card);
    }

    public int GetIndexInvocationCard(string nameCard)
    {
        for (var i = 0; i < invocationCards.Count; i++)
        {
            if (invocationCards[i].Title == nameCard)
            {
                return i;
            }
        }

        return -1;
    }

    private void OnInvocationCardAdded(InGameInvocationCard newInvocationCard)
    {
        var opponentPlayerCard = isPlayerOne
            ? GameObject.Find("Player2").GetComponent<PlayerCards>()
            : GameObject.Find("Player1").GetComponent<PlayerCards>();

        var opponentEffectCards = opponentPlayerCard.effectCards;

        foreach (var inGameInvocationCard in invocationCards)
        {
            foreach (var ability in inGameInvocationCard.Abilities)
            {
                ability.OnCardAdded(canvas, newInvocationCard, this, opponentPlayerCard);
            }
        }

        foreach (var effectAbility in effectCards.SelectMany(effectCard => effectCard.EffectAbilities))
        {
            effectAbility.OnInvocationCardAdded(this, newInvocationCard);
        }

        if (FieldCard?.FieldAbilities != null)
        {
            foreach (var fieldAbility in FieldCard.FieldAbilities)
            {
                fieldAbility.OnInvocationCardAdded(newInvocationCard);
            }
        }
    

        //var mustSkipAttack = opponentEffectCards.Select(effectCard => effectCard.EffectCardEffect.Keys)
         //   .Any(keys => keys.Contains(Effect.SkipAttack));


        /*for (var j = invocationCards.Count - 1; j >= 0; j--)
        {
            var invocationCard = invocationCards[j];

            if (mustSkipAttack)
            {
                invocationCard.BlockAttack();
            }

            var permEffect = invocationCard.InvocationPermEffect;
            if (permEffect == null) continue;
            var keys = permEffect.Keys;
            var values = permEffect.Values;

            var invocationCardsToChange = new List<InGameInvocationCard>();
            var sameFamilyInvocationCards = new List<InGameInvocationCard>();
            var mustHaveMinimumUndef = false;

            for (var i = 0; i < keys.Count; i++)
            {
                var value = values[i];
                switch (keys[i])
                {
                    case PermEffect.GiveStat:
                    {
                        GiveStatPermEffect(newInvocationCard, value, invocationCard, ref invocationCardsToChange);
                    }
                        break;
                    case PermEffect.Family:
                    {
                        FamilyPermEffect(newInvocationCard, value, invocationCard, ref sameFamilyInvocationCards);
                    }
                        break;
                    case PermEffect.Condition:
                    {
                        ConditionPermEffect(value, ref sameFamilyInvocationCards, ref mustHaveMinimumUndef);
                    }
                        break;
                    case PermEffect.IncreaseAtk:
                    {
                        IncreaseAtkPermEffect(invocationCardsToChange, value, sameFamilyInvocationCards,
                            ref invocationCard,
                            mustHaveMinimumUndef);
                    }
                        break;
                    case PermEffect.IncreaseDef:
                    {
                        IncreaseDefPermEffect(invocationCardsToChange, value, sameFamilyInvocationCards,
                            ref invocationCard,
                            mustHaveMinimumUndef);
                    }
                        break;
                    case PermEffect.CanOnlyAttackIt:
                    case PermEffect.PreventInvocationCards:
                    case PermEffect.ProtectBehind:
                    case PermEffect.ImpossibleAttackByInvocation:
                    case PermEffect.ImpossibleToBeAffectedByEffect:
                    case PermEffect.NumberTurn:
                    case PermEffect.checkCardsOnField:
                    default:
                        break;
                }
            }
        }*/

        for (var i = effectCards.Count - 1; i >= 0; i--)
        {
            var effectCard = effectCards[i];
            //var effectCardEffect = effectCard.EffectCardEffect;
            /*if (effectCardEffect != null)
            {
                if (effectCardEffect.Keys.Contains(Effect.SameFamily))
                {
                    if (field != null && !IsFieldDesactivate)
                    {
                        newInvocationCard.Families = new[]
                        {
                            field.GetFamily()
                        };
                    }
                }
                else if (effectCardEffect.Keys.Contains(Effect.NumberAttacks))
                {
                    var value = int.Parse(
                        effectCardEffect.Values[
                            effectCardEffect.Keys.FindIndex(effect => effect == Effect.NumberAttacks)]);
                    newInvocationCard.SetRemainedAttackThisTurn(value);
                }
            }*/
        }

        if (FieldCard != null && !IsFieldDesactivate)
        {
           /* var fieldCardEffect = field.FieldCardEffect;

            var fieldKeys = fieldCardEffect.Keys;
            var fieldValues = fieldCardEffect.Values;

            var family = field.GetFamily();
            for (var i = 0; i < fieldKeys.Count; i++)
            {
                var fieldValue = fieldValues[i];
                switch (fieldKeys[i])
                {
                    case FieldEffect.ATK:
                    {
                        ATKFieldEffect(ref newInvocationCard, fieldValue, family);
                    }
                        break;
                    case FieldEffect.DEF:
                    {
                        DEFFieldEffect(ref newInvocationCard, fieldValue, family);
                    }
                        break;
                    case FieldEffect.Change:
                    {
                        ChangeFieldEffect(ref newInvocationCard, fieldValue, family);
                    }
                        break;
                    case FieldEffect.GetCard:
                        break;
                    case FieldEffect.DrawCard:
                        break;
                    case FieldEffect.Life:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }*/
        }
    }

    private static void ChangeFieldEffect(ref InGameInvocationCard newInvocationCard, string fieldValue,
        CardFamily family)
    {
        var names = fieldValue.Split(';');
        if (names.Contains(newInvocationCard.Title))
        {
            newInvocationCard.Families = new[]
            {
                family
            };
        }
    }

    private static void DEFFieldEffect(ref InGameInvocationCard newInvocationCard, string fieldValue, CardFamily family)
    {
        var def = float.Parse(fieldValue);
        if (newInvocationCard.Families.Contains(family))
        {
            newInvocationCard.Defense += def;
        }
    }

    private static void ATKFieldEffect(ref InGameInvocationCard newInvocationCard, string fieldValue, CardFamily family)
    {
        var atk = float.Parse(fieldValue);
        if (newInvocationCard.Families.Contains(family))
        {
            newInvocationCard.Attack += atk;
        }
    }

    private static void IncreaseDefPermEffect(List<InGameInvocationCard> invocationCardsToChange, string value,
        List<InGameInvocationCard> sameFamilyInvocationCards, ref InGameInvocationCard invocationCard,
        bool mustHaveMinimumUndef)
    {
        if (invocationCardsToChange.Count > 0)
        {
            foreach (var invocationCardToChange in invocationCardsToChange)
            {
                invocationCardToChange.Defense += float.Parse(value);
            }
        }
        else if (sameFamilyInvocationCards.Count > 0)
        {
            invocationCard.Defense += float.Parse(value) * sameFamilyInvocationCards.Count;
        }
        else if (mustHaveMinimumUndef)
        {
            var minValue = float.Parse(value);
            if (invocationCard.GetCurrentDefense() < minValue)
            {
                invocationCard.Defense = minValue;
            }
        }
    }

    private static void IncreaseAtkPermEffect(List<InGameInvocationCard> invocationCardsToChange, string value,
        List<InGameInvocationCard> sameFamilyInvocationCards, ref InGameInvocationCard invocationCard,
        bool mustHaveMinimumUndef)
    {
        if (invocationCardsToChange.Count > 0)
        {
            foreach (var invocationCardToChange in invocationCardsToChange)
            {
                invocationCardToChange.Attack += float.Parse(value);
            }
        }
        else if (sameFamilyInvocationCards.Count > 0)
        {
            invocationCard.Attack += float.Parse(value) * sameFamilyInvocationCards.Count;
        }
        else if (mustHaveMinimumUndef)
        {
            // TODO : Set same value for this card than benzaie ATK And Def
            var minValue = float.Parse(value);
            if (invocationCard.GetCurrentAttack() < minValue)
            {
                invocationCard.Attack = minValue;
            }
        }
    }

    private void ConditionPermEffect(string value, ref List<InGameInvocationCard> sameFamilyInvocationCards,
        ref bool mustHaveMinimumUndef)
    {
        switch (value)
        {
            case "2 ATK 2 DEF":
            {
                for (var k = sameFamilyInvocationCards.Count - 1; k >= 0; k--)
                {
                    var invocationCardToCheck = sameFamilyInvocationCards[k];
                    if (invocationCardToCheck.Attack != 2f ||
                        invocationCardToCheck.Defense != 2f)
                    {
                        sameFamilyInvocationCards.Remove(invocationCardToCheck);
                    }
                }
            }
                break;
            case "Benzaie jeune":
            {
                if (invocationCards.Any(invocationCardToCheck =>
                        invocationCardToCheck.Title == value))
                {
                    mustHaveMinimumUndef = true;
                }
            }
                break;
        }
    }

    private void FamilyPermEffect(InGameInvocationCard newInvocationCard, string value,
        InGameInvocationCard invocationCard,
        ref List<InGameInvocationCard> sameFamilyInvocationCards)
    {
        if (Enum.TryParse(value, out CardFamily cardFamily))
        {
            if (newInvocationCard.Title == invocationCard.Title)
            {
                // Must catch up old cards
                sameFamilyInvocationCards.AddRange(invocationCards
                    .Where(invocationCardToCheck => invocationCardToCheck.Title != newInvocationCard.Title)
                    .Where(invocationCardToCheck =>
                        invocationCardToCheck.Families.Contains(cardFamily)));
            }
            else if (newInvocationCard.Families.Contains(cardFamily))
            {
                sameFamilyInvocationCards.Add(newInvocationCard);
            }
        }
    }

    private void GiveStatPermEffect(InGameInvocationCard newInvocationCard, string value,
        InGameInvocationCard invocationCard,
        ref List<InGameInvocationCard> invocationCardsToChange)
    {
        if (Enum.TryParse(value, out CardFamily cardFamily))
        {
            if (newInvocationCard.Title == invocationCard.Title)
            {
                // Must catch up old cards
                invocationCardsToChange.AddRange(invocationCards
                    .Where(invocationCardToCheck => invocationCardToCheck.Title != newInvocationCard.Title)
                    .Where(invocationCardToCheck =>
                        invocationCardToCheck.Families.Contains(cardFamily)));
            }
            else if (newInvocationCard.Families.Contains(cardFamily))
            {
                invocationCardsToChange.Add(newInvocationCard);
            }
        }
    }

    private void OnInvocationCardsRemoved(InGameInvocationCard removedInvocationCard)
    {
        var opponentPlayerCard = isPlayerOne
            ? GameObject.Find("Player2").GetComponent<PlayerCards>()
            : GameObject.Find("Player1").GetComponent<PlayerCards>();

        var cloneInvocationCards = invocationCards.ToList();
        // Apply onCardRemove for invocation card that are still alive
        foreach (var ability in cloneInvocationCards.SelectMany(inGameInvocationCard => inGameInvocationCard.Abilities))
        {
            ability.OnCardRemove(canvas, removedInvocationCard, this, opponentPlayerCard);
        }
        
        foreach (var effectAbility in effectCards.SelectMany(effectCard => effectCard.EffectAbilities))
        {
            effectAbility.OnInvocationCardRemoved(this, removedInvocationCard);
        }

        if (FieldCard != null)
        {

            /*for (var i = 0; i < fieldKeys.Count; i++)
            {
                switch (fieldKeys[i])
                {
                    case FieldEffect.Change:
                    {
                        ChangeFieldEffect(ref removedInvocationCard, fieldValues[i]);
                    }
                        break;
                    case FieldEffect.ATK:
                        break;
                    case FieldEffect.DEF:
                        break;
                    case FieldEffect.GetCard:
                        break;
                    case FieldEffect.DrawCard:
                        break;
                    case FieldEffect.Life:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }*/
        }
    }

    private static void ChangeFieldEffect(ref InGameInvocationCard removedInvocationCard, string fieldValue)
    {
        var names = fieldValue.Split(';');
        if (names.Contains(removedInvocationCard.Title))
        {
            removedInvocationCard.Families = removedInvocationCard.baseInvocationCard.BaseInvocationCardStats.Families;
        }
    }

    private void BeneficiaryActionEffect(InGameInvocationCard removedInvocationCard,
        InGameInvocationCard invocationCard,
        string value,
        float atk, float def, ref List<int> indexToDelete, int i)
    {
        if (removedInvocationCard.Title == invocationCard.Title)
        {
            foreach (var invocationCardToCheck in invocationCards)
            {
                if (invocationCardToCheck.Title != value) continue;
                invocationCardToCheck.Attack -= atk;
                invocationCardToCheck.Defense -= def;
                indexToDelete.Add(i);
                break;
            }
        }
    }

    private static void IncreaseDefPermEffectRemoved(List<InGameInvocationCard> invocationCardsToChange, string value,
        List<InGameInvocationCard> sameFamilyInvocationCards, ref InGameInvocationCard invocationCard,
        bool mustHaveMiminumAtkdef)
    {
        if (invocationCardsToChange.Count > 0)
        {
            foreach (var invocationCardToChange in invocationCardsToChange)
            {
                invocationCardToChange.Defense -= float.Parse(value);
            }
        }
        else if (sameFamilyInvocationCards.Count > 0)
        {
            invocationCard.Defense -= float.Parse(value) * sameFamilyInvocationCards.Count;
        }
        else if (mustHaveMiminumAtkdef)
        {
            invocationCard.Defense = invocationCard.baseInvocationCard.BaseInvocationCardStats.Defense;
        }
    }

    private static void IncreaseAtkPermEffectRemoved(List<InGameInvocationCard> invocationCardsToChange, string value,
        List<InGameInvocationCard> sameFamilyInvocationCards, ref InGameInvocationCard invocationCard,
        bool mustHaveMiminumAtkdef)
    {
        if (invocationCardsToChange.Count > 0)
        {
            foreach (var invocationCardToChange in invocationCardsToChange)
            {
                invocationCardToChange.Attack -= float.Parse(value);
            }
        }
        else if (sameFamilyInvocationCards.Count > 0)
        {
            invocationCard.Attack -= float.Parse(value) * sameFamilyInvocationCards.Count;
        }
        else if (mustHaveMiminumAtkdef)
        {
            invocationCard.Attack = invocationCard.baseInvocationCard.BaseInvocationCardStats.Attack;
        }
    }

    private static void ConditionPermEffectRemoved(InGameInvocationCard removedInvocationCard, string value,
        List<InGameInvocationCard> sameFamilyInvocationCards, InGameInvocationCard invocationCard,
        ref bool mustHaveMiminumAtkdef)
    {
        switch (value)
        {
            case "2 ATK 2 DEF":
            {
                for (var k = sameFamilyInvocationCards.Count - 1; k >= 0; k--)
                {
                    var invocationCardToCheck = sameFamilyInvocationCards[k];
                    if (invocationCardToCheck.Attack != 2f ||
                        invocationCardToCheck.Defense != 2f)
                    {
                        sameFamilyInvocationCards.Remove(invocationCardToCheck);
                    }
                }
            }
                break;
            case "Benzaie jeune":
            {
                if (removedInvocationCard.Title == invocationCard.Title)
                {
                    mustHaveMiminumAtkdef = true;
                }
            }
                break;
        }
    }

    private void FamilyPermEffectRemoved(InGameInvocationCard removedInvocationCard, string value,
        InGameInvocationCard invocationCard,
        ref List<InGameInvocationCard> sameFamilyInvocationCards)
    {
        if (Enum.TryParse(value, out CardFamily cardFamily))
        {
            if (removedInvocationCard.Title == invocationCard.Title)
            {
                // Delete itself so loose all advantage to itself
                foreach (var invocationCardToCheck in invocationCards)
                {
                    if (invocationCardToCheck.Title != removedInvocationCard.Title)
                    {
                        if (invocationCardToCheck.Families.Contains(cardFamily))
                        {
                            sameFamilyInvocationCards.Add(invocationCardToCheck);
                        }
                    }
                }
            }
            else if (removedInvocationCard.Families.Contains(cardFamily))
            {
                // Just loose 1 element
                sameFamilyInvocationCards.Add(removedInvocationCard);
            }
        }
    }

    private void GiveStatPermEffectRemoved(InGameInvocationCard removedInvocationCard, string value,
        InGameInvocationCard invocationCard, ref List<InGameInvocationCard> invocationCardsToChange)
    {
        if (Enum.TryParse(value, out CardFamily cardFamily))
        {
            if (removedInvocationCard.Title == invocationCard.Title)
            {
                // Delete itself everybody lose advantage
                foreach (var invocationCardToCheck in invocationCards)
                {
                    if (invocationCardToCheck.Title != removedInvocationCard.Title)
                    {
                        if (invocationCardToCheck.Families.Contains(cardFamily))
                        {
                            invocationCardsToChange.Add(invocationCardToCheck);
                        }
                    }
                }
            }
        }
    }

    private void OnInvocationCardsChanged()
    {
        /*for (var j = invocationCards.Count - 1; j >= 0; j--)
        {
            var invocationCard = invocationCards[j];
            var permEffect = invocationCard.InvocationPermEffect;
            if (permEffect == null) continue;
            var keys = permEffect.Keys;
            var values = permEffect.Values;

            for (var i = 0; i < keys.Count; i++)
            {
                switch (keys[i])
                {
                    case PermEffect.checkCardsOnField:
                    {
                        CheckCardOnFieldPermEffect(values[i], invocationCard);
                    }
                        break;
                    case PermEffect.CanOnlyAttackIt:
                    case PermEffect.GiveStat:
                    case PermEffect.IncreaseAtk:
                    case PermEffect.IncreaseDef:
                    case PermEffect.Family:
                    case PermEffect.PreventInvocationCards:
                    case PermEffect.ProtectBehind:
                    case PermEffect.ImpossibleAttackByInvocation:
                    case PermEffect.ImpossibleToBeAffectedByEffect:
                    case PermEffect.Condition:
                    case PermEffect.NumberTurn:
                    default:
                        break;
                }
            }
        }*/

        CardLocation.UpdateLocation.Invoke();
    }

    private void CheckCardOnFieldPermEffect(string value, InGameInvocationCard invocationCard)
    {
        var isFound = false;
        if (Enum.TryParse(value, out CardFamily cardFamily))
        {
            if (invocationCards
                .Where(otherInvocationCard => otherInvocationCard.Title != invocationCard.Title)
                .Any(otherInvocationCard => otherInvocationCard.Families.Contains(cardFamily)))
            {
                isFound = true;
            }
        }
        else
        {
            var cards = value.Split(';');

            if (invocationCards
                .Where(otherInvocationCard => otherInvocationCard.Title != invocationCard.Title)
                .Any(otherInvocationCard => cards.Contains(otherInvocationCard.Title)))
            {
                isFound = true;
            }
        }

        if (!isFound)
        {
            SendInvocationCardToYellowTrash(invocationCard);
        }
    }

    private void OnFieldCardDesactivate(InGameFieldCard oldFieldCard)
    {
    
        

       /* var family = oldFieldCard.GetFamily();
        for (var i = 0; i < fieldKeys.Count; i++)
        {
            var fieldValue = fieldValues[i];
            switch (fieldKeys[i])
            {
                case FieldEffect.ATK:
                {
                    AtkFieldEffect(fieldValue, family);
                }
                    break;
                case FieldEffect.DEF:
                {
                    DefFieldEffect(fieldValue, family);
                }
                    break;
                case FieldEffect.Change:
                {
                    ChangeFieldEffect(fieldValue);
                }
                    break;
                case FieldEffect.GetCard:
                    break;
                case FieldEffect.DrawCard:
                    break;
                case FieldEffect.Life:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }*/
    }

    private void OnYellowTrashAdded()
    {
        var newYellowTrashCard = yellowCards.Last();
        var invocationCard = newYellowTrashCard as InGameInvocationCard;
        if (invocationCard != null)
        {
            invocationCard.UnblockAttack();
            invocationCard.Attack = invocationCard.baseInvocationCard.BaseInvocationCardStats.Attack;
            invocationCard.Defense = invocationCard.baseInvocationCard.BaseInvocationCardStats.Defense;
            invocationCard.FreeCard();
            invocationCard.ResetNewTurn();
            foreach (var t in invocationCard.Abilities)
            {
                t.OnCardDeath(canvas, invocationCard, this);
            }
        }
    }

    private void OnFieldCardChanged(InGameFieldCard oldFieldCard)
    {

        if (oldFieldCard == null) return;

        SendInvocationCardToYellowTrashAfterFieldDestruction(oldFieldCard);
        
        
        /*var family = oldFieldCard.GetFamily();
        for (var i = 0; i < fieldKeys.Count; i++)
        {
            var fieldValue = fieldValues[i];
            switch (fieldKeys[i])
            {
                case FieldEffect.ATK:
                {
                    AtkFieldEffect(fieldValue, family);
                }
                    break;
                case FieldEffect.DEF:
                {
                    DefFieldEffect(fieldValue, family);
                }
                    break;
                case FieldEffect.Change:
                {
                    ChangeFieldEffect(fieldValue);
                }
                    break;
                case FieldEffect.GetCard:
                    break;
                case FieldEffect.DrawCard:
                    break;
                case FieldEffect.Life:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }*/
    }

    private void SendInvocationCardToYellowTrashAfterFieldDestruction(InGameFieldCard oldFieldCard)
    {
        /*if (oldField.Title == "Le grenier")
        {
            // Destroy all invocation cards
            for (var i = invocationCards.Count - 1; i >= 0; i--)
            {
                SendInvocationCardToYellowTrash(invocationCards[i]);
            }
        }
        else
        {
            // Picky destruction
            var familyFieldCard = oldFieldCard.GetFamily();
            var familySpecificCard = invocationCards.Where(card => card.Families.Contains(familyFieldCard)).ToList();
            for (var i = familySpecificCard.Count - 1; i >= 0; i--)
            {
                SendInvocationCardToYellowTrash(familySpecificCard[i]);
            }
        }*/
    }

    private void ChangeFieldEffect(string fieldValue)
    {
        var names = fieldValue.Split(';');
        foreach (var invocationCard in invocationCards.Where(invocationCard =>
                     names.Contains(invocationCard.Title)))
        {
            invocationCard.Families = invocationCard.baseInvocationCard.BaseInvocationCardStats.Families;
        }
    }

    private void DefFieldEffect(string fieldValue, CardFamily family)
    {
        var def = float.Parse(fieldValue);
        foreach (var invocationCard in invocationCards)
        {
            if (!invocationCard.Families.Contains(family)) continue;
            invocationCard.Defense -= def;
        }
    }

    private void AtkFieldEffect(string fieldValue, CardFamily family)
    {
        var atk = float.Parse(fieldValue);
        foreach (var invocationCard in invocationCards)
        {
            if (!invocationCard.Families.Contains(family)) continue;
            invocationCard.Attack -= atk;
        }
    }
}