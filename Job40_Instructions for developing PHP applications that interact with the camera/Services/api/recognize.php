<?php
if ($_SERVER['REQUEST_METHOD'] === 'POST') { 
    $documentRoot = $_SERVER['DOCUMENT_ROOT'] . "/Job40/";
    $imagePath = $documentRoot . $_POST['imagePath'];
    $pythonScriptPath = $documentRoot . "Services/Python/FaceRecognition.py";
    $encodingPath = $documentRoot . "Resources/models/encodings.txt";
    $outputPath = $documentRoot . "Resources/data/output.txt"; 

    $command = "python $pythonScriptPath recognize_faces $imagePath $encodingPath $outputPath";
    exec($command);
    // $command = escapeshellcmd("python $pythonScriptPath recognize_faces $imagePath $encodingPath $outputPath");
    // exec($command);

    // Đọc kết quả từ file output
    if (file_exists($outputPath)) {
        $recognizedName = file_get_contents($outputPath);
        echo($recognizedName);
    }
}
?>