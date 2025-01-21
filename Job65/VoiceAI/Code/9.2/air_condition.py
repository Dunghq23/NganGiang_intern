#!/usr/bin/env python3

import logging
import platform
import subprocess
import sys
import aiy.assistant.auth_helpers
from aiy.assistant.library import Assistant
import aiy.audio
import aiy.voicehat
from google.assistant.library.event import EventType
#import aiy.voicehat
import os, random

logging.basicConfig(
    level=logging.INFO,
    format="[%(asctime)s] %(levelname)s:%(name)s:%(message)s"
)

import RPi.GPIO as GPIO
from time import sleep
import aiy.device._textlcd as TLCD
import aiy.device._dht11 as DHT
import aiy.device._fan as FAN
import threading

detect_state = 0
auto_system_status = 0

def controlSystem(detect_state):
    global auto_system_status
    while detect_state:
#        print("status = %d "%auto_system_status)
        if auto_system_status == True:
            tempVal = DHT.readTemp()
            TLCD.displayText(str(tempVal))
            if (tempVal > 27):
                print("TEMP = %0.1f [C] "%tempVal)
                FAN.controlFan(FAN.ON)
            else:
                FAN.controlFan(FAN.OFF)
        else:
            TLCD.clearTextlcd()
            return 0

def process_event(assistant, event):
    global detect_state
    global t
    global auto_system_status
    status_ui = aiy.voicehat.get_status_ui()
#    print("status_ui : %s"%status_ui)
    if event.type == EventType.ON_START_FINISHED:
        status_ui.status('ready')
        if sys.stdout.isatty():
            print('Say "OK, Google" then speak, or press Ctrl+C to quit...')

    elif event.type == EventType.ON_CONVERSATION_TURN_STARTED:
        status_ui.status('listening')
        print("Yes, I'm listening...")

    elif event.type == EventType.ON_RECOGNIZING_SPEECH_FINISHED and event.args:
        print('You said:', event.args['text'])
        text = event.args['text'].lower()
        if text == 'ready system':
            assistant.stop_conversation()
            aiy.audio.say('ready air condition system')
            detect_state = True
            auto_system_status = True
            t = threading.Thread(target=controlSystem, args=(detect_state, ))
            t.daemon = True
            t.start()
        if text == 'system on':
#            detect_state = False
            auto_system_status = False
            assistant.stop_conversation()
            aiy.audio.say('system start')
            sleep(1)
            FAN.controlFan(FAN.ON)
        if text == 'system off':
#            detect_state = False
            auto_system_status = False 
            assistant.stop_conversation()
            aiy.audio.say('system off')
            FAN.controlFan(FAN.OFF)
            
    elif event.type == EventType.ON_END_OF_UTTERANCE:
        status_ui.status('thinking')

    elif (event.type == EventType.ON_CONVERSATION_TURN_FINISHED
          or event.type == EventType.ON_CONVERSATION_TURN_TIMEOUT
          or event.type == EventType.ON_NO_RESPONSE):
        status_ui.status('ready')

    elif event.type == EventType.ON_ASSISTANT_ERROR and event.args and event.args['is_fatal']:
        sys.exit(1)


def main():
    if platform.machine() == 'armv6l':              # Returns the machine type, e.g. 'i386'.
        print('Cannot run hotword demo on Pi Zero!')
        exit(-1)
    GPIO.setmode(GPIO.BCM)
    GPIO.setwarnings(False)

    GPIO.setup(DHT.DHT_PIN, GPIO.IN)

    FAN.initFan(FAN.FAN_PIN1,FAN.FAN_PIN2)
    TLCD.initTextlcd()

    credentials = aiy.assistant.auth_helpers.get_assistant_credentials()
    with Assistant(credentials) as assistant:
        for event in assistant.start():
            process_event(assistant, event)

if __name__ == '__main__':
    main()
