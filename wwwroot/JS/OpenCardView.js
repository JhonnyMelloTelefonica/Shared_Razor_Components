function FullView2() {

    document.getElementById("page-home").style.display = "none";

    // document.getElementById("FullImage2").src = Imglink;
    document.getElementById("FullImageView2").style.display = "block";
    // document.getElementById("logoElement").style.display = "none";
    setTimeout(function () {
        document.getElementById("FullImageView2").classList.add("show");
    }, 10);
}

function CloseFullView2() {

    document.getElementById("page-home").style.display = "block";

    document.getElementById("FullImageView2").style.display = "none";

    // document.getElementById("logoElement").style.display = "block";
    document.getElementById("FullImageView2").classList.remove("show");
}
