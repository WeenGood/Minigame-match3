using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxSettings : MonoBehaviour {


    public Material[] blocks;
    public int index = 0;
    public int cx, cy;

    public GameObject myMainGame;

	// Use this for initialization
	void Start () {
        randomColor();
        changeColor();
    }
	
	// Update is called once per frame
	void Update () {
        changeColor();
	}

    public void randomColor()
    {
        index = Random.Range(1, blocks.Length);
    }
    void changeColor()
    {
        if(index<blocks.Length)
        {
            Renderer r = this.GetComponent("Renderer") as Renderer;
            r.material = blocks[index];
        }
    }

    
    void OnMouseDown()
    {
        loadGame map = myMainGame.GetComponent<loadGame>();
        map.searchRow(cx,cy);
    }
}
