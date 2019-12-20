using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    public float x, y, v; //座標、速さ
    public float pow, redpow, whitepow;
    public float sumpow;
    public int No; //棒の番号
    private float redmax, whitemax;
    private float[] direction; //向き
    private float[] distance; //移動距離
    private float longth; //長さ
    public string owner = "neutral"; //所有者(red, white, neutral)
    private string team;
    private Game game;
    private Stick stick;
    private GameObject map;
    private GameObject agentN;
    private Agent agent;
    public float speed = 0.01f;
    //public GameObject stickPrehab;

    void Start()
    {
        map = GameObject.Find("map");
        game = map.GetComponent<Game>();
        direction = new float[2];
        distance = new float[2];
    }

    void Update()
    {
        decideAction();
        updatePosition();
        judge();
    }

    public void init (float x, float y, float length)
    {
        this.x = x;
        this.y = y;
        this.longth = longth;
        this.owner = "neutral";
    }

    public void judge()
    {
        if(x > 12f){
            owner = "red";
            x = 12f;
        }else if(x < -12f)
        {
            owner = "white";
            x = -12f;
        }
    }

    public void decideAction()
    {
        direction[1] = 0;
        if(redpow > whitepow){
            direction[0] = 1;
            v = redpow - whitepow;
        }else if(redpow < whitepow){
            direction[0] = -1;
            v = whitepow - redpow;
        }else if(redpow == whitepow){
            direction[0] = 0;
        }
        sumpow = redpow - whitepow;
        v = v * speed;
        if(v > 0.06){
            v = 0.06f;
        }
        if(redpow == 0){
            v = 0.06f;
        }
        if(whitepow == 0){
            v = 0.06f;
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
        if(owner == "neutral"){
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
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(owner == "neutral"){
            if(col.tag == "Agent"){
                agentN = col.gameObject;
                agent = agentN.GetComponent<Agent>();
                team = agent.getTeam();
                pow = agent.getPow();
                if(team == "red"){
                    redpow = redpow - pow;
                }else{
                    whitepow = whitepow - pow;
                }
            }
        }
    }
}
