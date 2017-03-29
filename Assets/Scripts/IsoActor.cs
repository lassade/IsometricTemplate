using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoActor : MonoBehaviour
{
    [SerializeField]
    private Transform graphic;
    [SerializeField]
    protected Animator graphicAnimator;
    [SerializeField]
    private float graphicRotationOffset = -135;
    private Quaternion graphicRotation;

    private const string graphicFacingProperty = "IsoSpriteFacingDirection";
    private static int graphicFacingPropertyId = -1;

    protected virtual void Awake()
    {
        graphicRotation = graphic.rotation;

        if (graphicFacingPropertyId == -1)
            graphicFacingPropertyId = Animator.StringToHash(graphicFacingProperty);
    }

    protected virtual void Reset()
    {
        graphic = transform.GetChild(0);
        if (graphic)
        {
            graphicAnimator = graphic.GetComponent<Animator>();
        }
    }

    //protected virtual void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawLine(transform.position,
    //        transform.position + (transform.rotation * Quaternion.Euler(0, graphicRotationOffset, 0) * Vector3.forward));
    //}

    protected virtual void Update()
    {
        var facting = graphicRotationOffset + transform.rotation.eulerAngles.y;
        facting = facting / 360;
        facting = (facting - (int)facting) * 360;

        graphicAnimator.SetFloat(graphicFacingPropertyId, facting);
        graphic.rotation = graphicRotation;
    }
}
