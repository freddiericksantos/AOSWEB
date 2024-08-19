function convertDate(inputDateString){
    var dateParts = inputDateString.split('-');
    var year = dateParts[0];
    var month = parseInt(dateParts[1], 10);
    var day = parseInt(dateParts[2], 10);

    // Convert month number to month name
    var months = [
        'January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'
    ];
    var monthName = months[month - 1]; // months are 0-indexed in the array

    // Format the date as "Month Day, Year"
    var formattedDate = monthName + ' ' + day + ', ' + year;
    return formattedDate;
}