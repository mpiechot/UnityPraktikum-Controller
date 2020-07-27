// *********************************************************
// UDP SPEECH RECOGNITION; merci an: http://forum.unity3d.com/threads/windows-udp-voice-recognition-server.172758/
// *********************************************************
using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Threading;
// Speech recognition client. Simple speech recognition class which relies on a UDP server.
// The server interfaces the windows speech API which does the actual recording. Paramterisation
// of the grammer should be done directly in the server files. Unfortunately the MONO version
// used by Unity does not directly support the speech API, hence we have to do this via a client / server architecture.
public class SpeechRecognitionClient : MonoBehaviour {
	// the background thead which does the asynchroneous event handling, in this case: processing of the messages received
	// from the UDP server
	private Thread messageThread;
	// the actual udp socket
	private UdpClient client;
	// default port in the server, you need to keep this consistent between the programs
	public int port = 26000;
	// last received text
	private string udpMessage = "";
	//Time Word started
	private DateTime wordStartTime;
	//recognized Word
	public String recognizedWord = "";

	public bool record;
	// local network infos, are obtained via default API calls
	private string localIP = String.Empty;
	private string hostname;
	private bool msgReceived = false;
	
	public void Start()
	{
		Application.runInBackground = true;

		this.messageThread = new Thread( new ThreadStart(this.ReceiveData));
		this.messageThread.IsBackground = true;
		this.messageThread.Start(); 
		this.hostname = Dns.GetHostName();
		IPAddress[] ips = Dns.GetHostAddresses(hostname);
		if (ips.Length > 0)
		{
			this.localIP = ips[0].ToString();
			Debug.Log("speech server assigned to ip : " + localIP);
		}
	}
	// event handling takes place here, this method is assigned to the thread in terms of a delegate
	private void ReceiveData()
	{
		// initialize and reuse the UDP client
		this.client = new UdpClient(this.port);
        // this is the endless listening loop
		while (true)
		{
			try
			{
				IPEndPoint anyIP = new IPEndPoint(IPAddress.Broadcast, this.port);
				byte[] data = client.Receive(ref anyIP);
				this.udpMessage = Encoding.ASCII.GetString(data);
				// obtain data
				if(record){
					this.ParseMsg(udpMessage);
				}
				//Debug.Log(udpMessage);
				String test = GetWord(udpMessage);
				Debug.Log(test);
				this.msgReceived= true;
			}
			catch (Exception err)
			{
				Debug.LogError(err.ToString());
			}
		}
	}

	public string GetRecognizedWord(){
		return recognizedWord;
	}
    
	public DateTime GetWordStartTime(){
		return wordStartTime;
	}

	public string GetLastUDPPackage()
	{
		return this.udpMessage;
	}
	
	void OnDisable()
	{
		if ( this.messageThread != null) 
			this.messageThread.Abort();
		if(client != null)
			this.client.Close();
	}

	public String GetUdpMessage()
	{
		return udpMessage;
	}


	public void Reset()
	{
		msgReceived = false;
		recognizedWord = "";
		udpMessage = "";
	}


	public bool HasMessageReceived()
	{
		return msgReceived;
	}

	private String GetWord(String udpMsg) {
		String[] parts = udpMsg.Split(' ');
		return parts[1];
	}

	private void ParseMsg(String udpMsg){
		string[] parts = udpMsg.Split (' ');
		for(int i = 0; i<parts.Length-1;  i++)
		{
			if(parts[i].Equals("W")){
				recognizedWord = parts[i+1];
			}
			if(parts[i].Equals("ST")){
				//long totalTicks =  0;
				//long.TryParse(parts[i+1] , out totalTicks);
				//wordStartTime = new DateTime(totalTicks);
				
				string[] timeInString = parts[i + 1].Split(':');
				if (timeInString.Length < 7)
				{
					UnityEngine.Debug.LogError("SpeechRecognitionClient: WRONG NUMBER OF TIME MSG COMPONENTS");
					this.wordStartTime = new DateTime(-1, -1, -1, -1, -1, -1, -1);
				}
				else
				{
					int year = -1, month = -1, day = -1, hour = -1, minute = -1, second = -1, msec = -1;
					bool translationWorked = true;
					translationWorked &= int.TryParse(timeInString[0], out year);
					translationWorked &= int.TryParse(timeInString[1], out month);
					translationWorked &= int.TryParse(timeInString[2], out day);
					translationWorked &= int.TryParse(timeInString[3], out hour);
					translationWorked &= int.TryParse(timeInString[4], out minute);
					translationWorked &= int.TryParse(timeInString[5], out second);
					translationWorked &= int.TryParse(timeInString[6], out msec);
					
					if (translationWorked)
						this.wordStartTime = new DateTime(year, month, day, hour, minute, second, msec);
					else
						UnityEngine.Debug.LogError("SpeechRecognitionClient: WRONG FROMAT OF TIME MSG");
				}
			}
		}
	}
}
