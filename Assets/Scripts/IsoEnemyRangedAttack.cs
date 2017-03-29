using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoEnemyRangedAttack : IsoEnemyMelee
{
    [SerializeField]
    GameObject projectile;

    public float projectileSpeed = 2;

    protected override void Update()
    {
        base.Update();
        attackRange = ((projectileSpeed * projectileSpeed) * Mathf.Sin( 2 * 45 * Mathf.Deg2Rad) / -Physics.gravity.y);
    }

    protected override void Attack()
    {
        var instance = Instantiate(projectile);
        var start = transform.position;
        var end = target.position;

        instance.transform.position = start;

        var physics = instance.GetComponent<Rigidbody>();
        
        var direction = (end - start);
        var magnitude = direction.magnitude;
        direction = direction / magnitude;
        var angle = Mathf.Asin((magnitude * -Physics.gravity.y) / (projectileSpeed * projectileSpeed)) * Mathf.Rad2Deg * 0.5f;
        
        if (angle < 45) // if the player gets too close be sure to let him have more time to react
            angle = 90 - angle;

        physics.velocity = Quaternion.AngleAxis(angle, Vector3.Cross(direction, Vector3.up)) * direction * projectileSpeed;
        graphicAnimator.SetTrigger("Attack");
    }
}
