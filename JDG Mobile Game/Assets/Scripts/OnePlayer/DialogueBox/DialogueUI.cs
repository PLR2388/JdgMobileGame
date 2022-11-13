using System;
using System.Collections;
using System.Linq;
using OnePlayer.DialogueBox;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class NextDialogueEvent : UnityEvent<NextDialogueTrigger>
{
}

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue;

    public static readonly NextDialogueEvent NextDialogueTriggerEvent = new NextDialogueEvent();
    private NextDialogueTrigger currentTrigger = NextDialogueTrigger.Undefined;

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;
    private int currentSoundIndex = 0;

    private AudioSource audioSource;

    private void Start()
    {
        currentSoundIndex = 0;
        NextDialogueTriggerEvent.AddListener(ReceiveNextDialogueEvent);
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
        audioSource = FindObjectOfType<AudioSource>();
        CloseDialogueBox();
        ShowDialogue(testDialogue);
    }

    private void ReceiveNextDialogueEvent(NextDialogueTrigger nextDialogueEvent)
    {
        currentTrigger = nextDialogueEvent;
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue((dialogueObject)));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        var soundDialogIndex = dialogueObject.SoundDialogueIndex;
        var audioClips = dialogueObject.AudioClips;
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            if (soundDialogIndex.Contains(i))
            {
                var currentAudioClip = audioClips[currentSoundIndex];
                PlaySound(dialogueObject, dialogue, soundDialogIndex, i, currentAudioClip);
            }
            
            
            yield return typewriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.hasResponses) break;


            yield return new WaitUntil(() =>
            {
                NextDialogueTrigger nextDialogueTrigger = dialogueObject.NextDialogueTriggers[i];
                switch (nextDialogueTrigger)
                {
                    case NextDialogueTrigger.Tap:
#if UNITY_EDITOR
                        return Input.GetKeyDown(KeyCode.Space);
#elif UNITY_ANDROID
                        return Input.GetTouch(0);
#endif
                        break;
                    case NextDialogueTrigger.Automatic:
                        return true;
                        break;
                    case NextDialogueTrigger.PutCard:
                        break;
                    case NextDialogueTrigger.NextPhase:
                        break;
                    case NextDialogueTrigger.PutEffectCard:
                        break;
                    case NextDialogueTrigger.Undefined:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return false;
            });
        }

        if (dialogueObject.hasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private void PlaySound(DialogueObject dialogueObject, string dialogue, int[] soundDialogIndex, int i,
        AudioClip audioClip)
    {
        var length = dialogue.Length;
        var indexInSound = Array.IndexOf(soundDialogIndex, i);
        if (indexInSound < (soundDialogIndex.Length - 1))
        {
            var nextSoundDialogIndex = soundDialogIndex[indexInSound + 1]; // next DialogIndex
            if (nextSoundDialogIndex > (i + 1)) // if greater than the next one, there is multiple text so duration is longer
            {
                for (int j = (i + 1); j < nextSoundDialogIndex; j++)
                {
                    length += dialogueObject.Dialogue[j].Length;
                }
            }
        }
        else
        {
            // indexSound == soundDialogIndex.Length - 1
            // Get all text till the end
            for (int j = (i + 1); j < dialogueObject.Dialogue.Length; j++)
            {
                length += dialogueObject.Dialogue[j].Length;
            }
        }

        audioSource.Stop();
        typewriterEffect.AdaptSpeedToLength(audioClip.length, length);
        audioSource.PlayOneShot(audioClip);
        currentSoundIndex++;
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = String.Empty;
    }
}