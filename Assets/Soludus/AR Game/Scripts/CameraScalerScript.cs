using UnityEngine;

public class CameraScalerScript : MonoBehaviour
{
    // 16:10
    public float targetAspect = 16.0f / 10.0f;

    private float prevScreenAspect = 0;

    private void Update()
    {
        // determine the game window's current aspect ratio
        float screenAspect = (float)Screen.width / (float)Screen.height;

        if (screenAspect != prevScreenAspect)
        {
            ClampCameraAspect(GetComponent<Camera>(), screenAspect, targetAspect);
        }

        prevScreenAspect = screenAspect;
    }

    private void OnEnable()
    {
        prevScreenAspect = 0;
    }

    private void OnDisable()
    {
        var cam = GetComponent<Camera>();
        if (cam != null)
            cam.rect = new Rect(0, 0, 1, 1);
    }

    private static void ClampCameraAspect(Camera camera, float screenAspect, float targetAspect)
    {
        // current viewport height should be scaled by this amount
        float scaleheight = screenAspect / targetAspect;

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
