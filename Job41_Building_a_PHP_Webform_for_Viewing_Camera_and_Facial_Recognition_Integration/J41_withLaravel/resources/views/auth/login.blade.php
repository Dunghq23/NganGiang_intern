<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>Đăng nhập</title>
    <!-- Link CSS của Bootstrap -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <!-- Link thư viện jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <!-- CSS tùy chỉnh -->
    <link rel="stylesheet" href="{{ asset('Assets/css/login.css') }}">
</head>

<body>
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">Đăng nhập</div>
                    <div class="card-body">
                        <form method="POST" action="{{ route('login.submit') }}">
                            @csrf
                            <div class="form-group">
                                <label for="username">Tên đăng nhập:</label>
                                <input type="text" class="form-control" id="username" name="username"
                                    value="{{ old('username') }}" required autofocus>
                            </div>
                            <div class="form-group">
                                <label for="password">Mật khẩu:</label>
                                <input type="password" class="form-control" id="password" name="password" required>
                            </div>
                            <button type="submit" class="btn btn-primary">Đăng nhập</button>

                            @error('loginError')
                                <div class="text-danger mt-2">{{ $message }}</div>
                            @enderror
                        </form>
                    </div>
                    <div class="card-footer">
                        <button type="button" class="btn btn-success" id="btnFaceLogin">Đăng nhập bằng khuôn
                            mặt</button>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div id="Recognize" class="recognizeface w-100 h-100 d-none">
                    <div class="wrapper">
                        <video class="w-100" id="video" autoplay class="img-fluid rounded"></video>
                        <div id="loadingIndicator" class="d-none" style="text-align: center;">
                            <img src="./assets/images/loading.gif" alt="Loading..." />
                        </div>
                    </div>
                    <div class="controls">
                        <button id="recognizeButton" class="btn btn-success">Nhận dạng</button>
                        <h1 id="personName" class="d-none"></h1>
                    </div>
                    <canvas id="canvas" class="d-none"></canvas>
                </div>
            </div>
        </div>
    </div>

    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="{{ asset('Assets/js/login.js') }}"></script>
</body>

</html>
