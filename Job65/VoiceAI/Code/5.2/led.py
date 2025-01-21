#!/usr/bin/env python3

import logging
import platform
import subprocess
import sys
import aiy.assistant.auth_helpers
#print(aiy.assistant.auth_helpers.__file__)

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
import aiy.device._led as LED

# device/led.py
#sys.path.insert(0,'./device')
#import led

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
#print(BASE_DIR)

def process_event(assistant, event):
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
        if text == 'led on':
            assistant.stop_conversation()
            aiy.audio.say('led on')
            LED.controlLed(LED.LED_1, LED.ON)
            sleep(0.5)
            LED.controlLed(LED.LED_2, LED.ON)
            sleep(0.5)
            LED.controlLed(LED.LED_3, LED.ON)
            sleep(0.5)
            LED.controlLed(LED.LED_4, LED.ON)
        elif text == 'led off':
            assistant.stop_conversation()
            aiy.audio.say('led off')
            LED.controlLed(LED.LED_1, LED.OFF)
            sleep(0.5)
            LED.controlLed(LED.LED_2, LED.OFF)
            sleep(0.5)
            LED.controlLed(LED.LED_3, LED.OFF)
            sleep(0.5)
            LED.controlLed(LED.LED_4, LED.OFF)

    elif event.type == EventType.ON_END_OF_UTTERANCE:
        status_ui.status('thinking')

    elif (event.type == EventType.ON_CONVERSATION_TURN_FINISHED
          or event.type == EventType.ON_CONVERSATION_TURN_TIMEOUT
          or event.type == EventType.ON_NO_RESPONSE):
        status_ui.status('ready')

    elif event.type == EventType.ON_ASSISTANT_ERROR and event.args and event.args['is_fatal']:
        sys.exit(1)


def main():
    if platform.machine() == 'armv6l':          # Returns the machine type, e.g. 'i386'.
        print('Cannot run hotword demo on Pi Zero!')
        exit(-1)
    GPIO.setmode(GPIO.BCM)
    GPIO.setwarnings(False)

    LED.initLed(LED.LED_1)
    LED.initLed(LED.LED_2)
    LED.initLed(LED.LED_3)
    LED.initLed(LED.LED_4)

    credentials = aiy.assistant.auth_helpers.get_assistant_credentials()
    with Assistant(credentials) as assistant:
        for event in assistant.start():
            process_event(assistant, event)


if __name__ == '__main__':
    main()
