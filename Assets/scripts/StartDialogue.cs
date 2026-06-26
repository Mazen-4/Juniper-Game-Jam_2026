using UnityEngine;
using Yarn.Unity;

public class StartDialogue : MonoBehaviour
{
    public DialogueRunner runner;

    void Start()
    {
        runner.onDialogueStart.AddListener(OnDialogueStart);
        runner.onDialogueComplete.AddListener(OnDialogueEnd);
        runner.StartDialogue("IntroDialogue");
    }

    private void OnDialogueStart()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) player.enabled = false;
    }

    private void OnDialogueEnd()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) player.enabled = true;
    }
}

public class OutroTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueEnd);
            dialogueRunner.StartDialogue("OutroDialogue");
        }
    }

    private void OnDialogueStart()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) player.enabled = false;
    }

    private void OnDialogueEnd()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) player.enabled = true;

        // Unsubscribe so it doesn't stack if player re-enters the trigger
        dialogueRunner.onDialogueStart.RemoveListener(OnDialogueStart);
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueEnd);
    }
}