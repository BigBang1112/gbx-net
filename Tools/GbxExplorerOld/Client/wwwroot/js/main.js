function resetFileInput() {
    document.getElementById("fileinput").value = "";
}

function onInputText(el) {
    el.style.width = el.value.length + 'ch';
}

// big thanks to ThaumicTom for this piece of code
function rgbaBufferToImage(inputBuffer, imageWidth, imageHeight) {
    const pixelAmount = inputBuffer.length / 4;

    // Throw error if buffer is not divisible by 4
    if (pixelAmount % 1 !== 0)
        throw new Error('inputBuffer not divisible by 4');

    let canvas = document.createElement('canvas');
    canvas.width = imageWidth;
    canvas.height = imageHeight;

    let context = canvas.getContext('2d');
    let imageData = context.createImageData(imageWidth, imageHeight);

    imageData.data.set(inputBuffer);
    context.putImageData(imageData, 0, 0);

    return canvas.toDataURL();
}