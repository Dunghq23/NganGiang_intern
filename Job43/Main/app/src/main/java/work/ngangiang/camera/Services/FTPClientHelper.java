package work.ngangiang.camera.Services;

import org.apache.commons.net.ftp.FTP;
import org.apache.commons.net.ftp.FTPClient;
import org.apache.commons.net.ftp.FTPSClient;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;

public class FTPClientHelper {
    private FTPClient ftpClient;

    public FTPClientHelper(boolean useSSL) {
        if (useSSL) {
            ftpClient = new FTPSClient();
            ((FTPSClient) ftpClient).setTrustManager(null); // Bỏ qua xác thực chứng chỉ (chỉ dùng trong môi trường phát triển)
        } else {
            ftpClient = new FTPClient();
        }
    }

    public boolean connect(String server, int port, String username, String password) {
        try {
            ftpClient.connect(server, port);
            return ftpClient.login(username, password);
        } catch (IOException ex) {
            ex.printStackTrace();
            return false;
        }
    }

    public void disconnect() {
        try {
            if (ftpClient.isConnected()) {
                ftpClient.logout();
                ftpClient.disconnect();
            }
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

    public boolean uploadFile(String localFilePath, String remoteFilePath) {
        try (FileInputStream fis = new FileInputStream(localFilePath)) {
            ftpClient.setFileType(FTP.BINARY_FILE_TYPE);
            return ftpClient.storeFile(remoteFilePath, fis);
        } catch (IOException ex) {
            ex.printStackTrace();
            return false;
        }
    }

    public boolean downloadFile(String remoteFilePath, String localFilePath) {
        try (OutputStream os = new FileOutputStream(localFilePath)) {
            ftpClient.setFileType(FTP.BINARY_FILE_TYPE);
            return ftpClient.retrieveFile(remoteFilePath, os);
        } catch (IOException ex) {
            ex.printStackTrace();
            return false;
        }
    }
}
