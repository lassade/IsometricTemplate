using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class IsoPlayer : IsoActor
{
    private CharacterController controler;

    public float moveUnitsPerSecond = 1;
    public float rotationOffset = 0;
    public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

    public float health = 100;

    bool dizzy = false;
    int deadAnimatorProperty;

    protected override void Awake()
    {
        base.Awake();
        controler = GetComponent<CharacterController>();
        deadAnimatorProperty = Animator.StringToHash("Dead");
    }

    private void OnDrawGizmosSelected()
    {
        var delta = Quaternion.Euler(0, rotationOffset, 0);
        var position = transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, position + (delta * new Vector3(1, 0, 0)));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(position, position + (delta * new Vector3(0, 0, 1)));
    }

    protected override void Update()
    {
        base.Update();
        controler.Move(Physics.gravity * Time.deltaTime);

        if (health <= 0)
        {
            graphicAnimator.SetBool(deadAnimatorProperty, true);
            return;
        }

        if (dizzy) return;        

        var input = new Vector3(Input.GetAxis(horizontalAxis), 0, Input.GetAxis(verticalAxis));
        input.Normalize();

        if (input.magnitude < 0.2f) return;

        input = Quaternion.Euler(0, rotationOffset, 0) * input;
        input = input * moveUnitsPerSecond * Time.deltaTime;

        transform.forward = input;
        controler.Move(input);
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
        dizzy = true;
        float timeLapsed = 0;
        while (timeLapsed <= duration)
        {
            controler.Move(force * Time.deltaTime / duration);
            timeLapsed += Time.deltaTime;
            yield return null;
        }
        dizzy = false;
    }
}
