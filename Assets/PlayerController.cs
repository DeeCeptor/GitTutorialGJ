using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    // Declare a new variable OUTSIDE the Update() function
    // By making it public, we can DRAG a reference to this component in the scene (not in our text editor)
    public Rigidbody2D rigidbody_2d_component;
    // How fast the player moves LEFT/RIGHT
    public float horizontal_speed = 10f;
    public float jump_force = 500f;
    public bool facing_right = true;

    // Get a reference to our animator component
    public Animator animations;

    // Update is called once per frame
    void Update()
    {
        // This is a variable. We can store specific data in a variable
        // bool is can only be true/false
        bool is_user_pressing_left;
        // Check if user is press A key on keyboard, store result in a variable
        is_user_pressing_left = Input.GetKey(KeyCode.A);
        bool is_user_pressing_right = Input.GetKey(KeyCode.D);

        // Get how fast we're currently going
        Vector2 new_velocity = rigidbody_2d_component.velocity;
        // 0 x speed so we stop IMMEDIATELY if user lets go of key
        new_velocity.x = 0f;
        if (is_user_pressing_left)
        {
            // Directly set our X speed to move LEFTWARDS
            new_velocity.x = -horizontal_speed * Time.deltaTime;
            facing_right = false;
        }
        else if (is_user_pressing_right)
        {
            // Directly set our X speed to move RIGHTWARDS
            new_velocity.x = horizontal_speed * Time.deltaTime;
            facing_right = true;
        }
        // Put newly calculated velocity back into our object
        rigidbody_2d_component.velocity = new_velocity;

        // Set our animation based on if we're walking or not
        animations.SetBool("IsWalking", is_user_pressing_left || is_user_pressing_right);

        /*
        // Set our scale's X to be positive or negative to change the orientation of our sprite
        // This statement evaluates if facing_right is true, and if it's not true, put a -1 in the variable
        float current_facing = facing_right ? 1f : -1f;
        // Our local scale's X determines the facing of our sprite
        Vector3 new_scale = this.transform.localScale;
        new_scale.x = new_scale.x * current_facing;
        this.transform.localScale = new_scale;
        */
        
        // Check if user pressed SPACEBAR to trigger a jump
        bool user_pressed_jump = Input.GetKeyDown(KeyCode.Space);
        if (user_pressed_jump)
        {
            // Check if we have any jumps remaining
            if (remaining_jumps_since_we_touched_ground > 0)
            {
                MakePlayerJumpUp();
                remaining_jumps_since_we_touched_ground = remaining_jumps_since_we_touched_ground - 1;
            }
        }
    }

    public int default_number_of_jumps = 2;
    public int remaining_jumps_since_we_touched_ground = 1;
    // This is called when we COLLIDE with something
    void OnCollisionEnter2D(Collision2D col)
    {
        // Print name of object we hit
        Debug.Log("Collided with " + col.gameObject.name);
        if (col.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("Touched a hazard. We ded");
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Goal"))
        {
            Debug.Log("VICTORY! Reached the end.");
            SceneManager.LoadScene("Scenes/Level 2");//, LoadSceneMode.Single);
        }
        else
        {
            remaining_jumps_since_we_touched_ground = default_number_of_jumps;
        }
    }

    public void MakePlayerJumpUp()
    {
        rigidbody_2d_component.AddForce(
            // Set the Y to go UPWARDS
            new Vector2(0f, jump_force)
        );
        Debug.Log("I'm jumping!");
    }
}
