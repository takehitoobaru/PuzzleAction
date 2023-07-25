using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    Block _block;

    void Start()
    {
        _block = GetComponent<Block>();
    }

    void OnMouseDown()
    {
        transform.position = new Vector2(0, -500);
        Destroy(gameObject);
    }
}
