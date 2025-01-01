using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherScript : MonoBehaviour
{
    [SerializeField] private Transform _arrow;
    [SerializeField] private FixedJoystick _joyStick;
    [SerializeField] private Vector2 rotate;

    private void Start()
    {
        _arrow = transform.GetChild(0);
        _joyStick = GameObject.FindGameObjectWithTag("JoyStick").GetComponent<FixedJoystick>();
    }

    private void Update(){
        rotate = new Vector2(_joyStick.Horizontal, _joyStick.Vertical);
        _arrow.transform.localPosition = rotate * 0.5f;
        
        float angleRad = Mathf.Atan2(_arrow.transform.position.y - transform.position.y, _arrow.transform.position.x - transform.position.x);
        float angleDeg = 180 / Mathf.PI * angleRad;

        _arrow.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
}
