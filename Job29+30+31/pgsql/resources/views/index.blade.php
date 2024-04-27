<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Main Page</title>
    <link rel="stylesheet" href="https://unpkg.com/leaflet/dist/leaflet.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.3.1/dist/css/bootstrap.min.css"
        integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">

    <link rel="stylesheet" href="{{ asset('css/index.css') }}">
    {{-- <style >
        body {
            background-image: url('https://static.vinwonders.com/production/ho-hoan-kiem-2.jpg');
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
        }

        .list-group-item:hover {
            background-color: rgb(153, 213, 251);
            cursor: pointer;
        }

        .list-group-item:active {
            background-color: rgb(60, 180, 255);
        }

        .text-with-image {
            font-weight: bold;
            color: transparent;
            background-image: url('https://st.quantrimang.com/photos/image/2021/02/04/Hinh-nen-Quoc-Ky-VN-9.jpg');
            background-size: cover;
            -webkit-background-clip: text;
            background-clip: text;
            background-position: center center;
        }
    </style> --}}
</head>

<body>
    <div id="app">
        <div id="Header">
            <div class="container-fluid">
                <div class="row align-items-center h-100">
                    <div class="col-lg-4">
                        <h1 class="text-white">Welcome to <span class="text-with-image">VIETNAM</span></h1>
                    </div>
                    <div class="col-lg-4"></div>
                    <div class="col-lg-4 d-flex align-items-center justify-content-center">
                        <div id="AreaInputGroup" class="input-group input-group-sm d-none">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-sm">Nhập diện tích</span>
                            </div>
                            <input id="AreaInput" type="text" class="form-control" aria-label="Small"
                                aria-describedby="inputGroup-sizing-sm">
                            <button id="SearchArea" style="height: 30px; width: 120px" type="button"
                                class="btn btn-info m-0 p-0">Tìm kiếm</button>
                        </div>
                        <div id="LineInputGroup" class="input-group input-group-sm d-none">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-sm">Nhập khoảng cách</span>
                            </div>
                            <input id="DistanceInput" type="text" class="form-control" aria-label="Small"
                                aria-describedby="inputGroup-sizing-sm">
                            <button id="SearchDistance" style="height: 30px; width: 120px" type="button"
                                class="btn btn-info m-0 p-0">Tìm kiếm</button>
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <main>
            <div class="container-fluid">
                <div class="row" style="height: 90vh;">
                    <div class="col-lg-2 h-100">
                        <div class="provinces-list h-100">
                            <h4 class="text-center text-white">DANH SÁCH TỈNH THÀNH</h4>
                            <ul id="Provinces_List" class="list-group h-75 overflow-auto">
                                @foreach ($provinces as $each)
                                    <li class="list-group-item">{{ $each->name_1 }}</li>
                                @endforeach
                            </ul>
                            <div class="text-center mt-3 btn-group btn-group-toggle" data-toggle="buttons">
                                <label class="btn btn-secondary active">
                                    <input type="radio" value="Point" name="GIS_Type" id="Point"
                                        autocomplete="off" checked>Point
                                </label>
                                <label class="btn btn-secondary">
                                    <input type="radio" value="Line" name="GIS_Type" id="Line"
                                        autocomplete="off"> Line
                                </label>
                                <label class="btn btn-secondary">
                                    <input type="radio" value="Polygon" name="GIS_Type" id="Polygon"
                                        autocomplete="off"> Polygon
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-10 h-100">
                        <div id="mapid" class="w-100 h-100 border rounded"
                            style="border: 5px solid #000 !important;"></div>
                    </div>
                </div>
            </div>
        </main>

        <div id="Footer">

        </div>
    </div>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://unpkg.com/leaflet/dist/leaflet.js"></script>
    <script src="{{ asset('js/index.js') }}"></script>
    <script>
        $(document).ready(function() {
            // Khởi tạo bản đồ:
            var mymap = L.map('mapid').setView([21.0285, 105.8542], 6); // Vị trí Hà Nội

            // Thêm lớp tile (lớp dạng ô vuông) từ OpenStreetMap
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
            }).addTo(mymap);

            // Lấy dữ liệu địa lý của Việt Nam và các tỉnh:
            var vietnam = {!! json_encode($vietnam) !!};
            var provinces = {!! json_encode($provinces) !!};

            // Tạo các nhóm lớp (Layer Group) cho các tỉnh và Việt Nam:
            var provincesLayerGroup = L.layerGroup().addTo(mymap);
            var vietnamLayerGroup = L.layerGroup().addTo(mymap);
            var roadsLayerGroup = L.layerGroup().addTo(mymap);
            var LineHighlightLayerGroup = L.layerGroup().addTo(mymap);

            var coordinatesVietnamProvinces = {
                'Hà Nội': {
                    lat: 21.0285,
                    lon: 105.8542
                },
                'Hồ Chí Minh': {
                    lat: 10.8231,
                    lon: 106.6297
                },
                'Đà Nẵng': {
                    lat: 16.0544,
                    lon: 108.2022
                },
                'Hải Phòng': {
                    lat: 20.8627,
                    lon: 106.6837
                },
                'Cần Thơ': {
                    lat: 10.0451,
                    lon: 105.7469
                },
                'An Giang': {
                    lat: 10.5372,
                    lon: 105.1924
                },
                'Bà Rịa - Vũng Tàu': {
                    lat: 10.4601,
                    lon: 107.1709
                },
                'Bạc Liêu': {
                    lat: 9.2850,
                    lon: 105.7246
                },
                'Bắc Kạn': {
                    lat: 22.1385,
                    lon: 105.8077
                },
                'Bắc Giang': {
                    lat: 21.2715,
                    lon: 106.1946
                },
                'Bắc Ninh': {
                    lat: 21.1860,
                    lon: 106.0560
                },
                'Bến Tre': {
                    lat: 10.2354,
                    lon: 106.3753
                },
                'Bình Dương': {
                    lat: 11.1665,
                    lon: 106.5304
                },
                'Bình Định': {
                    lat: 13.9454,
                    lon: 108.1265
                },
                'Bình Phước': {
                    lat: 11.7519,
                    lon: 106.8860
                },
                'Bình Thuận': {
                    lat: 10.9330,
                    lon: 108.0700
                },
                'Cà Mau': {
                    lat: 9.1769,
                    lon: 105.1500
                },
                'Cao Bằng': {
                    lat: 22.6664,
                    lon: 106.2519
                },
                'Đắk Lắk': {
                    lat: 12.6654,
                    lon: 108.0365
                },
                'Đắk Nông': {
                    lat: 12.0311,
                    lon: 107.6910
                },
                'Điện Biên': {
                    lat: 21.3926,
                    lon: 103.0072
                },
                'Đồng Nai': {
                    lat: 10.9465,
                    lon: 107.1485
                },
                'Đồng Tháp': {
                    lat: 10.5120,
                    lon: 105.0976
                },
                'Gia Lai': {
                    lat: 13.9750,
                    lon: 108.2081
                },
                'Hà Giang': {
                    lat: 22.8337,
                    lon: 104.9833
                },
                'Hà Nam': {
                    lat: 20.5420,
                    lon: 105.9227
                },
                'Hà Tĩnh': {
                    lat: 18.3428,
                    lon: 105.9057
                },
                'Hải Dương': {
                    lat: 20.9375,
                    lon: 106.3206
                },
                'Hậu Giang': {
                    lat: 9.7779,
                    lon: 105.5928
                },
                'Hòa Bình': {
                    lat: 20.6764,
                    lon: 105.3397
                },
                'Hưng Yên': {
                    lat: 20.6463,
                    lon: 106.0519
                },
                'Khánh Hòa': {
                    lat: 12.2388,
                    lon: 109.1963
                },
                'Kiên Giang': {
                    lat: 10.1377,
                    lon: 105.1119
                },
                'Kon Tum': {
                    lat: 14.3505,
                    lon: 107.9619
                },
                'Lai Châu': {
                    lat: 22.3964,
                    lon: 103.4583
                },
                'Lâm Đồng': {
                    lat: 11.9325,
                    lon: 108.4306
                },
                'Lạng Sơn': {
                    lat: 21.8500,
                    lon: 106.7500
                },
                'Lào Cai': {
                    lat: 22.4167,
                    lon: 103.9833
                },
                'Long An': {
                    lat: 10.7400,
                    lon: 106.2500
                },
                'Nam Định': {
                    lat: 20.4330,
                    lon: 106.1771
                },
                'Nghệ An': {
                    lat: 19.6258,
                    lon: 105.6121
                },
                'Ninh Bình': {
                    lat: 20.2525,
                    lon: 105.9750
                },
                'Ninh Thuận': {
                    lat: 11.7666,
                    lon: 108.3574
                },
                'Phú Thọ': {
                    lat: 21.4241,
                    lon: 105.2351
                },
                'Phú Yên': {
                    lat: 13.1113,
                    lon: 109.1126
                },
                'Quảng Bình': {
                    lat: 17.5367,
                    lon: 106.2405
                },
                'Quảng Nam': {
                    lat: 15.8700,
                    lon: 108.3500
                },
                'Quảng Ngãi': {
                    lat: 15.1219,
                    lon: 108.8000
                },
                'Quảng Ninh': {
                    lat: 21.0167,
                    lon: 107.2500
                },
                'Quảng Trị': {
                    lat: 16.6667,
                    lon: 107.2500
                },
                'Sóc Trăng': {
                    lat: 9.5984,
                    lon: 105.9781
                },
                'Sơn La': {
                    lat: 21.1667,
                    lon: 104.1667
                },
                'Tây Ninh': {
                    lat: 11.3029,
                    lon: 106.0996
                },
                'Thái Bình': {
                    lat: 20.4425,
                    lon: 106.3378
                },
                'Thái Nguyên': {
                    lat: 21.5928,
                    lon: 105.8442
                },
                'Thanh Hóa': {
                    lat: 19.8000,
                    lon: 105.7667
                },
                'Thừa Thiên Huế': {
                    lat: 16.4667,
                    lon: 107.6000
                },
                'Tiền Giang': {
                    lat: 10.3749,
                    lon: 106.3460
                },
                'Trà Vinh': {
                    lat: 9.9345,
                    lon: 106.3460
                },
                'Tuyên Quang': {
                    lat: 21.8167,
                    lon: 105.2167
                },
                'Vĩnh Long': {
                    lat: 10.2500,
                    lon: 105.9667
                },
                'Vĩnh Phúc': {
                    lat: 21.3086,
                    lon: 105.6044
                },
                'Yên Bái': {
                    lat: 21.7000,
                    lon: 104.8500
                }
            };


            // Hiển thị biên của Việt Nam trên bản đồ:
            var geomVNM = JSON.parse(vietnam[0].geometry);
            geomVNM.coordinates.forEach(function(polygonCoords) {
                var border = L.polyline(polygonCoords[0].map(function(coord) {
                    return [coord[1], coord[0]];
                }), {
                    color: 'red',
                    weight: 1
                });

                border.addTo(vietnamLayerGroup);
            });

            // Bắt sự kiện khi người dùng nhấp vào các tỉnh trên danh sách:
            $('#Provinces_List').on('click', 'li', function(event) {
                var target = $(this).text();
                var pointRadioButton = $('#Point');
                var lineRadioButton = $('#Line');
                var polygonRadioButton = $('#Polygon');

                provincesLayerGroup.clearLayers();
                roadsLayerGroup.clearLayers();

                if (polygonRadioButton.is(':checked')) {
                    renderPolygon(target);
                } else if (lineRadioButton.is(':checked')) {
                    renderLine(target);
                } else if (pointRadioButton.is(':checked')) {
                    renderPoint(target);
                }

            });

            mymap.on('click', function(e) {
                if ($('#Polygon').is(':checked')) {
                    provincesLayerGroup.clearLayers();
                    roadsLayerGroup.clearLayers();
                    getData(e.latlng.lat, e.latlng.lng);
                } else if ($('#Line').is(':checked')) {
                    provincesLayerGroup.clearLayers();
                    LineHighlightLayerGroup.clearLayers();
                    highlightLine_click(e.latlng.lat, e.latlng.lng);
                }
            });

            $('input[name="GIS_Type"]').change(function() {
                var selectedOption = $(this).val(); // Di chuyển khai báo biến vào đây
                $('label.btn').removeClass('active');
                $(this).closest('label.btn').addClass('active');

                $('#AreaInputGroup').addClass('d-none');
                $('#LineInputGroup').addClass('d-none');

                if (selectedOption === 'Polygon') {
                    $('#AreaInputGroup').removeClass('d-none');
                } else if (selectedOption === 'Line') {
                    $('#LineInputGroup').removeClass('d-none');
                }
            });


            // Bắt sự kiện khi người dùng nhấp vào một mục trên danh sách:
            $('.list-group-item').on('click', function() {
                $('.list-group-item').removeClass('active');
                $(this).addClass('active');
            });

            // Thực hiện validate:
            $("#AreaInput").on("input", function() {
                var inputValue = $(this).val();
                if (!/^\d*\.?\d*$/.test(inputValue)) {
                    alert("Vui lòng chỉ nhập số hoặc số thập phân.");
                    $(this).val("");
                }
            });

            // Kiểm tra và hạn chế nhập liệu cho ô nhập diện tích:
            $("#SearchArea").click(function() {
                var inputValue = $("#AreaInput").val();
                searchArea(inputValue);
            });
            $("#SearchDistance").click(function() {
                var inputValue = $("#DistanceInput").val();
                searchDistance(inputValue);
            });

            // Xử lý khi người dùng click vào tỉnh thành trên danh sách
            function renderPolygon(target) {
                provinces.forEach(function(province) {
                    if (province.name_1 === target) {
                        var geom = JSON.parse(province.geometry);
                        L.geoJSON(geom, {
                            style: {
                                fillColor: 'yellow',
                                fillOpacity: 0.7,
                                color: 'black',
                                weight: 2
                            }
                        }).addTo(provincesLayerGroup).bindPopup(province.name_1);
                    }
                });
            }

            function renderLine(target) {
                var provinceCoordinates = coordinatesVietnamProvinces[target];
                $.ajax({
                    url: '/get-line',
                    type: 'GET',
                    data: {
                        lat: provinceCoordinates.lat,
                        lon: provinceCoordinates.lon
                    },
                    success: function(response) {
                        response.forEach(function(item) {
                            var Roads = JSON.parse(item.road);
                            const roadCoordinates = Roads.coordinates;
                            roadCoordinates.forEach(function(coords) {
                                // Khai báo một mảng mới để lưu trữ tọa độ của polyline
                                var polylineCoords = [];

                                // Lặp qua từng cặp tọa độ trong tập hợp coords
                                for (var coord of coords) {
                                    // Đảo vị trí tọa độ (dữ liệu trả về dạng [long, lat] nhưng Leaflet cần [lat, long])
                                    polylineCoords.push([coord[1], coord[0]]);
                                }

                                // Tạo một polyline từ tọa độ và thêm nó vào roadsLayerGroup
                                L.polyline(polylineCoords, {
                                    color: 'green',
                                    weight: 3
                                }).addTo(roadsLayerGroup);
                            });
                        });
                    },
                    error: function(xhr) {
                        console.log('Error:', xhr);
                    }
                });
            }

            function renderPoint(target) {
                provincesLayerGroup.clearLayers();
                provinces.forEach(function(province) {
                    if (province.name_1 === target) {
                        var lat = parseFloat(province.lat);
                        var lon = parseFloat(province.lon);
                        L.marker([lat, lon]).addTo(provincesLayerGroup).bindPopup(province.name_1);
                    }
                });
            }

            // Hàm xử lý và render Polygon khi người dùng click vào điểm trên bản đồ
            function getData(lat, lng) {
                $.ajax({
                    url: '/get-data',
                    type: 'GET',
                    data: {
                        lat: lat,
                        lng: lng
                    },
                    success: function(response) {
                        provincesLayerGroup.clearLayers();
                        var provinceName = response[0].name_1;
                        provinces.forEach(function(province) {
                            if (province.name_1 === provinceName) {
                                var geom = JSON.parse(province.geometry);
                                L.geoJSON(geom, {
                                        style: {
                                            fillColor: 'yellow',
                                            fillOpacity: 0.7,
                                            color: 'black',
                                            weight: 2
                                        }
                                    }).addTo(provincesLayerGroup).bindPopup(province.name_1)
                                    .bindTooltip(province.name_1, {
                                        permanent: true,
                                        direction: 'top'
                                    });
                            }
                        });
                    },
                    error: function(xhr) {
                        console.log('Error:', xhr);
                    }
                });
            }

            // Hàm xử lý highlight khi click vào đường nào đó
            function highlightLine_click(lat, lng) {
                $.ajax({
                    url: '/get-highlightLine',
                    type: 'GET',
                    data: {
                        lat: lat,
                        lon: lng
                    },
                    success: function(response) {
                        response.forEach(function(item) {
                            var Roads = JSON.parse(item.road);

                            console.log(Roads);
                            const roadCoordinates = Roads.coordinates;
                            roadCoordinates.forEach(function(coords) {
                                // Khai báo một mảng mới để lưu trữ tọa độ của polyline
                                var polylineCoords = [];

                                // Lặp qua từng cặp tọa độ trong tập hợp coords
                                for (var coord of coords) {
                                    // Đảo vị trí tọa độ (dữ liệu trả về dạng [long, lat] nhưng Leaflet cần [lat, long])
                                    polylineCoords.push([coord[1], coord[0]]);
                                }

                                L.polyline(polylineCoords, {
                                    color: 'yellow',
                                    weight: 4
                                }).addTo(LineHighlightLayerGroup);
                            });
                        });
                    },
                    error: function(xhr) {
                        console.log('Error:', xhr);
                    }
                });
            }

            // Xử lý tô màu tỉnh có diện tích lớn hơn diện tích được nhập
            function searchArea(inputValue) {
                provincesLayerGroup.clearLayers();
                LineHighlightLayerGroup.clearLayers();
                roadsLayerGroup.clearLayers();

                $.ajax({
                    url: '/get-area',
                    type: 'GET',
                    data: {
                        area: inputValue
                    },
                    success: function(response) {
                        var provinceName = response;
                        provinceName.forEach(function(pro) {
                            console.log(pro.name_1);
                            provinces.forEach(function(province) {
                                if (province.name_1 === pro.name_1) {
                                    var geom = JSON.parse(province.geometry);
                                    L.geoJSON(geom, {
                                            style: {
                                                fillColor: 'yellow',
                                                fillOpacity: 0.7,
                                                color: 'black',
                                                weight: 2
                                            }
                                        }).addTo(provincesLayerGroup)
                                        .bindTooltip(province.name_1, {
                                            permanent: true,
                                            direction: 'top'
                                        });
                                }
                            });
                        });
                    },
                    error: function(xhr) {
                        console.log('Error:', xhr);
                    }
                });
            }

            // Xử lý tô màu đường có khoảng cách >= input
            function searchDistance(inputValue) {
                provincesLayerGroup.clearLayers();
                LineHighlightLayerGroup.clearLayers();
                roadsLayerGroup.clearLayers();
                $.ajax({
                    url: '/get-distance',
                    type: 'GET',
                    data: {
                        distance: inputValue,
                    },
                    success: function(response) {
                        response.forEach(function(item) {
                            var Roads = JSON.parse(item.road);

                            console.log(Roads);
                            const roadCoordinates = Roads.coordinates;
                            roadCoordinates.forEach(function(coords) {
                                // Khai báo một mảng mới để lưu trữ tọa độ của polyline
                                var polylineCoords = [];

                                // Lặp qua từng cặp tọa độ trong tập hợp coords
                                for (var coord of coords) {
                                    // Đảo vị trí tọa độ (dữ liệu trả về dạng [long, lat] nhưng Leaflet cần [lat, long])
                                    polylineCoords.push([coord[1], coord[0]]);
                                }

                                L.polyline(polylineCoords, {
                                    color: 'blue',
                                    weight: 4
                                }).addTo(roadsLayerGroup);
                            });
                        });
                    },
                    error: function(xhr) {
                        console.log('Error:', xhr);
                    }
                });
            }
        });
    </script>
</body>

</html>
