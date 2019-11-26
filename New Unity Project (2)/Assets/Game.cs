using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private int agentnum;//1チームのエージェントの数
    private int sticknum;//棒の数
    private float rLine = 11;//redチーム陣地
    private float wLine = -11;//whiteチーム陣地
    private float sticklen = 4;//棒の長さ
    public float[,] xy;
    private string[] tm = new string[2] {"red","white"};
    //Agent[] rA;
    //Agent[] wA;
    Stick[] St;
    //Team[] Tm = new Team[2];
    //Map map;
    public GameObject StickPrefab;
    public GameObject AgentPrehab;
    private Stick[] stickList;
    private Agent[] agentList;
    private string owner;

    void Start(){
        xy = new float[sticknum,2];
        init(20,7);
    }

    /*public Agent[] getAgent(string t){//agent取得用
        if(t=="red"){
            return rA;
        }else{
            return wA;
        }
    }*/
    public Stick[] getStick(){//stick取得用
        return St;
    }
    private void numInit(int anum, int snum){
        agentnum = anum;
        sticknum = snum;
    }
    void stInit(){//Stickの初期化関数
        //St = new Stick[sticknum];
        stickList = new Stick[sticknum];
        xy = new float[sticknum,2];
        for(int j=0;j<sticknum;j++){
            GameObject stick = Instantiate(StickPrefab) as GameObject;
            stickList[j] = stick.GetComponent<Stick>();
            stickList[j].init(0f, j, 1);
            xy[j,0] = stickList[j].getX();
            xy[j,1] = stickList[j].getY();
            //St[j] = new Stick();
            //x座標,y座標の決定
            //St[j].init(0f, (-1)*sticknum+1+2*j, sticklen);
        }
    }

    public void updateStick(){
        for(int j=0;j<sticknum;j++){
            owner = stickList[j].getOwner();
            if(owner == "neutral"){
                xy[j,0] = stickList[j].getX();
                xy[j,1] = stickList[j].getY();
            }else{
                xy[j,0] = 100f;
            }
        }
    }
    
    private void agentInit(string t){//Agentの初期化関数
        agentList = new Agent[10];
        for(int j = 0; j<10; j++){
            if(t == "red"){
            GameObject agent = Instantiate(AgentPrehab) as GameObject;
            agentList[j] = agent.GetComponent<Agent>();
            agentList[j].init(11.5f,j,2,0.1f,1,"red");
            }else{
            GameObject agent = Instantiate(AgentPrehab) as GameObject;
            agentList[j] = agent.GetComponent<Agent>();
            agentList[j].init(-11.5f,j,1,0.1f,1,"white"); 
            }
        }
    }
        /*Agent[] a;
        float startX;
        float pow;
        if(t=="red"){
            rA = new Agent[agentnum];
            a = rA;
            startX = rLine;
            pow = 0.2f;
        }else /*if(t=="white")*//*{
            wA = new Agent[agentnum];
            a = wA;
            startX = wLine;
            pow = 0.1f;
        }
        for(int j=0;j<agentnum;j++){
            a[j] = new Agent();
            //座標,力,速度,大きさなどの決定
            a[j].init(startX, (-0.25f)*(agentnum-1f)+0.5f*j, pow, 0.1f, 0.5f, t);
        }
    }*/
    /*void teamInit(){//teamの初期化関数
        for(int j=0;j<2;j++){
            Tm[j].init(tm[j]);
        }
    }*/
    void init(int anum, int snum){//初期化を行う関数
        //map = new Map();
        //map.mapInit(anum, snum);//Mapの初期化
        numInit(anum, snum);//エージェントや棒の数の初期化
        stInit();//Stickの初期化
        for(int k=0;k<2;k++){
            agentInit(tm[k]);//Agentの初期化
        }
        //teamInit();//Teamの初期化
    }/*
    void objMove(){//各objの移動を行う
        for(int j=0;j<2;j++){//Agent(Team)についてdecideAction()
            Tm[j].decideAction();
        }
        for(int j=0;j<sticknum;j++){//StickについてdecideAction()
            St[j].decideAction();
        }
        //各AgentについてStickを動かしているor移動中のどちらかチェック
        //移動中Agentの座標についてupdate()
        //Stickとそれを動かしているAgentの座標についてupdatePosition()
        //座標の重複の有無を確認
    }*/
    /*
    public void oneGame(int anum, int snum){
        //1ゲームで行う作業をこの関数内に記述
        int tlimit = 600;//時間制限
        init(anum, snum);//初期化
        for(int i=0;i<tlimit;i++){//以下の作業を制限時間内で繰り返し
            objMove();
            if(true/*すべての棒の所持者がredかwhite*//*){
                break;
            }
        }
    }*/
}
