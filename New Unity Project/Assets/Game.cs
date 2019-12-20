using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.IO;
using System.Text;

public class Game : MonoBehaviour
{
    public int agentnum;//1チームのエージェントの数
    public int sticknum;//棒の数
    public int n, r, w;
    public float[,] xy;
    public float[,] supxy;
    private string[] tm = new string[2] { "white", "red" };
    public GameObject StickPrefab;
    public GameObject AgentPrehab;
    private GameObject gameplayer;
    private GamePlayer gPlayer;
    private Stick[] stickList;
    public Agent[] agentList;
    private string owner;
    private float[,] status;
    private float[,] redStatus;
    private float[,] whiteStatus;
    private System.Random rnd = new System.Random();
    public float stickWidth = 1.5f;
    public float agentWidth = 0.5f;
    public float powAverage = 1.0f;
    public float vAverage = 0.1f;
    private float[] powStatus;
    private float[] vStatus;
    private float powUnit = 0.1f;
    private float vUnit = 0.01f;
    public int positionMode;
    public int flamecnt;
    private int flamelimit = 1000;
    private StreamWriter sw;
    public string resultFileName;
    public int[] crowdStick, onlyStick; //散開する棒と一人でいい棒のNoの配列
    public float thresholdSpeed = 0.02f; //supportする閾値(speed)
    public float crowdPowLevel = 4f; //散開する閾値(pow)

    void Start()
    {
        xy = new float[sticknum, 2];
        init(agentnum, sticknum);
        gameplayer = GameObject.Find("Game");
        gPlayer = gameplayer.GetComponent<GamePlayer>();
        flamecnt = 0;
    }

    void Update()
    {
        flamecnt++;
        checkEnd();
    }

    void stInit()
    {//Stickの初期化関数
        stickList = new Stick[sticknum];
        xy = new float[sticknum, 2];
        for (int j = 0; j < sticknum; j++)
        {
            GameObject stick = Instantiate(StickPrefab) as GameObject;
            stickList[j] = stick.GetComponent<Stick>();
            stickList[j].init(0f, (j - 0.5f * (sticknum - 1)) * stickWidth, 1);
            stickList[j].No = j + 1;
            xy[j, 0] = stickList[j].getX();
            xy[j, 1] = stickList[j].getY();
        }
    }

    void checkEnd()
    {
        n = 0;
        r = 0;
        w = 0;
        for (int j = 0; j < sticknum; j++)
        {
            owner = stickList[j].getOwner();
            if (owner == "neutral")
            {
                n++;
            }
            else if (owner == "red")
            {
                r++;
            }
            else if (owner == "white")
            {
                w++;
            }
        }
        if (r > sticknum / 2 | w > sticknum / 2 | flamecnt >= flamelimit)
        {
            Debug.Log("white:" + w + " vs red:" + r);
            Debug.Log("所要フレーム数： " + flamecnt);
            gPlayer.whiteScoreList.Add(w);
            gPlayer.redScoreList.Add(r);
            gPlayer.flameScoreList.Add(flamecnt);
            gPlayer.game--;
            if (gPlayer.game == 0)
            {
                sw = new StreamWriter(resultFileName);
                sw.WriteLine("whiteScore,redScore,flame");
                for (int k = 0; k < gPlayer.redScoreList.Count(); k++)
                {
                    sw.WriteLine("{0},{1},{2}", gPlayer.whiteScoreList[k], gPlayer.redScoreList[k], gPlayer.flameScoreList[k]);
                }
                sw.Close();
                EditorApplication.isPlaying = false;
            }
            if (gPlayer.game > 0)
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void updateStick()
    {
        int sticklive, k, s, t, u, supStick, crowdNum, onlyNum;
        sticklive = sticknum;
        supStick = 0;
        crowdNum = 0;
        onlyNum = 0;
        k = 0;
        s = 0;
        t = 0;
        u = 0;
        int p;
        for (int j = 0; j < sticknum; j++)
        {
            owner = stickList[j].getOwner();
            if (owner != "neutral")
            {
                sticklive--;
            }
            if (stickList[j].sumpow <= thresholdSpeed && owner == "neutral")
            {
                supStick++;
            }
            if(owner == "neutral" && stickList[j].sumpow > crowdPowLevel && stickList[j].whitepow != 0)
            {
                crowdNum++;
            }
            if(owner == "neutral" && stickList[j].whitepow == 0)
            {
                onlyNum++;
            }
        }
        xy = new float[sticklive, 2];
        supxy = new float[supStick, 2];
        crowdStick = new int[crowdNum];
        onlyStick = new int[onlyNum];
        for (int j = 0; j < sticknum; j++)
        {
            owner = stickList[j].getOwner();
            if (owner == "neutral")
            {
                xy[k, 0] = stickList[j].getX();
                xy[k, 1] = stickList[j].getY();
                k++;
                if (supStick > 0)
                {
                    if (stickList[j].sumpow <= thresholdSpeed)
                    {
                        supxy[s, 0] = stickList[j].getX();
                        supxy[s, 1] = stickList[j].getY();
                        s++;
                    }
                }
                if(crowdNum > 0){
                    if(stickList[j].sumpow > crowdPowLevel)
                    {
                        //crowdStick[t] = stickList[j].No;
                        t++;
                    }
                }
                if(onlyNum > 0){
                    if(stickList[j].whitepow == 0)
                    {
                        onlyStick[u] = stickList[j].No;
                        u++;
                    }
                }
            }
        }
    }

    public float[,] getStickXY()
    {//Stickの座標取得
        updateStick();
        return xy;//得られた座標を配列として戻す
    }

    public float[,] getSupStickXY()
    {//Stickの座標取得
        updateStick();
        return supxy;//得られた座標を配列として戻す
    }

    public int[] getCrowdStick()
    {
        updateStick();
        return crowdStick;
    }

    public int[] getOnlyStick()
    {
        updateStick();
        return onlyStick;
    }

    private float[,] changePos(float[,] preStatus)
    {
        status = new float[agentnum / 2, 2];
        for (int j = 0; j < agentnum / 2; j++)
        {
            if (j % 2 == 0)
            {
                status[j / 2, 0] = preStatus[j, 0];
                status[j / 2, 1] = preStatus[j, 1];
            }
            else
            {
                status[(agentnum / 2) - ((j + 1) / 2), 0] = preStatus[j, 0];
                status[(agentnum / 2) - ((j + 1) / 2), 1] = preStatus[j, 1];
            }
        }
        return status;
    }

    private List<List<int>> decideGroup(float[,] status)
    {
        int goalStickNum = sticknum / 2 + 2;
        List<List<int>> positionList = new List<List<int>>();
        for (int j = 0; j < goalStickNum; j++)
        {
            List<int> agentNumList = new List<int>();
            positionList.Add(agentNumList);
        }
        float[] powSum = new float[goalStickNum];
        for (int j = 0; j < goalStickNum; j++)
        {
            powSum[j] = 0f;
        }
        for (int j = 0; j < agentnum / 2; j++)
        {
            float minPS = powSum.Min();
            int listNum = Array.IndexOf(powSum, minPS);
            positionList[listNum].Add(j);
            powSum[listNum] += status[j, 0];
        }
        return positionList;
    }

    private float[,] statusInit(string t)
    {
        status = new float[agentnum / 2, 2];
        powStatus = new float[agentnum / 2];
        vStatus = new float[agentnum / 2];
        float minPow = powAverage - 0.5f;
        float maxPow = powAverage + 0.5f;
        float minV = vAverage - 0.05f;
        float maxV = vAverage + 0.05f;
        float sumPow = powAverage * (agentnum / 2);
        float sumV = vAverage * (agentnum / 2);
        List<int> powList = new List<int>();
        List<int> vList = new List<int>();
        for (int j = 0; j < agentnum / 2; j++)
        {
            powStatus[j] = minPow;
            sumPow -= minPow;
            powList.Add(j);
            vStatus[j] = minV;
            sumV -= minV;
            vList.Add(j);
        }
        for (int j = 0; j < (powAverage - minPow) * (agentnum / 2) / powUnit; j++)
        {
            int num = rnd.Next(powList.Count);
            powStatus[powList[num]] += powUnit;
            if (powStatus[powList[num]] == maxPow)
            {
                powList.RemoveAt(num);
            }
        }
        for (int j = 0; j < (vAverage - minV) * (agentnum / 2) / vUnit; j++)
        {
            int num = rnd.Next(vList.Count);
            vStatus[vList[num]] += vUnit;
            if (vStatus[vList[num]] == maxV)
            {
                vList.RemoveAt(num);
            }
        }
        if (t == tm[1] & positionMode == 1)
        {
            Array.Sort(vStatus);
            Array.Reverse(vStatus);
        }
        else if (t == tm[1] & positionMode == 2)
        {
            Array.Sort(powStatus);
            Array.Reverse(powStatus);
        }
        for (int j = 0; j < agentnum / 2; j++)
        {
            status[j, 0] = powStatus[j];
            status[j, 1] = vStatus[j];
        }
        return status;
    }

    void agentInit(string t)
    {//Agentの初期化関数
        agentList = new Agent[agentnum / 2];
        if (t == "red" & positionMode == 2)
        {
            List<List<int>> positionList = new List<List<int>>();
            positionList = decideGroup(redStatus);
            for (int j = 0; j < agentnum / 2; j++)
            {
                GameObject agent = Instantiate(AgentPrehab) as GameObject;
                agentList[j] = agent.GetComponent<Agent>();
            }
            int plLen = positionList.Count();
            float[,] stickPos = getStickXY();
            for (int j = 0; j < plLen; j++)
            {
                int nowStick = j + (sticknum - plLen) / 2;
                float nowStickPos = stickPos[nowStick, 1];
                List<int> thisStickList = new List<int>();
                thisStickList = positionList[j];
                for (int k = 0; k < thisStickList.Count(); k++)
                {
                    int an = thisStickList[k];
                    agentList[an].init(11.5f, (float)(nowStickPos + (k - 0.5f * (thisStickList.Count() - 1)) * 0.25), redStatus[an, 0], redStatus[an, 1], 1, "red");
                }
            }
        }
        else
        {
            if (t == "red" & positionMode == 1)
            {
                redStatus = changePos(redStatus);
            }
            for (int j = 0; j < agentnum / 2; j++)
            {
                if (t == "red")
                {
                    GameObject agent = Instantiate(AgentPrehab) as GameObject;
                    agentList[j] = agent.GetComponent<Agent>();
                    agentList[j].init(11.5f, (j - 0.5f * (agentnum / 2 - 1)) * agentWidth, redStatus[j, 0], redStatus[j, 1], 1, "red");
                }
                else
                {
                    GameObject agent = Instantiate(AgentPrehab) as GameObject;
                    agentList[j] = agent.GetComponent<Agent>();
                    agentList[j].init(-11.5f, (j - 0.5f * (agentnum / 2 - 1)) * agentWidth, whiteStatus[j, 0], whiteStatus[j, 1], 1, "white");
                }
            }
        }
    }

    void init(int anum, int snum)
    {//初期化を行う関数
        stInit();//Stickの初期化
        whiteStatus = statusInit(tm[0]);
        redStatus = statusInit(tm[1]);
        for (int k = 0; k < 2; k++)
        {
            agentInit(tm[k]);//Agentの初期化
        }
    }
}