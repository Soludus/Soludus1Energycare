using UnityEngine;

public class TimeScaleChanger : MonoBehaviour
{
    public float normalScale = 1;
    public float changedScale = 10;

    private bool changed = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            changed = !changed;
            Time.timeScale = changed ? changedScale : normalScale;
        }
    }
}
