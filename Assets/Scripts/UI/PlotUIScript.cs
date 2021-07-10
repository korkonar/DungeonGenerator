using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PlotUIScript : MonoBehaviour
{
    public GameObject toggle;
    public GameObject plotVertex;
    private int plotSize;
    public Text plotSizeText;
    private List<GameObject> vertexInput;
    private DrawPlot plotInf;
    public Text debugtext;
    public GameObject RoomsCont;
    // Start is called before the first frame update
    void Start()
    {
        vertexInput=new List<GameObject>();
        plotInf=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<DrawPlot>();
        plotSize=plotInf.plot.Length;
        transform.GetChild(1).GetComponentInChildren<InputField>().text=""+plotSize;
        for(int i=0;i<plotSize;i++){
            GameObject v=Instantiate(plotVertex,this.transform.GetChild(2));
            v.transform.GetChild(0).GetComponent<InputField>().text=""+plotInf.plot[i].x;
            v.transform.GetChild(1).GetComponent<InputField>().text=""+plotInf.plot[i].y;
            vertexInput.Add(v);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVertexInputs(){
        string size= plotSizeText.text;
        int sizeInt;
        int.TryParse(size,out sizeInt);
        if(sizeInt!=null){
            plotSize=sizeInt;
            vertexInput.Clear();
            for(int i=this.transform.GetChild(2).childCount-1;i>=0;i--){
                Destroy(this.transform.GetChild(2).GetChild(i).gameObject);
            }
            for(int i=0;i<plotSize;i++){
                GameObject v=Instantiate(plotVertex,this.transform.GetChild(2));
                vertexInput.Add(v);
            }
        }
    }

    public void UpdatePlot(){
        bool correctVal=true;

        Vector2[] newPlot=new Vector2[plotSize];

        int i=0;
        foreach(GameObject g in vertexInput){
            string sx=g.transform.GetChild(0).GetComponentInChildren<Text>().text;
            string sy=g.transform.GetChild(1).GetComponentInChildren<Text>().text;

            float x,y;
            float.TryParse(sx,out x);
            float.TryParse(sy,out y);
            if(x!=null && sy!=null){
                newPlot[i++]=new Vector2(x,y);
            }else{
                correctVal=false;
                debugtext.text="Vector values incorrect";
                break;
            }
            print(i);
        }
        if(correctVal){
            plotInf.plot=newPlot;
            plotInf.UpdatePlot();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GridDrawingScript>().UpdateGrid();
            HouseGraph.allRooms.Clear();
            RoomsUIScript.id=0;
            foreach(GameObject g in RoomsUIScript.roomsGO){
                Destroy(g.gameObject);
            }
            RoomsUIScript.roomsGO.Clear();
            StartCoroutine(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CellGridScript>().LateStart(0.1f));
        }
    }

    public void showHide(){
        if(transform.position.x==-110.0f){
            transform.DOMoveX(110.0f,0.5f);
            toggle.transform.GetChild(0).DORotate(new Vector3(0,0,0),0.5f);
            RoomsCont.transform.DOMoveY(600, 0.5f);
        }else if(transform.position.x==110.0f){
            transform.DOMoveX(-110.0f,0.5f);
            toggle.transform.GetChild(0).DORotate(new Vector3(0,0,90),0.5f);
            RoomsCont.transform.DOMoveY(960, 0.5f);
        }
    }
}
