package work.ngangiang.camera.Services;

import android.content.ContentResolver;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Matrix;
import android.net.Uri;
import android.util.Log;

import java.io.IOException;
import java.io.OutputStream;

public class ImageRotator {
    private static final String TAG = "ImageRotator";

    public static void rotateAndFlipImage(ContentResolver contentResolver, Uri uri, int rotationDegrees, boolean flipVertically) {
        try {
            // Đọc ảnh từ URI
            Bitmap bitmap = BitmapFactory.decodeStream(contentResolver.openInputStream(uri));

            // Tạo ma trận để xoay ảnh
            Matrix matrix = new Matrix();
            matrix.postRotate(rotationDegrees);

            // Nếu cần lật ảnh theo trục Y
            if (flipVertically) {
                matrix.postScale(-1, 1, bitmap.getWidth() / 2f, bitmap.getHeight() / 2f);
            }

            Bitmap transformedBitmap = Bitmap.createBitmap(bitmap, 0, 0, bitmap.getWidth(), bitmap.getHeight(), matrix, true);

            // Ghi ảnh đã xoay và lật trở lại vào bộ nhớ
            try (OutputStream outputStream = contentResolver.openOutputStream(uri)) {
                transformedBitmap.compress(Bitmap.CompressFormat.JPEG, 100, outputStream);
            }

            bitmap.recycle();
            transformedBitmap.recycle();
        } catch (IOException e) {
            Log.e(TAG, "Failed to transform image", e);
        }
    }
}
