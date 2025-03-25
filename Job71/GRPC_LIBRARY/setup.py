from setuptools import setup, find_packages

setup(
    name='yolo_grpc',
    version='0.1.0',
    packages=find_packages(include=['yolo_grpc', 'yolo_grpc.*']),
    install_requires=[
        'protobuf',
        'grpcio-tools',
        'grpcio',
        'opencv-python',
        'ultralytics',
        'numpy',
        'torch',
        'PyYAML',
    ],
    author='Ha Quang Dung',
    author_email='dungha.4work@gmail.com',
    description='A gRPC service for object detection using YOLO',
    long_description=open('README.md', encoding='utf-8').read(),
    long_description_content_type='text/markdown',
    url='https://github.com/<your-github-username>/yolo_grpc',
    classifiers=[
        'Programming Language :: Python :: 3',
        'License :: OSI Approved :: MIT License',
        'Operating System :: OS Independent',
    ],
    python_requires='>=3.7',
)
