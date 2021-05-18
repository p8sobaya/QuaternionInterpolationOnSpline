using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeManyCubes : MonoBehaviour
{
    public GameObject goOrigin;
    public int num;

    List<GameObject> golist;
    // Start is called before the first frame update
    void Start()
    {
        golist = new List<GameObject>();
        for (int i = 0; i < num; i++)
        {
            GameObject go = Instantiate(goOrigin);
            go.transform.position = Random.onUnitSphere * Random.Range(20f,50f);
            go.transform.localScale = new Vector3(1f,Random.Range(1f,10f),1f);
            golist.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
