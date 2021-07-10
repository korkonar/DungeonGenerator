using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveScript:MonoBehaviour
{
    public float currSize;
    public float currRatio;
    public int corners;
    public float boudingSize;
    public float boundingRatio;
    public float fitScore;
    public float qualityScore;
    public List<int>adj;

    public SaveScript(){
        adj=new List<int>();
    }
            
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
