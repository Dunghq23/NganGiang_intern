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
import aiy.device._adc as ADC
import threading

detect_state = 0
on_state = 0
auto_system_status = 0
flag_system_status = 0
GAS_LEVEL = 1500

#melodyList = [4,4,5,5,4,4,2,4,4,2,2,1,4,4,5,5,4,4,2,4,2,1,2,0]
#noteDurations = [0.5,0.5,0.5,0.5,0.5,0.5,1,0.5,0.5,0.5,0.5,1,
#                 0.5,0.5,0.5,0.5,0.5,0.5,1,0.5,0.5,0.5,0.5,1]

#melodyList      = [4,4,4,4,4,4,4,4,4,4]
#noteDurations   = [1,1,1,1,1,1,1,1,1,1]
melodyList      = [4]
noteDurations   = [1]

def controlSystem(detect_state):
    global auto_system_status
    while detect_state:
        if auto_system_status == True:
            gasVal = ADC.readSensor(ADC.GAS_CHANNEL)
            if (gasVal > GAS_LEVEL):
                print("GAS Value = %d "%gasVal)
                BUZZER.playBuzzer(melodyList, noteDurations)
            else:
                BUZZER.pwm.stop()
        else:
            return 0

def onSystem(on_state):
    global flag_system_status
    while on_state:
        if flag_system_status == True:
            BUZZER.playBuzzer(melodyList, noteDurations)
        else:
            BUZZER.pwm.stop()
            return 0

def process_event(assistant, event):
    global detect_state
    global on_state
    global t
    global t1
    global auto_system_status
    global flag_system_status
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
            aiy.audio.say('ready fire detection system')
            detect_state = True
            auto_system_status = True
            t = threading.Thread(target=controlSystem, args=(detect_state, ))
            t.daemon = True
            t.start()
        if text == 'system on':
            auto_system_status = False 
            flag_system_status = True
            on_state = True
            assistant.stop_conversation()
            aiy.audio.say('fire detection system start')
            t1 = threading.Thread(target=onSystem, args=(on_state, ))
#            BUZZER.playBuzzer(melodyList, noteDurations)
            t1.daemon = True
            t1.start()
        if text == 'system off':
            auto_system_status = False 
            flag_system_status = False 
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
    pwm = GPIO.PWM(BUZZER.BUZZER_PIN, 100)

    ADC.initMcp3208()

    credentials = aiy.assistant.auth_helpers.get_assistant_credentials()
    with Assistant(credentials) as assistant:
        for event in assistant.start():
            process_event(assistant, event)

if __name__ == '__main__':
    main()
