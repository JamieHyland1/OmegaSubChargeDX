// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// public class Cube : MonoBehaviour
// {
//     Vector2 move;
//     Vector2 rotate;
//     PlayerControls controls;
//     void Awake(){
//         controls = new PlayerControls();
//       //  controls.Gameplay.Grow.performed += ctx => Grow();
        
//         controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
//         controls.Gameplay.Move.canceled  += ctx => move = Vector2.zero;

//         controls.Gameplay.Rotate.performed += ctx => rotate = ctx.ReadValue<Vector2>();
//         controls.Gameplay.Rotate.canceled  += ctx => rotate = Vector2.zero;
//     }

//     void Update(){
//         Debug.Log(move);
//         Vector2 m = new Vector2(move.x,move.y) * Time.deltaTime;
//         transform.Translate(new Vector3(m.x,0,m.y), Space.World);

//         Vector2 r = new Vector2(rotate.y,rotate.x) * 100f * Time.deltaTime;
//         transform.Rotate(r, Space.World);
//     }

//     public void Grow(){
//         transform.localScale *= 1.1f;
//     }

//     void OnEnable(){
//         controls.Gameplay.Enable();
//     }
//     void OnDisable(){
//         controls.Gameplay.Disable();
//     }
// }
