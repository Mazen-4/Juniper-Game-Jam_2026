using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class doorTrans : MonoBehaviour
{
    [SerializeField] private float waitTime = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(LoadSceneAfterDelay());
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("willyWithBigBilly");
    }
}