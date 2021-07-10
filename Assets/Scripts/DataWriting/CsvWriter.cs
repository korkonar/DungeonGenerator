using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CsvWriter : MonoBehaviour
{
    public static string location="C:/Users/BigPC/Desktop/GoodResults/csv/";

    public class QualityRecords{
        public string Type;
        public int ID;
        public float sizeAccepted;
        public int connections;
        public float qualityScore;

        public List<roomQuality> rooms;
    }

    public class roomQuality{
        public int ID;
        public int size;
        public float ratio;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void writeFileTESTING(){
        List<QualityRecords> records = new List<QualityRecords>();
        QualityRecords quality=new QualityRecords{Type="3rooms",ID=0,sizeAccepted=1.5f,connections=2,qualityScore=10.0f};
        quality.rooms=new List<roomQuality>();
        quality.rooms.Add(new roomQuality{ID=0,size=1,ratio=1.5f});
        quality.rooms.Add(new roomQuality{ID=1,size=2,ratio=1.5f});
        quality.rooms.Add(new roomQuality{ID=2,size=4,ratio=1.5f});
        records.Add (quality);

        var writer = new StreamWriter(location+"file.csv");

        writer.WriteLine("Type, ID, sizeAccepted, connections");
        foreach(QualityRecords q in records){
            writer.WriteLine(q.Type+", "+q.ID+", "+q.sizeAccepted+", "+q.connections+","+q.qualityScore);
            writer.WriteLine(",ID, size, ratio");
            foreach(roomQuality r in q.rooms){
                writer.WriteLine(","+r.ID+", "+r.size+", "+r.ratio);
            }
        }
        
        writer.Close();
    }
}
