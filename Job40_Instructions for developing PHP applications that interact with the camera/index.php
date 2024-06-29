<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Camera Stream</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="assets/css/style.css">
</head>

<body>
    <div class="container">
        <div class="row main">
            <div class="col-md-6">
                <video class="w-100" id="video" autoplay class="img-fluid rounded"></video>
                <div class="controls">
                    <button id="toggleCamera" class="btn btn-primary">Tắt Camera</button>
                    <button id="recognize" class="btn btn-success">Nhận dạng</button>
                </div>
            </div>
            <div class="col-md-6">
                <canvas id="canvas" class="d-none"></canvas>
                <div class="wraper rounded" style="background-color: red;">
                    <img id="photo" class="rounded img-fluid " alt="Chụp ảnh" style="display:none;" />
                    <div id="loadingIndicator" class="d-none" style="text-align: center;">
                        <img src="./assets/images/loading.gif" alt="Loading..." />
                    </div>
                </div>
                <div class="controls">
                    <h1 id="personName" class="d-none"></h1>
                </div>
            </div>
        </div>

    </div>
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js"
        integrity="sha512-v2CJ7UaYy4JwqLDIrZUI/4hqeoQieOmAZNXBeQyjo21dadnwR+8ZaIJVT8EE2iyI61OV8e6M8PP2/4hpQINQ/g=="
        crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script src="assets/js/script.js"></script>
</body>

</html>