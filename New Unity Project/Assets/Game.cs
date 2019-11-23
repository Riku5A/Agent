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
    private string[] tm = new string[2] {"red","white"};
    Agent[] rA;
    Agent[] wA;
    Stick[] St;
    Team[] Tm = new Team[2];
    Map map;
    public GameObject StickPrehab;
    public GameObject AgentPrehab;

    public Agent[] getAgent(string t){//agent取得用
        if(t=="red"){
            return rA;
        }else{
            return wA;
        }
    }
    public Stick[] getStick(){//stick取得用
        return St;
    }
    private void numInit(int anum, int snum){
        agentnum = anum;
        sticknum = snum;
    }
    private void stInit(){//Stickの初期化関数
        GameObject stick = Instantiate(StickPrehab) as GameObject;
        St = new Stick[sticknum];
        for(int j=0;j<sticknum;j++){
            St[j] = new Stick();
            //x座標,y座標の決定
            St[j].init(0f, (-1)*sticknum+1+2*j, sticklen);
        }
    }
    private void agentInit(string t){//Agentの初期化関数
        Agent[] a;
        float startX;
        float pow;
        GameObject agent = Instantiate(AgentPrehab) as GameObject;
        if(t=="red"){
            rA = new Agent[agentnum];
            a = rA;
            startX = rLine;
            pow = 0.2f;
        }else /*if(t=="white")*/{
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
    }
    private void teamInit(){//teamの初期化関数
        for(int j=0;j<2;j++){
            Tm[j].init(tm[j]);
        }
    }
    private void init(int anum, int snum){//初期化を行う関数
        map = new Map();
        map.mapInit(anum, snum);//Mapの初期化
        numInit(anum, snum);//エージェントや棒の数の初期化
        stInit();//Stickの初期化
        for(int k=0;k<2;k++){
            agentInit(tm[k]);//Agentの初期化
        }
        teamInit();//Teamの初期化
    }
    private void objMove(){//各objの移動を行う
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
    }
    public void oneGame(int anum, int snum){
        //1ゲームで行う作業をこの関数内に記述
        int tlimit = 600;//時間制限
        init(anum, snum);//初期化
        for(int i=0;i<tlimit;i++){//以下の作業を制限時間内で繰り返し
            objMove();
            if(true/*すべての棒の所持者がredかwhite*/){
                break;
            }
        }
    }
}
