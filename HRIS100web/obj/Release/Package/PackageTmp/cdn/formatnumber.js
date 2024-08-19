function formatNumber(inputNumber){
    

    var number = parseFloat(inputNumber);
    if (!isNaN(number)) {
        var formattedNumber = number.toLocaleString('en-US', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
        return formattedNumber;
        
    } else {
        return '';
    }


}