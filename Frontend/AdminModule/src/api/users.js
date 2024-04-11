import {fetchData} from './fetch'

// function to GET users info from DB
export function getUsers () {
    return fetchData('../api/users');
}

// function to GET a specific user's info from DB
export function getUser (id) {
    return fetchData('../api/users/'+id);
}

// function to update (PUT) an existing user
export function putUser ({id, userJSON}) { // destructoring necessary as react-query mutate functions only take one input
    return fetch('../api/users/'+id, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: userJSON,
    });
}

export function deleteUser (id) {
    return fetch('../api/users/'+id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
    })
}