import {fetchData} from './fetch'

// function to GET comment info from DB
export function getComments () {
    return fetchData('../api/comments');
}

// function to GET comment info for a specific user from DB
export function getCommentsByUser (userId) {
    return fetchData('../api/comments/users/'+userId);
}

// function to POST a new comment to DB
export function postComment (commentJSON) {
    return fetch('../api/comments', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: commentJSON,
    });
}

// function to update (PUT) an existing comment
export function putComment ({id, commentJSON}) { // destructoring necessary as react-query mutate functions only take one input
    return fetch('../api/comments/users/'+id, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: commentJSON,
    });
}

// function to DELETE a comment
export function deleteComment({ id, recipeId }) {
    return fetch('../api/comments/users/' + id+'?recipeId='+recipeId, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
    })
}