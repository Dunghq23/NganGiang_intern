using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using NumSharp;
using YamlDotNet.RepresentationModel;
using VehicleDetection.src.CSharp.Models;
using System.Diagnostics;

namespace VehicleDetection.src.CSharp.Services
{
    public class YoloV8
    {
        private Dictionary<int, string> classNames;
        private static int test = 0;
        private static int imgWidth, imgHeight;
        private string _modelPath;
        private InferenceSession session;
        private List<int> _modelInput = new List<int>();
        private List<int> _modelOutput = new List<int>();
        private float _confidenceThreshold;
        private float _iouThreshold;
        DetectionResult detectionResult = new DetectionResult();
        private Stopwatch preProcessTime, detectTime, drawTime;
        public YoloV8(string modelPath, string yamlFilePath, float confidenceThreshold = 0.2f, float iouThreshold = 0.5f)
        {
            _modelPath = modelPath;
            session = new InferenceSession(modelPath);
            foreach (var input in session.InputMetadata)
            {
                var inputInfo = input.Value;
                var inputShape = inputInfo.Dimensions.Select(d => d.ToString()).ToArray(); // Chuyển thành mảng để dễ xử lý

                // Duyệt qua từng phần tử của mảng inputShape và thêm vào list
                foreach (var shapeElement in inputShape)
                {
                    if (int.TryParse(shapeElement, out int dimension))
                    {
                        _modelInput.Add(dimension);
                    }
                }
            }
            foreach (var output in session.OutputMetadata)
            {
                var outputInfo = output.Value;
                var outputShape = outputInfo.Dimensions.Select(d => d.ToString()).ToArray();
                // Duyệt qua từng phần tử của mảng inputShape và thêm vào list
                foreach (var shapeElement in outputShape)
                {
                    if (int.TryParse(shapeElement, out int dimension))
                    {
                        _modelOutput.Add(dimension);

                    }
                }
            }
            // Tải các tên class từ file YAML
            classNames = LoadClassNamesFromYaml(yamlFilePath);
            _confidenceThreshold = confidenceThreshold;
            _iouThreshold = iouThreshold;
        }

        private static DenseTensor<float> ConvertToTensor(NDArray ndArray)
        {
            var shape = ndArray.shape;
            var data = ndArray.Data<float>();  // Lấy dữ liệu float từ NDArray

            // Chuyển NDArray thành DenseTensor<float> với kích thước tương ứng
            return new DenseTensor<float>(data.ToArray(), shape);
        }

        private Dictionary<int, string> LoadClassNamesFromYaml(string yamlFilePath)
        {
            var classNames = new Dictionary<int, string>();

            // Đọc nội dung file YAML
            var yamlStream = new YamlStream();
            using (var reader = new StreamReader(yamlFilePath))
            {
                yamlStream.Load(reader);
            }

            // Lấy root node
            var root = (YamlMappingNode)yamlStream.Documents[0].RootNode;

            // Lấy phần 'names' từ file YAML
            var namesNode = (YamlSequenceNode)root.Children[new YamlScalarNode("names")];

            // Lặp qua các phần tử trong 'names' và thêm vào dictionary
            for (int i = 0; i < namesNode.Children.Count; i++)
            {
                classNames.Add(i, namesNode.Children[i].ToString());
            }

            return classNames;
        }

        #region Thực thi trên frame
        public DetectionResult Detect(Mat imagePath)
        {
            if (detectionResult.VehicleCounts != null)
            {
                detectionResult.VehicleCounts.Clear();
            }

            // Tiền xử lý ảnh
            preProcessTime = Stopwatch.StartNew();
            preProcessTime.Start();
            int inputWidth = _modelInput[_modelInput.Count - 2];
            int inputHeight = _modelInput[_modelInput.Count - 1];
            int outputdata1 = _modelOutput[0];
            int outputdata2 = _modelOutput[1];
            int outputdata3 = _modelOutput[2];
            var ndArray = PreprocessImage(imagePath, inputWidth, inputHeight);

            // Chuyển NDArray thành DenseTensor<float>
            var tensor = ConvertToTensor(ndArray);
            preProcessTime.Stop();

            // Tạo một Dictionary để chứa tensor đầu vào
            var inputs = new[] { NamedOnnxValue.CreateFromTensor("images", tensor) };

            detectTime = Stopwatch.StartNew();
            detectTime.Start();
            // Chạy mô hình và nhận kết quả
            using var results = session.Run(inputs);

            Tensor<float> outputTensor = results[0].AsTensor<float>();

            // Chuyển đổi sang ndarray sử dụng NumSharp
            var npOutput = np.array(outputTensor).reshape(outputdata1, outputdata2, outputdata3);

            // Thực hiện squeeze để loại bỏ chiều đầu tiên (1)
            var squeezedOutput = np.squeeze(npOutput);

            // Thực hiện transpose để hoán đổi trục 
            var transposedOutput = np.transpose(squeezedOutput);

            // Gọi hàm Postprocess với dữ liệu đầu ra từ mô hình
            Postprocess(imagePath, transposedOutput, imgWidth, imgHeight, 0.5f, 0.45f);

            this.detectionResult.PreprocessTime = (double)preProcessTime.Elapsed.TotalSeconds;
            this.detectionResult.DetectTime = (double)detectTime.Elapsed.TotalSeconds;
            this.detectionResult.DrawBoxTime = (double)drawTime.Elapsed.TotalSeconds;
            this.detectionResult.TotalTime = (double)preProcessTime.Elapsed.TotalSeconds + (double)detectTime.Elapsed.TotalSeconds + (double)drawTime.Elapsed.TotalSeconds;

            return this.detectionResult;
        }

        private unsafe NDArray PreprocessImage(Mat imagePath, int inputWidth, int inputHeight)
        {
            Mat img = imagePath;
            imgWidth = img.Width;
            imgHeight = img.Height;

            // Pre-allocate the final array with the correct shape
            float[] imageDataArray = new float[1 * 3 * inputHeight * inputWidth];

            // Resize directly to RGB format
            using Mat resizedImage = new Mat();
            CvInvoke.Resize(img, resizedImage, new Size(inputWidth, inputHeight));

            // Get the image data
            Image<Bgr, byte> imgBgr = resizedImage.ToImage<Bgr, byte>();
            byte* ptr = (byte*)imgBgr.MIplImage.ImageData.ToPointer();

            // Process pixels in parallel
            Parallel.For(0, inputHeight, y =>
            {
                int rowOffset = y * inputWidth;
                int stride = imgBgr.MIplImage.WidthStep;
                for (int x = 0; x < inputWidth; x++)
                {
                    int pixelOffset = y * stride + x * 3;
                    // BGR to RGB conversion and normalization in one step
                    // Channels are arranged as R,G,B planes in the final array
                    imageDataArray[0 * inputHeight * inputWidth + rowOffset + x] = ptr[pixelOffset + 2] / 255.0f; // R
                    imageDataArray[1 * inputHeight * inputWidth + rowOffset + x] = ptr[pixelOffset + 1] / 255.0f; // G
                    imageDataArray[2 * inputHeight * inputWidth + rowOffset + x] = ptr[pixelOffset] / 255.0f;     // B
                }
            });

            // Create NDArray with correct shape directly
            var shape = new int[] { 1, 3, inputHeight, inputWidth };
            return np.array(imageDataArray).reshape(shape);
        }
        private void Postprocess(Mat imagePath, NDArray outputData, int imgWidth, int imgHeight, float confidenceThres, float iouThres)
        {
            // Tính toán hệ số tỷ lệ cho tọa độ hộp giới hạn
            int inputWidth = _modelInput[_modelInput.Count - 2]; // Chiều rộng của ảnh đầu vào cho mô hình
            int inputHeight = _modelInput[_modelInput.Count - 1]; // Chiều cao của ảnh đầu vào cho mô hình

            float xFactor = (float)imgWidth / inputWidth;
            float yFactor = (float)imgHeight / inputHeight;

            var boxes = new List<Rectangle>();

            var scores = new List<float>();
            var classIds = new List<int>();

            // Xác định số lượng hàng và cột của dữ liệu đầu ra
            var shape = outputData.shape;
            int rows = shape[0];
            int cols = shape[1];
            int count = 0;
            for (int i = 0; i < rows; i++)
            {
                // Extract the current row
                var row = outputData[i, ":"];

                // Confidence score
                float confidence = row[4].GetValue<float>();

                // Find class with the highest score
                float maxScore = 0;
                int classId = -1;

                for (int j = 4; j < cols; j++)
                {
                    float score = row[j].GetValue<float>();
                    if (score > maxScore)
                    {
                        maxScore = score;
                        classId = j - 4;
                    }
                }

                if (maxScore >= confidenceThres)
                {
                    count++;
                    // Compute bounding box coordinates
                    float x = row[0].GetValue<float>();
                    float y = row[1].GetValue<float>();
                    float w = row[2].GetValue<float>();
                    float h = row[3].GetValue<float>();

                    int left = (int)((x - w / 2) * xFactor);
                    int top = (int)((y - h / 2) * yFactor);
                    int width = (int)(w * xFactor);
                    int height = (int)(h * yFactor);

                    boxes.Add(new Rectangle(left, top, width, height));
                    scores.Add(maxScore);

                    classIds.Add(classId);
                }
            }
            Console.WriteLine("So object: " + count);

            // Áp dụng Non-Maximum Suppression (NMS)
            var indices = DnnInvoke.NMSBoxes(boxes.ToArray(), scores.ToArray(), confidenceThres, iouThres);

            // Chuyển đổi chỉ số thành danh sách các hộp giới hạn
            // Khởi tạo danh sách để lưu các hộp, điểm số, và ID lớp cuối cùng
            var finalBoxes = new List<Rectangle>();
            var finalScores = new List<float>();
            var finalClassIds = new List<int>();

            // Duyệt qua các chỉ số sau NMS và thu thập thông tin
            foreach (var index in indices)
            {
                if (index >= 0 && index < boxes.Count)  // Đảm bảo chỉ số hợp lệ
                {
                    finalBoxes.Add(boxes[index]);
                    finalScores.Add(scores[index]);
                    finalClassIds.Add(classIds[index]);
                }
            }
            detectTime.Stop();
            // Vẽ các hộp giới hạn lên ảnh
            drawTime = Stopwatch.StartNew();
            drawTime.Start();
            DrawBoundingBoxes(imagePath, finalBoxes, finalScores, finalClassIds);
            drawTime.Stop();
        }
        private void DrawBoundingBoxes(Mat imagePath, List<Rectangle> boxes, List<float> scores, List<int> classIds)
        {
            using Bitmap image = imagePath.ToBitmap();  // Chuyển Mat sang Bitmap
            using Graphics graphics = Graphics.FromImage(image);
            var font = new Font("Arial", 18);
            var random = new Random();

            // Lock để đảm bảo không có luồng nào khác vẽ lên ảnh cùng lúc
            lock (image)
            {
                for (int i = 0; i < boxes.Count; i++)
                {
                    var box = boxes[i];
                    var score = scores[i];
                    var classId = classIds[i];

                    Color randomColor = Color.FromArgb(0, 0, 255);
                    var pen = new Pen(randomColor, 6);
                    var brush = new SolidBrush(randomColor);

                    graphics.DrawRectangle(pen, box);
                    string className = classNames.ContainsKey(classId) ? classNames[classId] : "Unknown";

                    if (detectionResult.VehicleCounts.ContainsKey(className))
                    {
                        detectionResult.VehicleCounts[className] += 1;
                    }
                    else
                    {
                        detectionResult.VehicleCounts.Add(className, 1);
                    }

                    string text = $"{className}: {score:0.00}";
                    SizeF textSize = graphics.MeasureString(text, font);
                    var backgroundBrush = new SolidBrush(Color.FromArgb(150, 0, 0, 0));
                    graphics.FillRectangle(backgroundBrush, box.X, box.Y - 20, textSize.Width, textSize.Height);
                    graphics.DrawString(text, font, Brushes.White, box.X, box.Y - 20);
                }
            }

            detectionResult.image = new Bitmap(image);
        }


        #endregion
    }
}