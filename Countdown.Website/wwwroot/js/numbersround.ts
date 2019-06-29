// Numbers round script

var target: number;
var availableNumbers: number[] = [];

function onClickNewTarget() {
    target = Math.floor(Math.random() * 900) + 100;

    document.getElementById("target").innerHTML = String(target);
}

function onClickNumber(buttonNumber:Element) {
    var numString = buttonNumber.parentElement.getAttribute("data-number");

    availableNumbers.push(parseInt(numString));
}