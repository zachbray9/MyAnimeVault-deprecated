// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("click", e => {
    const isDropdownButton = e.target.matches("[data-dropdown-button]");

    if (!isDropdownButton && e.target.closest('[data-dropdown]') != null)
        return

    let CurrentDropdown
    if (isDropdownButton)
    {
        CurrentDropdown = e.target.closest('[data-dropdown]')
        CurrentDropdown.classList.toggle('active')
    }

    document.querySelectorAll("[data-dropdown].active").forEach(dropdown => {
        if (dropdown === CurrentDropdown)
            return

        dropdown.classList.remove('active')
    })
})