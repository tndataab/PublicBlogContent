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

    // Retrieve the logout URL from the user's claims and redirect to it if available.
    // The URL is typically in the format of /bff/logout?sid=xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    GetLogOutUrl().then(function (logoutUrl) {
        window.location.href = logoutUrl && logoutUrl.length > 0 ? logoutUrl : '/bff/logout';
    });
}

function callLocalAPI() {
    updateContent('Calling Local API...');

    fetch('/api/local', {
        method: 'GET',
        headers: {
            'X-CSRF': '1'
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
            'X-CSRF': '1'
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



function getUser() {
    updateContent('Getting user information...');

    fetch('/bff/user', {
        method: 'GET',
        headers: {
            'X-CSRF': '1'
        },
        credentials: 'include'
    }).then(response => {
        if (!response.ok) {
            if (response.status === 401) {
                throw { status: 401, message: 'User not authenticated. Please login first.' };
            }
            return response.text().then(text => {
                throw { status: response.status, message: `Get User Error (${response.status}):\n${text}` };
            });
        }
        return response.json();
    }).then(data => {
        updateContent('User Information:\n' + JSON.stringify(data, null, 2));
    }).catch(error => {
        if (error.status === 401) {
            updateContent(error.message, true);
        } else {
            updateContent(error.message || 'An error occurred.', true);
        }
    });
}

// Retrieve the logout URL from the user's claims and redirect to it if available.
function GetLogOutUrl() {
    return fetch('/bff/user', {
        method: 'GET',
        headers: {
            'X-CSRF': '1'
        },
        credentials: 'include'
    })
        .then(response => {
            if (!response.ok) {
                return "";
            }
            return response.json();
        })
        .then(data => {
            if (Array.isArray(data)) {
                const logoutClaim = data.find(claim => claim.type === 'bff:logout_url');
                return logoutClaim && logoutClaim.value ? logoutClaim.value : "";
            }
            return "";
        })
        .catch(() => "");
}


function getDiagnostics() {
    updateContent('Getting BFF diagnostics...');

    fetch('/bff/diagnostics', {
        method: 'GET',
        credentials: 'include',
        headers: {
            'Accept': 'application/json'
        }
    }).then(response => {
        if (!response.ok) {
            if (response.status === 401) {
                throw { status: 401, message: 'User not authenticated. Please login first.' };
            }
            return response.text().then(text => {
                throw { status: response.status, message: `Diagnostics Error (${response.status}):\n${text}` };
            });
        }
        return response.json();
    }).then(data => {
        updateContent('BFF Diagnostics:\n' + JSON.stringify(data, null, 2));
    }).catch(error => {
        if (error.status === 401) {
            updateContent(error.message, true);
        } else {
            updateContent(error.message || 'An error occurred.', true);
        }
    });
}


function getIdentity() {
    updateContent('Getting user identity...');

    fetch('/User/GetIdentity', {
        method: 'GET',
        credentials: 'include',
        headers: {
            'Accept': 'application/json'
        }
    }).then(response => {
        if (!response.ok) {
            if (response.status === 401) {
                throw { status: 401, message: 'User not authenticated. Please login first.' };
            }
            return response.text().then(text => {
                throw { status: response.status, message: `Get Identity Error (${response.status}):\n${text}` };
            });
        }
        return response.json();
    }).then(data => {
        updateContent('User Identity:\n' + JSON.stringify(data, null, 2));
    }).catch(error => {
        if (error.status === 401) {
            updateContent(error.message, true);
        } else {
            updateContent(error.message || 'An error occurred.', true);
        }
    });
}
