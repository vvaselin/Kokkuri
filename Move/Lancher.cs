using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    // �o�ߎ���
    float timeCount1 = 0; 
    float timeCount2 = 0; 
    float timeCount3 = 0;
    // ���ˊp�x
    float shotAngle1 = 0; 
    float shotAngle2 = 0; 
    float shotAngle3 = 0;
    float angleStep = 1.0f;
    [SerializeField] GameObject shotBullet; // ���˂���e
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

                // Bullet�X�N���v�g��Init���Ăяo��
                bulletScript.Init(shotAngle1, 1.0f);
            }
        }
    }

    void ShotCycle()
    {
        timeCount2 += Time.deltaTime;

        shotAngle2 += -2 * timeCount2;

        // 1�b�𒴂��Ă��邩
        if (timeCount2 > 0.2f)
        {
            timeCount2 = 0;

            // �������F��������GameObject
            // �������F����������W
            // ��O�����F��������p�x
            GameObject createObject = Instantiate(shotBullet, transform.position + new Vector3(0, 0, 1), Quaternion.identity);

            Bullet bulletScript = createObject.GetComponent<Bullet>();

            // Bullet�X�N���v�g��Init���Ăяo��
            bulletScript.Init(shotAngle2, 1.0f);
        }
    }

    void ShotOscillate()
    {
        timeCount3 += Time.deltaTime;

        // �p�x�𑝌��i��������悤�Ɂj
        shotAngle3 += angleStep * timeCount3;

        // �p�x���͈͂𒴂�����������t�]
        if (shotAngle3 >= 0f || shotAngle3 <= -90f)
        {
            angleStep *= -1;
        }

        // ��莞�Ԃ��Ƃɒe�𔭎�
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