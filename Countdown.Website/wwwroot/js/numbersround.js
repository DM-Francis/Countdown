// Numbers round script
var target;
function onClickNewTarget() {
    target = Math.floor(Math.random() * 900) + 100;
    document.getElementById("target-input").setAttribute("value", String(target));
    document.getElementById("target-text").innerHTML = String(target);
}
function onClickGenerateNumbers() {
    var availableNums = [];
    var rawdatalarge = document.getElementById("num-data").getAttribute("data-large-nums");
    var rawdatasmall = document.getElementById("num-data").getAttribute("data-small-nums");
    var largenums = JSON.parse(rawdatalarge);
    var smallnums = JSON.parse(rawdatasmall);
    availableNums = availableNums.concat(largenums);
    availableNums = availableNums.concat(smallnums);
    availableNums = shuffle(availableNums);
    var elements = document.getElementsByClassName("num-input");
    for (var i = 0; i < elements.length; i++) {
        var inputElement = elements.item(i);
        inputElement.value = String(availableNums.pop());
    }
}
function shuffle(array) {
    var currentIndex = array.length, temporaryValue, randomIndex;
    // While there remain elements to shuffle...
    while (0 !== currentIndex) {
        // Pick a remaining element...
        randomIndex = Math.floor(Math.random() * currentIndex);
        currentIndex -= 1;
        // And swap it with the current element.
        temporaryValue = array[currentIndex];
        array[currentIndex] = array[randomIndex];
        array[randomIndex] = temporaryValue;
    }
    return array;
}
//# sourceMappingURL=numbersround.js.map