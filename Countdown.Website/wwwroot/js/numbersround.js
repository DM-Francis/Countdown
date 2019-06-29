// Numbers round script
var target;
var availableNumbers = [];
function onClickNewTarget() {
    target = Math.floor(Math.random() * 900) + 100;
    document.getElementById("target").innerHTML = String(target);
}
function onClickNumber(buttonNumber) {
    var numString = buttonNumber.parentElement.getAttribute("data-number");
    availableNumbers.push(parseInt(numString));
}
//# sourceMappingURL=numbersround.js.map