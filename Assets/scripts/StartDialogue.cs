using UnityEngine;
using Yarn.Unity;

public class StartDialogue : MonoBehaviour
{
    public DialogueRunner runner;

    void Start()
    {
        runner.StartDialogue("IntroDialogue");
    }
}
public class OutroTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.StartDialogue("OutroDialogue");
        }
    }
}
