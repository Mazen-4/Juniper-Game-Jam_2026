using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private PlayerMovement player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    public void EndAttack()
    {
        player.EndAttack();
    }
    public void destroyMe()
    {
        player.destroyMe();
    }
}
