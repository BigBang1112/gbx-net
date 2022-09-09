function saveAsFileUnmarshalled(fields, data) {
    saveAsFile(Blazor.platform.readStringField(fields, 0), Blazor.platform.toUint8Array(data));
}

function saveAsFile(fileName, bytes) {
    var blob = new Blob([bytes], { type: "application/octet-stream" });
    var link = document.createElement('a');

    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    link.click();
}