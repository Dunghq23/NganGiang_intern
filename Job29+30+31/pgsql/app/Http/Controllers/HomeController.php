<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class HomeController extends Controller
{
    public function index(){

        // Thực hiện truy vấn để lấy tọa độ địa lý Vietnam
        $vietnam = DB::table('gadm41_vnm_0')->select('country')->selectRaw('ST_AsGeoJSON(geom) AS geometry')->get();

        // Thực hiện truy vấn để lấy tọa độ của các tỉnh từ bảng 'gadm41_VNM_1'
        $provinces = DB::select("
            SELECT name_1, ST_X(ST_Centroid(geom)) AS lon, ST_Y(ST_Centroid(geom)) AS lat, ST_AsGeoJSON(geom) AS geometry
            FROM gadm41_VNM_1
        ");

        return view('index', compact('vietnam', 'provinces')); // Truyền dữ liệu $provinces vào view
    }

    public function getData(Request $request)
    {
        $lat = $request->input('lat');
        $lng = $request->input('lng');

        // Thực hiện truy vấn để lấy dữ liệu từ DB
        $provinces_geom = DB::select("
            SELECT name_1, ST_AsGeoJSON(geom) AS geometry
            FROM gadm41_vnm_1
            WHERE ST_Contains(geom, ST_MakePoint($lng, $lat))
        ");
        
        return response()->json($provinces_geom);
    }

    public function getArea(Request $request)
    {
        $area = $request->input('area');

        $provinces_geom = DB::select("
            SELECT name_1, ST_Area(geom) AS area
            FROM gadm41_vnm_1
            WHERE ST_Area(geom) >= $area;
        ");
        
        return response()->json($provinces_geom);
    }

    public function getLine(Request $request)
    {
        $lat = $request->input('lat');
        $lng = $request->input('lon');

        $roads = DB::select("
            SELECT g.name_1, ST_ASGeoJSON(o.geom) AS road
            FROM vnm_rdsl_2015_osm AS o
            JOIN gadm41_VNM_1 AS g ON ST_Within(o.geom, g.geom)
            WHERE ST_Within(ST_MakePoint($lng, $lat), g.geom)
        ");
        
        return response()->json($roads);
    }

    public function highlightLine_click(Request $request)
    {
        $lat = $request->input('lat');
        $lng = $request->input('lon');

        $roads = DB::select("
            SELECT ST_ASGeoJSON(geom) AS road
            FROM vnm_rdsl_2015_osm
            WHERE ST_Intersects(geom, ST_Buffer(ST_GeomFromText('POINT($lng $lat)'), 0.0001));
        ");
        
        return response()->json($roads);
    }

    public function getDistance(Request $request)
    {
        $distance = $request->input('distance');

        $roads = DB::select("
            SELECT ST_ASGeoJSON(geom) AS road
            FROM vnm_rdsl_2015_osm
            where shape_leng >= $distance
        ");
        
        return response()->json($roads);
    }
}
