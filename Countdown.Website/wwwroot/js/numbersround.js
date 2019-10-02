// Numbers round script

var target;

function onClickNewTarget() {
    target = Math.floor(Math.random() * 900) + 100;

    document.getElementById("target-input").value = String(target);
}

function onClickRandomNumbers() {
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

function onClickLargeSmallNumbers(large, small) {

    var rawdatalarge = document.getElementById("num-data").getAttribute("data-large-nums");
    var rawdatasmall = document.getElementById("num-data").getAttribute("data-small-nums");

    var largenums = JSON.parse(rawdatalarge);
    var smallnums = JSON.parse(rawdatasmall);

    largenums = shuffle(largenums);
    smallnums = shuffle(smallnums);

    var elements = document.getElementsByClassName("num-input");

    var currentElement = 0;
    // Set large numbers
    for (var i = 0; i < large; i++) {
        var inputElement = elements.item(currentElement);
        inputElement.value = String(largenums.pop());
        currentElement++;
    }

    // Set small numbers
    for (var i = 0; i < small; i++) {
        var inputElement = elements.item(currentElement);
        inputElement.value = String(smallnums.pop());
        currentElement++;
    }
}

function shuffle(array) {
    var currentIndex = array.length;
    var temporaryValue;
    var randomIndex;

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

window.addEventListener("load", overrideFormSubmit);

function overrideFormSubmit() {
    var form = document.getElementById("number-data-form");

    form.addEventListener("submit", event => { event.preventDefault(); sendNumberData(form) });
}

function sendNumberData(form) {
    document.getElementById("spinner-solving").hidden = false;
    document.getElementById("solutions").hidden = true;
    clearSolutionList();

    var btns = document.getElementsByClassName("btn")

    for (const key in btns) {
        btns[key].disabled = true;
    }

    $.ajax({
        url: form.action,
        dataType: "text",
        type: form.method,
        contentType: "application/x-www-form-urlencoded",
        data: $(form).serialize(),
        success: sendNumberSuccess,
        error: sendNumberError
    })
}

function sendNumberSuccess(data) {
    console.log(data);
    document.getElementById("solutions").hidden = false;
    document.getElementById("solutions-header").hidden = false;

    clearSolutionList();
    var solData = JSON.parse(data);

    if (solData["closestDiff"] != 0) {
        appendPElementToSolutions("No solutions found!");
        appendPElementToSolutions(`Found results ${solData["closestDiff"]} away.`);
    }

    for (const key in solData["solutions"]) {
        let solution = solData["solutions"][key];
        appendPElementToSolutions(solution);
    }

    disableSpinnerAndReenableButtons();
}

function sendNumberError(e) {
    console.log(e);
    disableSpinnerAndReenableButtons();

    clearSolutionList();
    document.getElementById("solutions").hidden = false;
    document.getElementById("solutions-header").hidden = true;

    appendPElementToSolutions("Something went wrong! Please check the values entered are valid.");

}

function clearSolutionList() {
    var solutionList = document.getElementById("solution-list");
    while (solutionList.hasChildNodes()) {
        solutionList.removeChild(solutionList.firstChild)
    }
}

function appendPElementToSolutions(text) {
    var solutionList = document.getElementById("solution-list");

    var newP = document.createElement("p");
    var textNode = document.createTextNode(text);
    newP.appendChild(textNode);
    solutionList.appendChild(newP);
}

function disableSpinnerAndReenableButtons() {
    document.getElementById("spinner-solving").hidden = true;
    var btns = document.getElementsByClassName("btn");
    for (const key in btns) {
        btns[key].disabled = false;
    }
}