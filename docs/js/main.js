function changeLang() {
    var selectBox = document.getElementById("lang");
    var selectedValue = selectBox.options[selectBox.selectedIndex].value;
    location.href = selectedValue + ".html";
}