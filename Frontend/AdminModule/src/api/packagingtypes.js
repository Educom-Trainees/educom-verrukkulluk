import {fetchData} from './fetch'

// function to GET packaging type info from DB
export function getPackagingTypes () {
    return fetchData('../api/packagingtypes');
}

// function to POST a new packaging type to DB
export function postPackagingType (packagingJSON) {
    return fetch('../api/packagingtypes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: packagingJSON,
    });
}

// function to update (PUT) an existing packaging type
export function putPackagingType ({id, packagingJSON}) { // destructoring necessary as react-query mutate functions only take one input
    return fetch('../api/packagingtypes/'+id, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: packagingJSON,
    });
}