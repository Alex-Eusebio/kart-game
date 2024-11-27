using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    public Camera _camera;
    [SerializeField]
    private Transform camPos;
    [SerializeField]
    private Transform camPos2;

    GameObject player;

    void Start()
    {
        player = this.gameObject;
    }

    private void FixedUpdate()
    {
        if (_camera)
        {
            Follow();
        }
    }

    private void Follow()
    {
        _camera.transform.position = Vector3.Lerp(camPos.position, camPos2.position, Time.deltaTime*player.GetComponent<CarSystem>().currentSpeed);
        Vector3 pos = player.gameObject.transform.position;
        pos.y += 1;
        _camera.transform.LookAt(pos);
    }
}
