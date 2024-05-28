using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;

    private Animator _anim;

    private AudioSource _audioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();


        if (_player == null )
        {
            Debug.LogError("The PLAYER is NULL.");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null )
        {
            Debug.LogError("The Animator is Null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4f, 8f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser =  Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); // the speed of the enemy

        if (transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-8.5f, 8.5f); // The variable which is used to spawn the enemy on random points on the X axis 
            transform.position = new Vector3(randomX, 5.5f, 0);
        }

    }
    private void OnTriggerEnter2D(Collider2D other) // If the enemy collides into the player then it destroys itself
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>(); //this is for accessing the player component and calling a method in that component 

            if (player != null)
            {
               player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.7f);
           
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.7f);
            
        }
    }
}
