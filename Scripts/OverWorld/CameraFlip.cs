using UnityEngine;

public class CameraFlip : MonoBehaviour
{
    private bool isFlipped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isFlipped)
            {
                transform.Rotate(0, 180, 0, Space.World);
                isFlipped = true;
            }
            else
            {
                transform.Rotate(0, -180, 0, Space.World);
                isFlipped = false;
            }
        }
    }
}