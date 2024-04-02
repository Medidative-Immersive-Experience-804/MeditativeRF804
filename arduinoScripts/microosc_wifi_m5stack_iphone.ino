
/*
  MicroOSC minimal WiFi UDP code for the M5Stack Atom Lite.
  By Thomas O Fredericks.
  2023-09-20

  WHAT IS DOES
  ======================
  Basic Framework for OSC communication.

  HARDWARE REQUIREMENTS
  ==================
  - M5Stack Atom Lite.

  REQUIRED LIBRARIES
  ==================
  - MicroOsc.
  - M5Atom.
  - WifiManager https://github.com/tzapu/WiFiManager

  REQUIRED CONFIGURATION
  ======================
  - Set myDestinationIp below to the IP of the destination device.
  - Set myDestinationPort below to the listening port of the destination device.

  - Set myPort to the listening port of this device.
  - The IP of this device will be set by the router. It will be printed in the Serial Monitor at the end of setup().


*/

#include <M5Atom.h>
#include <FastLED.h>

CRGB mesPixels[1];

// Load Wi-Fi library
#include <WiFi.h>

// https://github.com/tzapu/WiFiManager
#include <WiFiManager.h>
WiFiManager myWifiManager;

#include <WiFiUdp.h>
WiFiUDP myUdp;

IPAddress myDestinationIp(172, 20, 10, 10);

unsigned int myDestinationPort = 8000;

unsigned int myPort = 8888;
// The IP of this device will be set by the router. It will be printed in the Serial Monitor at the end of setup().

#include <MicroOscUdp.h>
// THE NUMBER 1024 BETWEEN THE < > SYMBOLS  BELOW IS THE MAXIMUM NUMBER OF BYTES RESERVED FOR INCOMMING MESSAGES.
// OUTGOING MESSAGES ARE WRITTEN DIRECTLY TO THE OUTPUT AND DO NOT NEED ANY RESERVED BYTES.
// PROVIDE A POINTER TO UDP, AND THE IP AND PORT FOR OUTGOING MESSAGES.
// DO NOT FORGET THAT THE UDP CONNEXION MUST BE INITIALIZED IN SETUP() WITH THE RECEIVE PORT.
MicroOscUdp<1024> myMicroOsc(&myUdp, myDestinationIp, myDestinationPort);


unsigned long myChronoStart = 0;  // VARIABLE USED TO LIMIT THE SPEED OF THE SENDING OF OSC MESSAGES

CRGB led(0, 0, 0);
double pitch, roll;  // Stores attitude related variables.  存储姿态相关变量
double r_rand = 180 / PI;
void setup() {
// public CRGB ReRGB(0, 0, 0);

// M5.begin(false, false, false);
// Init Atom-Matrix(Initialize serial port, LED matrix).
 M5.begin(true, true, true);  

// Init IMU sensor.  初始化姿态传感器
M5.IMU.Init(); 


  // FastLED.addLeds<WS2812, DATA_PIN, GRB>(mesPixels, 1);

  Serial.begin(9600);
  // Start-up animation
  // Gives time for the USB drivers to settle
  
  // while (millis() < 5000) {
  //   mesPixels[0] = CHSV((millis() / 5) % 255, 255, 255 - (millis() * 255 / 5000));
  //   FastLED.show();
  //   delay(50);
  // }
  // mesPixels[0] = CRGB(0, 0, 0);
  // FastLED.show();

  Serial.println("debug");

  // START WIFI
  // Automatically connect using stored credentials if any.
  // If connection fails, it starts an access point with the specified name ( "AutoConnectAP"),
  // to which you can connect with the specified password ("p455w0rd") to configure the network connection.
  bool res = myWifiManager.autoConnect("AutoConnectAP", "p455w0rd");
  if (res) Serial.println("Conntected to WiFi");
  else Serial.println("Failed to connect to WiFi");

  myUdp.begin(myPort);



  Serial.println();
  Serial.println(__FILE__);
  Serial.print("myDestinationIp: ");
  Serial.println(myDestinationIp);
  Serial.print("myDestinationPort: ");
  Serial.println(myDestinationPort);
  Serial.print("myIp: ");
  Serial.println(WiFi.localIP());
  Serial.print("myPort: ");
  Serial.println(myPort);
}


/****************
  myOnOscMessageReceived is triggered when a message is received
*****************/
void myOnOscMessageReceived(MicroOscMessage& oscMessage) {

  // CHECK THE ADDRESS OF THE OSC MESSAGE
  if (oscMessage.checkOscAddress("/pixel")) {

    int red = oscMessage.nextAsInt();
    int green = oscMessage.nextAsInt();
    int blue = oscMessage.nextAsInt();
    mesPixels[0] = CRGB(red, green, blue);
    FastLED.show();

  } else if (oscMessage.checkOscAddress("/address")) {

    // USE THE FOLLOWING METHODS TO PARSE INDIVIDUAL ARGUMENTS :
    /*
      // PARSE AN INT
      int32_t intArgument = receivedOscMessage.nextAsInt();
      // PARSE AN FLOAT
      float floatArgument = receivedOscMessage.nextAsFloat();
      // PARSE AN STRING
      const char * s = receivedOscMessage.nextAsString();
      // PARSE A BLOB
      const uint8_t* blob;
      uint32_t length = receivedOscMessage.nextAsBlob(&blob);
      // PARSE MIDI
      const uint8_t* midi;
      receivedOscMessage.nextAsMidi(&midi);
    */
  }
}

/*******
  LOOP
********/
void loop() {

  M5.update();

  // TRIGGER myOnOscMessageReceived() IF AN OSC MESSAGE IS RECEIVED :
  myMicroOsc.onOscMessageReceived(myOnOscMessageReceived);


    // IMu Sensors Send Attitude 
 M5.IMU.getAttitude(&pitch,
                       &roll);  // Read the attitude (pitch, heading) of the IMU
                                // and store it in relevant variables.
                             
    double arc = atan2(pitch, roll) * r_rand + 180;
    double val = sqrt(pitch * pitch + roll * roll);

    // Serial.printf("%.2f,%.2f,%.2f,%.2f\n", pitch, roll, arc,
    //               val);  // serial port output the formatted string.  串口输出

    val = (val * 6) > 100 ? 100 : val * 6;
    led = HSVtoRGB(arc, val, 100);
    M5.dis.fillpix(
        led);  // Fill the whole LED lattice with the color obtained according
               // to the attitude. 


 

  // SEND OSC MESSAGES (onButtonPress) :
  if(M5.Btn.isPressed())
  {
    Serial.println(pitch);
    myMicroOsc.sendFloat("/press", pitch);
    // myMicroOsc.sendMessage("/button", "button pressed" , pitch );
    // myMicroOsc.sendString("/stringPitch", pitch);
    // myMicroOsc.sendString("/stringRoll", roll);
    
  }
  // SEND OSC MESSAGES (EVERY 50 MILLISECONDS) :
  if (millis() - myChronoStart >= 50) {  // IF 50 MS HAVE ELLAPSED
    myChronoStart = millis();            // RESTART CHRONO
 
    // USE THE FOLLOWING METHODS TO SEND OSC MESSAGES :
    // myMicroOsc.sendMessage("/imuStuff" , "%.2f,%.2f,%.2f,%.2f\n", pitch, roll, arc, val);
    
    myMicroOsc.sendFloat("/rollF", roll);
    myMicroOsc.sendFloat("/pitchF", pitch);
    myMicroOsc.sendFloat("/arcF", arc);
    myMicroOsc.sendFloat("/valF", val);

    // myMicroOsc.sendMessage("/rollRollM" , roll);
    // myMicroOsc.sendMessage("/pitchRollM" , pitch);
    // myMicroOsc.sendMessage("/arcRollM" , arc);
    // myMicroOsc.sendMessage("/valRollM" , val);

    /*
      // SEND AN INT(32)
      myMicroOsc.sendInt(const char *address, int32_t i);
      // SEND A FLOAT
      myMicroOsc.sendFloat(const char *address, float f);
      // SEND A STRING
      myMicroOsc.sendString(const char *address, const char *str);
      // SEND A BLOB
      myMicroOsc.sendBlob(const char *address, unsigned char *b, int32_t length);
      // SEND DOUBLE
      myMicroOsc.sendDouble(const char *address,double d);
      // SEND MIDI
      myMicroOsc.sendMidi(const char *address,unsigned char *midi);
      // SEND INT64
      myMicroOsc.sendInt64(const char *address, uint64_t h);
      // SEND A MIXED TYPE VARIABLE LENGTH MESSAGE
      myMicroOsc.sendMessage(const char *address, const char *format, ...);
    */
       // SEND A STRING
     
      //  myMicroOsc.sendString("/string" , "stringstringg");
  }

  
}

// / colors and adjust 
CRGB HSVtoRGB(
    uint16_t h, uint16_t s,
    uint16_t
        v) {  // Adjust the color of Atom-Matrix LED Matrix according to posture
              // (optional).ATOM-Matrix LED
    CRGB ReRGB(0, 0, 0);
    int i;
    float RGB_min, RGB_max;
    RGB_max = v * 2.55f;
    RGB_min = RGB_max * (100 - s) / 100.0f;

    i             = h / 60;
    int difs      = h % 60;
    float RGB_Adj = (RGB_max - RGB_min) * difs / 60.0f;

    switch (i) {
        case 0:
            ReRGB.r = RGB_max;
            ReRGB.g = RGB_min + RGB_Adj;
            ReRGB.b = RGB_min;
            break;
        case 1:
            ReRGB.r = RGB_max - RGB_Adj;
            ReRGB.g = RGB_max;
            ReRGB.b = RGB_min;
            break;
        case 2:
            ReRGB.r = RGB_min;
            ReRGB.g = RGB_max;
            ReRGB.b = RGB_min + RGB_Adj;
            break;
        case 3:
            ReRGB.r = RGB_min;
            ReRGB.g = RGB_max - RGB_Adj;
            ReRGB.b = RGB_max;
            break;
        case 4:
            ReRGB.r = RGB_min + RGB_Adj;
            ReRGB.g = RGB_min;
            ReRGB.b = RGB_max;
            break;
        default:  // case 5:
            ReRGB.r = RGB_max;
            ReRGB.g = RGB_min;
            ReRGB.b = RGB_max - RGB_Adj;
            break;
    }
    return ReRGB;
    
}
