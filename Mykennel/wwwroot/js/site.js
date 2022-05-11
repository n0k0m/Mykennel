// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    // Fájlfeltöltés validálása JQuery-vel. Minden #uploadBox -on figyeli az "on change" eseményt.
    $('#uploadBox').on('change', function () {

        // visszaállítja a gombok letiltását, törli a hibaüzeneteket
        $('input[type="submit"]').removeAttr('disabled');
        $('#sizeAlert').remove();
        $('#formatAlert').remove();

        // engedélyezett fájlformátumok és méret mb-ban
        var allowedFormats = ['jpeg', 'jpg', 'png', 'gif', 'bmp'];
        var allowedSize = 1;

        // this.files[0] a kiválasztott fájl
        var fileSize = (this.files[0].size / 1024 / 1024).toFixed(2);

        // hibaüzenetek, ha valami nem volt rendben:
        if (fileSize > allowedSize) {
            $('#uploadBox').after('<span id="sizeAlert" class="text-danger field-validation-error">Maximum file size allowed: ' + allowedSize + ' mb! </span>')
            $('input[type="submit"]').attr('disabled', 'disabled')
        }
        if ($.inArray($(this).val().split('.').pop().toLowerCase(), allowedFormats) == -1) {
            $('#uploadBox').after('<span id="formatAlert" class="text-danger field-validation-error">Only image formats allowed: ' + allowedFormats.join(', ') + '! </span>')
            $('input[type="submit"]').attr('disabled', 'disabled')
        }
    });

    // Bármilyen sendPost stílusú elem elküldi a legközelebbi form-ot, így például gomb helyett használható sima hivatkozásra (<a>..</a>) kattintva is. 
    // Ezt használtam az admin oldalon, ahol a lakatra kattintva zárolható a felhasználó.
   $('.sendPost').click(function () {
         $(this).closest('form')[0].submit();
    });

    // Váltakozó színű alom kártyák, amit végül nem alkalmaztam
    /*
    $('.litter-card').each(function() {
        $(this).css('background-color', getColor());
    });

    function getColor() {
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += Math.floor(Math.random() * 10);
        }
        return color;
    }
    */
});