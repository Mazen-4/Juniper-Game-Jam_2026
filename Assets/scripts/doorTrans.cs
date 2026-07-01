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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (played) return;
        if (other.CompareTag("Player"))
        {
            played = true;
            GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Open");
            StartCoroutine(DisableAnimatorAfterOpen());

            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
            {
                player.enabled = false;
                player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }

            dialogueRunner.onDialogueComplete.AddListener(OnDialogueEnd);
            dialogueRunner.StartDialogue("OutroDialogue");
        }
    }

    private IEnumerator DisableAnimatorAfterOpen()
    {
        yield return null;
        yield return null;

        float length = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        // now in DoorOpen_Hold, freeze it on last frame
        animator.Play("DoorOpen_Hold", 0, 1f);
        yield return null;
        animator.speed = 0f;
    }

    private void OnDialogueEnd()
    {
        dialogueRunner.onDialogueComplete.RemoveListener(OnDialogueEnd);
        SceneManager.LoadScene("willyWithBigBilly");
    }
}