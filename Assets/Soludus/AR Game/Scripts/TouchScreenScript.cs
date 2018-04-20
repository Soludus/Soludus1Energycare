using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenScript : MonoBehaviour
{
    public Camera localCam;
    public bool allowInput;
    public int layermask;

    public bool touchScreenTouched;

    void Awake()
    {
        allowInput = false;
        touchScreenTouched = false;
    }

    void Update()
    {
        Vector3 inputVector = Vector3.zero;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                GetInputPos(Input.GetTouch(i).position);
                return;
            }
        }

        //if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)

        if (Input.GetMouseButtonDown(0))
        {
            GetInputPos(Input.mousePosition);
        }
        //    inputVector = Input.mousePosition;
    }

    public IEnumerator WaitForInput(float waitTime)
    {
        allowInput = false;
        yield return new WaitForSeconds(waitTime);
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

    void GetInputPos(Vector3 inputVector)
    {
        Ray ray = localCam.ScreenPointToRay(inputVector);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, 1 << 10))
        {

            if (hit.collider.gameObject.name == "TouchPanel" && allowInput)
            {
                //Debug.Log("TouchPanel touched!");
                touchScreenTouched = true;
            }
        }
    }
}
