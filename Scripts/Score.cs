using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    public Text scoreText; //Text用変数
    public int score = 0; //スコア計算用変数
    // Use this for initialization
    void Start () {
        scoreText.text = "好感度: 0"; //初期スコアを代入して画面に表示
    }
	
	// Update is called once per frame
	void Update () {
       
    }

    void onTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Gimmick")
        {
            score += 10; //scoreに加算
            scoreText.text = "Score: " + score.ToString();//スコアを更新して表示
        }
    }

}
