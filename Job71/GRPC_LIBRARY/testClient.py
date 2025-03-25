import grpc
from yolo_grpc import image_service_pb2
from yolo_grpc import image_service_pb2_grpc

def get_density(model_path, pathImageProcessed, pathImage):
    channel = grpc.insecure_channel('localhost:50051')
    stub = image_service_pb2_grpc.ImageTransferStub(channel)

    request = image_service_pb2.DensityRequest(
        model_path=model_path,
        pathImageProcessed=pathImageProcessed,
        pathImage=pathImage
    )
    response = stub.CalculateDensity(request)

    print(f"Success: {response.success}")
    print(f"Message: {response.message}")
    print(f"Density: {response.density}%")

if __name__ == "__main__":
    get_density(r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job71\yolov8s-seg.pt", r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job71\Image\img5_mask.jpg", r"D:\Documents\Work\NganGiang\HAQUANGDUNG\Job71\image\img5.jpg")
