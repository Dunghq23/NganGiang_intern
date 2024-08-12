from PIL import Image

def Open():
    img1_path = "D:\\HAQUANGDUNG\\FileTransferServerCsharp\\FileReceived\\1.jpg"
    img2_path = "D:\\HAQUANGDUNG\\FileTransferServerCsharp\\FileReceived\\2.jpg"
    img1_output_path = "D:\\HAQUANGDUNG\\FileTransferServerCsharp\\FileReceived\\1_flipped.jpg"
    
    # Open the images
    img1 = Image.open(img1_path)
    img2 = Image.open(img2_path)
    
    # Print properties of the images
    print(f"Image 1 ")
    print(f"  Format: {img1.format}")
    print(f"  Size: {img1.size}")
    print(f"  Mode: {img1.mode}")
    print()
    
    print(f"Image 2 ")
    print(f"  Format: {img2.format}")
    print(f"  Size: {img2.size}")
    print(f"  Mode: {img2.mode}")
    
    # Rotate img1 by 90 degrees
    img1_rotated = img1.transpose(Image.ROTATE_90)
    
    # Save the rotated image
    img1_rotated.save(img1_output_path)
    print(f"Rotated Image 1 saved ")

    print("\nSau khi xoay:\n")
    img1_flipped = Image.open(img1_output_path)
    
    # Print properties of the images
    print(f"Image 1 flipped :")
    print(f"  Format: {img1_flipped.format}")
    print(f"  Size: {img1_flipped.size}")
    print(f"  Mode: {img1_flipped.mode}")
    print()
    
    print(f"Image 2 :")
    print(f"  Format: {img2.format}")
    print(f"  Size: {img2.size}")
    print(f"  Mode: {img2.mode}")

Open()
