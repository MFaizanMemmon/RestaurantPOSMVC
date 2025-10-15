(function () {
    const isCollapsed = localStorage.getItem("sidebarCollapsed") === "true";
    if (isCollapsed) {
        document.documentElement.classList.add("sidebar-collapsed");
    }
})();

// Toggle sidebar and save state in localStorage
$("#toggleSidebar").on("click", function () {
    $("#sidebarMenu").toggleClass("sidebar-collapsed");

    // Save state (true = collapsed, false = expanded)
    const isCollapsed = $("#sidebarMenu").hasClass("sidebar-collapsed");
    localStorage.setItem("sidebarCollapsed", isCollapsed);

    // Toggle icon direction
    const $icon = $("#toggleSidebar .toggle-icon");
    if (isCollapsed) {
        $icon.removeClass("bi-arrow-left-circle").addClass("bi-arrow-right-circle");
    } else {
        $icon.removeClass("bi-arrow-right-circle").addClass("bi-arrow-left-circle");
    }
});

document.addEventListener("DOMContentLoaded", function () {
    const links = document.querySelectorAll("#sidebarMenu .nav-link");

    // --- Restore Sidebar State ---
    const isCollapsed = localStorage.getItem("sidebarCollapsed") === "true";
    if (isCollapsed) {
        $("#sidebarMenu").addClass("sidebar-collapsed");
        $("#toggleSidebar .toggle-icon")
            .removeClass("bi-arrow-left-circle")
            .addClass("bi-arrow-right-circle");
    } else {
        $("#toggleSidebar .toggle-icon")
            .removeClass("bi-arrow-right-circle")
            .addClass("bi-arrow-left-circle");
    }

    // --- Manage Active Link ---
    const activeLink = localStorage.getItem("activeSidebarLink");
    links.forEach(link => {
        link.classList.remove("active");

        if (activeLink && link.getAttribute("href") === activeLink) {
            link.classList.add("active");
        }

        link.addEventListener("click", function () {
            links.forEach(l => l.classList.remove("active"));
            this.classList.add("active");
            localStorage.setItem("activeSidebarLink", this.getAttribute("href"));
        });
    });
});


// --- On Page Load ---
document.addEventListener("DOMContentLoaded", function () {
    const isFullscreen = localStorage.getItem("isFullscreen") === "true";
    const $btn = $('#btnFullscreen');
    const $icon = $btn.find('i');

    // We CANNOT auto-enter fullscreen due to browser restrictions.
    // So only update the icon and tooltip to match last known state.
    if (isFullscreen) {
        $icon.removeClass('bi-arrows-fullscreen').addClass('bi-fullscreen-exit');
        $btn.attr('title', 'Exit Fullscreen');
    } else {
        $icon.removeClass('bi-fullscreen-exit').addClass('bi-arrows-fullscreen');
        $btn.attr('title', 'Enter Fullscreen');
    }
});

// --- Fullscreen toggle handler ---
$('#btnFullscreen').on('click', function (e) {
    e.preventDefault();

    const icon = $(this).find('i');
    const link = $(this);

    // Enter fullscreen
    if (!document.fullscreenElement) {
        document.documentElement.requestFullscreen();
        icon.removeClass('bi-arrows-fullscreen').addClass('bi-fullscreen-exit');
        link.attr('title', 'Exit Fullscreen');
        localStorage.setItem("isFullscreen", "true");
    }
    // Exit fullscreen
    else {
        if (document.exitFullscreen) document.exitFullscreen();
        icon.removeClass('bi-fullscreen-exit').addClass('bi-arrows-fullscreen');
        link.attr('title', 'Enter Fullscreen');
        localStorage.setItem("isFullscreen", "false");
    }
});

// --- Update localStorage when user exits manually (e.g. ESC) ---
document.addEventListener("fullscreenchange", function () {
    const $btn = $('#btnFullscreen');
    const $icon = $btn.find('i');

    if (!document.fullscreenElement) {
        $icon.removeClass('bi-fullscreen-exit').addClass('bi-arrows-fullscreen');
        $btn.attr('title', 'Enter Fullscreen');
        localStorage.setItem("isFullscreen", "false");
    } else {
        $icon.removeClass('bi-arrows-fullscreen').addClass('bi-fullscreen-exit');
        $btn.attr('title', 'Exit Fullscreen');
        localStorage.setItem("isFullscreen", "true");
    }
});



$('.dt').each(function () {
        $(this).DataTable({
            dom:
                "<'row align-items-center mb-3'<'col-md-4'l><'col-md-4 text-center'B><'col-md-4'f>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row mt-3'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            buttons: [
                { extend: 'copy', className: 'btn btn-secondary mx-1' },
                { extend: 'csv', className: 'btn btn-success mx-1' },
                { extend: 'excel', className: 'btn btn-primary mx-1' },
                { extend: 'pdf', className: 'btn btn-danger mx-1' },
                { extend: 'print', className: 'btn btn-info mx-1' }
            ],
            pageLength: 10,
            lengthMenu: [5, 10, 25, 50],
            language: {
                search: "Search:",
                lengthMenu: "Show _MENU_ entries",
                info: "Showing _START_ to _END_ of _TOTAL_ records"
            },
            pagingType: "simple_numbers",
            order: []
        });
});

function openModal(title, url, size = 'lg') {
    
    const $modal = $('#mainModal');
    const $dialog = $modal.find('.modal-dialog');

   
    // Remove previous size classes
    $dialog.removeClass('modal-sm modal-md modal-lg modal-xl modal-xxl');

    // Add new size class dynamically
    if (size) $dialog.addClass(`modal-${size}`);

    // Set modal title
    $('#mainModalTitle').text(title);

    // Clear and set modal body with loader
    $('#mainModalBody').empty();
    $('#mainModalBody').html(
        '<div class="text-center p-4" id="modalLoader">' +
        '<div class="spinner-border text-primary" role="status"></div>' +
        '<p class="mt-2 text-primary">Loading...</p>' +
        '</div>'
    );

    $('#modalLoader').show();


    $modal.modal('show');
    //$('#mainModalBody').empty();
    // Load partial view content
    $.get(url)
        .done(function (data) {
            $('#mainModalBody').empty();
            $('#mainModalBody').html(data);

        })
        .fail(function () {
            $('#modalLoader').hide();
            $('#mainModalBody').html('<p class="text-danger text-center">Failed to load content.</p>');
        });
}
function showConfirm(options) {
    const modalEl = document.getElementById('dynamicConfirmModal');
    const modal = new bootstrap.Modal(modalEl);

    // Set dynamic content
    document.getElementById('dynamicConfirmTitle').innerText = options.title || 'Confirm Action';
    document.getElementById('dynamicConfirmMessage').innerText = options.message || 'Are you sure you want to proceed?';
    document.getElementById('dynamicConfirmYes').innerText = options.confirmText || 'Yes';
    document.getElementById('dynamicConfirmCancel').innerText = options.cancelText || 'Cancel';

    // Remove previous click handlers
    const yesBtn = document.getElementById('dynamicConfirmYes');
    yesBtn.replaceWith(yesBtn.cloneNode(true));
    const newYesBtn = document.getElementById('dynamicConfirmYes');

    // Handle Yes button
    newYesBtn.addEventListener('click', () => {
        modal.hide();

        if (options.url) {
            // Navigate to a URL
            window.location.href = options.url;

            // Or for AJAX POST request:
            // fetch(options.url, { method: 'POST' })
            //     .then(res => res.json())
            //     .then(data => { if(options.onConfirm) options.onConfirm(data); })
            //     .catch(err => console.error(err));
        } else if (options.onConfirm && typeof options.onConfirm === 'function') {
            // Call a JS function
            options.onConfirm();
        }
    });

    modal.show();
}