using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private PlayerMovement player;
    private Vector3 originalLocalPos;
    public float weapon3YOffset = 0.5f; 

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
        originalLocalPos = transform.localPosition;
    }
    public void Update()
    {
        if (player.alginFlag == 1)
        {
            transform.localPosition = originalLocalPos;
        }
        if (player.alginFlag == 2)
        {
            transform.localPosition = new Vector3(originalLocalPos.x, originalLocalPos.y + 0.14f, originalLocalPos.z);
            
        }
        if (player.alginFlag == 3)
        {
            transform.localPosition = new Vector3(originalLocalPos.x, originalLocalPos.y + 0.3f, originalLocalPos.z);
        }
        if (player.alginFlag == 4)
        {
            transform.localPosition = new Vector3(originalLocalPos.x, originalLocalPos.y + 0.5f, originalLocalPos.z);

        }
       
    }

    public void EndAttack()
    {
        player.EndAttack();
    }

    public void fireUpBullut()
    {
        player.fireUpBullut();
    }

    public void fireUpBullutUP()
    {
        player.fireUpBullut();
    }

    public void disableScript()
    {
        player.disableScript();
    }

    public void destroyMe()
    {
        player.destroyMe();
    }
}