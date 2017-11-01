using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {
	private new Rigidbody2D rigidbody2D;//プレイヤーのrigidbody2D
    public int moveSpeed = 2;
    public int score = 20; //スコア計算用変数
    public int scoreUp1 = 10;
    public int scoreUp2 = 20;
    public int scoreDown1 = 1;

    public bool menu;//メニューが表示されているかのチェック
    public bool characterHelp1;//岩助けた判定
    public bool RockNow;//岩で止まった時
    public bool OboreNow;//溺れているとき
    public bool characterHelp2;//溺れ助けた判定

	public float scoreDown;//スコアを減らす

    public Text scoreText; //Text用変数

    public GameObject up;//溺れてる上判定
    public GameObject player;//プレイヤー
    public GameObject Rock_clear;//岩クリア判定
    public GameObject Obore_Clear;//溺れクリア判定
    public GameObject Gimmick1;//岩
    public GameObject HelpPoint;//溺れた時のヘルプ位置

    void Start()
    {
        //GetComponentの処理をキャッシュしておく
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
        score = 20;

        scoreText.text = "好感度: 20"; //初期スコアを代入して画面に表示
        //velocity: 速度
        //X方向へmoveSpeed分移動させる
    }

    void Update()
    {
        GameObject obj = getClickObject();//クリックされたものを取得

        if (obj != null&&obj.gameObject.name=="Gimmick")
        {
            // 以下岩ギミック時岩がクリックされた時の処理
            if (RockNow == true)
            {
                if (Input.GetMouseButtonDown(0) && menu == false)
                {
                    rigidbody2D.WakeUp();
                    characterHelp1 = true;
                    Destroy(Gimmick1);
                    rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
                    RockNow = false;
                }
            }
        }

        if (obj != null && obj.gameObject.name == "Player")
        {
            //以下溺れギミック時プレイヤーがクリックされたときの処理
            if (OboreNow == true)
            {
                if (Input.GetMouseButtonDown(0) && menu == false)
                {
                    Debug.Log("助かった！");
                    rigidbody2D.gravityScale = 1;
                    rigidbody2D.WakeUp();
                    Vector2 help = HelpPoint.transform.position;
                    player.transform.position = new Vector2(help.x, help.y);
                    characterHelp2 = true;
                    OboreNow = false;
                }
            }
        }

		if(score >= 100){
			SceneManager.LoadScene("Game_End");
		}

		if (score <= 0) {
			SceneManager.LoadScene("Game_Over");
		}
    }

    private GameObject getClickObject()
    {
        GameObject result = null;//中身を空に

        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d)
            {
                result = collition2d.transform.gameObject;
            }
        }
        return result;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Gimmick")
        {
            Debug.Log("岩が！");
            RockNow = true;
            rigidbody2D.velocity = Vector2.zero;
        }

        if (col.gameObject.tag == "OboreGimmick")
        {
            Debug.Log("溺れる！！");
            rigidbody2D.velocity = Vector2.zero;
            OboreNow = true;
            //溺れたときに上げる
            rigidbody2D.gravityScale = -1;
        }


        if (col.gameObject.tag == "Clear" && characterHelp1==true)
        {
            Debug.Log("ありがとう");
            score += scoreUp1; //scoreに加算
            scoreText.text = "好感度: " + score.ToString();//スコアを更新して表示
        }

        if (col.gameObject.tag == "Clear" && characterHelp2 == true)
        {
            Debug.Log("ありがとう2");
            score += scoreUp2; //scoreに加算
            scoreText.text = "好感度: " + score.ToString();//スコアを更新して表示
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
       //Debug.Log("Stay");

        if (col.gameObject.tag == "Up")
        {
            //溺れるときに落とす
            rigidbody2D.gravityScale = 1;
        }

        if (characterHelp1 == true)
        {
            Destroy(Rock_clear);//Rock_Clearの削除

            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);

            characterHelp1 = false;
        }

        if (characterHelp2 == true)
        {
            Destroy(Obore_Clear);//Obore_Clearの削除

            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);

            characterHelp2 = false;
        }

		if (col.gameObject.tag == "GMclear")
		{
			Debug.Log ("さいならー");
			SceneManager.LoadScene("Game_End");
		}
    }



    public void stay()
    {
        menu = true;
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.isKinematic = true;
    }
    public void stayUp()
    {
        if (RockNow == true) {
            Debug.Log("GimmickNow");
            menu = false;
        }
        else if (OboreNow == true)
        {
            menu = false;
            rigidbody2D.isKinematic = false;
        }
        else 
        {
            menu = false;
            rigidbody2D.velocity = new Vector2(moveSpeed, rigidbody2D.velocity.y);
            rigidbody2D.isKinematic = false;
        }
    }
}
