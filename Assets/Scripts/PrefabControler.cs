using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabControler : MonoBehaviour
{
    public static PrefabControler Instance;

    [SerializeField]
    private GameObject[] GroundsPrefabs;
    [SerializeField]
    private GameObject[] GroundsOnScene;
    public float GroundSize;
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private float distDestroy;

    [SerializeField]
    private Image healthBackground, health;

    private PlayerControl playerControl;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        playerControl = Player.GetComponent<PlayerControl>();

        distDestroy = 2f;
        //nb object in list
        GroundsOnScene = new GameObject[GroundsPrefabs.Length];
        for (int i=0; i< GroundsPrefabs.Length; i++)
        {
            int r = Random.Range(0, GroundsPrefabs.Length);
            GroundsOnScene[i] = Instantiate(GroundsPrefabs[r]);
            GroundSize = GroundsOnScene[i].GetComponentInChildren<Transform>().Find("Plane").GetComponent<Collider>().bounds.size.z;
        }

        float posZ = Player.transform.position.z + GroundSize/2-1.5f;
        foreach(var ground in GroundsOnScene)
        {
            ground.transform.position = new Vector3(0,0.2f, posZ);
            posZ += GroundSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = GroundsOnScene.Length-1; i>=0; i--)
        {
            GameObject ground = GroundsOnScene[i];
            if (ground.transform.position.z + GroundSize/2 < Player.transform.position.z - distDestroy)
            {
                float z = ground.transform.position.z;
                Destroy(ground);
                int r = Random.Range(0, GroundsPrefabs.Length);
                ground = Instantiate(GroundsPrefabs[r]);
                ground.transform.position = new Vector3(0, 0.2f, z+GroundSize*GroundsOnScene.Length);
                GroundsOnScene[i] = ground;
            }
        }

        //hp control
        health.rectTransform.sizeDelta = new Vector2(healthBackground.rectTransform.sizeDelta.x * playerControl.playerHp / 100, healthBackground.rectTransform.sizeDelta.y);
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
