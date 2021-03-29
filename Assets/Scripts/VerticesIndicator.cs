using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class VerticesIndicator : MonoBehaviour
{
    public GameObject directionObject;
    public BezierExample bezierScript;

    public float indicatorSize;

    public List<Vector3> directions;

    int numObjects;
    List<GameObject> objects;

    void Start()
    {
        objects = new List<GameObject>();
        foreach (var i in GetComponentsInChildren<DirectionObject>())
        {
            DestroyImmediate(i.gameObject);
        }
        numObjects = Mathf.Min(bezierScript.points.Count, directions.Count);
        for (int i = 0; i < numObjects; i++)
        {
            GameObject obj = Instantiate(directionObject);
            obj.transform.localScale = Vector3.one * indicatorSize;
            obj.transform.position = bezierScript.points[i];
            obj.transform.localEulerAngles = directions[i];
            obj.transform.parent = gameObject.transform;
            objects.Add(obj);
        }
    }


    void Update()
    {
        for (int i = 0; i < numObjects; i++)
        {
            GameObject obj = objects[i];
            obj.transform.localScale = Vector3.one * indicatorSize;
            obj.transform.position = bezierScript.points[i];
            obj.transform.localEulerAngles = directions[i];
            obj.transform.parent = gameObject.transform;
        }
    }


}
