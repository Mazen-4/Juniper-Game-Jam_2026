using UnityEngine;
using Yarn.Unity;

public class StartDialogue : MonoBehaviour
{
    public DialogueRunner runner;

    void Start()
    {
        runner.StartDialogue("Start");
    }
}
