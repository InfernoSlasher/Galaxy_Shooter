using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private bool _isEnemyLaser = false;
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false) 
        { 
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }   
    
    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime); //Makes the laser travel upward

        if (transform.position.y > 5.2f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject); // if the laser eaces outside the screen , it destroys itself 
        }
    
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime); //Makes the laser travel downward

        if (transform.position.y < -5.2f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject); // if the laser eaces outside the screen , it destroys itself 
        }

    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}

