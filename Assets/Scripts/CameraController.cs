using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float offset = 3;

    [SerializeField] private Camera camera;

    [SerializeField] private float cameraSpeed = 0.1f;
    [SerializeField] private int repositionMultiplier = 3;
    [SerializeField] private float distanceThreshhold = 0.1f;
    // Start is called before the first frame update
    void Awake()
    {
      if (camera == null)
      {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
      }
    }

    public void Reposition(Vector2Int size)
    {

      Vector3 newPosition = CalculateNewPosition(size);
        
       StartCoroutine(LerpToPosition(newPosition));
    }

    private Vector3 CalculateNewPosition(Vector2Int size)
    {
      int xAxis = size.y * repositionMultiplier;
      
      int zAxis = size.x * repositionMultiplier;

      int yAxis = (xAxis + zAxis);

      if (zAxis > xAxis)
      {
        yAxis = zAxis * (repositionMultiplier - 1);
      }
      
      return new Vector3(xAxis, yAxis, zAxis - offset);
    }

    private IEnumerator LerpToPosition(Vector3 position)
    {
      while (Vector3.Distance(camera.transform.position, position) > distanceThreshhold)
      {
       Vector3 lerpPos = Vector3.Lerp(camera.transform.position, position, cameraSpeed);
       camera.transform.position = lerpPos;
        yield return null;
      }

      yield return null;

    } 
}
