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
import aiy.device._buzzer as BUZZER
import aiy.device._pir as PIR
import aiy.device._keypad as KEYPAD
import GPIO_EX
import threading

auto_system_status = 0
buzzer_status = 0
g_counter = 0

inputPasswd = []
passwd = ['2','5','8','0']

#melodyList = [4,4,5,5,4,4,2,4,4,2,2,1,4,4,5,5,4,4,2,4,2,1,2,0]
#noteDurations = [0.5,0.5,0.5,0.5,0.5,0.5,1,0.5,0.5,0.5,0.5,1,
#                 0.5,0.5,0.5,0.5,0.5,0.5,1,0.5,0.5,0.5,0.5,1]

#melodyList = [4,4,5,5,4,4,2,4,4,2,2,1]
#noteDurations = [0.5,0.5,0.5,0.5,0.5,0.5,1,0.5,0.5,0.5,0.5,1]
                 

melodyList      = [7]
noteDurations   = [0.5]

openDoorBeep = [2,0,4,7]
noteDurations1 = [0.5,0.5,0.5,0.5]

errorBeep = [1,1]
noteDurations2 = [0.5,0.5]

def verifyPasswd():
    if passwd == inputPasswd:
        print("passwd correct!!")
        return 0
    else:
        return -1

def inputText(keyData):
    global g_counter

    if keyData == 0:
        inputPasswd.append('0')
        g_counter += 1
    elif keyData == 1:
        inputPasswd.append('1')
        g_counter += 1
    elif keyData == 2:
        inputPasswd.append('2')
        g_counter += 1
    elif keyData == 3:
        inputPasswd.append('3')
        g_counter += 1
    elif keyData == 4:
        inputPasswd.append('4')
        g_counter += 1
    elif keyData == 5:
        inputPasswd.append('5')
        g_counter += 1
    elif keyData == 6:
        inputPasswd.append('6')
        g_counter += 1
    elif keyData == 7:
        inputPasswd.append('7')
        g_counter += 1
    elif keyData == 8:
        inputPasswd.append('8')
        g_counter += 1
    elif keyData == 9:
        inputPasswd.append('9')
        g_counter += 1

def doorSystem():
    global g_counter
    global inputPasswd
    while True:
        keyData = KEYPAD.readKeypad()
        inputText(keyData)

        if g_counter == 4:
            print("passwd : %s"%inputPasswd)
            if verifyPasswd() == 0:
                BUZZER.playBuzzer(openDoorBeep, noteDurations1)
                BUZZER.pwm.stop()
                print("door is opened!!")
            else:
                BUZZER.playBuzzer(errorBeep, noteDurations2)
                BUZZER.pwm.stop()
            
            g_counter = 0
            del inputPasswd[0:4]

def buzzerSystem():
    global buzzer_status
    while True:
        if buzzer_status == True:
            BUZZER.playBuzzer(melodyList, noteDurations)
            sleep(0.5)
        else:
            BUZZER.pwm.stop()
            return 0

def controlSystem():
    global auto_system_status
    while True:
        if auto_system_status == True:
            motionVal = PIR.readPir(PIR.ON)
#            print("motion = %d "%motionVal)
            if motionVal == True:
                print("motion = %d "%motionVal)
                BUZZER.playBuzzer(melodyList, noteDurations)
                sleep(0.5)
            else:
                BUZZER.pwm.stop()
        else:
            return 0

def process_event(assistant, event):
    global buzzer_status
    global t
    global t1
    global t2
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
            aiy.audio.say('ready security system')
            auto_system_status = True
            buzzer_status = False
            t = threading.Thread(target=controlSystem, args=( ))
            t.daemon = True
            t.start()
        if text == 'ready door system':
            auto_system_status = False 
            assistant.stop_conversation()
            aiy.audio.say('ready door system')
            t1 = threading.Thread(target=doorSystem, args=( ))
            t1.daemon = True
            t1.start()
        if text == 'system on':
            auto_system_status = False 
            buzzer_status = True
            assistant.stop_conversation()
            aiy.audio.say('security system start')
            t2 = threading.Thread(target=buzzerSystem, args=( ))
            t2.daemon = True
            t2.start()
#            BUZZER.playBuzzer(melodyList, noteDurations)
        if text == 'system off':
            auto_system_status = False 
            buzzer_status = False
            assistant.stop_conversation()
            aiy.audio.say('system off')
#            BUZZER.pwm.stop()

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

    GPIO.setup(BUZZER.BUZZER_PIN, GPIO.OUT)
    #pwm = GPIO.PWM(BUZZER.BUZZER_PIN, 100)

    GPIO_EX.setup(PIR.PIR_PIN, GPIO_EX.IN)
#    GPIO.setup(PIR.PIR_PIN, GPIO.IN)

    KEYPAD.initKeypad()

    BUZZER.playBuzzer(melodyList, noteDurations)
    #pwm.stop()


    credentials = aiy.assistant.auth_helpers.get_assistant_credentials()
    with Assistant(credentials) as assistant:
        for event in assistant.start():
            process_event(assistant, event)

if __name__ == '__main__':
    main()
