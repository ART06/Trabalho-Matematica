using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] protected int cameraMinPos;
    [SerializeField] protected int cameraMaxPos;
    public int cameraOffset;

    protected void FixedUpdate()
    {
        CameraFollowPlayer();
    }
    protected void CameraFollowPlayer()
    {
        if (player.position.x <= cameraMinPos - cameraOffset || player.position.x >= cameraMaxPos - cameraOffset) return;
        else
        {
            Vector3 _newPos = transform.position;
            _newPos.x = player.position.x + cameraOffset;
            transform.position = _newPos;
        }
    }
}
