using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float hitRadius = 1;
    public float attackDamage = 50;

    bool exploded = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (exploded) return;
        exploded = true;

        Destroy(gameObject);

        var unluckBastards = Physics.SphereCastAll(transform.position, hitRadius, Vector3.up, 0.01f, -1);
        foreach (var hit in unluckBastards)
        {
            var player = hit.transform.GetComponent<IsoPlayer>();
            if (player)
            {
                var force = player.transform.position - transform.position;
                force.y = 0;
                player.HurtKcnockback(attackDamage, force.normalized);
                return;
            }
        }
    }
}
