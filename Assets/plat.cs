using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plat : MonoBehaviour
{
    void OnCollisionStay2D(Collision2D c) {
        c.transform.Translate(new Vector2(0, 0.01f));
    }
}
