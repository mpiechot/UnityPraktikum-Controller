using UnityEngine;
using System.Collections;
using System.Net;
/*
Gameobject component which is used to receive udp datagrams from the motion tracker
and to move/rotate the hand and cylinder accordingly
*/

using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;


public class UDPMotionTracker : MonoBehaviour
{

    // thread which will listen to udp datagrams
    Thread thread;

    // indicates when a datagram was received and processed
    bool[] processed_received_datagrams;

    // Transform of the hand to change its position
    public Transform hand_transform;

    // global vec3 hand position which is modified withing the thread and that is accessed inside the update 
    private Vector3 hand_position = new Vector3();

    // Transform of the hand to change its position and rotation
    public Transform cylinder_transform;

    // global vec3 cylinder position which is modified withing the thread and that is accessed inside the update
    private Vector3 cylinder_position;

    // global vec3 cylinder rotation which is modified withing the thread and that is accessed inside the update
    private Vector3 cylinder_rotation;

    // public vec3 which indicates the scaling of the received hand data
    public Vector3 scale_hand;

    // public vec3 which indicates the scaling of the received cylinder data
    public Vector3 scale_cylinder;

    // public vec3 which indicates the translation of the received hand data
    public Vector3 translate_hand;

    // public vec3 which indicates the translation of the received cylinder data
    public Vector3 translate_cylinder;
    
    // indicates when to stop listening for udp datagrams
    private bool receive_stop = false;

    void Start ()
    {
        // set up array where each field indicates whether data was received or not
        processed_received_datagrams = new bool[2];
        processed_received_datagrams[0] = false; // for the hand
        processed_received_datagrams[1] = false; // for the cylinder

        //init thread with the reading loop
        thread = new Thread(new ThreadStart(readingLoop));

        //start thread
        thread.Start();
    }

    void Update()
    {   
        // check, if a hand message was received and processed
        if(processed_received_datagrams[0])
        {
            //set it back to false, so that a new position can be processed here afterwards
            processed_received_datagrams[0] = false;
            hand_transform.position = hand_position;
        }

        // check, if a cylinder message was received and processed
        if(processed_received_datagrams[1]){
            processed_received_datagrams[1] = false;
            cylinder_transform.position = cylinder_position;
            cylinder_transform.rotation = Quaternion.Euler(cylinder_rotation);
        }
    }

    private void readingLoop()
    {
        // set up the udp client (has to be done within the thread)
        UdpClient udp = new UdpClient(44445);

        // reading loop
        while (!receive_stop)
        {
            // remote host = motion tracker
            IPEndPoint remote_host = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12346);

            
            //stops here until a message was received from the remote host
            byte[] receiveBytes = udp.Receive(ref remote_host);

            //after receiving data, convert it to a string array
            string[] data = Encoding.ASCII.GetString(receiveBytes).Split(';')[0].Split(',');
            
            if(float.Parse(data[0]) == 0){
                //extract real position out of the received string and set the global position variable accordingly
                hand_position.x = float.Parse(data[1], CultureInfo.InvariantCulture)/scale_hand.x + translate_hand.x;
                hand_position.y = float.Parse(data[2], CultureInfo.InvariantCulture)/scale_hand.y + translate_hand.y;
                hand_position.z = float.Parse(data[3], CultureInfo.InvariantCulture)/scale_hand.z + translate_hand.z;

                //set global bool true which tells the update method that a message was received and processed
                //-> rdy to be used further on in the next update invocation
                processed_received_datagrams[0] = true;
            }
            else if(float.Parse(data[0]) == 10){ //currently dont know why its 10 and not 1...
                //extract real position out of the received string and set the global position variable accordingly
                cylinder_position.x = float.Parse(data[1], CultureInfo.InvariantCulture)/scale_cylinder.x + translate_cylinder.x;
                cylinder_position.y = float.Parse(data[2], CultureInfo.InvariantCulture)/scale_cylinder.y + translate_cylinder.y;
                cylinder_position.z = float.Parse(data[3], CultureInfo.InvariantCulture)/scale_cylinder.z + translate_cylinder.z;
                

                cylinder_rotation.x = float.Parse(data[4], CultureInfo.InvariantCulture);
                cylinder_rotation.y = float.Parse(data[5], CultureInfo.InvariantCulture);
                cylinder_rotation.z = float.Parse(data[6], CultureInfo.InvariantCulture);

                //set global bool true which tells the update method that a message was received and processed
                //-> rdy to be used further on in the next update invocation
                processed_received_datagrams[1] = true;
            }
            
            
        }
            
    }


    void OnDestroy()
    {
        receive_stop = true;
        thread.Abort();
    }

}


