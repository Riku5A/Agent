using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public int game;
    public List<int> redScoreList = new List<int>();
    public List<int> whiteScoreList = new List<int>();
    public List<int> flameScoreList = new List<int>();
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject); //オブジェクトの保持
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}