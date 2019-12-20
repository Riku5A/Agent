using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    private GameObject map;
    private Game game;
    private int stickNum, agentNum;
    private int red, i, j, k;
    public int nSupStick, numStick;
    private float[,] stickPos;
    public float[,] supStickPos;
    public float[] supstickpos;//現在の目標stick確認用
    public float[] minPos;
    public Agent[] agentList;
    public string team;
    public bool last = false;
    public bool support = false;
    public bool spread = false;
    private int[] crowdSt, onlySt, only;
    public int crowdNum, onlyNum, onlyLen;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("map");
        game = map.GetComponent<Game>();
        minPos = new float[2];
        supstickpos = new float[2];
        only = new int[game.sticknum + 1];
        onlyLen = only.Length;
    }

    // Update is called once per frame
    void Update()
    {
        stickNum = game.sticknum;
        red = game.r;
        if(last == true){
            if(red == stickNum/2){
                Last();
                stickPos = game.getStickXY();
                numStick = stickPos.GetLength(0);
                if(numStick > 0){
                    minPos[0] = stickPos[0,0];
                    minPos[1] = stickPos[0,1];
                    for(i = 0; i < numStick; i++){
                        if(minPos[0] < stickPos[i,0]){
                            minPos[0] = stickPos[i,0];
                            minPos[1] = stickPos[i,1];
                        }
                    }
                }
            }
            if(red > stickNum/2){
                LastFree();
            }
        }
        supStickPos = game.getSupStickXY();
        nSupStick = supStickPos.GetLength(0);
        if(support == true){
            if(nSupStick > 0){
                supstickpos[0] = supStickPos[0,0];
                supstickpos[1] = supStickPos[0,1];
                Support();
            }else{
                SupFree();
            }
        }
        if(spread == true){
            crowdSt = game.getCrowdStick();
            onlySt = game.getOnlyStick();
            crowdNum = crowdSt.Length;
            onlyNum = onlySt.Length;
            Spread();
        }
    }

    public void Spread(){
        agentList = game.agentList;
        agentNum = agentList.Length;
        for(j=0; j < onlyLen; j++){
            only[j] = 0;
        }
        for(j=0; j < agentNum; j++){
            team = agentList[j].getTeam();
            if(team == "red"){
                if(crowdNum > 0){
                for(k=0; k<crowdNum; k++)
                    {
                        if(agentList[j].stNo == crowdSt[k])
                        {
                            if(nSupStick > 0){
                                supstickpos[0] = supStickPos[0,0];
                                supstickpos[1] = supStickPos[0,1];
                                agentList[j].support = true;
                                agentList[j].pull = false;
                            }else{
                                agentList[j].support = false;
                            }
                            crowdSt[k] = stickNum + 1;
                        }
                    }
                }
                if(onlyNum > 0){
                    for(k=0; k<onlyNum; k++)
                    {
                        if(agentList[j].stNo == onlySt[k])
                        {
                            if(only[onlySt[k]] == 0){
                                only[onlySt[k]] = 1;
                            }else if(only[onlySt[k]] == 1){
                                if(nSupStick > 0){
                                    supstickpos[0] = supStickPos[0,0];
                                    supstickpos[1] = supStickPos[0,1];
                                    agentList[j].support = true;
                                    agentList[j].pull = false;
                                }else{
                                    agentList[j].support = false;
                                }
                            }    
                        }
                    }
                }
                if(nSupStick == 0){
                    agentList[j].support = false;
                }
            }
        }
    }

    public void Last(){
        agentList = game.agentList;
        agentNum = agentList.Length;
        for(j=0; j < agentNum; j++){
            team = agentList[j].getTeam();
            if(team == "red"){
                agentList[j].last = true;
                agentList[j].pull = false;
            }
        }
    }

    public void Support()
    {
        agentList = game.agentList;
        agentNum = agentList.Length;
        for(j=0; j < agentNum; j++){
            team = agentList[j].getTeam();
            if(team == "red"){
                agentList[j].support = true;
            }
        }
    }

    public void SupFree(){
        agentList = game.agentList;
        agentNum = agentList.Length;
        for(j=0; j < agentNum; j++){
            team = agentList[j].getTeam();
            if(team == "red"){
                agentList[j].support = false;
            }
        }
    }

    public void LastFree(){
        agentList = game.agentList;
        agentNum = agentList.Length;
        for(j=0; j < agentNum; j++){
            team = agentList[j].getTeam();
            if(team == "red"){
                agentList[j].last = false;
            }
        }
    }

}
