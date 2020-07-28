using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Globalization;


public class UDPMotionTracker : MonoBehaviour
{

    public bool debug_mode = false;

    // thread which will listen to udp datagrams
    Thread thread;

    // indicates when a datagram was received and processed
    bool[] processed_received_datagrams;

    // global position variable, which is used to store the received information
    public Transform hand_transform;
    private Vector3 hand_position = new Vector3();

    public Transform cylinder_transform;
    private Vector3 cylinder_position;
    private Vector3 cylinder_rotation;


    public Vector3 scale_hand;

    public Vector3 scale_cylinder;

    public Vector3 translate_hand;

    public Vector3 translate_cylinder;
    
    // indicates when to stop listening for udp datagrams
    private bool receive_stop = false;

    void Start ()
    {
        processed_received_datagrams = new bool[2];
        processed_received_datagrams[0] = false;
        processed_received_datagrams[1] = false;

        //init thread with the reading loop
        thread = new Thread(new ThreadStart(readingLoop));

        //start it
        thread.Start();
    }

    void Update()
    {   
        if(!debug_mode){
            // check, if a message was received and processed
            if(processed_received_datagrams[0])
            {
                //set it back to false, so that a new position can be processed here afterwards
                processed_received_datagrams[0] = false;
                hand_transform.position = hand_position;
            }

            if(processed_received_datagrams[1]){
                processed_received_datagrams[1] = false;
                cylinder_transform.position = cylinder_position;
                //cylinder_transform.rotation = cylinder_rotation;
            }
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
            //Debug.Log(Encoding.ASCII.GetString(receiveBytes).ToString());
            //after receiving data, convert it to string
            string[] data = Encoding.ASCII.GetString(receiveBytes).Split(';')[0].Split(',');
            Debug.Log(float.Parse(data[0]));
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


