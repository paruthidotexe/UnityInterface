using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarGraphData : MonoBehaviour
{
    public Material[] materials;
    public MeshRenderer mr;
    int curBarType = 0;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void Init(int barVal)
    {
        curBarType = barVal;
        transform.localScale = new Vector3(1, (curBarType + 1) * 3, 1);
        mr.material = materials[curBarType];
    }
}
