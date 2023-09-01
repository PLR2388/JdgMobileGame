﻿using System.Collections.Generic;
using System.Linq;
using Cards;
using Sound;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Menu
{
    public class CardChoice : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private Transform canvas;

        [SerializeField] private GameObject choiceCardMenu;
        [SerializeField] private GameObject twoPlayerModeMenu;

        [SerializeField] private Text title;
        [SerializeField] private Text buttonText;

        public static readonly UnityEvent<int> ChangeChoicePlayer = new UnityEvent<int>();

        public bool isPlayerOneCardChosen;

        private int CheckCard(ICollection<Card> deck)
        {
            var numberSelected = 0;
            var children = container.GetComponentsInChildren<Transform>();
            foreach (var transformInChildren in children)
            {
                var cardGameObject = transformInChildren.gameObject;
                if (cardGameObject.GetComponent<OnHover>() == null) continue;
                var isSelected = cardGameObject.GetComponent<OnHover>().bIsSelected;
                if (!isSelected) continue;
                numberSelected++;
                deck.Add(cardGameObject.GetComponent<CardDisplay>().card);
            }

            return numberSelected;
        }

        private void DisplayMessageBox(int remainedCards)
        {
            var config = new MessageBoxConfig(
                LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.MODIFY_DECK_TITLE),
                string.Format(LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.MODIFY_DECK_MESSAGE), remainedCards),
                showOkButton: true
            );
            MessageBox.Instance.CreateMessageBox(canvas, config);
        }

        private void DeselectAllCards()
        {
            var children = container.GetComponentsInChildren<Transform>();
            foreach (var transformChildren in children)
            {
                var gameObjectChildren = transformChildren.gameObject;
                if (gameObjectChildren.GetComponent<OnHover>() != null)
                {
                    gameObjectChildren.GetComponent<OnHover>().bIsSelected = false;
                }
            }
        }

        public void CheckPlayerCards()
        {
            var deck = new List<Card>();
            var numberSelected = CheckCard(deck);

            if (numberSelected == GameState.MaxDeckCards)
            {
                if (isPlayerOneCardChosen)
                {
                    AudioSystem.Instance.StopMusic();
                    SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
                    title.text = LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.CARD_CHOICE_TITLE_PLAYER_ONE);
                    buttonText.text = LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.CARD_CHOICE_BUTTON_PLAYER_ONE);
                    isPlayerOneCardChosen = false;
                    ChangeChoicePlayer.Invoke(1);

                   GameState.Instance.deckP2 = deck.Select(card => InGameCard.CreateInGameCard(card, CardOwner.Player2)).ToList();
                }
                else
                {
                    title.text = LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.CARD_CHOICE_TITLE_PLAYER_TWO);
                    buttonText.text = LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.CARD_CHOICE_BUTTON_PLAYER_TWO);
                    isPlayerOneCardChosen = true;
                    ChangeChoicePlayer.Invoke(2);

                    GameState.Instance.deckP1 = deck.Select(card => InGameCard.CreateInGameCard(card, CardOwner.Player1)).ToList();
                    DeselectAllCards();
                }
            }
            else
            {
                var remainedCards = GameState.MaxDeckCards - numberSelected;
                DisplayMessageBox(remainedCards);
            }
        }

        public static void GetRandomDeck(int numberOfCards, ref List<Card> initialDeck, List<Card> cards)
        {
            var deckAllCard = cards.Where(card =>
                card.Type != CardType.Contre && card.Title != "Attaque de la tour Eiffel" &&
                card.Title != "Blague interdite" &&
                card.Title != "Un bon tuyau").ToList();

            while (initialDeck.Count != numberOfCards)
            {
                GetRandomCards(deckAllCard, initialDeck);
            }
        }

        public void RandomDeck()
        {
            var deck1 = new List<Card>();
            var deck2 = new List<Card>();

            var deck1AllCard = GameState.Instance.deck1AllCards.Where(card =>
                card.Type != CardType.Contre && card.Title != "Attaque de la tour Eiffel" &&
                card.Title != "Blague interdite" &&
                card.Title != "Un bon tuyau").ToList();

            var deck2AllCard = GameState.Instance.deck2AllCards.Where(card =>
                card.Type != CardType.Contre && card.Title != "Attaque de la tour Eiffel" &&
                card.Title != "Blague interdite" &&
                card.Title != "Un bon tuyau").ToList();

            while (deck1.Count != 30)
            {
                GetRandomCards(deck1AllCard, deck1);
            }

            while (deck2.Count != 30)
            {
                GetRandomCards(deck2AllCard, deck2);
            }

            GameState.Instance.deckP1 = deck1.Select(card1 => InGameCard.CreateInGameCard(card1, CardOwner.Player1)).ToList();
            GameState.Instance.deckP2 = deck2.Select(card2 => InGameCard.CreateInGameCard(card2, CardOwner.Player2)).ToList();
            AudioSystem.Instance.StopMusic();
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        public void DeckToTest()
        {
            var deck1 = new List<Card>();
            var deck2 = new List<Card>();

            var deck1AllCard = GameState.Instance.deck1AllCards;
            var deck2AllCard = GameState.Instance.deck2AllCards;

            deck2.Add(GetSpecificCard(CardNames.LycéeMagiqueGeorgesPompidou, deck2AllCard));
            deck1.Add(GetSpecificCard(CardNames.SandrineLePorteManteauExtraterrestre, deck1AllCard));
            deck1.Add(GetSpecificCard(CardNames.Mohammad, deck1AllCard));
            deck1.Add(GetSpecificCard(CardNames.MJCorrompu, deck1AllCard));
            deck1.Add(GetSpecificCard(CardNames.CaroleDuServiceMarketing, deck1AllCard));

            while (deck1.Count != 30)
            {
                GetRandomCards(deck1AllCard, deck1);
            }
            
            deck1.Reverse();


            while (deck2.Count != 30)
            {
                GetRandomCards(deck2AllCard, deck2);
            }

            deck2.Reverse();

            GameState.Instance.deckP1 = deck1.Select(card1 => InGameCard.CreateInGameCard(card1, CardOwner.Player1)).ToList();
            GameState.Instance.deckP2 = deck2.Select(card2 => InGameCard.CreateInGameCard(card2, CardOwner.Player2)).ToList();
            AudioSystem.Instance.StopMusic();
            SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);
        }

        public static Card GetSpecificCard(CardNames cardNames, List<Card> cards)
        {
            var nameCard = CardNameMappings.CardNameMap[cardNames];
            var card = cards.Find(x => x.Title == nameCard);
            if (card != null)
            {
                cards.Remove(card);
            }

            return card;
        }

        private static void GetRandomCards(IList<Card> allCards, ICollection<Card> deck)
        {
            var randomIndex = Random.Range(0, allCards.Count - 1);
            var card = allCards[randomIndex];
            if (card.Type == CardType.Contre) return;
            if (card == null) return;
            deck.Add(card);
            allCards.Remove(card);
        }

        public void Back()
        {
            if (isPlayerOneCardChosen)
            {
                title.text = LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.CARD_CHOICE_TITLE_PLAYER_ONE);
                buttonText.text = LocalizationSystem.Instance.GetLocalizedValue(LocalizationKeys.CARD_CHOICE_BUTTON_PLAYER_ONE);
                isPlayerOneCardChosen = false;
                GameState.Instance.deckP1 = new List<InGameCard>();
                DeselectAllCards();
                ChangeChoicePlayer.Invoke(1);
            }
            else
            {
                DeselectAllCards();
                choiceCardMenu.SetActive(false);
                twoPlayerModeMenu.SetActive(true);
            }
        }
    }
}