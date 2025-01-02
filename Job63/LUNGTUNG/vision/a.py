
"""This application demonstrates how to perform basic operations with the
Google Cloud Vision API.

Example Usage:
python a.py text ./resources/wakeupcat.jpg
python a.py labels ./resources/landmark.jpg
python a.py web ./resources/landmark.jpg
python a.py web-uri http://wheresgus.com/dog.JPG
python a.py web-geo ./resources/city.jpg
python a.py faces-uri gs://your-bucket/file.jpg
python a.py ocr-uri gs://python-docs-samples-tests/HodgeConj.pdf \
gs://BUCKET_NAME/PREFIX/
python a.py object-localization ./resources/puppies.jpg
python a.py object-localization-uri gs://...

For more information,the documentation at
https://cloud.google.com/vision/docs.
"""

import argparse
import re
# Edited by kenneth
from PIL import Image

def show_image(path):
    im = Image.open(path)
    size = (640, 640)
    #im = im.resize(size))
    # reduce image rate
    im.thumbnail(size)
    im.show()

def detect_labels(path):
    """Detects labels in the file"""
    from google.cloud import vision
    
    client = vision.ImageAnnotatorClient()
    
    with io.open(path, 'rb') as image_file:
        content =image_file.read()
        
    image = vision.types.Image(content=content)
    
    response = client.label_detection(image=image)
    labels=response.label_annotations
    print('labels')
    
    for label in labels:
        print(label.description)
    