@extends('layouts.main')

@section('title', 'Danh sách khuôn mặt chưa biết')

@section('content')
    <div class="container">
        <h2>Danh sách khuôn mặt chưa biết</h2>

        <table class="table">
            <thead class="thead-dark">
                <tr>
                    <th scope="col">STT</th>
                    <th scope="col">Hình ảnh</th>
                    <th scope="col">Ngày nhận diện</th>
                    <th scope="col">Huấn luyện</th>
                    <th scope="col">Xóa</th>
                </tr>
            </thead>
            <tbody>
                @foreach ($unknownRecognitions as $recognition)
                    <tr>
                        <td>{{ $recognition['id'] }}</td>
                        <td><img src="{{ $recognition['image'] }}" class="img-thumbnail" alt="Image"
                                style="max-width: 100px;"></td>
                        <td>{{ $recognition['date'] }}</td>
                        <td>
                            <button type="button" class="btn btn-primary btn-train" data-file="{{ $recognition['image'] }}"
                                data-bs-toggle="modal" data-bs-target="#trainModal">
                                Huấn luyện
                            </button>
                        </td>
                        <td>
                            <button class="btn btn-danger btn-delete" data-file="{{ $recognition['image'] }}">
                                <i class="fas fa-trash"></i>
                            </button>
                        </td>
                    </tr>
                @endforeach
            </tbody>
        </table>
    </div>

    <!-- Thêm modal này vào sau phần table -->
    <div class="modal fade" id="trainModal" tabindex="-1" aria-labelledby="trainModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="trainModalLabel">Huấn luyện khuôn mặt</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="trainForm">
                        <div class="mb-3">
                            <label for="faceUserName" class="form-label">Username</label>
                            <input type="text" class="form-control" id="faceUserName" name="faceUserName" required>
                        </div>
                        <input type="hidden" id="recognitionId" name="recognitionId">
                        <button type="submit" id="submit" class="btn btn-primary">Lưu</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

@endsection

@push('scripts')
    <script>
        $(document).ready(function() {
            var filePath;
            $('.btn-delete').click(function(e) {
                e.preventDefault();
                filePath = $(this).data('file');
                var token = "{{ csrf_token() }}";

                $.ajax({
                    url: "{{ route('delete.image') }}",
                    type: 'POST',
                    dataType: 'json',
                    data: {
                        _token: token,
                        filePath: filePath // Chắc chắn rằng đường dẫn được gửi đi đúng định dạng
                    },
                    success: function(response) {
                        if (response.success) {
                            // Tải lại danh sách sau khi xóa thành công
                            location.reload();
                        } else {
                            console.error(response.error);
                        }
                    },
                    error: function(xhr, status, error) {
                        console.error(xhr.responseText);
                        // Xử lý lỗi
                    }
                });
            });


            // Xử lý sự kiện huấn luyện
            $('.btn-train').click(function() {
                var recognitionId = $(this).closest('tr').find('td:first').text();
                $('#recognitionId').val(recognitionId);
                filePath = $(this).data('file');
            });

            // Xử lý sự kiện huấn luyện
            $('#trainForm').submit(function(e) {
                e.preventDefault();
                const username = $('#faceUserName').val();

                if (username !== '') {
                    console.log(filePath);
                    console.log(username);
                    $.ajax({
                        url: '/photo-train-image', // Đường dẫn xử lý huấn luyện
                        method: 'POST',
                        headers: {
                            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content'),
                        },
                        data: {
                            username: username,
                            filePath: filePath
                        },
                        success: function(response) {
                            console.log(response);
                            $('#trainModal').modal('hide');
                            location.reload();
                        },
                        error: function(error) {
                            console.error('Lỗi:', error);
                            // Xử lý lỗi
                        }
                    });
                }
            });
        });
    </script>
@endpush
