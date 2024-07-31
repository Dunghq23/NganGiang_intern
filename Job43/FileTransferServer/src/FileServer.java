import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;

import java.net.InetAddress;
import java.net.UnknownHostException;

public class FileServer {
    private static final int PORT = 100; // Cổng mà server sẽ lắng nghe

    public static void main(String[] args) {
        // In đường dẫn thư mục làm việc hiện tại
        System.out.println("Thư mục làm việc hiện tại: " + new File(".").getAbsolutePath());

        // Tạo thư mục FileReceived nếu nó không tồn tại
        File receivedDir = new File("FileReceived");
        if (!receivedDir.exists()) {
            if (receivedDir.mkdirs()) {
                System.out.println("Thư mục FileReceived đã được tạo.");
            } else {
                System.err.println("Không thể tạo thư mục FileReceived.");
                return;
            }
        }

        try {
            // Lấy địa chỉ IP của máy tính hiện tại
            InetAddress ip = InetAddress.getLocalHost();
            System.out.println("Địa chỉ IP của máy: " + ip.getHostAddress() + ":" + PORT);
        } catch (UnknownHostException e) {
            System.err.println("Không thể lấy địa chỉ IP: " + e.getMessage());
        }

        try (ServerSocket serverSocket = new ServerSocket(PORT)) {

            while (true) {
                Socket clientSocket = serverSocket.accept();
                System.out.println("Đã kết nối với client: " + clientSocket.getInetAddress().getHostAddress());

                // Nhận tên file từ client
                try (BufferedReader reader = new BufferedReader(new InputStreamReader(clientSocket.getInputStream()));
                     InputStream inputStream = clientSocket.getInputStream();
                     FileOutputStream fileOutputStream = new FileOutputStream(new File(receivedDir, getFileName(reader)))) {
                    // Nhận dữ liệu file
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = inputStream.read(buffer)) != -1) {
                        fileOutputStream.write(buffer, 0, bytesRead);
                    }
                    System.out.println("Ảnh đã được nhận thành công và lưu vào thư mục FileReceived!");
                } catch (Exception e) {
                    System.err.println("Lỗi trong khi nhận ảnh: " + e.getMessage());
                }
                clientSocket.close();
            }
        } catch (Exception e) {
            System.err.println("Lỗi khi khởi động server: " + e.getMessage());
        }
    }

    private static String getFileName(BufferedReader reader) throws IOException {
        return reader.readLine(); // Đọc tên file từ client
    }
}
