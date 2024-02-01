function toggle_expandable_nav() {
    document.getElementById("nav-expandable").classList.toggle("shown");
}

function handle_nav_pinned() {
    const nav = document.getElementById("nav-floating");
    if (window.scrollY > 50) {
        nav.classList.add("pinned");
    } else {
        nav.classList.remove("pinned");
    }
}

document.getElementById("expand-nav").onclick = toggle_expandable_nav;
window.addEventListener("scroll", handle_nav_pinned);
