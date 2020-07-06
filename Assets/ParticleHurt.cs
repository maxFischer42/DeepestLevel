using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHurt : MonoBehaviour
{
    public void OnParticleCollision(GameObject other) {
        if(other.gameObject.GetComponent<Controller>()) {
            if(other.gameObject.GetComponent<Controller>().isAttacking)
                return;
        }
        if(other.gameObject.tag == "Player" ) {
            if(other.gameObject.GetComponent<Controller>().isAttacking || other.gameObject.GetComponent<Controller>().isHit )
                return;
            other.gameObject.GetComponent<PlayerHealthManager>().hp--;
            Destroy(other.gameObject.GetComponent<PlayerHealthManager>().hpObjects[other.gameObject.GetComponent<PlayerHealthManager>().hp]);
            other.gameObject.GetComponent<PlayerHealthManager>().spriteRenderer.sprite = other.gameObject.GetComponent<PlayerHealthManager>().hurtSprite;
            StartCoroutine(other.gameObject.GetComponent<PlayerHealthManager>().doHit());
        }
    }
}
