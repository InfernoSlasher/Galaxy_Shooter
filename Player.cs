using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

 
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;

    [SerializeField]
    private GameObject _shieldsVisualizer;

    [SerializeField]
    private GameObject _rightEngine, _leftEngine;


    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //take the curent position and update it to the new position (0, 0, 0)
        if (_gameManager.isCoOpMode == false)
        {
            transform.position = new Vector3(0, 0, 0);
        }
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null )
        {
            Debug.LogError("The Spawn Manager is NULL"); 
        }

        if (_uiManager == null )
        {
            Debug.LogError("The UI Manager is NULL.");
        }

        if ( _audioSource == null )
        {
            Debug.LogError("The Audio Source on the player is NULL.");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement() 
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);  // This is used to move the player horizontally  
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);  // This is used to move the player verticallys

        //This if else if statement is used to keep the player in bounds on the y axis 
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -4.1f)
        {
            transform.position = new Vector3(transform.position.x, -4.1f, 0);
        }

        //This if else if statement is used to keep the player in bounds on the x axis
        if (transform.position.x >= 9.2f)
        {
            transform.position = new Vector3(-9.2f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.2f)
        {
            transform.position = new Vector3(9.2f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate; // The cooldown or fire delay

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); //  spawns laser every time space key is pressed at 1.05 units above player
        }
        
        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldsActive == true)
        {

            _isShieldsActive = false;
            _shieldsVisualizer.SetActive(false);
            return;
        }
        else
        {
            _lives--;

            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
            }

            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives); // calling the update lives metod from the ui manager and displaying it on the screen in the form of sprites  
        }

        if (_lives < 1) 
        {
            _spawnManager.OnPlayerDeath();      
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive() // The triple shot code and duration
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine() //The coroutine called that keeps triple shot active for 5 seconds and then turns false 
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive() // The speed boost code and duration
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    IEnumerator SpeedBoostDownRoutine() // The coroutine called that keeps the speed boost active for 5 seconds and then turns false  
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _isShieldsActive = true;
        _shieldsVisualizer.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
