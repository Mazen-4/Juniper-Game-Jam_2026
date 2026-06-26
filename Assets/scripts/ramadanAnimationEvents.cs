using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ramadanAnimationEvents : MonoBehaviour
{
    ramadanScript ramadan;
    void Start()
    {
        ramadan = GetComponentInParent<ramadanScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void destroyMe()
    {
        ramadan.destroyMe();
    }
    public void EndAttack()
    {
        ramadan.EndAttack();
    }
}
