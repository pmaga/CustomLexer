function ValidateSize(file) {
    var FileSize = file.files[0].size / 1024 / 1024; 
    if (FileSize > 10) {
        alert('File size exceeds 10 MB');
        $(file).val('');
    } 
}