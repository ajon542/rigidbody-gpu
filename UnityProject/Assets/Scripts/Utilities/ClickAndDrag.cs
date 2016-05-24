using UnityEngine;
using System.Collections;

public class ClickAndDrag : MonoBehaviour
{
    private float dist;
    private Transform toDrag;
    private bool dragging;
    private Vector3 offset;

    private void Update()
    {
        Vector3 v3;
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
            if(Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    toDrag = hit.transform;
                    dist = hit.transform.position.z - Camera.main.transform.position.z;
                    v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                    v3 = Camera.main.ScreenToWorldPoint(v3);
                    offset = toDrag.position - v3;
                    dragging = true;
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (dragging)
            {
                v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                toDrag.position = v3 + offset;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }
}
