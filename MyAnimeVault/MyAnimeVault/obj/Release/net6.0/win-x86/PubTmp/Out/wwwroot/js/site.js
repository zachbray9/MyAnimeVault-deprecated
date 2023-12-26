// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//dropdown menu
document.addEventListener("click", e => {
    const isDropdownButton = e.target.matches("[data-dropdown-button]");

    if (!isDropdownButton && e.target.closest('[data-dropdown]') != null)
        return;

    let CurrentDropdown;
    if (isDropdownButton) {
        CurrentDropdown = e.target.closest('[data-dropdown]');
        CurrentDropdown.classList.toggle('active');
    }

    document.querySelectorAll("[data-dropdown].active").forEach(dropdown => {
        if (dropdown === CurrentDropdown)
            return;

        dropdown.classList.remove('active');
    });
});

//UserAnime progress bar
document.addEventListener("DOMContentLoaded", function () {
    //get all progress bars
    const progressBars = document.querySelectorAll(".progress-bar");

    progressBars.forEach((progressBar) => {
        //get watched and total episodes from data attributes
        const watched = parseInt(progressBar.dataset.watched);
        const total = parseInt(progressBar.dataset.total);

        //calculate percentage
        let percentage = 50;
        if (total > 0) {
            percentage = (watched / total) * 100;
        }

        //create a fill element for the progress bar
        const fill = document.createElement("div");
        fill.classList.add("progress-bar-fill");
        fill.style.width = `${percentage}%`;

        progressBar.appendChild(fill);
    });
});
