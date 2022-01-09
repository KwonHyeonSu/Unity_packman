using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

public class Pojang_Maker : MonoBehaviour
{
    
    [Multiline(10)]
    [Tooltip("맵에서 벽이 생기길 원치 않는 부분을 \",\"로 구분하여 넣어준다. ")]
    public string forException = "";
    string value = "";
    public GameObject White;
    public GameObject Black;

    void Start()
    {
        StartCoroutine(Mapping("Pojang", 41, 32));
    }

    //by 현수 - 맵 전체의 가로세로 길이를 파라미터로 넣어준다.
    List<Tuple<int, int>> ExceptionPosList = new List<Tuple<int, int>>();
    IEnumerator Mapping(string name, int x, int y)
    {
        Debug.Log("매핑 시작");

        //Exception 인덱싱
        ExceptionPosList.Clear();
        string[] indexing = forException.Split('\n');
        foreach(var a in indexing)
        {
            string [] s = a.Split(',');
            Tuple<int, int> tmp = new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1]));
            if(!ExceptionPosList.Contains(tmp))
            {
                ExceptionPosList.Add(new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1])));
            }
        
        }

        GameObject MappingObject = new GameObject(); //mapping visualize 오브젝트들의 부모 오브젝트
        bool flag = false;
        for(int i=0;i<x;i++)
        {
            for(int j=0;j<y;j++){
                flag = false;
                //예외적인 부분이 있는지 확인
                if(ExceptionPosList.Contains(new Tuple<int, int>(i,j)))
                {
                    flag = true;
                }

                bool cols = Physics2D.OverlapCircle(new Vector3(i, j, 0), 0.5f);
                if(cols || flag)
                {
                    //ebug.LogWarning(i + ", " + j + "에 벽 발견");
                    MakeObject(i, j, 1, MappingObject);
                    value += i + " " + j + " " + 1 + " ";
                }
                else{
                    //Debug.Log(i + ", " + j + "에 아무것도 없음");
                    MakeObject(i, j, 0, MappingObject);
                    value += i + " " + j + " " + 0 + " ";
                }
                //yield return new WaitForSecondsRealtime(0.02f);
                
            }
        }
        
        CreateTextFile(name, value);
        yield return null;
    }

    //by 현수 - MappingObject의 자식으로 생성한다. - 22.01.09
    void MakeObject(int i, int j, int v, GameObject MappingObject)
    {
        GameObject Obj = null;
        if(v == 1){
            Obj = Instantiate(Black, new Vector3(i,j,0), Quaternion.identity);
        }
        else if(v == 0){
            Obj = Instantiate(White, new Vector3(i,j,0), Quaternion.identity);
            
        }
        string s = i + ", " + j + "\t->" + v;
        Obj.name = s;

        //부모 오브젝트로 수정
        Obj.transform.parent = MappingObject.transform;
    }

    void CreateTextFile(string name, string value)
    {
        string path = "Assets/TextMap/";
        path += name + ".txt";

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(value);
        writer.Close();

        Debug.Log(name + ".txt 가 만들어져습니다.");
    }

}
