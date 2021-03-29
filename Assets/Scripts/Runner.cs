using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Runner : MonoBehaviour
{
    public enum EnumHowToTurn
    {
        Lerp = 0,
        SobayaSpecial = 1
    }

    public VerticesIndicator ve;
    public EnumHowToTurn howToTurn; 
    public float cornerTurnSpeed;

    int numSegment;

    void Start()
    {
        numSegment = ve.directions.Count;

    }

    // Update is called once per frame
    void Update()
    {
        
        int idxSegment = (int)Mathf.Floor(Time.time) % numSegment;
        float phaseInSegment = Time.time % 1f;


        int n = numSegment;
        int i = idxSegment;
        BezierExample be = GetComponent<BezierExample>();

        {
            Vector3 start = be.points[i];
            Vector3 end = be.points[(i + 1) % n];
            Vector3 startTang = be.points[i] + (be.points[(i + 1) % n] - be.points[(i - 1 + n) % n]) * be.cornerVelocity;
            Vector3 endTang = be.points[(i + 1) % n] + (be.points[i] - be.points[(i + 2) % n]) * be.cornerVelocity;

            Vector3 pos11 = (1f - phaseInSegment) * start + phaseInSegment * startTang;
            Vector3 pos12 = (1f - phaseInSegment) * startTang + phaseInSegment * endTang;
            Vector3 pos13 = (1f - phaseInSegment) * endTang + phaseInSegment * end;

            Vector3 pos21 = (1f - phaseInSegment) * pos11 + phaseInSegment * pos12;
            Vector3 pos22 = (1f - phaseInSegment) * pos12 + phaseInSegment * pos13;

            Vector3 pos31 = (1f - phaseInSegment) * pos21 + phaseInSegment * pos22;

            transform.position = pos31;
        }

        {
            Quaternion start = Quaternion.Euler(ve.directions[i]);
            Quaternion end = Quaternion.Euler(ve.directions[(i + 1) % n]);
            Quaternion towardsEnd = end * Quaternion.Inverse(Quaternion.Euler(ve.directions[(i - 1 + n) % n]));
            Quaternion towardsStart = start * Quaternion.Inverse(Quaternion.Euler(ve.directions[(i + 2) % n]));


            Quaternion startTang = Quaternion.SlerpUnclamped( start, start *towardsEnd ,cornerTurnSpeed);
            Quaternion endTang = Quaternion.SlerpUnclamped(end, end * towardsStart, cornerTurnSpeed);

            Quaternion quat11 = Quaternion.Slerp(start, startTang, phaseInSegment);
            Quaternion quat12 = Quaternion.Slerp(startTang, endTang, phaseInSegment);
            Quaternion quat13 = Quaternion.Slerp(endTang, end, phaseInSegment);

            Quaternion quat21 = Quaternion.Slerp(quat11, quat12, phaseInSegment);
            Quaternion quat22 = Quaternion.Slerp(quat12, quat13, phaseInSegment);

            Quaternion quat31 = Quaternion.Slerp(quat21, quat22, phaseInSegment);

            if(howToTurn==0) transform.rotation = Quaternion.Slerp(start, end, phaseInSegment);
            else transform.rotation = quat31;
        }




    }


}
