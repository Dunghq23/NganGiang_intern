package work.ngangiang.camera.Services;

import android.content.ContentResolver;
import android.content.Context;
import android.database.Cursor;
import android.net.Uri;
import android.os.AsyncTask;
import android.util.Log;
import android.widget.Toast;

import java.io.BufferedInputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class FileTransferTask extends AsyncTask<Uri, Void, Boolean> {

    private static final String SERVER_IP = "192.168.1.30"; // Địa chỉ IP của server
    private static final int SERVER_PORT = 100; // Cổng của server
    private Context context;

    // Constructor để nhận Context
    public FileTransferTask(Context context) {
        this.context = context;
    }

    @Override
    protected Boolean doInBackground(Uri... uris) {
        Uri fileUri = uris[0];
        try (
                Socket socket = new Socket(SERVER_IP, SERVER_PORT);
                InputStream inputStream = context.getContentResolver().openInputStream(fileUri);
                BufferedInputStream bis = new BufferedInputStream(inputStream);
                OutputStream os = socket.getOutputStream()
        ) {

            if (inputStream == null) {
                Log.e("FileTransferTask", "InputStream is null.");
                return false;
            }

            // Lấy tên file từ URI
            String fileName = getFileName(fileUri);

            // Gửi tên file trước dữ liệu
            os.write((fileName + "\n").getBytes());

            // Gửi dữ liệu file
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = bis.read(buffer)) != -1) {
                os.write(buffer, 0, bytesRead);
            }
            os.flush();
            return true; // Gửi file thành công
        } catch (Exception e) {
            e.printStackTrace();
            return false; // Gửi file thất bại
        }
    }

    @Override
    protected void onPostExecute(Boolean result) {
        if (result) {
            Toast.makeText(context, "File ảnh đã được gửi thành công!", Toast.LENGTH_LONG).show();
        } else {
            Toast.makeText(context, "Không thể gửi file ảnh.", Toast.LENGTH_LONG).show();
        }
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
        return "unknown.jpg"; // Tên mặc định nếu không thể xác định
    }
}
