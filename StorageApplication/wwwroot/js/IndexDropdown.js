
    var dropdowns = document.querySelectorAll('.dropdownList');

    for (var i = 0; i < dropdowns.length; i++) {
        dropdowns[i].addEventListener('change', function () {
            var selectedOption = this.options[this.selectedIndex];
            var selectedProductId = selectedOption.value;

            for (var j = 0; j < dropdowns.length; j++) {
                if (dropdowns[j].id !== this.id) {
                    var options = dropdowns[j].querySelectorAll('option');
                    for (var k = 0; k < options.length; k++) {
                        var option = options[k];
                        if (option.value === selectedProductId) {
                            option.disabled = true;
                        } else {
                            option.disabled = false;
                        }
                    }
                }
            }
        });
    }








