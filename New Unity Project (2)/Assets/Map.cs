using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private int agentnum;//1チームのエージェントの数
    private int sticknum;//棒の数
    private Game game;

    void Start(){
        game = this.GetComponent<Game>();
    }
/*
    public void mapInit(int anum, int snum){
        //gm = new Game();
        agentnum = anum;
        sticknum = snum;
    }/*
    public float[,] getAgentXY(string t){//チーム内の全Agentの座標取得, 引数は取得したいチームの色
        float[,] xy = new float[agentnum,2];//座標保存用
        Agent[] a;
        a = gm.getAgent(t);//指定チームのエージェントの配列をaとする
        for(int i=0;i<agentnum;i++){//各エージェントの座標を取得、配列に格納
            xy[i,0] = a[i].getX();
            xy[i,1] = a[i].getY();
        }
        return xy;//得られた座標を配列として戻す
    }*/
    public float[,] getStickXY(){//Stickの座標取得
        float[,] xy = new float[sticknum,2];//座標保存用
        //Stick[] St;
        //St = gm.getStick();//棒オブジェクトの配列をStとする
        /*for(int i=0;i<sticknum;i++){//各棒の座標を取得、配列に格納
            if(true/*owner != newtral*//*){
                xy[i,0] = stickList[i].getX();
                xy[i,1] = stickList[i].getY();
            }
        }*/
        game.updateStick();
        xy = game.xy;
        return xy;//得られた座標を配列として戻す
    }/*
    public float[] getAgentMax(string t){//チーム内の全Agentの最高速度を取得, 引数は取得したいチームの色
        float[] m = new float[agentnum];//速度保存用
        Agent[] a;
        a = gm.getAgent(t);//指定チームのエージェントの配列をaとする
        for(int i=0;i<agentnum;i++){//各エージェントの速度を取得、配列に格納
            m[i] = a[i].getMax();
        }
        return m;
    }
    public float[] getAgentPow(string t){//チーム内の全Agentの力を取得, 引数は取得したいチームの色
        float[] p = new float[agentnum];//力保存用
        Agent[] a;
        a = gm.getAgent(t);//指定チームのエージェントの配列をaとする
        for(int i=0;i<agentnum;i++){//各エージェントの力を取得、配列に格納
            p[i] = a[i].getPow();
        }
        return p;
    }*/
}
