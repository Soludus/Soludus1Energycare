using System.Collections;
using UnityEngine;

public class TouchScreenScript : MonoBehaviour
{
    public bool allowInput;
    [Header("Raycast")]
    public string hitObjectName = "TouchPanel";
    public Camera raycastCam;
    public LayerMask layerMask = -1;

    internal bool touchScreenTouched;

    private void Awake()
    {
        touchScreenTouched = false;
    }

    private void Update()
    {
        Vector3 inputVector = Vector3.zero;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                UpdateTouchState(Input.GetTouch(i).position);
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            UpdateTouchState(Input.mousePosition);
        }
    }

    public IEnumerator WaitForInput(float delay)
    {
        touchScreenTouched = false;
        allowInput = false;
        yield return new WaitForSeconds(delay);
        allowInput = true;

        while (!touchScreenTouched)
        {
            yield return null;
        }
        touchScreenTouched = false;
        allowInput = false;
    }

    public static bool GetTouch(out Vector3 pos)
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                pos = Input.GetTouch(i).position;
                return true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            pos = Input.mousePosition;
            return true;
        }

        pos = default(Vector3);
        return false;
    }

    private void UpdateTouchState(Vector3 inputVector)
    {
        Ray ray = raycastCam.ScreenPointToRay(inputVector);
        RaycastHit hit;

        if (allowInput && Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject.name == hitObjectName)
            {
                //Debug.Log("TouchPanel touched!");
                touchScreenTouched = true;
            }
        }
    }
}
