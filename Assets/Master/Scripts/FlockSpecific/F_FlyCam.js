﻿/*
Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
Simple flycam I made, since I couldn't find any others made public.  
Made simple to use (drag and drop, done) for regular keyboard layout  
wasd : basic movement
shift : Makes camera accelerate
space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
 
 
var mainSpeed : float = 100.0; //regular speed
var shiftAdd : float = 250.0; //multiplied by how long shift is held.  Basically running
var maxShift : float = 1000.0; //Maximum speed when holdin gshift
var camSens : float = 0.25; //How sensitive it with mouse
private var lastMouse = Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
private var totalRun : float  = 1.0;

var minimumX = -360F;
var maximumX = 360F;
var minimumY = -60F;
var maximumY = 60F;

var rotationX=0;
var rotationY=0;
var originalRotation;
function Start(){
    originalRotation = transform.localRotation;

}
function Update () {
   
    // Read the mouse input axis
    rotationX += Input.GetAxis("Mouse X") * camSens;
    rotationY += Input.GetAxis("Mouse Y") * camSens;
    rotationX = ClampAngle (rotationX, minimumX, maximumX);
    rotationY = ClampAngle (rotationY, minimumY, maximumY);
    var xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
    var yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);
    transform.localRotation = originalRotation * xQuaternion * yQuaternion;


    //lastMouse = Input.mousePosition - lastMouse ;
    //lastMouse = Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
    //lastMouse = Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
    //transform.eulerAngles = lastMouse;
    //lastMouse =  Input.mousePosition;
    //Mouse  camera angle done.  
    //Keyboard commands
    var f : float = 0.0;
    var p = GetBaseInput();
    if (Input.GetKey (KeyCode.LeftShift)){
        totalRun += Time.deltaTime;
        p  = p * totalRun * shiftAdd;
        p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
        p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
        p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
    }
    else{
        totalRun = Mathf.Clamp(totalRun * 0.5, 1, 1000);
        p = p * mainSpeed;
    }
   
    p = p * Time.deltaTime;
    if (Input.GetKey(KeyCode.Space)){ //If player wants to move on X and Z axis only
        f = transform.position.y;
        transform.Translate(p);
        transform.position.y = f;
    }
    else{
        transform.Translate( p);
    }
   
}

public function ClampAngle (angle, min, max) : float
{
    if (angle < -360)
        angle += 360;
    if (angle > 360)
        angle -= 360;
    return Mathf.Clamp (angle, min, max);
}
 
private function GetBaseInput() : Vector3 { //returns the basic values, if it's 0 than it's not active.
    var p_Velocity : Vector3;
    if (Input.GetKey (KeyCode.W)){
        p_Velocity += Vector3(0, 0 , 1);
    }
    if (Input.GetKey (KeyCode.S)){
        p_Velocity += Vector3(0, 0 , -1);
    }
    if (Input.GetKey (KeyCode.A)){
        p_Velocity += Vector3(-1, 0 , 0);
    }
    if (Input.GetKey (KeyCode.D)){
        p_Velocity += Vector3(1, 0 , 0);
    }
    if (Input.GetKey (KeyCode.Q)){
        p_Velocity += Vector3(0, -1 , 0);
    }
    if (Input.GetKey (KeyCode.E)){
        p_Velocity += Vector3(0, 1 , 0);
    }
    return p_Velocity;
}