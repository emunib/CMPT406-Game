using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : GenericPlayer {
    private CrawlerInput crawler_input;

    public Transform tiltpoint;
    protected override void Start() {
        crawler_input = GetComponent<CrawlerInput>();
        SetInput(crawler_input);
        
        SetIgnoreFields(false);

        rigidbody.freezeRotation = true;
        
        base.Start();
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        Walk();
    }


    private bool getready = true;
    private void Walk() {
    /*
        if (getready) {
            foreach (RaycastHit2D ray in hits) {
                if (ray.collider != null) {
                    crawler_input.horg = -ray.normal.x;
                    crawler_input.verg = -ray.normal.y;
                    getready = false;

                }
            }
        }
        
        else if (is_grounded) {
            crawler_input.horizontal = transform.right.x;
            crawler_input.vertical = transform.right.y;

          
            }
        */    

        RaycastHit2D ray = Physics2D.Raycast(tiltpoint.position, -transform.up, .7f);
        Debug.DrawRay(tiltpoint.position,-transform.up*.7f, Color.green);
        rigidbody.freezeRotation = true;

        if (ray.collider == null&& getready) {

            
            transform.right = -transform.up;
            rigidbody.velocity = Vector2.zero;

            Vector2 pos = transform.localPosition;
            pos = pos + (Vector2)transform.right * 1f;

            transform.localPosition= pos;
            crawler_input.horg = -transform.up.x;
            crawler_input.verg = -transform.up.y;
            getready = false;
                
            
        }
        else if (ray.collider !=null) {

            getready = true;

        }

        crawler_input.horizontal = transform.right.x;
        crawler_input.vertical = transform.right.y;
        
        
        
        Debug.DrawRay(transform.position,transform.right*10f, Color.blue);

        
    }
}
