using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class IsoSortingOrder : MonoBehaviour
{
    public const float precision = 10;
    
    private void Update()
    {
        Sort();

        // only figure out the sorting layer once if the gameObject never changes position
        if (Application.isPlaying && gameObject.isStatic)
            enabled = false;
    }

    private void Sort()
    {
        var linear = -Camera.main.transform.forward.normalized;
        var position = transform.position;
        var renderer = GetComponent<Renderer>();
        renderer.sortingOrder = Mathf.FloorToInt((linear.x * position.x + linear.y * position.y + linear.z * position.z) * precision);
    }
}
