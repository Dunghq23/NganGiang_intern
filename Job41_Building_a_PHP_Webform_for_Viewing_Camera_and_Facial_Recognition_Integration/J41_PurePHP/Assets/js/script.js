$(document).ready(function () {
    // Khai báo các biến và phần tử DOM cần thiết
    const video = $('#video')[0];
    const canvas = $('#canvas')[0];
    const photo = $('#photo')[0];
    const recognizeButton = $('#recognize');
    const toggleCameraButton = $('#toggleCamera');
    const personName = $('#personName');
    let stream;

    // Bắt đầu camera khi tải trang
    startCamera();

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

    // Bắt sự kiện click vào nút Toggle Camera
    toggleCameraButton.click(function () {
        if (stream) {
            stopCamera();
        } else {
            startCamera();
        }
    });

    // Bắt sự kiện click vào nút Take Photo
    recognizeButton.click(function () {
        if (stream) {
            try {
                const imageData = takePhoto();
                // Gửi dữ liệu ảnh lên server bằng Ajax
                $.ajax({
                    type: 'POST',
                    url: 'Services/api/save_photo.php', // Thay đổi đường dẫn tới file PHP lưu ảnh
                    data: { imageBase64: imageData }, // Dữ liệu ảnh dưới dạng base64
                    success: function (response) {
                        const fileName = getFileNameFromResponse(response);
                        photo.src = fileName;
                        photo.style.display = 'block';
                        recognize(fileName);

                    },
                    error: function (xhr, status, error) {
                        handleAjaxError(error);
                    }, complete: function () {
                        console.log("DONE");
                    }
                });

            } catch (error) {
                console.error('Lỗi khi chụp ảnh từ video:', error);
                alert('Đã xảy ra lỗi khi chụp ảnh.');
            }
        }
    });



    function recognize(imagepath) {
        try {
            $('#loadingIndicator').removeClass("d-none");
            $.ajax({
                type: 'POST',
                url: 'Services/api/recognize.php', // Đường dẫn tới file PHP nhận diện
                data: { imagePath: imagepath },
                success: function (response) {
                    personName.text(response).removeClass('d-none').addClass('d-block');
                },
                error: function (xhr, status, error) {
                    console.error('Lỗi: ', error);
                    alert('Đã xảy ra lỗi', error);
                },
                complete: function () {
                    $('#loadingIndicator').addClass("d-none");
                }
            });
        } catch (error) {
            console.error('Lỗi khi chụp ảnh từ video:', error);
            alert('Đã xảy ra lỗi khi chụp ảnh.');
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
        return parts[parts.length - 3] + '/' + parts[parts.length - 2] + '/' + parts[parts.length - 1];
    }

});
