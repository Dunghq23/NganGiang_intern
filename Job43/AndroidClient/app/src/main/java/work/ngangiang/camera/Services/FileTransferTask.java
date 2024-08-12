package work.ngangiang.camera.Services;

import android.content.ContentResolver;
import android.content.Context;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.ImageView;
import android.widget.Toast;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.Socket;

import work.ngangiang.camera.CameraxActivity;

public class FileTransferTask extends AsyncTask<Uri, Void, String> {
    private Uri FILEURI;
    private static final String SERVER_IP = "192.168.1.7"; // Địa chỉ IP của server
    private static final int SERVER_PORT = 100; // Cổng của server
    private Context context;
    private ImageView imageView;

    // Constructor để nhận Context và ImageView
    public FileTransferTask(Context context, ImageView imageView) {
        this.context = context;
        this.imageView = imageView;
    }

    @Override
    protected String doInBackground(Uri... uris) {
        Uri fileUri = uris[0];
        setFILEURI(fileUri);
        String responseMessage = "";
        try (
                Socket socket = new Socket(SERVER_IP, SERVER_PORT);
                InputStream inputStream = context.getContentResolver().openInputStream(fileUri);
                OutputStream outputStream = socket.getOutputStream();
                BufferedInputStream bis = new BufferedInputStream(inputStream);
                BufferedOutputStream bos = new BufferedOutputStream(outputStream);
                BufferedReader reader = new BufferedReader(new InputStreamReader(socket.getInputStream(), "UTF-8"));
        ) {

            if (inputStream == null) {
                Log.e("FileTransferTask", "InputStream is null.");
                return "Luồng dữ liệu không hợp lệ!";
            }

            // Lấy tên file từ URI
            String fileName = getFileName(fileUri);

            // Gửi tên file trước dữ liệu
            bos.write((fileName + "\n").getBytes());
            bos.flush();

            // Gửi dữ liệu file
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = bis.read(buffer)) != -1) {
                bos.write(buffer, 0, bytesRead);
            }
            bos.flush();
            socket.shutdownOutput();

            // Nhận thông điệp từ server
            responseMessage = reader.readLine();

            return responseMessage;
        } catch (Exception e) {
            e.printStackTrace();
            responseMessage = "Gửi file thất bại: " + e.getMessage();
            return responseMessage;
        }
    }

    @Override
    protected void onPostExecute(String result) {
        Toast.makeText(context, result, Toast.LENGTH_SHORT).show();

        if (result.equals("Không có khuôn mặt được tìm thấy")) {
            if (context instanceof CameraxActivity) {
                ((CameraxActivity) context).returnToCamera();
            }
            return;
        };

        // Vẽ bounding box sau khi gửi file
        if (FILEURI != null) {
            Bitmap bitmap = getBitmapFromUri(FILEURI);
            if (bitmap != null) {
                Bitmap bitmapWithBoundingBox = drawBoundingBox(bitmap, result);
                imageView.setVisibility(ImageView.VISIBLE);
                imageView.setImageBitmap(bitmapWithBoundingBox);
            }
        }

        Handler handler = new Handler(Looper.getMainLooper());
        handler.postDelayed(() -> {
            if (context instanceof CameraxActivity) {
                ((CameraxActivity) context).returnToCamera();
            }
        }, 1000); // 1000 milliseconds = 1 second
    }

    private Bitmap getBitmapFromUri(Uri uri) {
        try {
            InputStream inputStream = context.getContentResolver().openInputStream(uri);
            return BitmapFactory.decodeStream(inputStream);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    private Bitmap drawBoundingBox(Bitmap bitmap, String boundingBoxInfo) {
        Bitmap mutableBitmap = bitmap.copy(Bitmap.Config.ARGB_8888, true);
        Canvas canvas = new Canvas(mutableBitmap);
        Paint paint = new Paint();

        // Vẽ hình chữ nhật
        paint.setColor(Color.GREEN);
        paint.setStyle(Paint.Style.STROKE);
        paint.setStrokeWidth(5);

        String[] parts = boundingBoxInfo.split("[(), ]+");
        int y1 = Integer.parseInt(parts[1]);
        int x1 = Integer.parseInt(parts[2]);
        int y2 = Integer.parseInt(parts[3]);
        int x2 = Integer.parseInt(parts[4]);

        canvas.drawRect(x1, y1, x2, y2, paint);

        // Vẽ tên
        paint.setColor(Color.RED);
        paint.setStyle(Paint.Style.FILL);
        paint.setTextSize(50);
        canvas.drawText(parts[0], x2, y2 - 10, paint);

        return mutableBitmap;
    }

    private String getFileName(Uri uri) {
        ContentResolver contentResolver = context.getContentResolver();
        String[] projection = {android.provider.MediaStore.Images.Media.DISPLAY_NAME};
        try (Cursor cursor = contentResolver.query(uri, projection, null, null, null)) {
            if (cursor != null && cursor.moveToFirst()) {
                int nameIndex = cursor.getColumnIndexOrThrow(android.provider.MediaStore.Images.Media.DISPLAY_NAME);
                return cursor.getString(nameIndex);
            }
        }
        return "unknown.jpg";
    }

    public void setFILEURI(Uri FILEURI) {
        this.FILEURI = FILEURI;
    }
}
