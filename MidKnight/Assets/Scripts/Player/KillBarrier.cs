using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class KillBarrier : MonoBehaviour
{
    public void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Collectable");
    }
    [Range(0, 200)]
    public int barrierDamage = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController p = PlayerController.Player;
        p.TakeDamage(barrierDamage);
        CharacterController c = p.GetComponent<CharacterController>();
        c.enabled = false;
        p.transform.position = p.SafePoint;
        c.enabled = true;
    }
}
