package work.ngangiang.camera;

import android.Manifest;
import android.annotation.SuppressLint;
import android.content.ContentValues;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.os.StrictMode;
import android.provider.MediaStore;
import android.util.Log;
import android.view.ScaleGestureDetector;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.LinearLayout;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;
import androidx.camera.core.AspectRatio;
import androidx.camera.core.Camera;
import androidx.camera.core.CameraControl;
import androidx.camera.core.CameraInfo;
import androidx.camera.core.CameraSelector;
import androidx.camera.core.ImageCapture;
import androidx.camera.core.ImageCaptureException;
import androidx.camera.core.Preview;
import androidx.camera.lifecycle.ProcessCameraProvider;
import androidx.camera.view.PreviewView;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import com.google.common.util.concurrent.ListenableFuture;

import java.util.Objects;
import java.util.concurrent.ExecutionException;

import work.ngangiang.camera.Services.FileTransferTask;

public class CameraxActivity extends AppCompatActivity {
    private static final String TAG = "CameraxActivity";
    private static final int REQUEST_PERMISSIONE = 100;
    private PreviewView previewView;
    private ImageCapture imageCapture;
    private CameraControl cameraControl;
    private CameraInfo cameraInfo;
    private ImageButton btnCapture, btnFlash, btnSend, btnClose;
    private int currentAspectRatio = AspectRatio.RATIO_4_3;
    private ProcessCameraProvider cameraProvider;
    private Uri imageUri;


    @SuppressLint("MissingInflatedId")
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_camerax);

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        initViews();
        requestAppPermissions();

        ListenableFuture<ProcessCameraProvider> cameraProviderFuture = ProcessCameraProvider.getInstance(this);
        cameraProviderFuture.addListener(() -> {
            try {
                cameraProvider = cameraProviderFuture.get();
                bindPreview();
            } catch (ExecutionException | InterruptedException e) {
                Log.e(TAG, "Failed to get camera provider", e);
            }
        }, ContextCompat.getMainExecutor(this));

        btnCapture.setOnClickListener(v -> takePhoto());
        btnFlash.setOnClickListener(v -> toggleFlash());
        btnClose.setOnClickListener(v -> {
            bindPreview();
            btnCapture.setVisibility(Button.VISIBLE);
            btnSend.setVisibility(Button.GONE);
            btnClose.setVisibility(Button.GONE);
            findViewById(R.id.linearLayout).setVisibility(LinearLayout.VISIBLE);

        });
        btnSend.setOnClickListener(v -> {
            if (imageUri != null) {
                // Gửi file ảnh
                new FileTransferTask(CameraxActivity.this).execute(imageUri);
                bindPreview();
                btnCapture.setVisibility(Button.VISIBLE);
                btnSend.setVisibility(Button.GONE);
                btnClose.setVisibility(Button.GONE);
                findViewById(R.id.linearLayout).setVisibility(LinearLayout.VISIBLE);
            }
        });

        // Bỏ qua lỗi mạng trên main thread (không khuyến khích trong ứng dụng thực tế)
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder().permitAll().build();
        StrictMode.setThreadPolicy(policy);

    }

    private void initViews() {
        previewView = findViewById(R.id.viewFinder);
        btnCapture = findViewById(R.id.btnCapture);
        btnFlash = findViewById(R.id.btnFlash);
        btnSend = findViewById(R.id.btnSend);
        btnClose = findViewById(R.id.btnClose);

        findViewById(R.id.btnAspectRatio3_4).setOnClickListener(v -> setAspectRatio(AspectRatio.RATIO_4_3)); // 3:4
        findViewById(R.id.btnAspectRatio9_16).setOnClickListener(v -> setAspectRatio(AspectRatio.RATIO_16_9)); // 9:16
    }

    private void requestAppPermissions() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP &&
                (!hasReadPermissions() || !hasWritePermissions() || !hasCameraPermissions())) {
            // Yêu cầu các quyền cần thiết, bao gồm quyền camera
            ActivityCompat.requestPermissions(this,
                    new String[]{
                            Manifest.permission.READ_EXTERNAL_STORAGE,
                            Manifest.permission.WRITE_EXTERNAL_STORAGE,
                            Manifest.permission.CAMERA
                    }, REQUEST_PERMISSIONE);
        }
    }

    private boolean hasCameraPermissions() {
        return ActivityCompat.checkSelfPermission(this, Manifest.permission.CAMERA) == PackageManager.PERMISSION_GRANTED;
    }

    private boolean hasReadPermissions() {
        return ContextCompat.checkSelfPermission(getBaseContext(), Manifest.permission.READ_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED;
    }

    private boolean hasWritePermissions() {
        return ContextCompat.checkSelfPermission(getBaseContext(), Manifest.permission.WRITE_EXTERNAL_STORAGE) == PackageManager.PERMISSION_GRANTED;
    }

    private void bindPreview() {
        if (cameraProvider == null) return; // Add this check

        Preview preview = new Preview.Builder()
                .setTargetAspectRatio(currentAspectRatio)
                .build();
        preview.setSurfaceProvider(previewView.getSurfaceProvider());

        CameraSelector cameraSelector = new CameraSelector.Builder()
                .requireLensFacing(CameraSelector.LENS_FACING_BACK)
                .build();

        imageCapture = new ImageCapture.Builder()
                .setTargetAspectRatio(currentAspectRatio)
                .build();

        cameraProvider.unbindAll();
        Camera camera = cameraProvider.bindToLifecycle(this, cameraSelector, preview, imageCapture);
        cameraControl = camera.getCameraControl();
        cameraInfo = camera.getCameraInfo();

        enablePinchToZoom();
    }

    private void setAspectRatio(int aspectRatio) {
        currentAspectRatio = aspectRatio;
        bindPreview();
    }

    @SuppressLint("ClickableViewAccessibility")
    private void enablePinchToZoom() {
        ScaleGestureDetector scaleGestureDetector = new ScaleGestureDetector(this, new ScaleGestureDetector.SimpleOnScaleGestureListener() {
            @Override
            public boolean onScale(@NonNull ScaleGestureDetector detector) {
                float currentZoomRatio = Objects.requireNonNull(cameraInfo.getZoomState().getValue()).getZoomRatio();
                float delta = detector.getScaleFactor();
                cameraControl.setZoomRatio(currentZoomRatio * delta);
                return true;
            }
        });

        previewView.setScaleType(PreviewView.ScaleType.FIT_CENTER);

        previewView.setOnTouchListener((v, event) -> {
            scaleGestureDetector.onTouchEvent(event);
            return true;
        });
    }

    private void takePhoto() {
        ContentValues contentValues = new ContentValues();
        contentValues.put(MediaStore.MediaColumns.DISPLAY_NAME, "photo_" + System.currentTimeMillis() + ".jpg");
        contentValues.put(MediaStore.MediaColumns.MIME_TYPE, "image/jpeg");
        contentValues.put(MediaStore.Images.Media.RELATIVE_PATH, Environment.DIRECTORY_PICTURES + "/CameraX");

        ImageCapture.OutputFileOptions outputOptions =
                new ImageCapture.OutputFileOptions.Builder(
                        getContentResolver(),
                        MediaStore.Images.Media.EXTERNAL_CONTENT_URI,
                        contentValues
                ).build();

        imageCapture.takePicture(outputOptions, ContextCompat.getMainExecutor(this), new ImageCapture.OnImageSavedCallback() {
            @Override
            public void onImageSaved(@NonNull ImageCapture.OutputFileResults outputFileResults) {
                imageUri = outputFileResults.getSavedUri(); // Gán URI vào biến imageUri

                if (imageUri != null) {
                    String msg = "Photo capture succeeded: " + imageUri.toString();
                    Log.d(TAG, msg);
                } else {
                    Log.e(TAG, "Photo capture succeeded but URI is null");
                }

                // Dừng camera sau khi chụp
                if (cameraProvider != null) {
                    cameraProvider.unbindAll();
                }
                viewSend();
            }

            @Override
            public void onError(@NonNull ImageCaptureException exception) {
                String msg = "Photo capture failed: " + exception.getMessage();
                Log.e(TAG, msg);
                Toast.makeText(getApplicationContext(), msg, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void toggleFlash() {
        int flashMode = imageCapture.getFlashMode() == ImageCapture.FLASH_MODE_OFF
                ? ImageCapture.FLASH_MODE_ON
                : ImageCapture.FLASH_MODE_OFF;

        imageCapture.setFlashMode(flashMode);

        btnFlash.setImageResource(flashMode == ImageCapture.FLASH_MODE_ON
                ? R.drawable.ic_flash_on
                : R.drawable.ic_flash_off);
    }

    private void viewSend() {
        btnCapture.setVisibility(Button.GONE);
        btnSend.setVisibility(Button.VISIBLE);
        btnClose.setVisibility(Button.VISIBLE);
        findViewById(R.id.linearLayout).setVisibility(LinearLayout.GONE);

        btnFlash.setImageResource(R.drawable.ic_flash_off);
    }
}
