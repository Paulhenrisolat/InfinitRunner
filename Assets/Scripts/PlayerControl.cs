using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float speedMultiplier;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    public int playerHp { get; private set; }

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    [SerializeField]
    private ParticleSystem Flame1;
    [SerializeField]
    private ParticleSystem Flame2;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
        runSpeed = 100f;
        currentSpeed = speed;
        speedMultiplier = 15f;
        jumpSpeed = 10f;
        playerHp = 100;
        minX = -10f;
        maxX = 10f;
        minY = 0.2f;
        maxY = 2f;

        Flame1 = GameObject.Find("Flame1").GetComponent<ParticleSystem>();
        Flame2 = GameObject.Find("Flame2").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //move
        float horizontalMove = Input.GetAxis("Horizontal")* currentSpeed * Time.deltaTime;
        transform.position += transform.right * horizontalMove;
        float verticalMove = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        transform.position += transform.forward * verticalMove;
        float upMove = Input.GetAxis("Jump") * jumpSpeed * Time.deltaTime;
        transform.position += transform.up * upMove;

        //limit
        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
        if (transform.position.y < minY)
        {
            transform.position = new Vector3(transform.position.x, minY, transform.position.z);
        }
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }

        //fall
        if (transform.position.y > minY)
        {
            float posY = transform.position.y - 2f * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, posY, transform.position.z);
        }

        //speed up
        var main1 = Flame1.main;
        var main2 = Flame2.main;
        if (Input.GetButton("Fire2"))
        {
            if(currentSpeed < runSpeed)
            {
                currentSpeed += speedMultiplier * Time.deltaTime;
                
                if (currentSpeed > runSpeed)
                {
                    currentSpeed = runSpeed;
                }
            }
            main1.startLifetime = 2.40f;
            main2.startLifetime = 2.40f;
        }
        else
        {
            if (currentSpeed > speed)
            {
                currentSpeed -= speedMultiplier * Time.deltaTime;

                if (currentSpeed < speed)
                {
                    currentSpeed = speed;
                }
            }
            main1.startLifetime = 0.18f;
            main2.startLifetime = 0.18f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision !");
        playerHp -= 1;
        if(playerHp <= 0)
        {
            Debug.Log("GameOver !");
        }
    }
}
