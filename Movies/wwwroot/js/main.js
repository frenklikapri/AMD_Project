window.getSelectedValues = function (elemId) {
    var sel = document.getElementById(elemId)
    var results = [];
    var i;
    for (i = 0; i < sel.options.length; i++) {
        if (sel.options[i].selected) {
            results[results.length] = sel.options[i].value;
        }
    }
    return results;
};

window.setMultipleValues = function (elemId, values) {
    var val = values.split(',').map(function (item) {
        console.log(item)
        return parseInt(item, 10);
    });
    console.log(val)
    $(`#${elemId}`).val(val);
}