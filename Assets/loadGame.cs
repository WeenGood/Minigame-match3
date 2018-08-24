using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadGame : MonoBehaviour {

    public GameObject box;
    // Use this for initialization

    static int col = 12, row = 6;

    GameObject[,] map = new GameObject[col, row];

    struct point
    {
        public int x, y;
    }

    void createGameArea()
    {
        float dx = 1.1f, dy = 1.1f;
        Vector3 myPose = new Vector3(-6, -2, -3);
        for (int yy = 0; yy < row; yy++)
        {
            for (int xx = 0; xx < col; xx++)
            {
                map[xx,yy] = Instantiate(box, myPose, Quaternion.identity) as GameObject;
                myPose.x += dx;
                boxSettings cxcy = map[xx, yy].GetComponent<boxSettings>();
                cxcy.cx = xx;
                cxcy.cy = yy;

                cxcy.myMainGame = this.gameObject;
            }
            myPose.y += dy;
            myPose.x = -6;
        }
    }


    void Start () {
        createGameArea();
	}

    // Update is called once per frame
    

    void Update () {
        boxSettings cxcy = new boxSettings();
        point p;
        for (int x = 0; x < col; x++)
            for (int y = 0; y < row; y++)
            {
                cxcy = map[x, y].GetComponent<boxSettings>();
                p.x = x; p.y = y;
                testBlockMatch(p, cxcy.index);
            }
        MoverDown();
    }

    List<point> around = new List<point>();
    boxSettings save = new boxSettings();
   
    public void searchRow(int x, int y)
    {
        boxSettings cxcy = map[x, y].GetComponent<boxSettings>();
        point p, p2;
        int flag = 0, flag2 = 0;
        p.x = x; p.y = y;
        p2.x = save.cx;
        p2.y = save.cy;
        int find = around.IndexOf(p);
        if(find == -1)
        {
            around = getAroundBlocks(p);
            save = cxcy;
        }
        else
        {
            int id = save.index;
            save.index = cxcy.index;
            cxcy.index = id;
            flag = testBlockMatch(p, cxcy.index);
            flag2 = testBlockMatch(p2, save.index);
            if (flag == 1 || flag2 ==1)
            {
                id = save.index;
                save.index = cxcy.index;
                cxcy.index = id;
            }
            around.Clear();
        }
    }

    int testBlockMatch(point coord, int idColor)
    {
        int flag = 0;
        List<point> test = indexListMatch(coord, idColor);
        if (test.Count == 0) flag = 1;
        clearBlockIndex(test);
        test.Clear();
        return flag;
    }

    List<point> indexListMatch(point coord, int index)
    {
        List<point> p = new List<point>();
        List<point> pY = new List<point>();
        List<point> ppY = new List<point>();
        point t;
        int indexColor = map[coord.x, coord.y].GetComponent<boxSettings>().index;
        int indexColor2;
        int countX = 0;
        int countY = 0;
        for (int i = 1; ((coord.x + i) < col); i++)
        {
            indexColor2 = map[coord.x + i, coord.y].GetComponent<boxSettings>().index;
            if (indexColor == indexColor2)
            {
                countX++;
                t.x = coord.x + i;
                t.y = coord.y;
                p.Add(t);
                ppY.Add(t);
            }
            else break;
        }
        for (int i = 1; ((coord.x - i) >= 0); i++)
        {
            indexColor2 = map[coord.x - i, coord.y].GetComponent<boxSettings>().index;
            if (indexColor == indexColor2)
            {
                countX++;
                t.x = coord.x - i;
                t.y = coord.y;
                p.Add(t);
                ppY.Add(t);
            }
            else break;
        }

        for (int i = 1; ((coord.y + i) < row); i++)
        {
            indexColor2 = map[coord.x, coord.y + i].GetComponent<boxSettings>().index;
            if (indexColor == indexColor2)
            {
                countY++;
                t.x = coord.x;
                t.y = coord.y + i;
                pY.Add(t);
                ppY.Add(t);
            }
            else break;
        }
        for (int i = 1; ((coord.y - i) >= 0); i++)
        {
            indexColor2 = map[coord.x, coord.y - i].GetComponent<boxSettings>().index;
            if (indexColor == indexColor2)
            {
                countY++;
                t.x = coord.x;
                t.y = coord.y - i;
                pY.Add(t);
                ppY.Add(t);
            }
            else break;
        }
        t.x = coord.x;
        t.y = coord.y;
        p.Add(t);
        pY.Add(t);
        ppY.Add(t);
        if (countY >= 2 && countX >= 2) return ppY;
        if (countX >= 2) return p;
        if (countY >= 2) return pY;
        List<point> zero = new List<point>();
        return zero;
    }

    void clearBlockIndex(List<point> test)
    {
        foreach (point clear in test)
        {
            map[clear.x, clear.y].GetComponent<boxSettings>().index = 0;
        }
    }

    List<point> getAroundBlocks(point coord)
    {
        List<point> p = new List<point>();
        point t;
        if ((coord.x + 1) < col)
        {
            t.x = coord.x + 1;
            t.y = coord.y;
            p.Add(t);
        }
        if((coord.y + 1) < row)
        {
            t.x = coord.x;
            t.y = coord.y + 1;
            p.Add(t);
        }
        if((coord.x - 1) >= 0)
        {
            t.x = coord.x - 1;
            t.y = coord.y;
            p.Add(t);
        }
        if((coord.y - 1) >= 0)
        {
            t.x = coord.x;
            t.y = coord.y - 1;
            p.Add(t);
        }
        return p;
    }

    void MoverDown()
    {
        for(int y =0; y< row; y++)
            for(int x =0; x<col; x++)
            if(map[x,y].GetComponent<boxSettings>().index == 0)
                {
                    int dy = y + 1;
                    if(dy < row)
                    {
                        map[x, y].GetComponent<boxSettings>().index = map[x, dy].GetComponent<boxSettings>().index;
                        map[x, dy].GetComponent<boxSettings>().index = 0;
                    }
                    else
                    {
                        map[x, y].GetComponent<boxSettings>().randomColor();
                    }
                }
    }
}
