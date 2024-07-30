<?php

namespace App\Models;

use Illuminate\Contracts\Auth\Authenticatable;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Auth\Authenticatable as AuthenticableTrait;

class User extends Model implements Authenticatable
{
    use HasFactory, AuthenticableTrait;

    protected $table = 'dbo.User'; // Tên bảng trong cơ sở dữ liệu

    protected $primaryKey = 'Id_User'; // Khóa chính của bảng

    protected $fillable = [
        'Name',
        'UserName',
        'Password',
    ];

    protected $hidden = [
        'Password',
        'remember_token',
    ];

    // Không sử dụng mutator để băm mật khẩu
}