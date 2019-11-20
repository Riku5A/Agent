using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private float x, y, v; //座標、速さ
    private float[] direction; //向き
    private float[] distance; //移動距離
    private float pow, max_v, size; //力と最大速度と大きさ
    private string team; //チーム情報
    private bool pull; //Agentの状態
    // Start is called before the first frame update
    void Start()
    {
        x = 0.1f;
        y = 0.1f;
        direction = new float[2];
        distance = new float[2];
        direction[0] = 1f;
        direction[1] = 1f; 
        v = 0.1f;
        transform.position = new Vector3(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        update();
        /*distance[0] = direction[0] * v;
        distance[1] = direction[1] * v;
        x = x + distance[0];
        y = y + distance[1];
        transform.position = new Vector3(x, y);*/
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
/*マップと決める
    public void decideAction(map)
    {
        direction[0] = 
    }
*/
    public void update()
    {
        distance[0] = direction[0] * v;
        distance[1] = direction[1] * v;
        x = x + distance[0];
        y = y + distance[1];
        transform.position = new Vector3(x, y);
    }
}
