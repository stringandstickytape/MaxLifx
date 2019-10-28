// This just-barely works.  Just so you know.

#include <ESP8266WiFi.h>
#include <WiFiUdp.h>

#ifndef STASSID
#define STASSID "sid"
#define STAPSK  "pw"
#endif

#define UDP_TX_PACKET_MAX_SIZE 2000

#define PACKET_ALL_DEVICES ((packetBuffer[3] >> 5)& 0x01) && packetBuffer[0] == 0x24
#define PACKET_GET_SERVICE packetBuffer[32] == 0x02 && packetBuffer[33] == 0x00 && packetBuffer[0] == 0x24
#define PACKET_GET_LABEL packetBuffer[32] == 0x17 && packetBuffer[33] == 0x00 && packetBuffer[0] == 0x24
#define PACKET_SET_COLOUR packetBuffer[32] == 0x66 && packetBuffer[33] == 0x00 && packetBuffer[0] == 0x31
#define PACKET_SET_COLOUR_ZONE packetBuffer[32] == 0xF5 && packetBuffer[33] == 0x01


#include "FastLED.h"
#define PIXEL_TYPE WS2812B
#define NUM_LEDS 144
#define DATA_PIN 5

#define BRIGHTNESS_DIVISOR 4

CRGB leds[NUM_LEDS];

unsigned int localPort = 56700;      // local port to listen on

// buffers for receiving and sending data
char packetBuffer[UDP_TX_PACKET_MAX_SIZE + 1]; //buffer to hold incoming packet,
int rd; // the number of bytes read

WiFiUDP Udp;

byte reply[76];

bool flashTest;

long runtime;

uint hue, sat, bri;
int ctr;

void setup() {
  Serial.begin(115200);
  //WiFi.mode(WIFI_STA);
  WiFi.begin(STASSID, STAPSK);
  while (WiFi.status() != WL_CONNECTED) {
    Serial.print('.');
    delay(500);
  }
  Serial.print("Connected! IP address: ");
  Serial.println(WiFi.localIP());
  Serial.printf("UDP server on port %d\n", localPort);
  Udp.begin(localPort);

  FastLED.addLeds<PIXEL_TYPE, DATA_PIN, GRB>(leds, NUM_LEDS); 
  runtime = millis();
}

void loop() {
  // if there's data available, read a packet
  int packetSize = Udp.parsePacket();
  
  if (packetSize) {
    rd = Udp.read(packetBuffer,UDP_TX_PACKET_MAX_SIZE);
    if(rd>0 ) {
      //DumpIP();
      //DumpPacket();

// Process Header
      
//      Serial.print(packetBuffer[2], HEX); // originAddressTaggedProtocol
//      Serial.print(packetBuffer[3], HEX); // originAddressTaggedProtocol
// 4 - 7 are source
// 8 - 15 are targetMacAddress
// 16 - 21 are reserved
// 22 - ackResRequired
// 23 - sequence
// 24 - 31 reserved protocol header
// 32 - 33 are messageType
// 34 - 35 are reserved

      //Serial.print(packetBuffer[32], HEX); // messageType
      //Serial.print(packetBuffer[33], HEX); // messageType

//      Serial.println();
      //Serial.print(((packetBuffer[3] >> 2)& 0x01)); // The final 12 bits are the protocol field. This must be set to 1024 which is 0100 0000 0000 in binary. Now our two bytes are complete.
      //Serial.print(((packetBuffer[3] >> 4)& 0x01)); // The next bit represents the addressable field. This indicates that the next header will be a frame address header. Since all of our frames require this it will always be set to 1.
      //Serial.print(PACKET_ALL_DEVICES); // The next bit represents the tagged field.We want this packet to be processed by all devices that receive it so we need to set it to one (1).
      
      if(PACKET_ALL_DEVICES) {
        //Serial.println("All devices");

        if (PACKET_GET_SERVICE) {
          //Serial.println("GetService packet");  

          for(int i = 0; i < 17; i++) {
            reply[i] = 0;
          }
          
          setPacketMac();
          
          Udp.beginPacket(Udp.remoteIP(),56700);
          Udp.write(reply, 17);
          Udp.endPacket();

          //Serial.println("Reply sent");  
        }
      } else 
        if(packetBuffer[8] == 0x01 && packetBuffer[9] == 0x02 && packetBuffer[10] == 0x03 && packetBuffer[11] == 0x04 && packetBuffer[12] == 0x05 && packetBuffer[13] == 0x06) {
        //Serial.println("Aimed at us");

          if (PACKET_GET_LABEL) {
            //Serial.println("GetLabel packet");  

            for(int i = 0; i < 76; i++) {
              reply[i] = 0;
            }
  
            reply[0] = 0x44;
  
            copyIntoReply(" ESP32\0", 36);
            setPacketMac();
  
            sendReply(76);
          }/* else if (PACKET_SET_COLOUR) {
            //Serial.println("SetColour packet"); 
            // payload = reply + 37
            //Serial.println(packetBuffer[36], HEX); // start of payload
            //Serial.println(packetBuffer[37], HEX); // hsbkcolour
            //Serial.println(packetBuffer[38], HEX); // hsbkcolour
            //Serial.println(packetBuffer[39], HEX); 
            //Serial.println(packetBuffer[40], HEX);
            //Serial.println(packetBuffer[41], HEX);

            hue = (packetBuffer[38] * 256 + packetBuffer[37]); // 0 - 65535
            sat = (packetBuffer[40] * 256 + packetBuffer[39]); // 0 - 65535
            bri = (packetBuffer[42] * 256 + packetBuffer[41]) / BRIGHTNESS_DIVISOR; // 0 - 65535
            uint kel = (packetBuffer[44] * 256 + packetBuffer[43]); // 0 - 65535
            uint tra = (packetBuffer[46] * 256 + packetBuffer[45]); // 0 - 65535, messes with long transitions

            /*Serial.println(hue); 
            Serial.println(sat); 
            Serial.println(bri); 
            Serial.println(kel); 
            Serial.println(tra); 

            leds[0] = CHSV(hue/256, sat/256, bri/256);
            // packetBuffer[36] first byte is reserved
            
             
          } */ else if (packetBuffer[32] == 0xF6 && packetBuffer[33] == 0x01) {
            //Serial.println("ColourZones Enquiry packet"); 

            for(int i = 0; i < 37; i++) {
              reply[i] = 0;
            }
  
            reply[0] = 0x2E;
            reply[36] = NUM_LEDS;
            setPacketMac();
  
            sendReply(37);
          // Serial.println("Reply sent"); 
          } else if (PACKET_SET_COLOUR_ZONE) {

            //byte startZone = PACKET_SET_COLOUR_ZONE_START_ZONE;
            //byte endZone = packetBuffer[37];

            //hue = (packetBuffer[39] * 256 + packetBuffer[38]); // 0 - 65535
            //sat = (packetBuffer[41] * 256 + packetBuffer[40]); // 0 - 65535
            //bri = (packetBuffer[43] * 256 + packetBuffer[42]) / BRIGHTNESS_DIVISOR; // 0 - 65535
            //long kel = (packetBuffer[45] * 256 + packetBuffer[44]); // 0 - 65535
            //long tra = (packetBuffer[47] * 256 + packetBuffer[46]); // 0 - 65535, messes with long transitions
            //byte apply = packetBuffer[48];

            

            for(ctr = 36; ctr < packetSize; ctr = ctr + 14) {
              leds[packetBuffer[ctr]] = CHSV(packetBuffer[ctr+3], packetBuffer[ctr+5], packetBuffer[ctr+7] >> 2);              

              //leds[packetBuffer[ctr]] = flashTest ? CHSV(0,255,63) : CHSV(128,255,63);              
              
              
            }
            //flashTest = !flashTest;
            //Serial.print(".");

            
          } else DumpPacket();
      }
      
      //Serial.println();
    }
  }
  else {//Serial.println("No packet");
             /*
           delay(10); */
  }

    if(millis() - runtime > 5) {
    runtime = millis();
    FastLED.show();
  }
  
}

void setPacketMac() {
    reply[8] = 0x01;
    reply[9] = 0x02;
    reply[10] = 0x03;
    reply[11] = 0x04;
    reply[12] = 0x05;
    reply[13] = 0x06;
}

void copyIntoReply(char* string, int location) {
    int i = 0;
    while (string[i] != 0) reply[location + i++] = string[i];
}

void sendReply(int length) {
    Udp.beginPacket(Udp.remoteIP(),56700);
    Udp.write(reply, length);
    Udp.endPacket();
}

void DumpPacket() {
  for(int ctr = 0; ctr < rd; ctr ++ ) {
        Serial.print(packetBuffer[ctr], HEX);
      }
      Serial.println();
}

void DumpIP() {
      IPAddress remote = Udp.remoteIP();
      for (int i =0; i < 4; i++)
      {
        Serial.print(remote[i], DEC);
        if (i < 3)
        {
          Serial.print(".");
        }
      }
      Serial.print(":");
}

/*
  test (shell/netcat):
  --------------------
	  nc -u 192.168.esp.address 8888
*/
