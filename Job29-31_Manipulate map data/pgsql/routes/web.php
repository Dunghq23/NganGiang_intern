<?php

use Illuminate\Support\Facades\Route;
use Illuminate\Support\Facades\DB;
use App\Http\Controllers\HomeController;

Route::get('/', [HomeController::class, 'index']);
Route::get('/get-data', [HomeController::class, 'getData']);
Route::get('/get-area', [HomeController::class, 'getArea']);
Route::get('/get-line', [HomeController::class, 'getLine']);
Route::get('/get-highlightLine', [HomeController::class, 'highlightLine_click']);
Route::get('/get-distance', [HomeController::class, 'getDistance']);