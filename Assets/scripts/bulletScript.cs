using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField]private int bulletSpeed;
    [SerializeField] public int bulletDir;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * bulletDir * Time.deltaTime * bulletSpeed;
    }
}
