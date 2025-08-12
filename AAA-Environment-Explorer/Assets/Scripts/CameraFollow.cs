using Unity.Mathematics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Vector3 cameraOffset = new Vector3(0,1,0);
    [SerializeField] Vector3 lookOffset = new Vector3(0,0,0);
    [SerializeField] GameObject Camera;

    private void Update()
    {
        Camera.transform.position = playerTransform.position + cameraOffset;
        Vector3 lookDir = (playerTransform.transform.position - Camera.transform.position).normalized;
        quaternion lookAt = Quaternion.LookRotation(lookDir+lookOffset);
        Camera.transform.rotation = lookAt;
    }
}
