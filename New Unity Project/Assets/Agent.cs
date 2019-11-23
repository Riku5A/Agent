﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private float x, y, v; //座標、速さ
    private float[] direction; //向き
    private float[] distance; //移動距離
    private float pow, max_v, size; //力と最大速度と大きさ
    private float[,] stickPos;
    private string team; //チーム情報
    private bool pull; //Agentの状態
    private GameObject pullStick;
    private GameObject map;
    private Map mapS;
    private Stick stick;
    //public GameObject AgentPrehab;

    void Start()
    {
        map = GameObject.Find("map");
        mapS = map.GetComponent<Map>();
        //x = 0.1f;
        //y = 0.1f;
        direction = new float[2];
        distance = new float[2];
        //direction[0] = 1f;
        //direction[1] = 1f; 
        //v = 0.1f;
        //transform.position = new Vector3(x, y);*/
    }

    // Update is called once per frame
    void Update()
    {
        updatePosition();
    }

    public void init(float x, float y, float pow, float max_v, float size, string team)
    {
        //GameObject agent = Instantiate(Resources.Load("AgentPrehab")) as GameObject;
        this.x = x;
        this.y = y;
        this.pow = pow;
        this.max_v = max_v;
        this.size = size;
        this.team = team;
        pull = false;
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
        float[] stickMin;
        stickPos = mapS.getStickXY();
        stickNum = stickPos.GetLength(0);
        stickMin = new float[2];
        stickMin[0] = stickPos[0,0] - x;
        stickMin[1] = stickPos[0,1] - y;
        for(i = 1; i < stickNum; i++){
            if(stickMin[0]*stickMin[0] + stickMin[1]*stickMin[1] > stickPos[i,0]*stickPos[i,0] + stickPos[i,1] * stickPos[i,1]){
                stickMin[0] = stickPos[i,0];
                stickMin[1] = stickPos[i,1];
            }
        }
        direction[0] = stickMin[0];
        direction[1] = stickMin[1];
    }

    public void updatePosition()
    {
        if(pull == false){
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
            pull = true;
            pullStick = col.gameObject;
            stick = pullStick.GetComponent<Stick>();
            //Debug.Log("stick!!");
        }
    }
/*
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.name == "stick"){
            pull = true;
            pullStick = col.gameObject;
            stick = pullStick.GetComponent<Stick>();
            Debug.Log("stick!!");
        }
    }
    */
}
