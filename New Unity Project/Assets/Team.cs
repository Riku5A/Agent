using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    private string team;//チーム情報
    private Agent[] agents;//自チームのエージェント
    private int agentnum;
    Game game;

    public void init(string team)//初期化関数
    {
        game = this.GetComponent<Game>();
        this.team = team;
        agents = game.getAgent(team);
        agentnum = agents.Length;
        //agentsに自チームエージェントを取得
    }

    public void decideAction()//チームの作戦指示
    {
        int i;
        for(i=0; i < agentnum; i++){
            agents[i].decideAction();
        }
        //自チームAgentに任意の順番で行動の指示を与えるagents[1].decideAction(map);
    }
}
