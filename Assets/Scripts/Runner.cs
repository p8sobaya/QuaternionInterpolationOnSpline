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

    public BezierExample be;
    public VerticesIndicator ve;
    public EnumHowToTurn howToTurn; 
    public float cornerTurnSpeed;

    List<Quaternion> towardsEnds;
    List<Quaternion> towardsStarts;

    int numSegment;

    void Start()
    {
        int n = numSegment = be.points.Count;

        towardsEnds = new List<Quaternion>();
        towardsStarts = new List<Quaternion>();
        for (int i = 0; i < numSegment; i++)
        {
            Quaternion start = Quaternion.Euler(ve.directions[i]);
            Quaternion end = Quaternion.Euler(ve.directions[(i + 1) % n]);
            Quaternion towardsEnd = FindRotation(Quaternion.Euler(ve.directions[(i - 1 + n) % n]), end, 10000);
            Quaternion towardsStart = FindRotation(Quaternion.Euler(ve.directions[(i + 2) % n]), start, 10000);
            towardsEnds.Add(towardsEnd);
            towardsStarts.Add(towardsStart);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        int idxSegment = (int)Mathf.Floor(Time.time) % numSegment;
        float phaseInSegment = Time.time % 1f;


        int n = numSegment;
        int i = idxSegment;

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
            Quaternion towardsEnd = towardsEnds[i];
            Quaternion towardsStart = towardsStarts[i];


            //Quaternion towardsEnd = end * Quaternion.Inverse(Quaternion.Euler(ve.directions[(i - 1 + n) % n]));
            //Quaternion towardsStart = start * Quaternion.Inverse(Quaternion.Euler(ve.directions[(i + 2) % n]));
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

    public Quaternion FindRotation(Quaternion q0, Quaternion q1, int iteration)
    {
        float epsilon = 0.001f;
        Quaternion nowQuat = Quaternion.identity;
        for (int i = 0; i < iteration; i++)
        {
            float delta = 360f / Mathf.Pow(i+1, 1.2f);
            Quaternion initNext1 = nowQuat;
            Quaternion initNext2 = nowQuat;
            Quaternion initNext3 = nowQuat;
            initNext1 *= Quaternion.AngleAxis(delta * Random.value, Random.onUnitSphere);
            initNext2 *= Quaternion.AngleAxis(delta * Random.value, Random.onUnitSphere);
            initNext3 *= Quaternion.AngleAxis(delta * Random.value, Random.onUnitSphere);
            float diff1 = Quaternion.Angle(q1, q0 * initNext1);
            float diff2 = Quaternion.Angle(q1, q0 * initNext2);
            float diff3 = Quaternion.Angle(q1, q0 * initNext3);
            float diff0 = Quaternion.Angle(q1, q0 * nowQuat);
            float minDiff = Mathf.Min(diff1,diff2,diff3,diff0);
            if (minDiff == diff1) nowQuat = initNext1;
            if (minDiff == diff2) nowQuat = initNext2;
            if (minDiff == diff3) nowQuat = initNext3;

            if(i%10==0) Debug1s("        " +i + " : " + nowQuat + "   diff:" + minDiff);

            if (minDiff < epsilon) break;
        }
        return nowQuat;
    }

    public void Debug1s(object s)
    {
        if (Time.frameCount % 30 == 0) Debug.Log("" + Time.frameCount/30 + " : " + s);

    }
}
