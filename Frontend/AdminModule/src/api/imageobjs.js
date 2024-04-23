// function to POST a new image to DB
export function postImage (imgJSON) {
    return fetch('../api/imageobj', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: imgJSON,
    });
}