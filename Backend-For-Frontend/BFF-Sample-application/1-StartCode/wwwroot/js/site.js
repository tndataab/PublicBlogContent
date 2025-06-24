// BFF Demo JavaScript Functions

function updateContent(message, isError = false) {
    const contentDiv = document.getElementById('content');
    contentDiv.innerHTML = message;
    if (isError) {
        contentDiv.className = 'border p-3 bg-danger text-white';
    } else {
        contentDiv.className = 'border p-3 bg-light';
    }
}

function login() {
    updateContent('Redirecting to login...');
    window.location.href = '/bff/login';
}

function logout() {
    updateContent('Logging out...');
    window.location.href = '/bff/logout';
}

function getSession() {
    updateContent('Getting session details...');
    
    $.ajax({
        url: '/bff/session',
        type: 'POST',
        contentType: 'application/json',
        headers: {
        },
        success: function(data) {
            updateContent('Session Response:\n' + JSON.stringify(data, null, 2));
        },
        error: function(xhr, status, error) {
            updateContent(`Session Error (${xhr.status}):\n${xhr.responseText || error}`, true);
        }
    });
}

function callLocalAPI() {
    updateContent('Calling Local API...');
    
    $.ajax({
        url: '/api/Local',
        type: 'GET',
        headers: {
        },
        success: function(data) {
            updateContent('Local API Response:\n' + JSON.stringify(data, null, 2));
        },
        error: function(xhr, status, error) {
            updateContent(`Local API Error (${xhr.status}):\n${xhr.responseText || error}`, true);
        }
    });
}

function callRemoteAPI() {
    updateContent('Calling Remote API...');
    
    $.ajax({
        url: '/api/remote',
        type: 'GET',
        headers: {
        },
        success: function(data) {
            updateContent('Remote API Response:\n' + JSON.stringify(data, null, 2));
        },
        error: function(xhr, status, error) {
            updateContent(`Remote API Error (${xhr.status}):\n${xhr.responseText || error}`, true);
        }
    });
}
