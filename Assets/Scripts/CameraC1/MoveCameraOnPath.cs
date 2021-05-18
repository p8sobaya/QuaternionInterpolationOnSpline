using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraOnPath : MonoBehaviour
{
    public enum PathShape
    {
        c1_straight = 10,
        c1_doubleCircle = 11,
        c1_doubleParabola = 12,

        c0_doubleCircle = 0,

        c2_double3rdFuncs = 20

    }

    public GameObject floorObject;
    public PathShape shape;

    public bool showFloor;
    public float sizeOfMovement = 1f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(shape==PathShape.c1_straight) PathC1Straight(Time.time, gameObject);
        if (shape == PathShape.c1_doubleCircle) PathC1DoubleCircle(Time.time, gameObject);
    }

    public void PathC1Straight(float t, GameObject g)
    {
        float total = 4f + 2f * Mathf.PI;
        float phase = t % total;
        if (phase < 2f)
        {
            g.transform.position = new Vector3(1f, 0f, -1f) + phase * Vector3.forward;
            
            g.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (phase < 2f + Mathf.PI)
        {
            float angle = (phase - 2f);
            g.transform.position = new Vector3(0f, 0f, 1f) + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            g.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);
        }
        else if (phase < 4f + Mathf.PI)
        {
            g.transform.position = new Vector3(-1f, 0f, 1f) - (phase - 2f - Mathf.PI) * Vector3.forward;
            g.transform.localEulerAngles = new Vector3(0f, 180f, 0f);

        }
        else
        {
            float angle = (phase - 4f);
            g.transform.position = new Vector3(0f, 0f, -1f) + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            g.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);

        }

        PathAdjustment(g);
    }

    public void PathC1DoubleCircle(float t, GameObject g)
    {
        float total = Mathf.PI * 3f;
        float phase = t % total;
        float sq2 = 1.41421356f;
        float angle;
        if (phase < 1f * Mathf.PI)
        {
            angle = phase*0.5f + 0.25f * Mathf.PI;
            g.transform.position = new Vector3(0f, 0f, -sq2 * 0.5f) + 2f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            g.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);
        }
        else if (phase < 1.5f * Mathf.PI)
        {
            angle = (phase - 1f * Mathf.PI) + 0.75f * Mathf.PI;
            g.transform.position = new Vector3(-sq2 * 0.5f, 0f, 0f) + 1f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            g.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);
        }
        else if (phase < 2.5f * Mathf.PI)
        {
            angle = (phase - 1.5f * Mathf.PI) * 0.5f + 1.25f * Mathf.PI;
            g.transform.position = new Vector3(0f, 0f, sq2 * 0.5f) + 2f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            g.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);
        }
        else
        {
            angle = (phase - 2.5f * Mathf.PI) + 1.75f * Mathf.PI;
            g.transform.position = new Vector3(sq2 * 0.5f, 0f, 0f) + 1f * new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
            g.transform.localEulerAngles = new Vector3(0f, -angle * Mathf.Rad2Deg, 0f);
        }
        Debug.Log("phase/pi: "+  phase / Mathf.PI);
        Debug.Log("angle: "+angle);
        PathAdjustment(g);

    }

    void PathAdjustment(GameObject g)
    {
        g.transform.position *= sizeOfMovement;

        Material m = floorObject.GetComponent<MeshRenderer>().material;
        m.SetInt("mode", (int)shape);
        m.SetFloat("_size", sizeOfMovement);
    }
}
