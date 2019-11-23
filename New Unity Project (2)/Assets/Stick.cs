using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    private float x, y, v; //座標、速さ
    public float pow, redpow, whitepow;
    private float redmax, whitemax;
    private float[] direction; //向き
    private float[] distance; //移動距離
    private float longth; //長さ
    private string owner = "neutral"; //所有者(red, white, neutral)
    private string team;
    private Map mapS;
    private Stick stick;
    private GameObject map;
    private GameObject agentN;
    private Agent agent;
    //public GameObject stickPrehab;

    void Start()
    {
        map = GameObject.Find("map");
        mapS = map.GetComponent<Map>();
        //x = 0f;
        //y = 2f;
        direction = new float[2];
        distance = new float[2];
        //direction[0] = 1f;
        //direction[1] = 0f; 
        //v = 0.1f;
        //transform.position = new Vector3(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        decideAction();
        updatePosition();
    }

    public void init (float x, float y, float length)
    {
        //GameObject stick = Instantiate(Resources.Load("stickPrehab")) as GameObject;
        this.x = x;
        this.y = y;
        this.longth = longth;
    }
/*
    public int judge()
    {
        if(){
            return 1;
        }else
        {
            return 0;
        }
    }
*/

    public void decideAction()
    {
        v = 0.05f;
        direction[1] = 0;
        if(redpow > whitepow){
            direction[0] = 1;
        }else if(redpow < whitepow){
            direction[0] = -1;
        }else if(redpow == whitepow){
            direction[0] = 0;
        }
    }

    public void updatePosition()
    {
        distance[0] = direction[0] * v;
        distance[1] = direction[1] * v;
        x = x + distance[0];
        y = y + distance[1];
        transform.position = new Vector3(x, y);
    }

    public float getX(){
        return x;
    }

    public float getY(){
        return y;
    }

    public float getV(){
        return v;
    }

    public float[] getDirect(){
        return direction;
    }

    public string getOwner(){
        return owner;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Agent"){
            agentN = col.gameObject;
            agent = agentN.GetComponent<Agent>();
            team = agent.getTeam();
            pow = agent.getPow();

            if(team == "red"){
                redpow = redpow + pow;
            }else{
                whitepow = whitepow + pow;
            }
        }
        //Debug.Log("stick!!");
    }
}
