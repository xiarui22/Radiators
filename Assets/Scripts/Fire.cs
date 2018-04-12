using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Characters {

    List<GameObject> onFireObjects = new List<GameObject>();

    public int spawnRate = 5;
    public float spawnTime = 20;

    float time = 0;

    bool onPutoutFire;

    static int fireNumber = 0;
    static int cap = 100;

    static int[,] FireRecord = new int[100, 100];

    void Awake()
    {
        fireNumber++;
    }

    new void Start() {
        // base.Start();
        hp = Random.Range(10, 30);

        onPutoutFire = false;

        StartCoroutine(burn());
        StartCoroutine(getBigger());

        FireRecord[(int)(transform.position.x + 300) / 10, (int)(transform.position.z + 300) / 10] = 1;
    }

    // Update is called once per frame
    void Update() {

        if (hp <= 0 || transform.position.y < 0)
        {
            Destroy(gameObject);
            FireRecord[(int)(transform.position.x + 300) / 10, (int)(transform.position.z + 300) / 10] = 0;
        }
            
        if (fireNumber < cap && time > spawnTime)
        {     
            spawn();
            time = 0;
        }

        time += UnityEngine.Time.deltaTime;
    }

    public override int GetMaxHP()
    {
        return 100;
    }

    public override int GetMaxPAIN()
    {
        return 100;
    }

    IEnumerator getBigger()
    {
        while (true)
        {
            hp += 1;
            yield return new WaitForSeconds(2);
        }
    }

    public void spawn()
    {
        for (int i = 0; i < 3; i++)
        {
            int chance = Random.Range(0, 100);

            if (chance < spawnRate)
            {
                int row = (int)(transform.position.x + 300) / 10;
                int col = (int)(transform.position.z + 300) / 10;

                if (i == 0)
                {
                    row--;
                }
                if (i == 1)
                {
                    row++;
                }
                if (i == 2)
                {
                    col--;
                }
                if (i == 3)
                {
                    col++;
                }

                if (FireRecord[row, col] == 0)
                {
                    GameObject newFire = (GameObject)Instantiate(gameObject, new Vector3(row * 10 - 300, transform.position.y, col * 10 - 300) + new Vector3(Random.Range(2.0f, 8.0f), 0, Random.Range(2.0f, 8.0f)), transform.rotation);
                    newFire.GetComponent<Fire>().hp = Random.Range(10, 30);

                    FireRecord[row, col] = 1;
                }
            }
        }
    }

    IEnumerator burn()
    {
        while (true)
        {
            foreach (GameObject onFireObject in onFireObjects)
            {
                onFireObject.GetComponent<Characters>().GetDamage(hp / 25 + 1);
            }
            yield return new WaitForSeconds(2);
        }
    }

    public void getDamaged()
    {
        if (onPutoutFire == false)
        {
            onPutoutFire = true;
            StartCoroutine("ReduceFire");
        }
    }

    IEnumerator ReduceFire()
    {
        hp -= 3;
        yield return new WaitForSeconds(1);
        onPutoutFire = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            onFireObjects.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            onFireObjects.Remove(other.gameObject);
        }
    }
}
