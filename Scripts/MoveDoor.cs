using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveDoor : MonoBehaviour {
    private Rigidbody2D RB2D;//プレイヤーのrigidbody2D
    public int moveSpeed = 2;
    public int score = 20; //スコア計算用変数
    public int scoreUp1 = 10;
    public int scoreDown1 = 1;

    public GameObject Shutter;//Shutterオープン、クローズ変更用
    public GameObject cage;//cage
    public GameObject CameraArea;//カメラエリア消す用
    public Text scoreText; //Text用変数


    public bool menu;//メニューが表示されているかのチェック
    public bool Being_monitored;//監視中かのチェック
    public bool gimicknow;//あかない判定

   

    private void Start()
    {

        scoreText.text = "好感度: 20"; //初期スコアを代入して画面に表示

        //GetComponentの処理をキャッシュしておく
        RB2D = GetComponent<Rigidbody2D>();
        RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);
    }

    private void Update()
    {
        GameObject obj = getClickObject();//クリックされたものを取得
        if (obj != null && obj.gameObject.name == "Surveillance_Camera"&&gimicknow==true)
        {
            Debug.Log("ありがとうございました。");
            gimicknow = false;
            D_stayUp();
            Destroy(CameraArea);
            Shutter.transform.position = new Vector3(2.5f, 11.2f, 0);

        }

        if (score >= 100)
        {
            SceneManager.LoadScene("Game_End");
        }

        if (score <= 0)
        {
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
        //カメラの範囲に入った
        if (col.gameObject.tag == "Surveillance_Camera")
        {
            Debug.Log("監視中");
            Being_monitored = true;//監視中
        }
        //シャッター判定
        if (col.gameObject.tag == "Gimmick")
        {
            if (Being_monitored == true)
            {
                Shutter.transform.position = new Vector3(2.5f, 6.71f, 0);
                Debug.Log("あぶない！！");
                RB2D.velocity = Vector2.zero;
                gimicknow = true;
            }
        }
        //cage判定
        if (col.gameObject.tag == "cagecol")
        {
            cage.transform.position = new Vector3(11.04f,0,0);
        }
        //クリア判定
        if(col.gameObject.tag == "Clear")
        {
            score += 10;
            scoreText.text = "好感度: " + score;
        }
    }
    public void D_stay()
    {
        menu = true;
        RB2D.velocity = Vector2.zero;
        RB2D.isKinematic = true;
    }
    public void D_stayUp()
    {
        if (gimicknow == true)
        {
            menu = false;
            RB2D.isKinematic = false;
        }
        else
        {
            menu = false;
            RB2D.velocity = new Vector2(moveSpeed, RB2D.velocity.y);
            RB2D.isKinematic = false;
        }
    }
}