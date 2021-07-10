using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RoomsUIScript : MonoBehaviour
{
    public GameObject toggle;
    public GameObject room;
    public Transform Content;
    public Dropdown typeSelect;
    public Transform arrayPanel;
    public static List<GameObject> roomsGO;
    private ToggleGroup toggles;
    public  static int id=0;

    // Start is called before the first frame update
    void Start()
    {
        roomsGO=new List<GameObject>();
        toggles=GetComponent<ToggleGroup>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRooms(){
        foreach(GameObject g in roomsGO){
            string ids = g.transform.GetChild(7).GetComponent<Text>().text;
            ids= ids.Substring(3);
            int id=int.Parse(ids); 
            HouseGraph.allRooms[id].minSize=float.Parse(g.transform.GetChild(0).GetComponent<InputField>().text);
            HouseGraph.allRooms[id].maxSize=float.Parse(g.transform.GetChild(1).GetComponent<InputField>().text);
            HouseGraph.allRooms[id].minRatio=float.Parse(g.transform.GetChild(2).GetComponent<InputField>().text);
            HouseGraph.allRooms[id].maxRatio=float.Parse(g.transform.GetChild(3).GetComponent<InputField>().text);
        }
        int child=0;
    }

    public void AddRoom(){
        roomsGO.Add(Instantiate(room, Content));
        string name=typeSelect.GetComponentInChildren<Text>().text;
        roomsGO[roomsGO.Count-1].transform.GetChild(4).GetComponent<Toggle>().group=toggles;
        roomsGO[roomsGO.Count-1].transform.GetChild(5).GetComponent<Text>().text=name;
        roomsGO[roomsGO.Count-1].transform.GetChild(6).GetComponent<Text>().text="Type:"+typeSelect.value;
        roomsGO[roomsGO.Count-1].transform.GetChild(7).GetComponent<Text>().text="ID:"+id;
        HouseGraph.allRooms.Add(id,new Room(0,0,0,0,name,id++,(Room.roomType)typeSelect.value));


    }

    public void showHide(){
        if(transform.position.x==-110.0f){
            transform.DOMoveX(110.0f,0.5f);
            toggle.transform.GetChild(0).DORotate(new Vector3(0,0,0),0.5f);
        }else if(transform.position.x==110.0f){
            transform.DOMoveX(-110.0f,0.5f);
            toggle.transform.GetChild(0).DORotate(new Vector3(0,0,90),0.5f);
        }
    }
}
