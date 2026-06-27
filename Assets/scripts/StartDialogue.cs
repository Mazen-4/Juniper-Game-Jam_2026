using UnityEngine;
using Yarn.Unity;

public class StartDialogue : MonoBehaviour
{
    public DialogueRunner runner;
    private PlayerMovement player;
    private Rigidbody2D playerRb;

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        playerRb = player.GetComponent<Rigidbody2D>();
        runner.onDialogueComplete.AddListener(OnDialogueEnd);
        StartCoroutine(StartAfterLanding());
    }

    private System.Collections.IEnumerator StartAfterLanding()
    {
        // wait until player is on the ground
        yield return new WaitUntil(() => playerRb.velocity.y == 0f);

        // one extra frame to let idle animation kick in
        yield return null;
        yield return null;

        player.enabled = false;
        playerRb.velocity = Vector2.zero;
        playerRb.bodyType = RigidbodyType2D.Static;

        runner.StartDialogue("IntroDialogue");
    }

    private void OnDialogueEnd()
    {
        if (player != null)
        {
            playerRb.bodyType = RigidbodyType2D.Dynamic;
            player.enabled = true;
        }
    }
}