using System.Collections;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine;
using extOSC;

namespace IAV
{
  public class TestingHand : MonoBehaviour
  {
    private HandTrackingSolution _handTrackingSolution;
    private OSCTransmitter _transmitter;
    private string address = "/IAV/hand/openness/";
    private OSCMessage _message;
    public bool isThumbUp;
    public float distanceThumbPinky;

    // Start is called before the first frame update
    void Start()
    {
      _handTrackingSolution = GetComponent<HandTrackingSolution>();
      _handTrackingSolution.HandDetected += OnHandDetected;
      _transmitter = gameObject.AddComponent<OSCTransmitter>();
      _transmitter.RemotePort = 9000;
      _transmitter.RemoteHost = "127.0.0.1";
    }

    // Update is called once per frame
    void OnHandDetected()
    {
      Debug.Log(_handTrackingSolution.HandLandmarks.Count);
      distanceThumbPinky = DistanceThumbPinky(_handTrackingSolution.HandLandmarks[0]);
      isThumbUp = IsThumbOutstretched(_handTrackingSolution.HandLandmarks[0]);

      _message = new OSCMessage(address);
      _message.AddValue(OSCValue.Float(distanceThumbPinky));
      _message.AddValue(OSCValue.Int(isThumbUp ? 1 : 0));
      _transmitter.Send(_message);

      Debug.Log(distanceThumbPinky);
      _handTrackingSolution.HandLandmarks.Clear();
    }
  

    private float DistanceThumbPinky(NormalizedLandmarkList landmarks)
    {
      var thumb = landmarks.Landmark[4];
      var pinky = landmarks.Landmark[20];

      Vector3 thumbV = new Vector3(thumb.X, thumb.Y, thumb.Z);
      Vector3 pinkyV = new Vector3(pinky.X, pinky.Y, pinky.Z);

      return Vector3.Distance(thumbV, pinkyV);
    }

    private bool IsThumbOutstretched(NormalizedLandmarkList landmarks)
    {
      var thumbV = new Vector3(landmarks.Landmark[4].X, landmarks.Landmark[4].Y, landmarks.Landmark[4].Z);
      var indexV = new Vector3(landmarks.Landmark[8].X, landmarks.Landmark[8].Y, landmarks.Landmark[8].Z);
      var middleV = new Vector3(landmarks.Landmark[12].X, landmarks.Landmark[12].Y, landmarks.Landmark[12].Z);
      var ringV = new Vector3(landmarks.Landmark[16].X, landmarks.Landmark[16].Y, landmarks.Landmark[16].Z);
      var pinkyV = new Vector3(landmarks.Landmark[20].X, landmarks.Landmark[20].Y, landmarks.Landmark[20].Z);
      var palmV = new Vector3(landmarks.Landmark[0].X, landmarks.Landmark[0].Y, landmarks.Landmark[0].Z);

      var thumbdistance = Vector3.Distance(thumbV, palmV);
      var indexdistance = Vector3.Distance(indexV, palmV);
      var middledistance = Vector3.Distance(middleV, palmV);
      var ringdistance = Vector3.Distance(ringV, palmV);
      var pinkydistance = Vector3.Distance(pinkyV, palmV);

      return thumbdistance > 1.5*indexdistance && 
             thumbdistance > 1.5*middledistance &&
             thumbdistance > 1.5*ringdistance && 
             thumbdistance > 1.5*pinkydistance;
    }
  }


}
