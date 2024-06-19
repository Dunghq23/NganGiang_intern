<!DOCTYPE html>
<html>
<head>
    <title>Form Example with Bootstrap</title>
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .container-form {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            padding: 20px;
        }
        .form-container {
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .result-container {
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <div class="container-form">
        <div class="form-container">
            <form id="sum-form">
                <h2 class="text-center mb-4">Sum Calculator</h2>
                <div class="form-group">
                    <label for="num1">Number 1:</label>
                    <input type="number" id="num1" name="num1" class="form-control" required>
                </div>
                <div class="form-group">
                    <label for="num2">Number 2:</label>
                    <input type="number" id="num2" name="num2" class="form-control" required>
                </div>
                <button type="submit" class="btn btn-primary btn-block">Submit</button>
            </form>
            <div id="result-container" class="result-container"></div>
        </div>
    </div>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script>
        $(document).ready(function() {
            $('#sum-form').submit(function(event) {
                event.preventDefault(); // Ngăn ngừa submit form mặc định

                var num1 = $('#num1').val();
                var num2 = $('#num2').val();

                // Gửi yêu cầu AJAX đến process.php
                $.ajax({
                    type: 'POST',
                    url: 'process.php',
                    data: { num1: num1, num2: num2 },
                    success: function(response) {
                        $('#result-container').html(response); // Hiển thị kết quả vào phần tử HTML
                    },
                    error: function(xhr, status, error) {
                        console.error(error);
                        $('#result-container').html('<div class="alert alert-danger">Failed to execute calculation.</div>');
                    }
                });
            });
        });
    </script>
</body>
</html>
