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

    fetch('/bff/session', {
        method: 'POST',
        headers: {
        }
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Session Error (${response.status}):\n${text}`);
            });
        }
        return response.json();
    }).then(data => {
        updateContent('Session Response:\n' + JSON.stringify(data, null, 2));
    }).catch(error => {
        updateContent(error.message, true);
    });
}

function callLocalAPI() {
    updateContent('Calling Local API...');

    fetch('/api/local', {
        method: 'GET',
        headers: {
        }
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Local API Error (${response.status}):\n${text}`);
            });
        }
        return response.json();
    }).then(data => {
        updateContent('Local API Response:\n' + JSON.stringify(data, null, 2));
    }).catch(error => {
        updateContent(error.message, true);
    });
}

function callRemoteAPI() {
    updateContent('Calling Remote API...');

    fetch('/api/remote', {
        method: 'GET',
        headers: {
        }
    }).then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`Remote API Error (${response.status}):\n${text}`);
            });
        }
        return response.json();
    }).then(data => {
        updateContent('Remote API Response:\n' + JSON.stringify(data, null, 2));
    }).catch(error => {
        updateContent(error.message, true);
    });
}

