using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float minXClamp = -126.18f;
    public float maxXClamp = 62.5f;
    public float minYClamp = -30.44f;
    public float maxYClamp = 67.5f;
    private void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.y, minYClamp, maxYClamp);

        transform.position = cameraPos;
    }
}
