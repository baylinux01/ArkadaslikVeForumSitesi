function updateScroll()
{
    var element = document.getElementById("mesajlasmasaydiv0");
    element.scrollTop = element.scrollHeight;
}
setTimeout(updateScroll(), 0);