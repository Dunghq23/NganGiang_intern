$(document).ready(async function () {
    let stream;

    async function startCamera() {
        try {
            stream = await navigator.mediaDevices.getUserMedia({ video: true });
            const video = $('#video')[0];
            video.srcObject = stream;
        } catch (error) {
            console.error('Lỗi khi truy cập camera:', error);
        }
    }

    function stopCamera() {
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
            const video = $('#video')[0];
            video.srcObject = null;
            stream = null;
        }
    }

    $('#TrainButton').click(async function () {
        const username = $('#UsernameInput').val();
        if (username === '') {
            $('.message').removeClass('d-none').addClass('d-block');
            return;
        }

        $('.message').removeClass('d-block').addClass('d-none');

        try {
            await displayMessage('Vui lòng nhìn vào màn hình', 3);
            await takePhoto(1);
            await delay(250);
            await takePhoto(2);
            await delay(250);
            await takePhoto(3);
            await delay(250);
            await takePhoto(4);
            await delay(250);
            await takePhoto(5);

            await $('#image1').attr('src', "ImageTrain/photo_1.png");

            await displayMessage('Vui lòng nhìn thẳng', 1);
            await takePhoto(6);
            await delay(250);
            await takePhoto(7);
            await delay(250);
            await takePhoto(8);
            await delay(250);
            await takePhoto(9);
            await delay(250);
            await takePhoto(10);
            await $('#image2').attr('src', "ImageTrain/photo_6.png");

            await displayMessage('Vui lòng hơi ngẩng mặt', 1);
            await takePhoto(11);
            await delay(250);
            await takePhoto(12);
            await delay(250);
            await takePhoto(13);
            await delay(250);
            await takePhoto(14);
            await delay(250);
            await takePhoto(15);
            await $('#image3').attr('src', "ImageTrain/photo_11.png");

            await displayMessage('Vui lòng hơi cúi mặt', 1);
            await takePhoto(16);
            await delay(250);
            await takePhoto(17);
            await delay(250)
            await takePhoto(18);
            await delay(250);
            await takePhoto(19);
            await delay(250);
            await takePhoto(20);
            await $('#image4').attr('src', "ImageTrain/photo_16.png");

            await displayMessage('Vui lòng nghiêng mặt sang trái', 1);
            await takePhoto(21);
            await delay(250);
            await takePhoto(22);
            await delay(250);
            await takePhoto(23);
            await delay(250);
            await takePhoto(24);
            await delay(250);
            await takePhoto(25);
            await $('#image5').attr('src', "ImageTrain/photo_21.png");

            await displayMessage('Vui lòng nghiêng mặt sang phải', 1);
            await takePhoto(26);
            await delay(250);
            await takePhoto(27);
            await delay(250);
            await takePhoto(28);
            await delay(250);
            await takePhoto(29);
            await delay(250);
            await takePhoto(30);
            await $('#image6').attr('src', "ImageTrain/photo_26.png");

            await displayMessage('Vui lòng ngẩng mặt và nghiêng sang trái', 1);
            await takePhoto(31);
            await delay(250);
            await takePhoto(32);
            await delay(250);
            await takePhoto(33);
            await delay(250);
            await takePhoto(34);
            await delay(250);
            await takePhoto(35);
            await $('#image7').attr('src', "ImageTrain/photo_31.png");

            await displayMessage('Vui lòng ngẩng mặt và nghiêng sang phải', 1);
            await takePhoto(36);
            await delay(250);
            await takePhoto(37);
            await delay(250);
            await takePhoto(38);
            await delay(250);
            await takePhoto(39);
            await delay(250);
            await takePhoto(40);
            await $('#image8').attr('src', "ImageTrain/photo_36.png");

            await displayMessage('Vui lòng cúi mặt và nghiêng sang trái', 1);
            await takePhoto(41);
            await delay(250);
            await takePhoto(42);
            await delay(250);
            await takePhoto(43);
            await delay(250);
            await takePhoto(44);
            await delay(250);
            await takePhoto(45);
            await $('#image9').attr('src', "ImageTrain/photo_41.png");

            await displayMessage('Vui lòng cúi mặt và nghiêng sang phải', 1);
            await takePhoto(46);
            await delay(250);
            await takePhoto(47);
            await delay(250);
            await takePhoto(48);
            await delay(250);
            await takePhoto(49);
            await delay(250);
            await takePhoto(50);
            await $('#image10').attr('src', "ImageTrain/photo_46.png");

            await displayMessage('Đang xử lý', 'Đang xử lý');
            $('.messageInFrame').text("Đang xử lý...").removeClass('d-none').addClass('d-flex');

            await $.ajax({
                url: '/photo-train',
                method: 'POST',
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content'),
                },
                data: { username },
                success: function (response) {
                    console.log(response.commands);
                    $('.messageInFrame').removeClass('d-flex').addClass('d-none');
                },
                error: function (error) {
                    console.error('Lỗi:', error);
                    $('.messageInFrame').text('Đã xảy ra lỗi. Vui lòng thử lại sau.').removeClass('d-none').addClass('d-flex');
                }
            });


            $('.messageInFrame').removeClass('d-flex').addClass('d-none');
        } catch (error) {
            console.error('Lỗi:', error);
        }
    });

    async function delay(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }

    async function takePhoto(count) {
        const video = $('#video')[0];
        const canvas = $('#canvas')[0];
        const context = canvas.getContext('2d');

        canvas.width = video.videoWidth;
        canvas.height = video.videoHeight;

        context.translate(canvas.width, 0);
        context.scale(-1, 1);
        context.drawImage(video, 0, 0, canvas.width, canvas.height);

        const imageBase64 = canvas.toDataURL('image/png');

        try {
            const response = await $.ajax({
                url: '/save-photo-train',
                method: 'POST',
                headers: {
                    'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content'),
                    'Content-Type': 'application/json',
                },
                data: JSON.stringify({ imageBase64, count }),
            });

            console.log('Ảnh đã được lưu:', response.fileUrl);
        } catch (error) {
            console.error('Lỗi:', error);
        }
    }

    async function displayMessage(message, count) {
        $('.messageInFrame').removeClass('d-none').text(message);

        if (!isNaN(count)) {
            $('.messageInFrame').text(message);
            await new Promise(resolve => setTimeout(resolve, 1000));
            for (let i = count; i >= 0; i--) {
                $('.messageInFrame').text(i).addClass('fontLarge');
                await new Promise(resolve => setTimeout(resolve, 1000));
            }
        }

        $('.messageInFrame').text('').removeClass('fontLarge');
        $('.messageInFrame').addClass('d-none');
    }

    // Bắt đầu camera khi tài liệu sẵn sàng
    await startCamera();

    // Dừng camera khi rời khỏi trang
    $(window).on('unload', function () {
        stopCamera();
    });

    // Xóa ảnh khi tải lại trang
    $(window).on('load', function () {
        stopCamera();
        $.ajax({
            url: '/delete-images',
            method: 'GET',
            success: function (response) {
                console.log(response.message);
            },
            error: function (xhr, status, error) {
                console.error('Lỗi:', error);
            }
        });
    });
});
