using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class doorTrans : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    private Animator animator;
    private bool played = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        dialogueRunner.onDialogueComplete.AddListener(OnDialogueEnd);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (played) return;
        if (other.CompareTag("Player"))
        {
            played = true;

            // disable this trigger so it can't fire again
            GetComponent<Collider2D>().enabled = false;

            animator.SetTrigger("Open");

            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                player.enabled = false;
                player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }

            dialogueRunner.StartDialogue("OutroDialogue");
        }
    }

    private void OnDialogueEnd()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueEnd);
        SceneManager.LoadScene("willyWithBigBilly");
    }
}