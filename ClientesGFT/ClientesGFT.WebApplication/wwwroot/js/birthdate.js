(function ($) {
    const date = new Date();
    const year = date.getFullYear();
    const month = date.getMonth();
    const day = date.getDate();

    const nowLess18years = new Date(year - 18, month, day);
    const nowLess100years = new Date(year - 100, month, day);

    document.getElementById("BirthDate").max = nowLess18years.toISOString().split("T")[0];
    document.getElementById("BirthDate").min = nowLess100years.toISOString().split("T")[0];
})(jQuery)