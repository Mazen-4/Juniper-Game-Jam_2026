using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using UnityEngine;

public class Outro_Dialogue : MonoBehaviour
{
    
    public DialogueRunner dialogueRunner;
    [SerializeField] public Collider2D outroCollider;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player") && !dialogueRunner.IsDialogueRunning)
        {
            triggered = true;
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null) player.enabled = false;
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueEnd);
            dialogueRunner.StartDialogue("OutroDialogue");
        }
    }

    private void OnDialogueEnd()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) player.enabled = true;
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueEnd);
    }
    
}
