// Numbers round script

var target: number;

function onClickNewTarget() {
    target = Math.floor(Math.random() * 900) + 100;

    document.getElementById("target-input").setAttribute("value", String(target));
    document.getElementById("target-text").innerHTML = String(target);
}

function onClickGenerateNumbers() {
    var availableNums: number[] = [];

    var rawdatalarge = document.getElementById("num-data").getAttribute("data-large-nums");
    var rawdatasmall = document.getElementById("num-data").getAttribute("data-small-nums");

    var largenums: number[] = JSON.parse(rawdatalarge);
    var smallnums: number[] = JSON.parse(rawdatasmall);

    availableNums = availableNums.concat(largenums);
    availableNums = availableNums.concat(smallnums);
    availableNums = shuffle(availableNums);

    var elements = document.getElementsByClassName("num-input");

    for (var i = 0; i < elements.length; i++) {
        var inputElement = <HTMLInputElement>elements.item(i);
        inputElement.value = String(availableNums.pop());
    }
}

function shuffle<T>(array: T[]) {
    var currentIndex: number = array.length;
    var temporaryValue: T;
    var randomIndex: number;

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