using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    // 経過時間
    float timeCount1 = 0; 
    float timeCount2 = 0; 
    float timeCount3 = 0;
    // 発射角度
    float shotAngle1 = 0; 
    float shotAngle2 = 0; 
    float shotAngle3 = 0;
    float angleStep = 1.0f;
    [SerializeField] GameObject shotBullet; // 発射する弾
    [SerializeField] GameObject shotEffect1;

    void Start()
    {
        
    }

    void Update()
    {
        ShotCircle();
        //ShotOscillate();
        ShotCycle();
    }

    void ShotCircle()
    {
        timeCount1 += Time.deltaTime;

        if (timeCount1>2.0f)
        {
            Vector3 pos = new Vector3(1.8f, 0, 1.2f);
            CreateShotEffect(1.0f, pos);

            timeCount1 = 0;
            shotAngle1 = 0;

            for (int i = 0; i <= 6; i++)
            {
                shotAngle1 = 180 +  15 * i;

                GameObject createObject = Instantiate(shotBullet, transform.position+new Vector3(1.8f, 0, 1.2f), Quaternion.identity);

                Bullet bulletScript = createObject.GetComponent<Bullet>();

                // BulletスクリプトのInitを呼び出す
                bulletScript.Init(shotAngle1, 1.0f);
            }
        }
    }

    void ShotCycle()
    {
        timeCount2 += Time.deltaTime;

        shotAngle2 += -2 * timeCount2;

        // 1秒を超えているか
        if (timeCount2 > 0.2f)
        {
            timeCount2 = 0;

            // 第一引数：生成するGameObject
            // 第二引数：生成する座標
            // 第三引数：生成する角度
            GameObject createObject = Instantiate(shotBullet, transform.position + new Vector3(0, 0, 1), Quaternion.identity);

            Bullet bulletScript = createObject.GetComponent<Bullet>();

            // BulletスクリプトのInitを呼び出す
            bulletScript.Init(shotAngle2, 1.0f);
        }
    }

    void ShotOscillate()
    {
        timeCount3 += Time.deltaTime;

        // 角度を増減（往復するように）
        shotAngle3 += angleStep * timeCount3;

        // 角度が範囲を超えたら方向を逆転
        if (shotAngle3 >= 0f || shotAngle3 <= -90f)
        {
            angleStep *= -1;
        }

        // 一定時間ごとに弾を発射
        if (timeCount3 > 0.5f)
        {
            timeCount3 = 0;

            GameObject createObject = Instantiate(shotBullet, transform.position + new Vector3(-1.8f, 0, 1.2f), Quaternion.identity);
            Bullet bulletScript = createObject.GetComponent<Bullet>();
            bulletScript.Init(shotAngle3, 2.0f);
        }
    }

    void CreateShotEffect(float Lifetime, Vector3 pos)
    {
        GameObject shotObject = Instantiate(shotEffect1, transform.position + pos, Quaternion.identity);
        ParticleSystem particleSystem = shotObject.GetComponent<ParticleSystem>();
        particleSystem.Play();
        Destroy(shotObject, Lifetime);
    }
}