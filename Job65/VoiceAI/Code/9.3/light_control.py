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
import aiy.device._adc as ADC
import aiy.device._led as LED
import threading

detect_state = 0
auto_system_status = 0
led_status = 0

CDS_LEVEL = 1000

def controlSystem(detect_state):
    global auto_system_status
    global led_status
    while detect_state:
        if auto_system_status == True:
            cdsVal = ADC.readSensor(ADC.CDS_CHANNEL)
            TLCD.displayText(str(cdsVal))
                
            if ( cdsVal<CDS_LEVEL and led_status==0 ):
                print("illuminance = %d "%cdsVal)
                LED.controlLed(LED.LED_1, LED.ON)
                sleep(0.1)
                LED.controlLed(LED.LED_2, LED.ON)
                sleep(0.1)
                LED.controlLed(LED.LED_3, LED.ON)
                sleep(0.1)
                LED.controlLed(LED.LED_4, LED.ON)
                led_status = 1
            elif ( cdsVal>=CDS_LEVEL and led_status==1 ):
                LED.controlLed(LED.LED_1, LED.OFF)
                sleep(0.1)
                LED.controlLed(LED.LED_2, LED.OFF)
                sleep(0.1)
                LED.controlLed(LED.LED_3, LED.OFF)
                sleep(0.1)
                LED.controlLed(LED.LED_4, LED.OFF)
                led_status = 0
        else:
            TLCD.clearTextlcd()
            return 0

def process_event(assistant, event):
    global detect_state
    global led_status
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
            aiy.audio.say('ready light control system')
            detect_state = True
            auto_system_status = True
            t = threading.Thread(target=controlSystem, args=(detect_state, ))
            t.daemon = True
            t.start()
        if text == 'system on':
            auto_system_status = False
            assistant.stop_conversation()
            aiy.audio.say('system start')
            LED.controlLed(LED.LED_1, LED.ON)
            sleep(0.1)
            LED.controlLed(LED.LED_2, LED.ON)
            sleep(0.1)
            LED.controlLed(LED.LED_3, LED.ON)
            sleep(0.1)
            LED.controlLed(LED.LED_4, LED.ON)
            led_status = 1
        if text == 'system off':
            auto_system_status = False
            assistant.stop_conversation()
            aiy.audio.say('system off')
            LED.controlLed(LED.LED_1, LED.OFF)
            sleep(0.1)
            LED.controlLed(LED.LED_2, LED.OFF)
            sleep(0.1)
            LED.controlLed(LED.LED_3, LED.OFF)
            sleep(0.1)
            LED.controlLed(LED.LED_4, LED.OFF)
            led_status = 0

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

    ADC.initMcp3208()

    LED.initLed(LED.LED_1)
    LED.initLed(LED.LED_2)
    LED.initLed(LED.LED_3)
    LED.initLed(LED.LED_4)
    TLCD.initTextlcd()

    credentials = aiy.assistant.auth_helpers.get_assistant_credentials()
    with Assistant(credentials) as assistant:
        for event in assistant.start():
            process_event(assistant, event)

if __name__ == '__main__':
    main()
