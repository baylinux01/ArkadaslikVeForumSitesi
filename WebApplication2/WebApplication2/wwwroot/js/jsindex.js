document.getElementById("menu").addEventListener("click", function () {

    if (document.getElementById("absolute").style.display === "none" && window.innerWidth >= 975) {
        document.getElementById("absolute").style.display = "flex";
        document.getElementById("menu").style.marginLeft = "200px";
    }
    else if (document.getElementById("absolute").style.display === "flex") {
        document.getElementById("absolute").style.display = "none";
        document.getElementById("menu").style.marginLeft = "1%";
    }
    else { document.getElementById("absolute").style.display = "flex" }
})