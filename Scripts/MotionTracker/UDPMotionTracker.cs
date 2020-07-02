using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPMotionTracker : MonoBehaviour
{
    //record option which can be also set from the outside
    public bool record = false;

    // thread which will listen to udp datagrams
    Thread thread;

    // indicates when a datagram was received and processed
    bool processed_received_datagrams = false;

    // global position variable, which is used to store the received information
    Vector3 position = new Vector3();
    
    // indicates when to stop listening for udp datagrams
    private bool receive_stop = false;

    void Start ()
    {
        //init thread with the reading loop
        thread = new Thread(new ThreadStart(readingLoop));

        //start it
        thread.Start();
    }

    void Update()
    {   
        // check, if a message was received and processed
        if (processed_received_datagrams)
        {
            //set it back to false, so that a new position can be processed here afterwards
            processed_received_datagrams = false;

            //Process received data
            Debug.Log("Received: " + position);
            transform.position = position;

            if(record){
                //TODO
                //record the current position
                //InformationManager.actual_positions.Add(position);
            }
        }
    }

    private void readingLoop()
    {
        // set up the udp client (has to be done within the thread)
        UdpClient udp = new UdpClient(12345);

        // reading loop
        while (!receive_stop)
        {
            // remote host = motion tracker
            IPEndPoint remote_host = new IPEndPoint(IPAddress.Any, 0);

            //stops here until a message was received from the remote host
            byte[] receiveBytes = udp.Receive(ref remote_host);
            
            //after receiving data, convert it to string
            string position_text = Encoding.ASCII.GetString(receiveBytes);

            // TODO...
            //extract real position out of the received string and set the global position variable accordingly
            //position = ...

            //set global bool true which tells the update method that a message was received and processed
            //-> rdy to be used further on in the next update invocation
            processed_received_datagrams = true;
        }
            
    }


    void OnDestroy()
    {
        receive_stop = true;
        thread.Abort();
    }

}


