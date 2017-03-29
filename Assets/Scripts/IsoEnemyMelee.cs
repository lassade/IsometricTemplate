using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoEnemyMelee : IsoAgent
{
    IsoPlayer player;

    [SerializeField]
    protected float attackRange = 2;

    [SerializeField]
    float attackCooldowm = 1;

    [SerializeField]
    protected float attackDamage = 15;

    bool attackEnabled = true;

    [SerializeField]
    protected float health = 100;

    protected override void Awake()
    {
        base.Awake();
        
        player = FindObjectOfType<IsoPlayer>();
        if (player)
        {
            target = player.transform;
        }
        else
        {
            target = null;
        }
    }

    private void OnEnable()
    {
        attackEnabled = true;
    }

    protected override void Update()
    {
        base.Update();

        if (attackEnabled)
        {
            // stop a little bit before the attack range
            agent.stoppingDistance = attackRange - .01f;

            var distance = (player.transform.position - transform.position).magnitude;
            if (distance <= attackRange)
            {
                Attack();
                StartCoroutine(RecoverFromAttackCooldown());
            }
        }
    }

    IEnumerator RecoverFromAttackCooldown()
    {
        attackEnabled = false;
        target = null; // stop pursuing the player
        yield return new WaitForSeconds(attackCooldowm);
        attackEnabled = true;
        target = player.transform;
    }

    protected virtual void Attack()
    {
        graphicAnimator.SetTrigger("Attack");
        var force = player.transform.position - transform.position;
        force.y = 0;
        player.HurtKcnockback(attackDamage, force.normalized);
    }

    public void Hurt(float damage)
    {
        health -= Mathf.Abs(damage);
        if (health > 0) graphicAnimator.SetTrigger("Hurt");
    }

    public void HurtKcnockback(float damage, Vector3 force)
    {
        if (health <= 0) return;
        Hurt(damage);
        StartCoroutine(Kcnockback(force, 0.1f));
    }

    IEnumerator Kcnockback(Vector3 force, float duration)
    {
        var a = transform.position;
        var b = a + force;
        float timeLapsed = 0;
        while(timeLapsed <= duration)
        {
            transform.position = Vector3.Lerp(a, b, Mathf.Clamp01(timeLapsed / duration));
            timeLapsed += Time.deltaTime;
            yield return null;
        }
    }
}
