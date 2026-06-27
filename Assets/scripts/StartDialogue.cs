using UnityEngine;
using Yarn.Unity;
public class StartDialogue : MonoBehaviour
{
    public DialogueRunner runner;

    void Start()
    {
        runner.onDialogueComplete.AddListener(OnDialogueEnd);

        // Disable player directly before starting — don't rely on onDialogueStart
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null) player.enabled = false;

        runner.StartDialogue("IntroDialogue");
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
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player") && !dialogueRunner.IsDialogueRunning)
        {
            triggered = true;

            // Disable player directly here too, same reason
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