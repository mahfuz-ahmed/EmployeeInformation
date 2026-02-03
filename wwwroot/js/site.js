document.addEventListener('DOMContentLoaded', function () {

    var commonDeleteModal = document.getElementById('commonDeleteModal');

    if (!commonDeleteModal) return;

    commonDeleteModal.addEventListener('show.bs.modal', function (event) {

        var button = event.relatedTarget;
        var id = button.getAttribute('data-id');
        var url = button.getAttribute('data-url');
        var message = button.getAttribute('data-message')
            || 'Are you sure you want to delete this record?';

        // Set modal message
        document.getElementById('commonDeleteMessage').textContent = message;

        // Set form action dynamically
        var form = document.getElementById('commonDeleteForm');
        form.action = url + id;
    });
});
