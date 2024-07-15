$(document).ready(function () {
    let stream;

    // Hàm bắt đầu camera
    async function startCamera() {
        try {
            stream = await navigator.mediaDevices.getUserMedia({ video: true });
            const video = $('#video')[0];
            video.srcObject = stream;
        } catch (error) {
            console.error('Lỗi khi truy cập camera:', error);
        }
    }

    // Hàm dừng camera
    function stopCamera() {
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
            const video = $('#video')[0];
            video.srcObject = null;
            stream = null;
        }
    }

    // Xử lý sự kiện click vào nút "Đăng nhập bằng khuôn mặt"
    $('#btnFaceLogin').click(function () {
        $('#Recognize').toggle(); // Hiển thị hoặc ẩn phần nhận dạng khuôn mặt
        if (stream) {
            stopCamera();
            $('#Recognize').removeClass('d-block').addClass('d-none');
        } else {
            startCamera();
            $('#Recognize').removeClass('d-none').addClass('d-block');
        }
    });

    // Xử lý sự kiện click vào nút "Nhận dạng"
    $('#recognizeButton').click(function () {
        takephoto();
    });

    function takephoto() {
        // Lấy video và canvas elements
        const video = $('#video')[0];
        const canvas = $('#canvas')[0];
        const context = canvas.getContext('2d');

        // Đặt kích thước canvas bằng với kích thước video
        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        // Đảo ngược hình ảnh nếu cần thiết
        context.translate(canvas.width, 0);
        context.scale(-1, 1);

        // Vẽ khung hình hiện tại của video lên canvas
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        // Lấy dữ liệu ảnh từ canvas dưới dạng base64
        const imageBase64 = canvas.toDataURL('image/png');
        $('#loadingIndicator').removeClass('d-none').addClass('d-block');
        $('#personName').text("");
        // Gửi dữ liệu ảnh base64 đến máy chủ qua AJAX
        $.ajax({
            url: '/save-photo',
            method: 'POST',
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content'),
                'Content-Type': 'application/json'
            },
            data: JSON.stringify({ imageBase64: imageBase64 }),
            success: function (response) {
                console.log('Ảnh đã được lưu:', response.filepath);
                recognizeFace(response.filepath);
            },
            error: function (xhr, status, error) {
                console.error('Lỗi:', error);
            }
        });
    }

    function recognizeFace(imagePath) {
        $.ajax({
            url: '/recognize-face',
            method: 'POST',
            headers: {
                'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content'),
                'Content-Type': 'application/json'
            },
            data: JSON.stringify({
                imagePath: imagePath
            }),
            success: function (response) {
                console.log('Tên người được nhận dạng:', response.recognizedName);
                console.log(response.command);
                $('#personName').text(response.recognizedName).removeClass('d-none').addClass('d-block');
            },
            error: function (xhr, status, error) {
                console.error('Lỗi:', error);
            },
            complete: function () {
                $('#loadingIndicator').removeClass('d-block').addClass('d-none');
            }
        });
    }
});