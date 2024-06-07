document.getElementById('formFile').addEventListener('change', function () {
    var fileExt = this.value.substring(this.value.lastIndexOf('.'));
    if (fileExt !== '.xls' && fileExt !== '.xlsx') {
        document.getElementById('file-error').classList.remove('d-none');
        this.value = '';
    } else {
        document.getElementById('file-error').classList.add('d-none');
    }
});