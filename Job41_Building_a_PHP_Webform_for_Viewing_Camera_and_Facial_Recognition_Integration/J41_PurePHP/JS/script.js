$(document).ready(function() {
    // Khai báo các biến và phần tử DOM cần thiết
    const video = $('#video')[0];
    const canvas = $('#canvas')[0];
    const photo = $('#photo')[0];
    const toggleCameraButton = $('#toggleCamera');
    const takePhotoButton = $('#takePhoto');
    let stream;

    // Bắt đầu camera khi tải trang
    startCamera();

    // Bắt sự kiện click vào nút Toggle Camera
    toggleCameraButton.click(function() {
        if (stream) {
            stopCamera();
        } else {
            startCamera();
        }
    });

    // Bắt sự kiện click vào nút Take Photo
    takePhotoButton.click(function() {
        if (stream) {
            try {
                const imageData = takePhoto();
                // Gửi dữ liệu ảnh lên server bằng Ajax
                $.ajax({
                    type: 'POST',
                    url: 'Services/save_photo.php', // Thay đổi đường dẫn tới file PHP lưu ảnh
                    data: { imageBase64: imageData }, // Dữ liệu ảnh dưới dạng base64
                    success: function(response) {
                        const fileName = getFileNameFromResponse(response);
                        displayPhoto(fileName);
                    },
                    error: function(xhr, status, error) {
                        handleAjaxError(error);
                    }
                });
            } catch (error) {
                console.error('Lỗi khi chụp ảnh từ video:', error);
                alert('Đã xảy ra lỗi khi chụp ảnh.');
            }
        }
    });

    // Hàm bắt đầu camera
    async function startCamera() {
        try {
            stream = await navigator.mediaDevices.getUserMedia({ video: true });
            video.srcObject = stream;
            toggleCameraButton.text('Tắt Camera');
        } catch (error) {
            console.error('Lỗi khi truy cập camera:', error);
        }
    }

    // Hàm dừng camera
    function stopCamera() {
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
            video.srcObject = null;
            stream = null;
            toggleCameraButton.text('Bật Camera');
        }
    }

    // Hàm chụp ảnh từ video và trả về dữ liệu base64
    function takePhoto() {
        const context = canvas.getContext('2d');
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        // Đảo ngược hình ảnh nếu cần thiết
        context.translate(canvas.width, 0);
        context.scale(-1, 1);

        // Vẽ hình ảnh từ video lên canvas
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        // Chuyển dữ liệu thành base64
        const data = canvas.toDataURL('image/png');

        // Đặt lại ma trận biến đổi để không ảnh hưởng đến lần chụp ảnh tiếp theo
        context.setTransform(1, 0, 0, 1, 0, 0);

        return data;
    }

    // Hàm xử lý response từ server để lấy tên file ảnh
    function getFileNameFromResponse(response) {
        const parts = response.split(/[\\\/]/);
        return parts[parts.length - 2] + '/' + parts[parts.length - 1];
    }

    // Hàm hiển thị ảnh sau khi lưu thành công
    function displayPhoto(fileName) {
        photo.src = fileName;
        photo.style.display = 'block';
    }

    // Hàm xử lý lỗi khi gửi Ajax
    function handleAjaxError(error) {
        console.error('Lỗi khi gửi dữ liệu ảnh lên server:', error);
        alert('Đã xảy ra lỗi khi lưu ảnh.');
    }
});
