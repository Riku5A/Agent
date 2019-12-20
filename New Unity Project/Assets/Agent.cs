using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float x, y, v; //座標、速さ
    public float[] direction; //向き
    private float[] distance; //移動距離
    public float pow, max_v, size; //力と最大速度と大きさ
    public float[,] stickPos;
    public float[] stickMin;
    public float[] lastPos;
    public float[] tagestick;
    public int stNo; //自分の持つ棒のNo
    public string team; //チーム情報
    public bool pull; //Agentの状態
    private GameObject pullStick;
    private GameObject map;
    private GameObject Team;
    private Game game;
    private TeamController tController;
    private Stick stick;
    private string owner;
    public bool last;
    public bool support;
    private bool touch;
    //public GameObject AgentPrehab;

    void Start()
    {
        map = GameObject.Find("map");
        game = map.GetComponent<Game>();
        Team = GameObject.Find("Team");
        tController = Team.GetComponent<TeamController>();
        direction = new float[2];
        distance = new float[2];
        stickMin = new float[2];
        direction[0] = -1f;
        //direction[1] = 1f; 
        tagestick = new float[2];
        v = 0.1f;
    }

    void Update()
    {
        decideAction();
        updatePosition();
        if(stNo == 0){
            pull = false;
        }
        if(touch == true){
            owner = stick.getOwner();
            if(owner != "neutral"){
                pull = false;
            }
        }
    }

    public void init(float x, float y, float pow, float max_v, float size, string team)
    {
        this.x = x;
        this.y = y;
        this.pow = pow;
        this.max_v = max_v;
        this.size = size;
        this.team = team;
        pull = false;
        transform.position = new Vector3(x, y);
        v = max_v;
        if(team == "red"){
            GetComponent<Renderer>().material.color = Color.red;
        }
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

    public float getPow(){
        return pow;
    }

    public float getMax(){
        return max_v;
    }

    public string getTeam(){
        return team;
    }

    public bool getpull(){
        return pull;
    }

    public void decideAction()
    {
        int stickNum, i;
        float p;
        stickPos = game.getStickXY();
        stickNum = stickPos.GetLength(0);
        if(stickNum > 0){
            if(last == false){
                if(pull == false){
                    if(support == true){
                        stickPos = tController.supStickPos;
                        stickNum = stickPos.GetLength(0);
                    }
                    stickMin[0] = stickPos[0,0] - x;
                    stickMin[1] = stickPos[0,1] - y;
                    for(i = 1; i < stickNum; i++){
                        if(stickMin[0]*stickMin[0] + stickMin[1]*stickMin[1] > (stickPos[i,0] - x)*(stickPos[i,0] - x) + (stickPos[i,1] - y) * (stickPos[i,1] - y)){
                            stickMin[0] = stickPos[i,0] - x;
                            stickMin[1] = stickPos[i,1] - y;
                            tagestick[0] = stickPos[i,0];
                            tagestick[1] = stickPos[i,1];
                        }
                    }
                    p = (float)System.Math.Sqrt(stickMin[0]*stickMin[0] + stickMin[1] * stickMin[1]);
                    direction[0] = stickMin[0]/p;
                    direction[1] = stickMin[1]/p;
                }
            }else if(last == true){
                lastPos = tController.minPos;
                stickMin[0] = lastPos[0] - x;
                stickMin[1] = lastPos[1] - y;
                p = (float)System.Math.Sqrt(stickMin[0]*stickMin[0] + stickMin[1] * stickMin[1]);
                direction[0] = stickMin[0]/p;
                direction[1] = stickMin[1]/p;
            }
        }
    }

    public void updatePosition()
    {
        if(pull == false){
            v = max_v;
            distance[0] = direction[0] * v;
            distance[1] = direction[1] * v;
            x = x + distance[0];
            y = y + distance[1];
        }else{
            v = stick.getV();
            direction = stick.getDirect();
            distance[0] = direction[0] * v;
            distance[1] = direction[1] * v;
            x = x + distance[0];
            y = y + distance[1];
        }
        transform.position = new Vector3(x, y);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag == "Stick"){
            pullStick = col.gameObject;
            stick = pullStick.GetComponent<Stick>();
            touch = true;
            owner = stick.getOwner();
            if(owner == "neutral")
            {
                pull = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.tag == "Stick"){
            pullStick = col.gameObject;
            stick = pullStick.GetComponent<Stick>();
            owner = stick.getOwner();
            stNo = stick.No;
            if(owner == "neutral"){
                pull = true;
            }
            if(owner != "neutral"){
                pull = false;
                v = max_v;
            }
        }
    }

}
